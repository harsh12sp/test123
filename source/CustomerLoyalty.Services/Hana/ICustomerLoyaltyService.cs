using System.Threading.Tasks;
using BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Entities;

namespace BenjaminMoore.Api.Retail.Pos.CustomerLoyalty.Services.Hana
{
    public interface ICustomerLoyaltyService
    {
        Task<CustomerLoyaltyIndicator> CreateCustomerLoyalty(Customer customer);
    }
}