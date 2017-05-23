using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineQuotes.Models.PDF_Models
{
    public class xmlPDF
    {
        public xmlPDF()
        {
            pdf_footer = new Footer();
            pdf_header = new Header();
            pdf_machines = new List<Row>();
            pdf_total = new Total();

        }

        public Header pdf_header;
        public List<Row> pdf_machines;
        public Footer pdf_footer;
        public Total pdf_total;

    }
}