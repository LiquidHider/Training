using Microsoft.EntityFrameworkCore;
using Training.Web.Models;

namespace Training.Web.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base (options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<RegisteredInvoice> RegisteredInvoices { get; set; }
        public DbSet<Sell> Sells { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
    }

}
