using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data.Repositpry;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _products;

        public HomeController(IProductRepository products)
        {

            _products = products;
        }
        
 
        public async Task<IActionResult> Products(int page = 0)
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.IsAdmin = User.IsInRole("Admin");
                var option = new CookieOptions();
                option.Expires = DateTime.Now.AddMinutes(10);
                Response.Cookies.Append("IsAdmin", "true", option);

            }



            var products = await _products.GetAllPoducts();
            if (page != 0)
            {
                products = await _products.GetPagedPoducts(page * 6, 6);
            }
            var model = new ProductsDisplayModel
            {
                Products = products
            };


            return View("Index", model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddProductDb(AddProductModel model)
        {
            var product = new Product
            {
                Description = model.Description,
                imageUrl = model.ImageUrl,
                Name = model.ProductName,
                Price = model.Price
            };
            _products.AddProduct(product).Wait();
            ViewBag.IsAdmin = User.IsInRole("admin");
            return View("AddProduct");
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var model = await _products.GetCart(User.Identity.Name);

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit([FromQuery]string name)
        {
            if (name == null) name = User.Identity.Name;
            var orders = _products.GetUserOrders(name);
            var model = new EditViewModel
            {
                Oreders = orders.ToList(),
                User = name
            };
            return View(model);
        }

        [HttpGet]
        public async Task AddToCart([FromQuery]string prodName)
        {
            var product = await _products.GetProduct(prodName);
            var order = new Order
            {
                Product = product,
                Status = Status.Active,
                User = User.Identity.Name
            };
             _products.AddOrder(order).Wait();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public Task RemoveProduct([FromQuery]string prodName)
        {
            return _products.RemoveProduct(prodName);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task AddDiscount([FromBody] AddDiscountModel model)
        {
            var prod = await _products.GetProduct(model.ProdName);            
            prod.Discount = Convert.ToInt32(model.Discount);
            await _products.UpdateProduct(prod);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task EditOrder([FromBody] UpdateOrderModel model)
        {
            var order = await _products.GetOrder(Convert.ToInt32(model.OrderId));
            order.Status = getStatus(model.newStatus);
            await _products.UpdateOrder(order);
        }

        public Status getStatus(string stat)
        {
            switch (stat)
            {
                case "Active":
                    {
                        return Status.Active;
                    }
                case "Delivered": { return Status.Delivered; }
                case "Canceled": { return Status.Canceled; }
                case "Declined": { return Status.Declined; }
                default: throw new InvalidOperationException();

            }
        }
    }
}
