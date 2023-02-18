using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Web.Models
{
    [Table("RegisteredInvoices")]
    public class RegisteredInvoice
    {
        [Key]
        public int Id { get; set; }
        [Required, BindProperty, Display(Name = "Дата прийняття")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ReceiptDate { get; set; }
        [Required, BindProperty, Display(Name = "Дата закінчення зберігання")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StorageDate  { get; set; }

        //--First FK--//
        [ValidateNever]
        public int GoodId { get; set; }
        [ForeignKey("GoodId")]
        [ValidateNever]
        public Good Good { get; set; }

        //--Second FK--//
        [ValidateNever]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        [ValidateNever]
        public Client Client { get; set; }

    }
}
