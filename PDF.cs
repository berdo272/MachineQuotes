using MachineQuotes.Models.PDF_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;

namespace MachineQuotes.Models
{
    public class PDF
    {

        private DataSet pdfData;
        private string xml_file;

        public PDF(string XML_Filepath, Quote quote)
        {
            xmlPDF pdf = new xmlPDF();

            int SubTotal = 0;
            double tax = 0;
            xml_file = XML_Filepath;

            pdf.pdf_header.customerId = quote.CustomerId;
            pdf.pdf_header.invoiceId = quote.Id.ToString();
            pdf.pdf_footer.salesman = quote.SalesmanId;


            foreach(Machine machine in quote.Machines)
            {
                Row row = new Row();

                row.machineName = machine.MachineName;
                row.machinePrice = machine.Price.ToString("C");

                SubTotal += machine.Price;
                               
                List<Option> selected = machine.Options.Where(o => o.Selected == true).ToList();


                foreach (Option o in selected)
                {
                    subRow sub = new subRow();

                    sub.optionDescription = o.Description;
                    sub.optionGroup = o.Group;
                    sub.price = o.Price.ToString("c");

                    row.subrows.Add(sub);

                    SubTotal += o.Price;
                }

                pdf.pdf_machines.Add(row);
            }

            tax = SubTotal * .06;

            pdf.pdf_total.freight = "$0";
            pdf.pdf_total.subTotal = SubTotal.ToString("c");
            pdf.pdf_total.tax = tax.ToString("c");
            pdf.pdf_total.total = (SubTotal + tax).ToString("c");

            XmlSerializer xs = new XmlSerializer(typeof(xmlPDF));

            FileStream fs = new FileStream(XML_Filepath, FileMode.Create);

            StreamWriter sw = new StreamWriter(fs);

            xs.Serialize(sw, pdf);

            sw.Close();
            fs.Close();

            readXML();
        }
        private void readXML()
        {

                pdfData = new DataSet();
                pdfData.ReadXml(xml_file);
            
        }
        public DataTable GetPDFHeader()
        {

                

                return pdfData.Tables["pdf_header"];

        }

        public DataTable GetPDFRows()
        {

                return pdfData.Tables["row"];

        }

        public DataTable GetPDFTotal()
        {

                return pdfData.Tables["pdf_total"];

        }

        public DataTable GetPDFFooter()
        {

                return pdfData.Tables["pdf_footer"];
            
        }
    }
}