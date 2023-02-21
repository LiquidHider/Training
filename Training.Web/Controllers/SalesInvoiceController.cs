using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Training.Web.Data;
using Training.Web.Models;
using Training.Web.Services;

namespace Training.Web.Controllers
{
    public class SalesInvoiceController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IGoodsService _goodsService;

        public SalesInvoiceController(ApplicationDBContext db, IGoodsService goodsService)
        {
            _db = db;
            _goodsService = goodsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SalesInvoice> objSalesInvoiceList = await _db.SalesInvoices.
                                                                   Include(x=>x.Sell).
                                                                   Include(x => x.Sell.Good).
                                                                   ToListAsync();
            return View(objSalesInvoiceList);
        }

        [HttpGet, ActionName("Add")]
        public async Task<IActionResult> AddGetAsync(int? id)
        {
            SalesInvoice invoice = new();

            Sell sell = new();
            var goods = await _db.Goods.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            sell.Price = (goods.AppraisedValue + Math.Round((goods.AppraisedValue * goods.Category.Commision) / 100, 2));
            sell.Good = goods;
            invoice.Sell = sell;

            return View(invoice);
        }

        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> AddPostAsync(SalesInvoice invoice)
        {
            if (ModelState.IsValid)
            {
                var sellJson = Request.Form["sellClass"].ToString();
                invoice.Sell = JsonConvert.DeserializeObject<Sell>(sellJson);
                var goods = invoice.Sell.Good;
                _goodsService.Sell(goods);
                _db.Update(goods);
                _db.Add(invoice.Sell);
                _db.Add(invoice);
                await _db.SaveChangesAsync();

                TempData["success"] = "Видаткову накладну успішно сформовано!";
                return RedirectToAction(nameof(Index), nameof(SalesInvoice));
            }
            return View();
        }
    }
}
