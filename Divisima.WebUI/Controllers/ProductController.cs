using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Divisima.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Divisima.WebUI.Controllers
{
    public class ProductController : Controller
    {
        IRepository<Product> repoProduct;
        public ProductController(IRepository<Product> _repoProduct)
        {
            repoProduct = _repoProduct;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/detay/{name}-{id}")]
        public IActionResult Detail(string name, int id)
        {
			Product product = repoProduct.GetAll().Include(x => x.ProductPictures).FirstOrDefault(x => x.ID == id);
			ProductVM productVM = new ProductVM
			{
				Product = product,
				RelatedProducts = repoProduct.GetAll(x => x.BrandID == product.BrandID && x.ID != id).Include(i=>i.ProductPictures).OrderBy(x => Guid.NewGuid()).Take(4)
			};
			return View(productVM);
		}
    }
}