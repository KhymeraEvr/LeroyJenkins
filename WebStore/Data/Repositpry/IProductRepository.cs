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
        Task<Product> GetProduct(int id);
        Task<Product> AddProduct(Product newProd);
    }
}
