using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Linq;
using Training.Web.Data;
using Training.Web.Models;
using Training.Web.Services;

namespace Training.Web.Controllers
{
    public class GoodController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IGoodsService _goodsService;
        public GoodController(ApplicationDBContext db, IGoodsService goodsService)
        {
            _db = db;
            _goodsService = goodsService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = await _db.Categories.ToListAsync();
            IEnumerable<RegisteredInvoice> registeredInvoices = await _db.RegisteredInvoices.Include(x => x.Good).ToListAsync();
            IEnumerable<Good> objGoodList = registeredInvoices.Select(x => x.Good).ToList();
            foreach (var item in registeredInvoices) 
            {
                _goodsService.CheckStorageExpirationDate(item);
            }
            
            var objGoodsTableModel = objGoodList.Select(p => 
                new GoodsTableModel {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    AppraisedValue = p.AppraisedValue,
                    Category = p.Category,
                    CategoryId = p.CategoryId,
                    Commision = Math.Round((p.AppraisedValue * p.Category.Commision) / 100, 2)}
            ).ToList();


            var model = new GeneralModel { CategoryList = objCategoryList, GoodList = objGoodsTableModel };
            return View(model);
        }

        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
           
            Good good = new();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            if (id == null || id == 0)
            {
                return View(good);
            }
            else
            {
                good = await _db.Goods.FirstOrDefaultAsync(u => u.Id == id);
                return View(good);
            } 
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(Good obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id != 0)
                {
                    _db.Goods.Update(obj);
                    await _db.SaveChangesAsync();
                }
                var serializedObj = JsonConvert.SerializeObject(obj);
                TempData["currentGoods"] = serializedObj;
                return RedirectToAction("Add", nameof(RegisteredInvoice));
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

            var good = await _db.Goods.Include(p => p.Category).FirstOrDefaultAsync(u => u.Id == id);

            if (good == null)
            {
                return NotFound();
            }

            return View(good);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
            var good = await _db.Goods.FirstOrDefaultAsync(u => u.Id == id);
            
            if (good == null)
            {
                return NotFound();
            }
            _db.Goods.Remove(good);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
