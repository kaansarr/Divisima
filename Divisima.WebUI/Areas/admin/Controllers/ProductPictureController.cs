using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Divisima.WebUI.Areas.admin.Controllers
{
    [Area("admin"),Authorize]
    public class ProductPictureController : Controller
    {
        IRepository<ProductPicture> repoProductPicture;
        public ProductPictureController(IRepository<ProductPicture> _repoProductPicture)
        {
            repoProductPicture = _repoProductPicture;
        }

        [Route("/admin/resim")]
        public IActionResult Index(int productid)
        {
            ViewBag.ProductID = productid;
            return View(repoProductPicture.GetAll(x=>x.ProductID== productid));
        }

        [Route("/admin/resim/yeni")]
        public IActionResult New(int productid)
        {
            ViewBag.ProductID = productid;
            return View();
        }

        [Route("/admin/resim/yeni"),HttpPost]
        public async Task<IActionResult> New(ProductPicture model)
        {
            if (Request.Form.Files.Any())
            {
                if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","img","productPicture"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productPicture"));
                string dosyaAdi = (DateTime.Now.Minute+DateTime.Now.Millisecond)+Request.Form.Files["Picture"].FileName;
                using (FileStream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productPicture", dosyaAdi), FileMode.Create))//resmin fiziksel kopyası için
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/img/productPicture/" + dosyaAdi;//db deki veri için
            }
            await repoProductPicture.Add(model);
            return RedirectToAction("Index", "ProductPicture", new { productid= model.ProductID});
        }

        [Route("/admin/resim/duzenle")]
        public IActionResult Edit(int id)
        {
            return View(repoProductPicture.GetBy(x=>x.ID==id));
        }

        [Route("/admin/resim/duzenle"), HttpPost]
        public async Task<IActionResult> Edit(ProductPicture model)
        {
            if (Request.Form.Files.Any())
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productPicture"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productPicture"));
                string dosyaAdi = (DateTime.Now.Minute + DateTime.Now.Millisecond) + Request.Form.Files["Picture"].FileName;
                using (FileStream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "productPicture", dosyaAdi), FileMode.Create))//resmin fiziksel kopyası için
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/img/productPicture/" + dosyaAdi;//db deki veri için
            }
           await repoProductPicture.Update(model);
            return RedirectToAction("Index", "ProductPicture", new { productid = model.ProductID });
        }

        [Route("/admin/resim/sil"),HttpPost]
        public async Task<string> Delete(int id)
        {
            try
            {
                ProductPicture productPicture = repoProductPicture.GetBy(x => x.ID == id) ?? null;
                if (productPicture != null) await repoProductPicture.Delete(productPicture);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
