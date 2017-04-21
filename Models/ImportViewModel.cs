using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineQuotes.Models
{
    public class ImportViewModel
    {
        public string Group { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsStandard { get; set; }
    }
}