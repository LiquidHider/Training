using Microsoft.AspNetCore.Http;

namespace Training.Web
{
    public class GoodsStatusString
    {
        public const string STORING = "На зберіганні";
        public const string EXPIRED = "Просрочено";
        public const string ONSALE = "На продажі";
        public const string SOLD = "Продано";
        public const string RETURNED = "Повернено";
    }
}
