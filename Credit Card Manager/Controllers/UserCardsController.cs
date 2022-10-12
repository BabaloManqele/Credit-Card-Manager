using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Credit_Card_Manager.Models;
using Credit_Card_Manager.ViewModels;
using CreditCardValidator;
using PagedList;

namespace Credit_Card_Manager.Controllers
{
    public class UserCardsController : Controller
    {
        private CreditCardDBContext db = new CreditCardDBContext();

        // GET: UserCards
        public ActionResult Index(string brandSearch, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var brandList = new List<string>();

            var cards = from c in db.UserCards
                        .Include(i=> i.Brand)
                        .OrderBy(i=> i.Brand.Name).OrderBy(i=> i.CardNumber)
                        select c;

            var brandsQuery = from d in db.UserCards
                           orderby d.Brand.Name
                           select d.Brand.Name;

            brandList.AddRange(brandsQuery.Distinct());
            ViewBag.brandSearch = new SelectList(brandList);

            if (!string.IsNullOrEmpty(searchString))
            {
                cards = cards.Where(c => c.CardNumber.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(brandSearch))
            {
                cards = cards.Where(x => x.Brand.Name == brandSearch);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(cards.ToPagedList(pageNumber, pageSize));
            //return View(cards);
        }

        // GET: UserCards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCard userCard = db.UserCards.Find(id);
            if (userCard == null)
            {
                return HttpNotFound();
            }
            return View(userCard);
        }

        // GET: UserCards/Create
        public ActionResult Create()
        {
            //ViewBag.RandomCard = CreditCardFactory.RandomCardNumber(CardIssuer.AmericanExpress);
            ViewBag.CreditCardID = new SelectList(db.CreditCards, "ID", "Name");
            return View();
        }

        // POST: UserCards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CardNumber,CreditCardID")] UserCard userCard)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    CreditCardChecker cc = new CreditCardChecker(userCard.CardNumber.ToString());
                    if (cc.IsCardValid)
                    {
                        if (db.UserCards.Any(i => i.CardNumber == userCard.CardNumber))
                           ModelState.AddModelError("Duplicate", "Credit Card Already Exists in the system.");
                        else
                        {
                            userCard.Brand = db.CreditCards.Single(c => c.ID == cc.CreditCardBrand.ID);
                            db.UserCards.Add(userCard);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch(RetryLimitExceededException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            ViewBag.CreditCardID = new SelectList(db.CreditCards, "ID", "Name", userCard.CreditCardID);
            return View(userCard);
        }

        // GET: UserCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCard userCard = db.UserCards.Find(id);
            if (userCard == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreditCardID = new SelectList(db.CreditCards, "ID", "Name", userCard.CreditCardID);
            return View(userCard);
        }

        // POST: UserCards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CardNumber,CreditCardID")] UserCard userCard)
        {

            if (ModelState.IsValid)
            {
                CreditCardChecker cc = new CreditCardChecker(userCard.CardNumber.ToString());
                if (cc.IsCardValid)
                {
                    if (db.UserCards.Any(i => i.CardNumber == userCard.CardNumber))
                    {
                        ModelState.AddModelError("Duplicate", "Credit Card Already Exists in the system.");
                        return View(userCard);
                    }
                    else
                    {
                        userCard.Brand = db.CreditCards.Single(c => c.ID == cc.CreditCardBrand.ID);
                        db.Entry(userCard).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.CreditCardID = new SelectList(db.CreditCards, "ID", "Name", userCard.CreditCardID);
            return View(userCard);
        }

        // GET: UserCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCard userCard = db.UserCards.Find(id);
            if (userCard == null)
            {
                return HttpNotFound();
            }
            return View(userCard);
        }

        // POST: UserCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserCard userCard = db.UserCards.Find(id);
            db.UserCards.Remove(userCard);
            db.SaveChanges();
            return RedirectToAction("Index");
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
