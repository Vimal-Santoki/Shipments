using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shipments.Models
{
    [Table("Brand")]
    public class Brand
    {
        public int ID { get; set; }
        [Required(ErrorMessage="Brand name should not be empty",AllowEmptyStrings=false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Manufacturer name should not be empty", AllowEmptyStrings = false)]
        public string Manufacturer { get; set; }
        [NotMapped]
        public List<ShipmentSummary> ShipmentSummaryDetails { get; set; }
        [NotMapped]
        public List<PurchaseOrderSummary> PurchaseOrderSummaryDetails { get; set; }
    }
}