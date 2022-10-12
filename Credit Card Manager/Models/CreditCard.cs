using Credit_Card_Manager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Credit_Card_Manager.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
            Name = "Unknown";
        }
        
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Rule> Rules { get; set; }
    }
    public class Rule
    {
        public Rule()
        {
            SkipLuhnCheck = false;
        }

        public int ID { get; set; }
        public int Length { get; set; }
        public int Prefix { get; set; }
        [Display(Name ="Skip Luhn Check")]
        public bool SkipLuhnCheck { get; set; }
        
        public virtual int CreditCardID { get; set; }
        public virtual CreditCard CreditCard { get; set; }
    }
    public class UserCard
    {
        public int ID { get; set; }

        [StringLength(40)]
        [Display(Name = "Card Number")]
        [CreditCardNumberValid(ErrorMessage = "Invalid number. Just numbers and white spaces are accepted on the string")]
        [CustomCardValid(ErrorMessage = "Please enter a valid Card Number")]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
        public virtual int CreditCardID { get; set; }

        [Display(Name = "Card Issuer")]
        public virtual CreditCard Brand { get; set; }
    }

    public class CreditCardDBContext : DbContext
    {
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<UserCard> UserCards { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCard>()
                .HasIndex(u => u.CardNumber)
                .IsUnique();
        }
    }

}