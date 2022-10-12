using Credit_Card_Manager.Models;
using System.Collections.Generic;
using System.Linq;

namespace Credit_Card_Manager.Helpers
{
    internal static class ValidationHelper
    {
        public static bool IsAValidNumber(string number)
        {
            number = number.RemoveWhiteSpace();

            return (number
                .ToCharArray()
                .All(char.IsNumber) &&
                    !string.IsNullOrEmpty(number));
        }
    }
}