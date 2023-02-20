using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Web.Models
{

    [Table("Goods")]
    public class Good
    {
        [Key]
        public int Id { get; set; }


        [Required, Display(Name = "Назва"), MaxLength(50)]
        public string Name { get; set; }

        [Required, Display(Name = "Статус"), MaxLength(15)]
        public string Status { get; set; }

        [Display(Name = "Опис"), MaxLength(100)]
        public string Description { get; set; }

        [Display(Name = "Оціночна вартість")]
        public decimal AppraisedValue { get; set; }

        [ValidateNever]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        [Display(Name = "Категорія товару")]
        public Category Category { get; set; }

    }
}
