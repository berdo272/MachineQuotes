using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MachineQuotes.Models
{
    public class Machine
    {
        public int Id { get; set; }

        [DisplayName("Machine Name")]
        public string MachineName { get; set; }
        public int Price { get; set; }
        public List<Option> Options { get; set; }

    }
}