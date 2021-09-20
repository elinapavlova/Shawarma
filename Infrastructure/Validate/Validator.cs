using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dadata;
using Dadata.Model;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Validate
{
    public class Validator
    {
        private readonly IConfiguration _configuration;
        public Validator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private static readonly Regex ValidEmailRegex = CreateValidEmailRegex();
        
        private static Regex CreateValidEmailRegex()
        {
            const string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                             + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                             + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        public static bool EmailIsValid(string emailAddress)
        {
            var isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }
        
        public async Task<string> ValidateAddress(string address)
        {
            var token = _configuration["DadataApiSettings:Token"];
            var secret = _configuration["DadataApiSettings:Secret"];
            
            var api = new CleanClientAsync(token, secret);
            var result = await api.Clean<Address>(address);
            return result.qc;
        }
    }
}