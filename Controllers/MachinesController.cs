using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MachineQuotes.Models;
using CsvHelper;
using System.IO;


namespace MachineQuotes.Controllers
{
    public class MachinesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Machines
        public ActionResult Index()
        {
            List<Machine> machines = db.Machines.ToList();
            List<Quote> quotes = db.Quotes.Include(o => o.Machines).ToList();
            
            foreach(Quote q in quotes)
            {
                foreach(Machine m in q.Machines)
                {
                    machines.Remove(m);
                }
            }

            return View(machines);
        }

        // GET: Machines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = db.Machines.Find(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            ViewBag.options = machine.Options;
            return View(machine);
        }

        // GET: Machines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Machines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MachineName,Price")] Machine machine)
        {


            if (ModelState.IsValid)
            {
                var file = Request.Files["CSVupload"];


                if (file != null && file.ContentLength > 0)
                {
                    List<ImportViewModel> imports = new List<ImportViewModel>();

                    StreamReader sr = new StreamReader(file.InputStream);
                    CsvReader SV = new CsvReader(sr);

                    imports = SV.GetRecords<ImportViewModel>().ToList();

                    List<Option> MachineOptions = new List<Option>();

                    foreach (ImportViewModel import in imports)
                    {
                        Option newOption = new Option()
                        {
                            Group = import.Group,
                            Description = import.Description,
                            Price = import.Price,
                            IsStandard = import.IsStandard,
                        };
                        MachineOptions.Add(newOption);
                    }
                    machine.Options = MachineOptions;
                }

                db.Machines.Add(machine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(machine);
        }

        // GET: Machines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = db.Machines.Find(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            return View(machine);
        }

        // POST: Machines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MachineName,Price")] Machine machine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(machine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(machine);
        }

        // GET: Machines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = db.Machines.Find(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            return View(machine);
        }

        // POST: Machines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Machine machine = db.Machines.Find(id);
            db.Options.RemoveRange(machine.Options);
            db.Machines.Remove(machine);
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
