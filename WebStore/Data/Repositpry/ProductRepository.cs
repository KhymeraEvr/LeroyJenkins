using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.Data.Repositpry
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context; 

        public ProductRepository()
        {
            _context = new ProductDbContext();
        }

        public async Task<Product> AddProduct(Product newProd)
        {
            var result = await _context.Products.AddAsync(newProd);
            return result.Entity;
        }

        public Task<Product[]> GetAllPoducts()
        {
            return _context.Products.ToArrayAsync(); 
        }

        public Task<Product[]> GetPagedPoducts(int skip, int take)
        {
            return _context.Products.Skip(skip).Take(take).ToArrayAsync();
        }

        public Task<Product> GetProduct(int id)
        {
            return _context.Products.FirstOrDefaultAsync(prod => prod.Id == id);
        }
    }
}
