using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Shipments.Models
{
    [Table("SKU")]
    public class SKUs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int BrandID { get; set; }
        [NotMapped]
        public string Brand { get; set; }
        [NotMapped]
        public SelectList Brands { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
    }
}