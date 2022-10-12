using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credit_Card_Manager.Helpers
{
    internal static class StringHelper
    {
        public static string RemoveWhiteSpace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}