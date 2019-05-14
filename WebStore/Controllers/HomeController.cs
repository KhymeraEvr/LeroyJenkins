using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebStore.Data.Repositpry;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _products;
        private IHubContext<GausHub> _hub;
        public HomeController(IProductRepository products, IHubContext<GausHub> hub)
        {
            _hub = hub;
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

        //[HttpPost("Home/startCalc/{size}")]
        public IActionResult RunGaus([FromForm]Calcmodel model )
        {
            var server1load = _products.Context.Gauses.Where(g => g.server == 1 && g.isActive)?.Sum(f => f.size) ?? 0;
            var server2load = _products.Context.Gauses.Where(g => g.server == 2 && g.isActive)?.Sum(f => f.size) ?? 0;

            if (server1load > server2load)
            {
                return Redirect("http://funnypicture.org/wallpaper/2015/04/funny-black-cat-pictures-16-cool-wallpaper.jpg");

            }
            return RedirectToAction("Calculate", new { size = model.size, name = User.Identity.Name });
        }

        [HttpGet("Home/getHistory")]
        public IActionResult GetHistory()
        {
            var history = _products.Context.Gauses.ToList();
            return Ok(history);
        }

        public async Task<IActionResult> Calculate(int Size, string Name)
        {
            var args = new Calcmodel
            {
                server = 1,
                name = Name,
                size = Size,
                isActive = true
            };

            var model = await _products.Context.Gauses.AddAsync(args);
            var calcMod = model.Entity;
             await _products.Context.SaveChangesAsync();

            await Task.Run(() => Main(args.size, args.name)).ConfigureAwait(false);

            calcMod.isActive = false;
            _products.Context.Update(calcMod);
            await _products.Context.SaveChangesAsync();
            return View("CalcPage");

        }

        public IActionResult CalcPage()
        {
            return View();
        }

        public async Task Send(string name, string message)
        {

            await _hub.Clients.All.SendAsync("Send", name, message);
        }


        public async Task Main(int n, string name)
        {
            double[,] mat = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        mat[i, j] = i + 1;
                    }
                    else
                    {
                        mat[i, j] = i + 2;
                    }
                }
            }
            double[] free = new double[n];
            for (int i = 1; i <= n; i++)
            {
                free[i - 1] = 7;
            }
            await Gauss(name, mat, free, n);
        }

        public async Task<double[]> Gauss(string name, double[,] mat, double[] free, int n)
        {
            float progressStep = 100 / (float)n;
            float progres = 0;
            double[] x = new double[n];
            for (int i = 0; i < n; i++)
            {
                double val = mat[i, i];
                for (int j = i; j < n; j++)
                {
                    double koef = mat[j, i] / val;
                    for (int q = i; q < n; q++)
                    {
                        if (j != i)
                        {
                            mat[j, q] -= mat[i, q] * koef;
                        }
                    }
                }
                progres += progressStep;
                await Send(name, progres.ToString());
                await Task.Delay(100);
            }

            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = free[i];
                for (int j = 0; j < i; j++)
                {
                    free[j] -= x[i] * mat[j, i];
                }
            }
            await Send(name, "100");
            return x;
        }
    }
}
