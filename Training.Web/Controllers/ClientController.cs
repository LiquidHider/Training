using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            IEnumerable<Client> objClientList = _db.Clients;
            return View(objClientList);
        }
        //GET
        public IActionResult Upsert(int? id)
        {
            Client client = new();
            if(id ==null || id == 0)
            {
                return View(client);
            }
            else
            {
                client = _db.Clients.FirstOrDefault(u => u.Id == id);
                return View(client);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Client obj)
        {
            if(ModelState.IsValid)
            {
                if(obj.Id == 0)
                {
                    _db.Clients.Add(obj);
                }
                else
                {
                    _db.Clients.Update(obj);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var client = _db.Clients.FirstOrDefault(u => u.Id == id);
            if(client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
           var client = _db.Clients.FirstOrDefault(u => u.Id == id);
           if(client == null)
           {
               return NotFound();
           }
            _db.Clients.Remove(client);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
