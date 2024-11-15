using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    /// <summary>
    /// Exists for development testing and should not be used in production.
    /// This is to account for both off-premises access restrictions and that
    /// azure functions are deliberately only allowed to communicate with
    /// azure api management.
    /// </summary>
    public class FakeCustomerLoyaltyService:ICustomerLoyaltyService
    {
        public Task<CustomerLoyaltyIndicator> CreateCustomerLoyalty(Customer customer)
        {
            return Task.FromResult(new CustomerLoyaltyIndicator
                {Id = customer.CustomerId, LoyaltyIndicator = "Y"});
        }
    }
}