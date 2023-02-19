using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Web.Data;
using Training.Web.Models;

namespace Training.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDBContext _db;

        public ClientController(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Client> objClientList = await _db.Clients.ToListAsync();
            return View(objClientList);
        }

        //GET
        [HttpGet, ActionName("Upsert")]
        public async Task<IActionResult> UpsertGetAsync(int? id)
        {
            Client client = new();

            if(id ==null || id == 0)
            {
                return View(client);
            }
            else
            {
                client = await _db.Clients.FirstOrDefaultAsync(u => u.Id == id);
                return View(client);
            }
        }

        [HttpPost, ActionName("Upsert")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertPostAsync(Client obj)
        {
            
            if(ModelState.IsValid)
            {
                if(obj.Id == 0)
                {
                    await _db.Clients.AddAsync(obj);
                }
                else
                {
                    _db.Clients.Update(obj);
                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var client = await _db.Clients.FirstOrDefaultAsync(u => u.Id == id);

            if(client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(int? id)
        {
           var client = await _db.Clients.FirstOrDefaultAsync(u => u.Id == id);

           if(client == null)
           {
               return NotFound();
           }
            _db.Clients.Remove(client);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
