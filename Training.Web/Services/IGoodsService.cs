using Training.Web.Models;

namespace Training.Web.Services
{
    public interface IGoodsService
    {
        void CheckStorageExpirationDate(RegisteredInvoice invoice);
        void Sell(Good goods);
        void PutUpOnSale(Good goods);
        void Return(Good goods);
    }
}
