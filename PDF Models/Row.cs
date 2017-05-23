using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MachineQuotes.Models.PDF_Models
{
    public class Row
    {
        public Row()
        {
            subrows = new List<subRow>();
        }

        public string machineName;
        public string machinePrice;
        [XmlArray("SubRow")]
        [XmlArrayItem("Options")]
        public List<subRow> subrows;

    }
}