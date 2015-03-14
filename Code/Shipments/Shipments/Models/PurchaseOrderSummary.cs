using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shipments.Models
{
    [Table("PurchaseOrderSummary")]
    public class PurchaseOrderSummary
    {
        public int ID { get; set; }
        public string SKU { get; set; }
        [NotMapped]
        public int BrandID { get; set; }
        [NotMapped]
        public string Brand { get; set; }
        public int PurchaseOrderCount { get; set; }
        public decimal PerOrderCost { get; set; }
        public decimal OrderAmount { get; set; }
        public int LastUpdatedBy { get; set; }
        [NotMapped]
        public string LastUpdatedByName { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTimeStamp { get; set; }
        [NotMapped]
        public List<PurchaseOrder> PurchaseOrderDetails { get; set; }
    }
}