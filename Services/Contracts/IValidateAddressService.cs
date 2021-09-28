using System.Threading.Tasks;

namespace DadataService
{
    public interface IValidateAddressService
    {
        Task<string> ValidateAddress(string address);
    }
}