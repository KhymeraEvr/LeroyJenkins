using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public string UserName { get; set; }

        public bool IsAdmin { get; set; }
    }
}
