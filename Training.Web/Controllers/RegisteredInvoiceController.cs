using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Training.Web.Data;
using Training.Web.Models;

namespace Training.Web.Controllers
{
    public class RegisteredInvoiceController : Controller
    {
        private readonly ApplicationDBContext _db;

        public RegisteredInvoiceController(ApplicationDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? Id)
        {
            var data = _db.RegisteredInvoices
                .Include(x => x.Good)
                .Include(x => x.Client)
                .ToList().Where(x => Id == null ||  x.Good.Id == Id);

            return View(data);
        }


        [HttpGet, ActionName("Add")]
        public async Task<IActionResult> AddGetAsync()
        {
            RegisteredInvoice invoice = new();
            Good goods = JsonConvert.DeserializeObject<Good>(TempData["currentGoods"] as string);
            invoice.Good = goods;
            invoice.GoodId = goods.Id;
            ViewBag.Clients = await _db.Clients.ToListAsync();
            return View(invoice);

        }

        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> AddPostAsync(RegisteredInvoice invoice) 
        {
            if (ModelState.IsValid) 
            {
                invoice.Client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == invoice.ClientId);
                var goodsJson = Request.Form["goodsClass"].ToString();
                invoice.Good = JsonConvert.DeserializeObject<Good>(goodsJson);
                _db.Goods.Add(invoice.Good);
                _db.RegisteredInvoices.Add(invoice);
                await _db.SaveChangesAsync();
                TempData["success"] = "Товар та Імену накладну успішно сформовано!";
                return RedirectToAction(nameof(Index), nameof(RegisteredInvoice));
            }
            return View();
        }
    }
}
