using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Divisima.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Divisima.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Slide> repoSlide;
        IRepository<Product> repoProduct;
        public HomeController(IRepository<Slide> _repoSlide, IRepository<Product> _repoProduct)
        {
            repoSlide = _repoSlide;
            repoProduct= _repoProduct;
        }
        public IActionResult Index()
        {

            IndexVM indexVM = new IndexVM
            {
                Slides = repoSlide.GetAll().OrderBy(o => o.DisplayIndex),
                LatestProducts = repoProduct.GetAll().Include(x => x.ProductPictures).OrderByDescending(o => o.ID).Take(10),
                BestSalesProducts = repoProduct.GetAll().Include(x => x.ProductPictures).OrderBy(o => Guid.NewGuid()).Take(8),
            };
            return View(indexVM);
        }
    }
}
