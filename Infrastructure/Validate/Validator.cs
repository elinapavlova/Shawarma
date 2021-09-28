﻿using System.Text.RegularExpressions;

namespace Infrastructure.Validate
{
    public static class Validator
    {
        private const string ValidEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                                 + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                                 + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public static bool EmailIsValid(string email) 
            => Regex.IsMatch(email, ValidEmailPattern);
    }
}