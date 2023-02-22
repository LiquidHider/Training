using Training.Web.Models;

namespace Training.Web.Services
{
    public class GoodsTableModelService
    {
        public List<GoodsTableModel> ToGoodsTableModelList(IEnumerable<Good> goods, Func<GoodsTableModel,bool> action) 
        {
            return goods.Select(p =>
                new GoodsTableModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    AppraisedValue = p.AppraisedValue,
                    Category = p.Category,
                    CategoryId = p.CategoryId,
                    Commision = Math.Round((p.AppraisedValue * p.Category.Commision) / 100, 2)
                }
            ).Where(action).ToList();
        }
    }
}
