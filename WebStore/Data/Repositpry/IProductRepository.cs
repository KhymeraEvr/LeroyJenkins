using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Data.Repositpry
{
    public interface IProductRepository
    {
        Task<Product[]> GetPagedPoducts(int skip, int take);
        Task<Product[]> GetAllPoducts();
        Task<Product> GetProduct(string name);
        Task<Order> GetOrder(int id);
        Task<Product> AddProduct(Product newProd);
        Task<CartViewModel> GetCart(string user);
        Task<Order> AddOrder(Order order);
        Task RemoveProduct(string name);
        Task RemoveOrder(int id);
        IEnumerable<Order> GetUserOrders(string name);
        Task UpdateOrder(Order order);
        Task UpdateProduct(Product prod);
        ProductDbContext Context { get; }
    }
}
