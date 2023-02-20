namespace Training.Web.Models
{
    public class GeneralModel
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public IEnumerable<GoodsTableModel> GoodList { get; set; }
    }
}
