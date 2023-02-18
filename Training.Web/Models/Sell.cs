using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Web.Models
{
    [Table("Sells")]
    public class Sell
    {
        [Key]
        public int Id { get; set; }
        [Required, Display(Name = "Ціна")]
        public decimal Price { get; set; }
        [ValidateNever]
        public int GoodId { get; set; }
        [ForeignKey("GoodId")]
        [ValidateNever]
        public Good Good { get; set; }
    }
}
