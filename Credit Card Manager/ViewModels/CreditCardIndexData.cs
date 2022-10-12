using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Credit_Card_Manager.Models;

namespace Credit_Card_Manager.ViewModels
{
    public class CreditCardIndexData
    {
        public IEnumerable<CreditCard> CreditCards { get; set; }
        public IEnumerable<Rule> Rules { get; set; }
    }
}