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
using System.Linq.Expressions;
using System.Web.Routing;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using MachineQuotes.Models.PDF_Models;
using System.Net.Mail;
using System.Net.Mime;
using CsvHelper;

namespace MachineQuotes.Controllers
{
    [Authorize]
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
            string userId = User.Identity.GetUserName();


            List<Quote> userQuotes = db.Quotes.Select(m => m).Where(s => s.SalesmanId == userId).ToList();
            return View(userQuotes.ToList());
        }

        // GET: Quotes/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quote quote = db.Quotes.Include(o => o.Machines).Where(s => s.Id == id).Single();

            foreach (Machine m in quote.Machines)
            {
                Machine machine = db.Machines.Include(o => o.Options).Where(o => o.Id == m.Id).Single();
                m.Options = machine.Options;
            }

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
                foreach (Machine m in q.Machines)
                {
                    machines.Remove(m);
                }
            }

            Quote quote = new Quote()
            {
                Machines = machines,
            };


            return View(quote);
            ;
        }

        // POST: Quotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SalesmanId,CustomerId,Date,Note,Machines,CustomerEmail")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                List<Machine> newMachines = new List<Machine>();


                List<Machine> DbMachines = db.Machines.Include(o => o.Options).ToList();

                foreach (Machine x in quote.Machines)
                {
                    Machine add = DbMachines.Where(o => o.Id == x.Id).Single();

                    if (x.isSelected == true)
                    {
                        List<Option> newOptions = new List<Option>();

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
                            if (cloneOption.IsStandard == true)
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
                    }


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
            Quote quote = db.Quotes.Include(o => o.Machines).Where(o => o.Id == id).Single();
            List<Machine> machines = quote.Machines;
            List<Option> options = new List<Option>();

            foreach (Machine m in machines)
            {
                Machine temp = db.Machines.Include(o => o.Options).Where(s => s.Id == m.Id).Single();

                foreach (Option o in temp.Options)
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

            Machine machine = db.Machines.Include(o => o.Options).Where(o => o.Id == id).Single();

            return PartialView(machine.Options);
        }
        [HttpPost]
        public ActionResult Details(Quote model)
        {
            if (ModelState.IsValid)
            {
                Quote q = db.Quotes.Find(model.Id);

                q.Note = model.Note;
                q.CustomerEmail = model.CustomerEmail;

                foreach (Machine m in model.Machines)
                {
                    foreach (Option o in m.Options)
                    {
                        Option op = db.Options.Find(o.Id);
                        op.Selected = o.Selected;
                    }
                }

                db.SaveChanges();


            }

            return RedirectToAction("details", model.Id);
        }
        public ActionResult GeneratePDF(int id)
        {
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            Quote quote = db.Quotes.Include(o => o.Machines).Where(s => s.Id == id).Single();
            List<Option> selectedOptions = new List<Option>();

            foreach (Machine m in quote.Machines)
            {
                Machine machine = db.Machines.Include(o => o.Options).Where(o => o.Id == m.Id).Single();
                m.Options = machine.Options;
            }

            string path = HttpContext.Server.MapPath("~/PDF/quote " + quote.Id + ".pdf");
            string xmlpath = HttpContext.Server.MapPath("~/PDF/quote " + quote.Id + ".xml");

            PDF pdf = new PDF(xmlpath, quote);

            quote.FilePath = path;

            DataRow header = pdf.GetPDFHeader().Rows[0];
            DataRow footer = pdf.GetPDFFooter().Rows[0];
            DataRow machines = pdf.GetPDFRows().Rows[0];
            DataRow total = pdf.GetPDFTotal().Rows[0];

            FileStream fs = new FileStream(path, FileMode.Create);

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            document.AddAuthor(quote.SalesmanId);

            document.AddCreator("High Tech Tooling");

            document.AddKeywords("Machine Quote");

            document.AddTitle("Quote for " + quote.CustomerId);

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            cb.AddTemplate(PdfFooter(cb, footer), 30, 1);

            Image png = Image.GetInstance(HttpContext.Server.MapPath("~/PDF/HTTlogo.png"));
            png.ScaleAbsolute(275, 60);
            png.SetAbsolutePosition(40, 750);
            cb.AddImage(png);


            cb.BeginText();

            writeText(cb, header["invoiceType"].ToString(), 350, 820, f_cb, 14);
            writeText(cb, "Quote No", 350, 800, f_cb, 10);
            writeText(cb, header["invoiceId"].ToString(), 420, 800, f_cn, 10);
            writeText(cb, "Quote date", 350, 788, f_cb, 10);
            writeText(cb, header["invoiceDate"].ToString(), 420, 788, f_cn, 10);
            writeText(cb, "Terms", 350, 776, f_cb, 10);
            writeText(cb, header["payTerms"].ToString(), 420, 776, f_cn, 10);
            writeText(cb, "Customer", 350, 764, f_cb, 10);
            writeText(cb, header["customerId"].ToString(), 420, 764, f_cn, 10);

            int left_margin = 40;
            int top_margin = 720;
            int lastwriteposition = 100;


            writeText(cb, "Item Name", left_margin, top_margin, f_cb, 10);
            writeText(cb, "Description", left_margin + 175, top_margin, f_cb, 10);
            writeText(cb, "Price", left_margin + 400, top_margin, f_cb, 10);


            foreach (DataRow drItem in pdf.GetPDFRows().Rows)
            {
                DataRow SubRow = drItem.GetChildRows("Row_SubRow").Single();

                DataRow[] Options = SubRow.GetChildRows("SubRow_Options");



                if ((top_margin - 150 - (12 * Options.Count())) <= lastwriteposition)
                {
                    // We need to end the writing before we change the page
                    cb.EndText();
                    // Make the page break
                    document.NewPage();
                    // Start the writing again
                    cb.BeginText();

                    png.ScaleAbsolute(275, 60);
                    png.SetAbsolutePosition(40, 750);
                    cb.AddImage(png);

                    top_margin = 780;

                    writeText(cb, header["invoiceType"].ToString(), 350, 820, f_cb, 14);
                    writeText(cb, "Quote No", 350, 800, f_cb, 10);
                    writeText(cb, header["invoiceId"].ToString(), 420, 800, f_cn, 10);
                    writeText(cb, "Quote date", 350, 788, f_cb, 10);
                    writeText(cb, header["invoiceDate"].ToString(), 420, 788, f_cn, 10);
                    writeText(cb, "Terms", 350, 776, f_cb, 10);
                    writeText(cb, header["payTerms"].ToString(), 420, 776, f_cn, 10);
                    writeText(cb, "Customer", 350, 764, f_cb, 10);
                    writeText(cb, header["customerId"].ToString(), 420, 764, f_cn, 10);


                    left_margin = 40;
                    top_margin = 720;
                    lastwriteposition = 100;


                    writeText(cb, "Item Name", left_margin, top_margin, f_cb, 10);
                    writeText(cb, "Description", left_margin + 175, top_margin, f_cb, 10);
                    writeText(cb, "Price", left_margin + 400, top_margin, f_cb, 10);
                }
                top_margin -= 24;
                writeText(cb, drItem["machineName"].ToString(), left_margin, top_margin, f_cb, 10);
                writeText(cb, drItem["machinePrice"].ToString(), left_margin + 400, top_margin, f_cn, 10);

                // This is the line spacing, if you change the font size, you might want to change this as well.
                top_margin -= 12;

                cb.EndText();

                cb.SetLineWidth(0f);
                cb.MoveTo(40, top_margin);
                cb.LineTo(560, top_margin);
                cb.Stroke();

                cb.BeginText();

                top_margin -= 12;




                for (int i = 0; i < Options.Count(); i++)
                {
                    top_margin -= 12;

                    writeText(cb, Options[i]["optionGroup"].ToString(), left_margin, top_margin, f_cn, 10);
                    writeText(cb, Options[i]["optionDescription"].ToString(), left_margin + 175, top_margin, f_cn, 10);
                    writeText(cb, Options[i]["price"].ToString(), left_margin + 400, top_margin, f_cn, 10);

                }



            }

            top_margin -= 40;
            left_margin = 350;

            // First the headers
            writeText(cb, "SubTotal", left_margin, top_margin, f_cb, 10);
            writeText(cb, "Freight fee", left_margin, top_margin - 12, f_cb, 10);
            writeText(cb, "Tax", left_margin, top_margin - 24, f_cb, 10);
            writeText(cb, "Total", left_margin, top_margin - 48, f_cb, 10);
            // Move right to write out the values
            left_margin = 475;
            cb.SetFontAndSize(f_cn, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, total["subTotal"].ToString(), left_margin, top_margin, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, total["freight"].ToString(), left_margin, top_margin - 12, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, total["tax"].ToString(), left_margin, top_margin - 24, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, total["total"].ToString(), left_margin, top_margin - 48, 0);

            cb.EndText();
            document.Close();
            writer.Close();
            fs.Close();

            db.SaveChanges();

            return File(path, "application/pdf");
        }

        public ActionResult MailPDF(int Id)
        {
            Quote quote = db.Quotes.Find(Id);


            var fromAddress = new MailAddress("sussextoolmailer@gmail.com", "High Tech Tooling");

            const string fromPassword = "H@/L140pd";
            string subject = "High Tech Tooling Quote";
            string Attachment = quote.FilePath;
            string body = "Your quote from High Tech Tooling is Attached. <br><br> Please note, this is an automated email service, this inbox is not monitored. <br> Please contact your sales representive with inquiries. ";
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            MailMessage message = new MailMessage();
            Attachment pdf = new Attachment(Attachment);

            ContentDisposition disposition = pdf.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(Attachment);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(Attachment);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(Attachment);

            message.Attachments.Add(pdf);
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = body;
            message.From = fromAddress;
            message.To.Add(quote.CustomerEmail);

            smtp.Send(message);

            return RedirectToAction("Index");
        }
        public ActionResult exportCSV(int id)
        {
            Quote quote = db.Quotes.Include(x => x.Machines).Where(s => s.Id == id).Single();

            int TotalCost = 0;

            CSVexport cv = new CSVexport()
            {
                CustomerID = "Customer: " + quote.CustomerId,
                QuoteDate = quote.Date.ToShortDateString(),
                quoteID = "Quote " + quote.Id.ToString(),
                SalesmanID = "Salesman: " + quote.SalesmanId,
            };

            foreach (Machine m in quote.Machines)
            {
                TotalCost += m.Price;
                Machine x = db.Machines.Include(p => p.Options).Where(a => a.Id == m.Id).Single();
                m.Options = x.Options.Where(r => r.Selected == true).ToList();
                
                foreach(Option o in m.Options)
                {
                    TotalCost += o.Price;
                }
            }

            cv.TotalPrice = "Total: " + TotalCost.ToString("c");

            

            StreamWriter sw = new StreamWriter(HttpContext.Server.MapPath("~/PDF/quote " + quote.Id + ".csv"),false);
            CsvWriter writer = new CsvWriter(sw);

            writer.WriteRecord(cv);

            foreach (Machine m in quote.Machines)
            {
                writer.WriteField(m.MachineName);
                writer.WriteField(m.Price.ToString("c"));

                writer.NextRecord();

                foreach (Option o in m.Options)
                {
                    writer.WriteField(o.Group);
                    writer.WriteField(o.Description);
                    writer.WriteField(o.Price.ToString("c"));

                    writer.NextRecord();
                }
            }

            sw.Close();


            return File(HttpContext.Server.MapPath("~/PDF/quote " + quote.Id + ".csv"), "text/csv", "quote" + quote.Id + ".csv");
        }

        private void writeText(PdfContentByte cb, string Text, int X, int Y, BaseFont font, int Size)
        {
            cb.SetFontAndSize(font, Size);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Text, X, Y, 0);
        }

        private PdfTemplate PdfFooter(PdfContentByte cb, DataRow footer)
        {
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\calibrib.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            // Create the template and assign height
            PdfTemplate tmpFooter = cb.CreateTemplate(580, 70);
            // Move to the bottom left corner of the template
            tmpFooter.MoveTo(1, 1);
            // Place the footer content
            tmpFooter.Stroke();
            // Begin writing the footer
            tmpFooter.BeginText();
            // Set the font and size
            tmpFooter.SetFontAndSize(f_cn, 8);
            // Write out details from the payee table
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["supplier"].ToString(), 0, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["address2"].ToString(), 0, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["address3"].ToString(), 0, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["city"].ToString() + " " + footer["state"].ToString() + " " + footer["zip"].ToString(), 0, 29, 0);
            // Bold text for ther headers
            tmpFooter.SetFontAndSize(f_cb, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Phone", 215, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Mail", 215, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Web", 215, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Salesman", 215, 29, 0);
            // Regular text for infomation fields
            tmpFooter.SetFontAndSize(f_cn, 8);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["phone"].ToString(), 265, 53, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["mail"].ToString(), 265, 45, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["web"].ToString(), 265, 37, 0);
            tmpFooter.ShowTextAligned(PdfContentByte.ALIGN_LEFT, footer["salesman"].ToString(), 265, 29, 0);
            // End text
            tmpFooter.EndText();
            // Stamp a line above the page footer
            cb.SetLineWidth(0f);
            cb.MoveTo(30, 60);
            cb.LineTo(570, 60);
            cb.Stroke();
            // Return the footer template
            return tmpFooter;
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
