﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class ProductsDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
