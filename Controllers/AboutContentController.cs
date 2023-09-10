using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Controllers {
    public class AboutContentController : Controller {
        private readonly ApplicationDbContext _db;

        public AboutContentController(ApplicationDbContext db) {
            _db = db;
        }

        public IActionResult Index() {
            var obj = _db.AboutContentModels.FirstOrDefault();

            if (obj == null) {
                obj = new AboutContentModel() {
                    Content = "This is the default content!"
                };
                _db.AboutContentModels.Add(obj);
                _db.SaveChanges();
            }

            return View(obj);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AboutContentModel model) {
            if (ModelState.IsValid) {
                _db.AboutContentModels.Update(model);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
