namespace Credit_Card_Manager.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CreditCardDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Credit_Card_Manager.Models.CreditCardDBContext";
        }

        protected override void Seed(CreditCardDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var creditCards = new List<CreditCard>
            {
                 new CreditCard(){Name = "Visa"},
            };

            creditCards.ForEach(s => context.CreditCards.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var rules = new List<Rule>
            {
                new Rule(){Length=15,Prefix = 4,CreditCardID=creditCards.Single(s=> s.Name == "Visa").ID},
                new Rule(){Length=13,Prefix = 4,CreditCardID=creditCards.Single(s=> s.Name == "Visa").ID}
            };

            rules.ForEach(s => context.Rules.AddOrUpdate(p => new { p.Prefix, p.Length }, s));
            context.SaveChanges();
        }
    }
}
