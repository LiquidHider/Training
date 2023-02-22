using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Training.Web.Data;
using Training.Web.Models;
using Training.Web.Services;
using static iTextSharp.text.pdf.AcroFields;

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

        [HttpGet]
        public async Task<IActionResult> Index(string? status)
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


            if (String.IsNullOrEmpty(status) || status == "-1")
            {
                    objGoodsTableModel = objGoodList.Select(p =>
                    new GoodsTableModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Status = p.Status,
                        AppraisedValue = p.AppraisedValue,
                        Category = p.Category,
                        CategoryId = p.CategoryId,
                        Commision = Math.Round((p.AppraisedValue * p.Category.Commision) / 100, 2)
                    }
                ).ToList();
            }
            else
            {
                objGoodsTableModel = objGoodsTableModel.Where(x => x.Status.ToString() == status).ToList();
            }

            SelectList StatusGoodsList = new SelectList(
                    new List<SelectListItem>
                    {
                        new SelectListItem { Text = $"Всі", Value = "-1"},
                        new SelectListItem { Text = $"{GoodsStatusString.STORING}", Value = GoodsStatus.Storing.ToString()},
                        new SelectListItem { Text = $"{GoodsStatusString.EXPIRED}", Value = GoodsStatus.Expired.ToString()},
                        new SelectListItem { Text = $"{GoodsStatusString.ONSALE}", Value = GoodsStatus.OnSale.ToString()},
                        new SelectListItem { Text = $"{GoodsStatusString.SOLD}", Value = GoodsStatus.Sold.ToString()},
                        new SelectListItem { Text = $"{GoodsStatusString.RETURNED}", Value =  GoodsStatus.Returned.ToString()},
                    }, "Value", "Text");

            var model = new GeneralModel { CategoryList = objCategoryList, GoodList = objGoodsTableModel , StatusGoodsList = StatusGoodsList };
            return View(model);
        }

        [HttpGet, ActionName("PutOnSale")]
        public async Task<IActionResult> PutUpForSale(int? id) 
        {
            var goods = _db.Goods.FirstOrDefaultAsync(x => x.Id == id).Result;
            _goodsService.PutUpOnSale(goods);
            _db.Update(goods);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", nameof(Good));
        }

        [HttpGet, ActionName("Return")]
        public async Task<IActionResult> Return(int? id)
        {
            var goods = _db.Goods.FirstOrDefaultAsync(x => x.Id == id).Result;
            _goodsService.Return(goods);
            _db.Update(goods);

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", nameof(Good));
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
