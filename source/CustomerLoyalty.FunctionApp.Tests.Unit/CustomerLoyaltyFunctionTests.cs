using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.FunctionApp.Tests.Unit
{
    [ExcludeFromCodeCoverage]
    public class CustomerLoyaltyFunctionTests
    {
        private readonly Mock<ICustomerLoyaltyService> _customerLoyaltyServiceMock;
        private readonly CustomerLoyaltyFunction _customerLoyaltyFunction;

        public CustomerLoyaltyFunctionTests()
        {
            _customerLoyaltyServiceMock = new Mock<ICustomerLoyaltyService>();
            _customerLoyaltyServiceMock.Setup(setup => setup.CreateCustomerLoyalty(It.IsAny<Customer>()))
                .ReturnsAsync(new CustomerLoyaltyIndicator());

            _customerLoyaltyFunction = new CustomerLoyaltyFunction(_customerLoyaltyServiceMock.Object);
        }

        [Fact]
        public void Ctor_WhenCalledWithANullReference_ShouldThrowAnArugmentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CustomerLoyaltyFunction(default(ICustomerLoyaltyService)));
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenRequestBodyIsEmpty_ShouldReturnA400BadRequest()
        {
            // Arrange
            using (Stream stream = new MemoryStream())
            {
                HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {Body = stream};

                // Act
                BadRequestObjectResult result =
                    await _customerLoyaltyFunction.CreateCustomerLoyalty(request, new Mock<ILogger>().Object) as
                        BadRequestObjectResult;

                // Assert
                Assert.NotNull(result);
                _customerLoyaltyServiceMock.Verify(verify => verify.CreateCustomerLoyalty(It.IsAny<Customer>()),
                    Times.Never);
            }
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenRequestHanaRequestFails_ShouldReturnA400BadRequest()
        {
            string customer = JsonConvert.SerializeObject(new Customer
            {
                Address1 = "123 Test Drive",
                BenjaminMooreCustomerId = "AG1",
                Address2 = "Apartment 101",
                BusinessEmailAddress = "test@test.test",
                BusinessName = "Test",
                BusinessPhoneNumber = "5555555555",
                BusinessType = "paint test",
                City = "Test",
                ContactEmailAddress = "test@test.test",
                ContactPhoneNumber = "5555555555",
                FirstName = "T",
                LastName = "Est",
                LoyaltyEmailAddress = "test@test.test",
                PostalCode = "12345",
                RetailerId = "AG1R",
                State = "NY",
                SegmentCode = "1234",
                LanguageCode = "EN"
            });

            // Arrange
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(customer)))
            {
                HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {Body = stream};
                _customerLoyaltyServiceMock.Setup(setup => setup.CreateCustomerLoyalty(It.IsAny<Customer>()))
                    .Throws(new HanaRequestException(ErrorCollection.GetSerializedErrors()));


                // Act
                BadRequestObjectResult result =
                    await _customerLoyaltyFunction.CreateCustomerLoyalty(request, new Mock<ILogger>().Object) as
                        BadRequestObjectResult;

                // Assert
                Assert.NotNull(result);
                _customerLoyaltyServiceMock.Verify(verify => verify.CreateCustomerLoyalty(It.IsAny<Customer>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task CreateCustomerLoyalty_WhenRequestBodyIsValid_ShouldReturnA200Ok()
        {
            // Arrange

            string customer = JsonConvert.SerializeObject(new Customer
            {
                Address1 = "123 Test Drive",
                BenjaminMooreCustomerId = "AG1",
                Address2 = "Apartment 101",
                BusinessEmailAddress = "test@test.test",
                BusinessName = "Test",
                BusinessPhoneNumber = "5555555555",
                BusinessType = "paint test",
                City = "Test",
                ContactEmailAddress = "test@test.test",
                ContactPhoneNumber = "5555555555",
                FirstName = "T",
                LastName = "Est",
                LoyaltyEmailAddress = "test@test.test",
                PostalCode = "12345",
                RetailerId = "AG1R",
                State = "NY",
                SegmentCode = "1234",
                LanguageCode = "EN"
            });

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(customer)))
            {
                HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext()) {Body = stream};

                // Act
                OkObjectResult result =
                    await _customerLoyaltyFunction.CreateCustomerLoyalty(request, new Mock<ILogger>().Object) as
                        OkObjectResult;

                // Assert
                Assert.NotNull(result);
                _customerLoyaltyServiceMock.Verify(verify => verify.CreateCustomerLoyalty(It.IsAny<Customer>()),
                    Times.Once);
            }
        }

        internal class ErrorCollection
        {
            public static string GetSerializedErrors()
            {
                ErrorCollection root = new ErrorCollection(new Error {Code = "Failed", Message = "Missing Email"});

                return JsonConvert.SerializeObject(root);
            }

            [JsonProperty("errors")] public Error[] Errors { get; set; }

            public ErrorCollection()
            {

            }

            public ErrorCollection(params Error[] errors)
            {
                Errors = errors;
            }
        }

        internal class Error
        {
            public string Code { get; set; }
            public string Message { get; set; }
        }
    }
}
