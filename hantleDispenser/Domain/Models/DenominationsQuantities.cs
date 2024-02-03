using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hantleDispenser.Domain.Models
{
    public class DenominationsQuantities
    {
        public string COM { get; set; } = string.Empty;
        public List<string> Denomination { get; set; } = new();
        public List<int> Quantity { get; set; } = new();
    }
}
