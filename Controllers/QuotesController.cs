using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MachineQuotes.Models;
using Microsoft.AspNet.Identity;

namespace MachineQuotes.Controllers
{
    public class QuotesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Quotes
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return View(db.Quotes.ToList());
            }
            string userId = User.Identity.GetUserId();
            
            List<Quote> userQuotes = db.Quotes.Select(m => m).Where(s => s.SalesmanId == userId).ToList();
            return View(userQuotes.ToList());
        }

        // GET: Quotes/Details/5
        public ActionResult Details(int? id)
        {
            TempData.Clear();
           
            List<Machine> Machines = new List<Machine>();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Include(o => o.Machines).Where(s => s.Id == id).Single();


            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // GET: Quotes/Create
        public ActionResult Create()
        {

            List<Quote> quotes = db.Quotes.Include(s => s.Machines).ToList();

            List<Machine> machines = db.Machines.ToList();

            foreach (Quote q in quotes)
            {
                foreach(Machine m in q.Machines)
                {
                    machines.Remove(m);
                }
            }


            TempData.Clear();
            TempData.Add("Machines", new MultiSelectList(machines, "Id", "MachineName"));
       
            return View();
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult  Create(int[] Machines, [Bind(Include = "Id,SalesmanId,CustomerId,Date,Note")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                List<int>MachineIdList = Machines.ToList();

                List<Machine> newMachines = new List<Machine>();
                List<Option> newOptions = new List<Option>();

                List<Machine> DbMachines = db.Machines.Include(o => o.Options).ToList();

                foreach(int x in MachineIdList)
                {   Machine add = DbMachines.Where(o => o.Id == x).Single();

                    foreach (Option o in add.Options)
                    {
                        Option cloneOption = new Option
                        {
                            Description = o.Description,
                            Group = o.Group,
                            IsStandard = o.IsStandard,
                            Price = o.Price,
                            Selected = false
                        };
                        if(cloneOption.IsStandard == true)
                        {
                            cloneOption.Selected = true;
                        }

                        newOptions.Add(cloneOption);
                    }

                    Machine newAdd = new Machine()
                    {
                        MachineName = add.MachineName,
                        Price = add.Price,
                        Options = newOptions,
                    };
                    newMachines.Add(newAdd);
                    
                };

                quote.Machines = newMachines;
                quote.SalesmanId = User.Identity.GetUserName();
                quote.Date = DateTime.Today.Date;
                db.Quotes.Add(quote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quote);
        }

        // GET: Quotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SalesmanId,CustomerId,Date,Note")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quote);
        }

        // GET: Quotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Find(id);
            if (quote == null)
            {
                return HttpNotFound();
            }
            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quote quote = db.Quotes.Include(o=>o.Machines).Where(o=>o.Id == id).Single();
            List<Machine> machines = quote.Machines;
            List<Option> options = new List<Option>();

            foreach(Machine m in machines)
            {
                Machine temp = db.Machines.Include(o => o.Options).Where(s => s.Id == m.Id).Single();

                foreach(Option o in temp.Options)
                {
                    options.Add(o);
                }
            }
            
            db.Machines.RemoveRange(machines);
            db.Quotes.Remove(quote);
            db.Options.RemoveRange(options);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SelectOptions(int id)
        {

            Machine machine = db.Machines.Include(o=>o.Options).Where(o=>o.Id == id).Single();

            return PartialView(machine.Options);
        }
        [HttpPost]
        public ActionResult SelectOptions(List<Option> model)
        {
            if (ModelState.IsValid)
            {
                foreach(Option o in model)
                {
                    var option = db.Options.Find(o.Id);

                    option.Selected = o.Selected;
                }
                
                db.SaveChanges();
                
            }

            return RedirectToAction("SelectOptions");
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
