using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Training.Web.Data;
using Training.Web.Models;

namespace Training.Web.Controllers
{
    public class SalesInvoiceController : Controller
    {
        private readonly ApplicationDBContext _db;

        public SalesInvoiceController(ApplicationDBContext db)
        {
            _db = db;
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
        public async Task<IActionResult> AddGetAsync()
        {
            SalesInvoice invoice = new();
            //RegisteredInvoice invoice = new();
            //Good goods = JsonConvert.DeserializeObject<Good>(TempData["currentGoods"] as string);
            //invoice.Good = goods;
            //invoice.GoodId = goods.Id;
            //ViewBag.Clients = await _db.Clients.ToListAsync();
            return View();

        }

        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> AddPostAsync(RegisteredInvoice invoice)
        {
            if (ModelState.IsValid)
            {
                //invoice.Client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == invoice.ClientId);
                //var goodsJson = Request.Form["goodsClass"].ToString();
                //invoice.Good = JsonConvert.DeserializeObject<Good>(goodsJson);
                //_db.Goods.Add(invoice.Good);
                //_db.RegisteredInvoices.Add(invoice);
                //await _db.SaveChangesAsync();
                //TempData["success"] = "Товар та Імену накладну успішно сформовано!";
                //return RedirectToAction(nameof(Index), nameof(RegisteredInvoice));
            }
            return View();
        }
    }
}
