using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Divisima.WebUI.Areas.admin.Controllers
{
    [Area("admin"), Authorize]
    public class BrandController : Controller
    {
        IRepository<Brand> repoBrand;
        public BrandController(IRepository<Brand> _repoBrand)
        {
            repoBrand = _repoBrand;
        }

        [Route("/admin/marka")]
        public IActionResult Index()
        {
            return View(repoBrand.GetAll());
        }

        [Route("/admin/marka/yeni")]
        public IActionResult New()
        {
            return View();
        }

        [Route("/admin/marka/yeni"), HttpPost]
        public async Task<IActionResult> New(Brand model)
        {
            await repoBrand.Add(model);
            return Redirect("/admin/marka");
        }

        [Route("/admin/marka/duzenle")]
        public IActionResult Edit(int id)
        {
            return View(repoBrand.GetBy(x => x.ID == id));
        }

        [Route("/admin/marka/duzenle"), HttpPost]
        public async Task<IActionResult> Edit(Brand model)
        {
            await repoBrand.Update(model);
            return Redirect("/admin/marka");
        }

        [Route("/admin/marka/sil"), HttpPost]
        public async Task<string> Delete(int id)
        {
            try
            {
                Brand brand = repoBrand.GetBy(x => x.ID == id) ?? null;
                if (brand != null) await repoBrand.Delete(brand);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
