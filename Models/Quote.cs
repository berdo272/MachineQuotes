using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MachineQuotes.Models
{
    public class Quote
    {
        public int Id { get; set; }
        [DisplayName("Salesman Id")]
        public string SalesmanId { get; set; }
        [DisplayName("Customer Id")]
        public string CustomerId { get; set; }
        public DateTime Date { get; set; }
        public List<Machine> Machines { get; set; }
        public string Note { get; set; }

    }
}