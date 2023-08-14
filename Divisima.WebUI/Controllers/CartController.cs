using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Divisima.WebUI.Models;
using Divisima.WebUI.Tools;
using Divisima.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Divisima.WebUI.Controllers
{
    public class CartController : Controller
    {
        IRepository<Product> repoProduct;
        IRepository<City> repoCity;
        IRepository<Order> repoOrder;
        IRepository<OrderDetail> repoOrderDetail;
        public CartController(IRepository<Product> _repoProduct, IRepository<City> _repoCity, IRepository<Order> _repoOrder, IRepository<OrderDetail> _repoOrderDetail)
        {
            repoProduct = _repoProduct;
            repoCity = _repoCity;
            repoOrder = _repoOrder;
            repoOrderDetail = _repoOrderDetail;
        }

        [Route("/sepetim")]
        public IActionResult Index()
        {
            if (Request.Cookies["MyCart"] != null)
            {
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                if (carts.Count() == 0) return Redirect("/");
                else
                {
                    CartVM cartVM = new CartVM
                    {
                        Carts = carts,
                        Products = repoProduct.GetAll().Include(x => x.ProductPictures).OrderBy(x => Guid.NewGuid()).Take(4)
                    };
                    return View(cartVM);
                }
            }
            else return Redirect("/");
        }

        [Route("/sepetim/sayiver")]
        public int GetCartCount()
        {
            if (Request.Cookies["MyCart"] != null)
            {
                return JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]).Sum(x => x.Quantity);
            }
            else return 0;
        }

        [Route("/sepetim/sil")]
        public string RemoveCart(int productid)
        {
            if (Request.Cookies["MyCart"] != null)
            {
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);
                carts.Remove(carts.FirstOrDefault(x => x.ID == productid));
                CookieOptions cookieOptions = new();
                cookieOptions.Expires = DateTime.Now.AddDays(3);
                Response.Cookies.Append("MyCart", JsonConvert.SerializeObject(carts), cookieOptions);
                return "OK";
            }
            else return "";
        }

        [Route("/sepetim/ekle")]
        public string AddCart(int productid, int quantity)
        {
            Product product = repoProduct.GetAll(x => x.ID == productid).Include(x => x.ProductPictures).FirstOrDefault() ?? null;
            if (product != null)//sepete ekleme işlemleri
            {
                Cart cart = new Cart
                {
                    ID = product.ID,
                    Name = product.Name,
                    Picture = product.ProductPictures.Any() ? product.ProductPictures.FirstOrDefault().Picture : "/img/urunHazirlaniyor.png",
                    Price = product.Price,
                    Quantity = quantity
                };
                List<Cart> carts = new List<Cart>();
                bool urunVarmi = false;
                if (Request.Cookies["MyCart"] != null)//daha önce sepete eklenmiş bir ürün varsa
                {
                    carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]);

                    foreach (Cart _cart in carts)
                    {
                        if (_cart.ID == productid)
                        {
                            urunVarmi = true;
                            _cart.Quantity += quantity;
                            if (product.Stock < _cart.Quantity) _cart.Quantity = product.Stock;
                            break;
                        }
                    }
                }
                if (urunVarmi == false) carts.Add(cart);
                CookieOptions cookieOptions = new();
                cookieOptions.Expires = DateTime.Now.AddDays(3);
                Response.Cookies.Append("MyCart", JsonConvert.SerializeObject(carts), cookieOptions);
                return product.Name;
            }
            else return "";
        }

        [Route("/sepetim/tamamla")]
        public IActionResult Complete()
        {
            //JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]) fiyatları güncellemekde fayda var
            OrderVM orderVM = new OrderVM
            {
                Carts = JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]),
                Cities = repoCity.GetAll().OrderBy(o => o.Name)
            };
            return View(orderVM);
        }

        [Route("/sepetim/tamamla"), HttpPost]
        public async Task<IActionResult> Complete(OrderVM model)
        {
            model.Order.RecDate = DateTime.Now;
            model.Order.IPNO = HttpContext.Connection.RemoteIpAddress.ToString();
            model.Order.OrderStatus = EOrderStatus.Hazırlanıyor;
            string orderNumber = repoOrder.GetAll().Any() ? repoOrder.GetAll().OrderByDescending(x => x.ID).FirstOrDefault().ID.ToString() : "1" + DateTime.Now.Millisecond.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Hour.ToString();
            if (orderNumber.Length > 10) orderNumber = orderNumber.Substring(0, 10);
            model.Order.OrderNumber = orderNumber;
            await repoOrder.Add(model.Order);
            foreach (Cart cart in JsonConvert.DeserializeObject<List<Cart>>(Request.Cookies["MyCart"]))
            {
                OrderDetail orderDetail = new OrderDetail { OrderID = model.Order.ID, Name = cart.Name, Picture = cart.Picture, Price = cart.Price, ProductID = cart.ID, Quantity = cart.Quantity };
                await repoOrderDetail.Add(orderDetail);
            }
            Response.Cookies.Delete("MyCart");
            GeneralTool.MailGonder(model.Order.Mail, "Siparişini Alındı", "Sayın " + model.Order.Name + " " + model.Order.Surname + " siparişiniz başarıyla alındı...");
            TempData["Siparis"] = model.Order.Name + " " + model.Order.Surname + " siparişiniz başarıyla alındı...";
            return Redirect("/");
        }
    }
}
