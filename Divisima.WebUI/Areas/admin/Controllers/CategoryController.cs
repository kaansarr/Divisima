using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Divisima.WebUI.Areas.admin.Controllers
{
    [Area("admin"), Authorize]
    public class CategoryController : Controller
    {
        IRepository<Category> repoCategory;
        public CategoryController(IRepository<Category> _repoCategory)
        {
            repoCategory = _repoCategory;
        }

        [Route("/admin/kategori")]
        public IActionResult Index()
        {
            return View(repoCategory.GetAll().Include(x=>x.ParentCategory));
        }

        [Route("/admin/kategori/yeni")]
        public IActionResult New()
        {
            ViewBag.ParentCategories=repoCategory.GetAll().Select(x=>new SelectListItem { Text=x.Name,Value=x.ID.ToString()});

            //<option value='11'>Erkek</option>
            //<option value='12'>Kadın</option>
            //<option value='13'>Çocuk</option>
            return View();
        }

        [Route("/admin/kategori/yeni"), HttpPost]
        public async Task<IActionResult> New(Category model)
        {
            await repoCategory.Add(model);
            return Redirect("/admin/kategori");
        }

        [Route("/admin/kategori/duzenle")]
        public IActionResult Edit(int id)
        {
            ViewBag.ParentCategories = repoCategory.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.ID.ToString() });
            return View(repoCategory.GetBy(x => x.ID == id));
        }

        [Route("/admin/kategori/duzenle"), HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            await repoCategory.Update(model);
            return Redirect("/admin/kategori");
        }

        [Route("/admin/kategori/sil"), HttpPost]
        public async Task<string> Delete(int id)
        {
            try
            {
                Category category = repoCategory.GetBy(x => x.ID == id) ?? null;
                if (category != null) await repoCategory.Delete(category);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
