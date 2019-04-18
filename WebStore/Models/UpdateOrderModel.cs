using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class UpdateOrderModel
    {
        public int OrderId { get; set; }
        public string newStatus { get; set; }
    }
}
