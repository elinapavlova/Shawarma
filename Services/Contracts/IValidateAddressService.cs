using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IValidateAddressService
    {
        Task<string> ValidateAddress(string address);
    }
}