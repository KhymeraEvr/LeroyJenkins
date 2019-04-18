using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Models
{
    public class ProductDbContext : DbContext
    {

        public ProductDbContext( DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();

        }
        private static readonly object prodLock = new object();


        public virtual DbSet<Product> Products
        {
            get;

            set;
        }
        public virtual DbSet<Order> Orders { get; set; }

    }
}
