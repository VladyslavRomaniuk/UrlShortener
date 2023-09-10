using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UrlShortener.Controllers {
    public class UrlController : Controller {
        private readonly ApplicationDbContext _db;

        public UrlController(ApplicationDbContext db) {
            _db = db;
        }

        public IActionResult Table() {
            IEnumerable<Url> objUrlList = _db.Urls;

            return View(objUrlList);
        }

        // GET
        public IActionResult Create() {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Url obj) {
            if (_db.Urls.FirstOrDefault(item => item.FullUrl == obj.FullUrl) != null) {
                TempData["error"] = "This URL already exists!";
                return Create();
            }
            obj.CreatedDate = DateTime.Now;
            var user = User as ClaimsPrincipal;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null) { obj.CreatedBy = userIdClaim.Value; }
            
            Shortener shortener = new Shortener();
            while (_db.Urls.FirstOrDefault(item => item.ShortenedUrl == obj.ShortenedUrl) != null) {
                obj.ShortenedUrl = shortener.GenerateShortKey(obj.FullUrl);
            }

            _db.Urls.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Url shortened successfully!";

            return RedirectToAction("Table");
        }

        // GET
        public IActionResult Info(int id) {
            var obj = _db.Urls.Find(id);

            if (obj == null) {
                return NotFound();
            }

            return View(obj);
        }

        // GET
        public IActionResult Delete(int? id) {
            var obj = _db.Urls.Find(id);

            if (obj == null) {
                return NotFound();
            }

            return DeletePOST(obj);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(Url obj) {
            _db.Urls.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Url deleted successfully!";

            return RedirectToAction("Table");
        }
    }
}
