using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Training.Web.Models
{
    public class GoodsReportModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public GoodsStatus Status { get; set; }

        public decimal AppraisedValue { get; set; }

        public decimal Commision { get; set; }

        public string Category { get; set; }

        public DateTime ReceiptDate { get; set; }

        public DateTime StorageDate { get; set; }
    }
}
