using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shipments.Models
{
    [Table("PurchaseOrder")]
    public class PurchaseOrder
    {
        public int ID { get; set; }
        [NotMapped]
        public int BrandID { get; set; }
        [NotMapped]
        public string Brand { get; set; }
        public string SKU { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal TotalCost { get; set; }
        public string ForASIN { get; set; }
        public int UserID { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime EntryTimeStamp { get; set; }
    }
}