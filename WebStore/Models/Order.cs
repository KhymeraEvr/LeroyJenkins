using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public Status Status { get; set; }

        public string User { get; set; }

        public Product Product { get; set; }
    }

    public enum Status
    {        
        Active,
        Delivered,
        Canceled,
        Declined
    }
}


