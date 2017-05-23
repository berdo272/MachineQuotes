using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineQuotes.Models.PDF_Models
{
    public class Header
    {
        public string invoiceId;
        public string invoiceType = "Quotation";
        public string invoiceDate = DateTime.Today.Date.ToShortDateString();
        public string customerId;
        public string payTerms = "Quotation Valid for 30 Days";
    }
}