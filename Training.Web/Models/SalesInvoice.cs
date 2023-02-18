using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Web.Models
{
    [Table("SalesInvoices")]
    public class SalesInvoice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Сount { get; set; }
        [ValidateNever]
        public int SellId { get; set; }
        [ForeignKey("SellId")]
        [ValidateNever]
        public Sell Sell { get; set; }
    }
}
