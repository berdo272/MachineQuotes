using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MachineQuotes.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        [DisplayName("Standard Option")]
        public bool IsStandard { get; set; }
        public bool Selected { get; set; }
    }
}