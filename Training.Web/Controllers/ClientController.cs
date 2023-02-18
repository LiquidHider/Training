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
    }
}
