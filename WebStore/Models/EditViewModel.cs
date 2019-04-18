using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class EditViewModel
    {
        public ICollection<Order> Oreders { get; set; }
        public string User { get; set; }
    }
}
