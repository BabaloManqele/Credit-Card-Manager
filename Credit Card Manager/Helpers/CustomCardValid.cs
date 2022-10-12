using Credit_Card_Manager.ViewModels;
using Credit_Card_Manager.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Credit_Card_Manager.Helpers
{
    public class CustomCardValid : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            CreditCardChecker cc = new CreditCardChecker(value.ToString());
            return cc.IsCardValid;
        }
    }
    public class CreditCardNumberValid : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return ValidationHelper.IsAValidNumber(value.ToString());
        }
    }
}