using System.Text.RegularExpressions;

namespace Infrastructure.Validate
{
    public class Validator
    {
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
    }
}