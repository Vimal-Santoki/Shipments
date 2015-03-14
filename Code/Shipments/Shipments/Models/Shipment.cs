using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shipments.Models
{
    [Table("Shipments")]
    public class Shipment
    {
        [Key]
        public int ID { get; set; }
        public int BrandID { get; set; }
        [NotMapped]
        public string Brand { get; set; }
        [NotMapped]
        public SelectList SKUs { get; set; }
        public string SKU { get; set; }
        public string ShipmentID { get; set; }
        public DateTime FBAReceivedDate { get; set; }
        public int Quantity { get; set; }
        public decimal ShipmentCost { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime ShipmentDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime EntryTimeStamp { get; set; }
    }
}