using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Divisima.WebUI.Areas.admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Divisima.WebUI.Areas.admin.Controllers
{
    [Area("admin"), Authorize]
    public class ProductController : Controller
    {
        IRepository<Product> repoProduct;
        IRepository<Brand> repoBrand;
        IRepository<Category> repoCategory;
        IRepository<ProductCategory> repoProductCategory;
        public ProductController(IRepository<Product> _repoProduct, IRepository<Brand> _repoBrand, IRepository<Category> _repoCategory, IRepository<ProductCategory> _repoProductCategory)
        {
            repoProduct = _repoProduct;
            repoBrand = _repoBrand;
            repoCategory = _repoCategory;
            repoProductCategory = _repoProductCategory;
        }

        [Route("/admin/urun")]
        public IActionResult Index()
        {
            return View(repoProduct.GetAll().Include(i => i.Brand).Include(i => i.ProductCategories).ThenInclude(t => t.Category));//eager loading
        }

        [Route("/admin/urun/yeni")]
        public IActionResult New()
        {
            ProductVM productVM = new ProductVM
            {
                Brands = repoBrand.GetAll().OrderBy(x => x.Name),
                Categories = repoCategory.GetAll().OrderBy(x => x.Name).ToList()
            };
            //ViewBag.Brands = repoBrand.GetAll().OrderBy(x => x.Name).Select(x=>new SelectListItem {Text=x.Name,Value=x.ID.ToString()});
            return View(productVM);
        }

        [Route("/admin/urun/yeni"), HttpPost]
        public async Task<IActionResult> New(ProductVM model)
        {
            await repoProduct.Add(model.Product);
            if (model.CategoriyIDs.Length > 0)
            {
                for (int i = 0; i < model.CategoriyIDs.Length; i++)
                {
                    await repoProductCategory.Add(new ProductCategory { ProductID = model.Product.ID, CategoryID = model.CategoriyIDs[i] });
                }
            }
            return Redirect("/admin/urun");
        }

        [Route("/admin/urun/duzenle")]
        public IActionResult Edit(int id)
        {
            ProductVM productVM = new ProductVM
            {
                Brands = repoBrand.GetAll().OrderBy(x => x.Name),
                Categories = repoCategory.GetAll().OrderBy(x => x.Name).ToList(),
                Product = repoProduct.GetAll().Include(x => x.ProductCategories).FirstOrDefault(x => x.ID == id)
            };
            return View(productVM);
        }

        [Route("/admin/urun/duzenle"), HttpPost]
        public async Task<IActionResult> Edit(ProductVM model)
        {
            await repoProduct.Update(model.Product);
            await repoProductCategory.DeleteRange(repoProductCategory.GetAll(x => x.ProductID == model.Product.ID));
            if (model.CategoriyIDs.Length > 0)
            {
                for (int i = 0; i < model.CategoriyIDs.Length; i++)
                {
                    await repoProductCategory.Add(new ProductCategory { ProductID = model.Product.ID, CategoryID = model.CategoriyIDs[i] });
                }
            }
            return Redirect("/admin/urun");
        }

        [Route("/admin/urun/sil"), HttpPost]
        public async Task<string> Delete(int id)
        {
            try
            {
                Product product = repoProduct.GetBy(x => x.ID == id) ?? null;
                if (product != null) await repoProduct.Delete(product);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
