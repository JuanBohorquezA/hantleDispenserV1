using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hantleDispenser.Domain.Models
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string pwd { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;

    }
}
