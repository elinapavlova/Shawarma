using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using ViewModels;

namespace DadataService.Controllers
{
    public class ValidateController : Controller
    {
        private readonly IValidateAddressService _validateService;

        public ValidateController
        (
            IValidateAddressService validateService
        )
        {
            _validateService = validateService;
        }
        
        [HttpPost]
        public async Task<string> ValidateAddress(AddressViewModel address)
        {
            return await _validateService.ValidateAddress(address.Name);
        }
    }
}