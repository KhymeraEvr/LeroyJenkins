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

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProduct(Product newProd)
        {                       
            var result = await _context.Products.AddAsync(newProd);
            await _context.SaveChangesAsync();
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

        public Task<Product> GetProduct(string name)
        {
            return _context.Products.FirstOrDefaultAsync(prod => prod.Name == name);
        }

        public async Task UpdateOrder(Order order)
        {
             _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product prod)
        {
            _context.Products.Update(prod);
            await _context.SaveChangesAsync();
        }

        public Task<Order> GetOrder(int id)
        {
            return _context.Orders.Include(or => or.Product).FirstOrDefaultAsync(prod => prod.Id == id);
            
        }

        public async Task<CartViewModel> GetCart(string user)
        {
            var orders = _context.Orders.Where(o => o.User == user).Include(prod => prod.Product);
            var result = new CartViewModel
            {
                Orders = orders.ToArray(),
                TotalSum = orders.Sum(c => c.Product.Price - c.Product.Price * c.Product.Discount/100),

            };
            return result;
        }

        public async Task<Order> AddOrder(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task RemoveOrder(int id)
        {
            var ord = _context.Orders.FirstOrDefault(pr => pr.Id == id);
            _context.Orders.Remove(ord);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProduct(string name)
        {
            var prod =  _context.Products.FirstOrDefault(p => p.Name == name);
            var orders = _context.Orders.Where(or => or.Product.Id == prod.Id).Select( or => or.Id).ToList();
            foreach (var order in orders)
            {
                await RemoveOrder(order);
            }
             _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Order> GetUserOrders(string name)
        {
            var res = _context.Orders.Where(or => or.User == name).Include( or => or.Product).ToList();
            return res;
        }

    }
}

