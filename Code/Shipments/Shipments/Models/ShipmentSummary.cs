using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipments.Models
{
    [Table("ShipmentSummary")]
    public class ShipmentSummary
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int BrandID { get; set; }
        [NotMapped]
        public string Brand { get; set; }
        public string SKU { get; set; }
        public int ShipmentCount { get; set; }
        public decimal PerShipmentCost { get; set; }
        public decimal ShipmentAmount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastUpdatedTimeStamp { get; set; }
        [NotMapped]
        public List<Shipment> ShipmentDetails { get; set; }
    }
}