using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class CartViewModel
    {
        public ICollection<Order> Orders { get; set; }
        public double TotalSum { get; set; }
    }
}
