using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Calcmodel
    {
        [Key]
        public int id { get; set; }
        public int size { get; set; }
        public string name { get; set; }
        public int server { get; set; }
        public bool isActive { get; set; }
    }
}
