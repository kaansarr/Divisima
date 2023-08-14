using Divisima.BL.Repositories;
using Divisima.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Divisima.WebUI.Areas.admin.Controllers
{
    [Area("admin"),Authorize]
    public class SlideController : Controller
    {
        IRepository<Slide> repoSlide;
        public SlideController(IRepository<Slide> _repoSlide)
        {
            repoSlide = _repoSlide;
        }

        [Route("/admin/slayt")]
        public IActionResult Index()
        {
            return View(repoSlide.GetAll());
        }

        [Route("/admin/slayt/yeni")]
        public IActionResult New()
        {
            return View();
        }

        [Route("/admin/slayt/yeni"),HttpPost]
        public async Task<IActionResult> New(Slide model)
        {
            if (Request.Form.Files.Any())
            {
                if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","img","slide"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "slide"));
                string dosyaAdi = (DateTime.Now.Minute+DateTime.Now.Millisecond)+Request.Form.Files["Picture"].FileName;
                using (FileStream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "slide", dosyaAdi), FileMode.Create))//resmin fiziksel kopyası için
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/img/slide/" + dosyaAdi;//db deki veri için
            }
            await repoSlide.Add(model);
            return Redirect("/admin/slayt");
        }

        [Route("/admin/slayt/duzenle")]
        public IActionResult Edit(int id)
        {
            return View(repoSlide.GetBy(x=>x.ID==id));
        }

        [Route("/admin/slayt/duzenle"), HttpPost]
        public async Task<IActionResult> Edit(Slide model)
        {
            if (Request.Form.Files.Any())
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "slide"))) Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "slide"));
                string dosyaAdi = (DateTime.Now.Minute + DateTime.Now.Millisecond) + Request.Form.Files["Picture"].FileName;
                using (FileStream stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "slide", dosyaAdi), FileMode.Create))//resmin fiziksel kopyası için
                {
                    await Request.Form.Files["Picture"].CopyToAsync(stream);
                }
                model.Picture = "/img/slide/" + dosyaAdi;//db deki veri için
            }
           await repoSlide.Update(model);
            return Redirect("/admin/slayt");
        }

        [Route("/admin/slayt/sil"),HttpPost]
        public async Task<string> Delete(int id)
        {
            try
            {
                Slide slide = repoSlide.GetBy(x => x.ID == id) ?? null;
                if (slide != null) await repoSlide.Delete(slide);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
