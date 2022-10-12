using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Credit_Card_Manager.Models;
using Rule = Credit_Card_Manager.Models.Rule;
using Credit_Card_Manager.ViewModels;
using System.Data.Entity.Infrastructure;

namespace Credit_Card_Manager.Controllers
{
    public class RulesController : Controller
    {
        private CreditCardDBContext db = new CreditCardDBContext();

        // GET: Rules
        public ActionResult Index()
        {
            var rules = db.Rules.Include(c => c.CreditCard);
            return View(rules.ToList());
        }

        // GET: Rules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rule rule = db.Rules.Find(id);
            if (rule == null)
            {
                return HttpNotFound();
            }
            return View(rule);
        }

        // GET: Rules/Create
        public ActionResult Create(int? id)
        {
            if(id == null)
            {
                PopulateCreditCardsDropDownList();
                return View();
            }
            PopulateCreditCardsDropDownList(id);
            return View();
        }

        // POST: Rules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Length,Prefix,SkipLuhnCheck,CreditCardID")] Rule rule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Rules.Add(rule);
                    db.SaveChanges();
                    return RedirectToAction("Index","CreditCards",new { id = rule.CreditCardID });
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateCreditCardsDropDownList(rule.CreditCard.ID);
            return View(rule);
        }

        // GET: Rules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rule rule = db.Rules.Find(id);
            if (rule == null)
            {
                return HttpNotFound();
            }
            PopulateCreditCardsDropDownList(rule.CreditCard.ID);
            return View(rule);
        }

        //// POST: Rules/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Length,Prefix,SkipLuhnCheck,CreditCardID")] Rule rule)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(rule).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(rule);
        //}

        // POST: Rules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ruleToUpdate = db.Rules.Find(id);
            if (TryUpdateModel(ruleToUpdate, "",
               new string[] { "Length", "Prefix", "SkipLuhnCheck", "CreditCard" }))
            {
                try
                {
                    db.SaveChanges();

                    //return RedirectToAction("Index","CreditCards", new { ruleToUpdate.CreditCardID });
                    return RedirectToAction("Index","CreditCards",new { id = ruleToUpdate.CreditCardID });
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateCreditCardsDropDownList(ruleToUpdate.CreditCard.ID);
            return View(ruleToUpdate);
        }

        // GET: Rules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rule rule = db.Rules.Find(id);
            if (rule == null)
            {
                return HttpNotFound();
            }
            return View(rule);
        }

        // POST: Rules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rule rule = db.Rules.Find(id);
            db.Rules.Remove(rule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void PopulateCreditCardsDropDownList(object selectedCreditCard = null)
        {
            var creditCardsQuery = from d in db.CreditCards
                                   orderby d.Name
                                   select d;
            ViewBag.CreditCardID = new SelectList(creditCardsQuery, "ID", "Name", selectedCreditCard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
