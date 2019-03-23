using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext()
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Product> Products { get; set; }

               
    }
}
