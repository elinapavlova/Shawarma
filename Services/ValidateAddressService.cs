using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dadata.Model;
using Newtonsoft.Json;
using Services.Contracts;

namespace Services
{
    public class ValidateAddressService : IValidateAddressService
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public ValidateAddressService (IHttpClientFactory clientFactory )
        {
            _clientFactory = clientFactory;
        }
        
        public async Task<string> ValidateAddress(string address)
        {
            using var client = _clientFactory.CreateClient("Dadata");

            var content = new StringContent
            (
                "[ \"" + address + "\" ]", 
                Encoding.UTF8, 
                "application/json"
            );
            
            var response = await client.PostAsync(client.BaseAddress, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) 
                return null;
            
            var fiasId = DeserializeAddress(result);
            return fiasId;
        }

        private static string DeserializeAddress(string result)
        {
            var address = JsonConvert.DeserializeObject<List<Address>>(result);
            var fias = address.First().fias_id;
            return fias;
        }
    }
}

// "Token" : "fecdd694d70f0851b4e75879cfc26a60af4cdb09",
// "Secret": "13815be7661b07d5b2eab98473c176698a2f9b1d",