using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DVLD.Global_Classes
{
    public class clsValidation
    {
        public static bool ValidateEmail(string EmailAddress)
        {
            var Pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var Regex = new Regex(Pattern);

            return Regex.IsMatch(EmailAddress);
        }
        public static bool ValidateInteger(string Number)
        {
            var Pattern = @"^[0-9]*$";
            var Regex = new Regex(Pattern);
            return Regex.IsMatch(Number);
        }
        public static bool ValidateFloat(string Number)
        {
            var Pattern = @"^[0-9]*(?:\.[0-9]*)?$";
            var Regex = new Regex(Pattern);
            return Regex.IsMatch(Number);
        }
        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number) ||  ValidateFloat(Number));
        }
    }
}
