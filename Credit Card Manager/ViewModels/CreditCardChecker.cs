using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Credit_Card_Manager.Helpers;
using Credit_Card_Manager.Models;
using CreditCardValidator;

namespace Credit_Card_Manager.ViewModels
{
    public class CreditCardChecker
    {
        private CreditCardDBContext db = new CreditCardDBContext();
        public string CardNumber { get; private set; }
        public CreditCard CreditCardBrand { get; private set; }

        public bool IsCardValid { get; private set; }

        public CreditCardChecker(string cardNumber)
        {
            if (!ValidationHelper.IsAValidNumber(cardNumber))
            {
                IsCardValid = false;
                return;
            }

            CardNumber = cardNumber.RemoveWhiteSpace();
            LoadCard();
        }

        private void LoadCard()
        {
            var creditCardBrands = db.CreditCards.Include(u => u.Rules).OrderBy(i => i.Name);
            foreach (var brandData in creditCardBrands)
            {
                // CardInfo from one brand.

                foreach (var rule in brandData.Rules)
                {
                    if (rule.Length == CardNumber.Length &&
                        CardNumber.StartsWith(rule.Prefix.ToString()))
                    {
                        CreditCardBrand = brandData;
                        if(!rule.SkipLuhnCheck)
                        {
                            IsCardValid = IsValid();
                        }
                        return;
                    }
                }
            }
        }

        private bool IsValid()
        {
            // The Brand rules were already checked by LoadCard(). So, if a card has a brand, means
            // that the number meets at least one of the rule requirements.
            return Luhn.CheckLuhn(CardNumber);
        }

    }
}