using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Training.Web.Models
{
    [Table("Clients")]
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required , Display(Name = "П.І.Б"), MaxLength(50)]
        public string FullName  { get; set; }

        [Display(Name = "Серія Паспорту"), MaxLength(8)]
        public string PassportSerial { get; set; }

        [Display(Name = "Номер Паспорту"), MaxLength(9)]
        public string PassportNumber { get; set; }

        [Display(Name = "Номер Телефону"), MaxLength(13), RegularExpression(@"^\+380\d{9}$")]
        public string PhoneNumber { get; set; }
    }
}
