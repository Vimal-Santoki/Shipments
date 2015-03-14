using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Shipments.Models;

namespace Shipments.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentSummary> ShipmentSummary { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<SKUs> SKUs { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderSummary> PurchaseOrderSummary { get; set; }
    }
}