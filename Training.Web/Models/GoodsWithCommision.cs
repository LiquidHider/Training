using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Training.Web.Models
{
    public class GoodsWithCommision
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal AppraisedValue { get; set; }

        [ValidateNever]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
        public decimal Commision { get; set; } 

    }
}
