using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Web.Data;
using Training.Web.Models;

namespace Training.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;

        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }

        //GET
        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
            Category category = new();

            if (id == null || id == 0)
            {
                return View(category);
            }
            else
            {
                category = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);
                return View(category);
            }
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(Category obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    await _db.Categories.AddAsync(obj);
                }
                else
                {
                    _db.Categories.Update(obj);
                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), nameof(Good));
            }

            return View(obj);
        }

        //GET
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteGetAsync(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
         
            var category = await _db.Categories.FirstOrDefaultAsync(u => u.Id == id);
            bool isGoodsWithSelectedCategoryExists = _db.Goods.Any(i => i.CategoryId == id);
            if(isGoodsWithSelectedCategoryExists)
            {
                TempData["error"] = "Не можна видалити Категорію , бо існують товари з заданою категорією";
                return RedirectToAction(nameof(Index), nameof(Good));
            }

            if (category == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), nameof(Good));
        }
    }
}
