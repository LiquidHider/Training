using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Web.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required, Display(Name = "Назва"),MaxLength(50)]
        public string Name { get; set; }

        [Required, Display(Name = "Комісійні")]
        public decimal Commision { get; set; }
    }
}
