using System.Threading.Tasks;
using Dadata;
using Dadata.Model;
using DadataService;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class ValidateAddressService : IValidateAddressService
    {
        private readonly IConfiguration _configuration;
        
        public ValidateAddressService (IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<string> ValidateAddress(string address)
        {
            var token = _configuration["DadataApiSettings:Token"];
            var secret = _configuration["DadataApiSettings:Secret"];
            
            var api = new CleanClientAsync(token, secret);
            var result = await api.Clean<Address>(address);
            return result.fias_id;
        }
    }
}