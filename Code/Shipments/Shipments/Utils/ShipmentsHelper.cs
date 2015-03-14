using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.IO;
using System.Web.UI;
using System.Text;
using Shipments.Models;
using Shipments.DAL;

namespace Shipments.Utils
{
    public class ShipmentsHelper
    {
        static SessionHelper sessionHelper = new SessionHelper();
        #region Shipments
        public static List<Brand> GetNestedShipments(Shipment sm)
        {
            DatabaseContext db = new DatabaseContext();
            List<Brand> data2 = null;
            if (sm == null)
            {
                var data = (from d in db.Brands select new { ID = d.ID, Name = d.Name, ShipmentSummaryDetails = 0 }).ToList();
                data2 = (from k in data select new Brand { ID = k.ID, Name = k.Name, ShipmentSummaryDetails = GetShipments(k.ID, "") }).ToList();
            }
            else
            {
                var data = (from d in db.Brands.Where(q => q.Name == sm.Brand || sm.Brand == null) select new { ID = d.ID, Name = d.Name, ShipmentSummaryDetails = 0 }).ToList();
                data2 = (from k in data select new Brand { ID = k.ID, Name = k.Name, ShipmentSummaryDetails = GetShipments(k.ID, sm.SKU) }).ToList();
            }
            data2.RemoveAll(q => q.ShipmentSummaryDetails.Count == 0);
            return data2;
        }
        public static List<ShipmentSummary> GetShipments(int BrandID, string SKU)
        {
            DatabaseContext db = new DatabaseContext();
            var Data = (from d in db.ShipmentSummary.Where(q => q.BrandID == BrandID && (SKU == "" || SKU == null || q.SKU == SKU)) select new { ID = d.ID, PerShipmentCost = d.PerShipmentCost, ShipmentAmount = d.ShipmentAmount, ShipmentCount = d.ShipmentCount, SKU = d.SKU, LastUpdatedTimeStamp = d.LastUpdatedTimeStamp, ShipmentDetails = 0 }).ToList();
            return (from k in Data select new ShipmentSummary { ID = k.ID, PerShipmentCost = k.PerShipmentCost, ShipmentAmount = k.ShipmentAmount, ShipmentCount = k.ShipmentCount, SKU = k.SKU, LastUpdatedTimeStamp = k.LastUpdatedTimeStamp, ShipmentDetails = GetShipmentDetailsBySKU(k.SKU) }).ToList();
        }
        public static Shipment GetShipmentByID(int ID)
        {
            DatabaseContext db = new DatabaseContext();
            Shipment s = null;
            if (ID > 0)
                s = db.Shipments.AsNoTracking().Where(q => q.ID == ID).FirstOrDefault();
            else
                s = new Shipment();
            //s.Brands = new System.Web.Mvc.SelectList(db.Brands, "ID", "Name");
            return s;
        }
        public static List<Shipment> GetShipmentDetailsBySKU(string SKU)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Shipments.AsNoTracking().Where(q => q.SKU == SKU).ToList<Shipment>();
        }

        public static void SaveShipment(Shipment sm)
        {
            Shipment PrevShipment = null;
            using (var dbCtx1 = new DatabaseContext())
            {
                if (sm.ID == 0)
                {

                    dbCtx1.Entry(sm).State = EntityState.Added;
                    dbCtx1.Shipments.Add(sm);
                    dbCtx1.SaveChanges();
                }
                else
                {
                    PrevShipment = GetShipmentByID(sm.ID);

                    dbCtx1.Entry(sm).State = EntityState.Modified;
                    dbCtx1.SaveChanges();
                }
                UpdateShipmentSummary(sm.SKU, sm.Quantity, sm.TotalCost, sm.BrandID);
                if (sm.ID > 0 && PrevShipment != null && PrevShipment.SKU != sm.SKU)
                {
                    UpdateShipmentSummary(PrevShipment.SKU, PrevShipment.Quantity, PrevShipment.TotalCost, PrevShipment.BrandID);
                }
            }
        }
        private static void UpdateShipmentSummary(string SKU, int ShipmentCount, decimal ShipmentAmount, int BrandID)
        {
            using (var dbCtx = new DatabaseContext())
            {
                ShipmentSummary ss = GetShipmentSummaryBySKU(SKU);
                if (ss == null)
                {
                    ss = new ShipmentSummary();
                    ss.SKU = SKU;
                    ss.ShipmentCount = ShipmentCount;
                    ss.PerShipmentCost = ShipmentAmount / ShipmentCount;
                    ss.ShipmentAmount = ShipmentAmount;
                    ss.BrandID = BrandID;
                    dbCtx.Entry(ss).State = EntityState.Added;
                    dbCtx.ShipmentSummary.Add(ss);
                }
                else
                {
                    List<Shipment> shipments = GetShipmentDetailsBySKU(SKU);
                    ss.ShipmentCount = shipments.Sum(q => q.Quantity);
                    ss.ShipmentAmount = shipments.Sum(q => q.TotalCost);

                    ss.BrandID = BrandID;
                    if (ss.ShipmentAmount == 0)
                    {
                        dbCtx.Entry(ss).State = EntityState.Deleted;
                    }
                    else
                    {
                        ss.PerShipmentCost = ss.ShipmentAmount / ss.ShipmentCount;
                        dbCtx.Entry(ss).State = EntityState.Modified;
                    }
                }
                dbCtx.SaveChanges();
            }
        }
        public static ShipmentSummary GetShipmentSummaryBySKU(string SKU)
        {
            DatabaseContext db = new DatabaseContext();
            return db.ShipmentSummary.AsNoTracking().Where(q => q.SKU == SKU).FirstOrDefault();
        }
        public static void DeleteShipment(Shipment s)
        {
            Delete<Shipment>(s);
            UpdateShipmentSummary(s.SKU, 0, 0, s.BrandID);
        }
        #endregion
        #region Brand
        public static List<Brand> GetBrand(Brand b)
        {
            DatabaseContext db = new DatabaseContext();
            List<Brand> brandList = null;
            if (b == null)
                brandList = db.Brands.ToList<Brand>();
            else
                brandList = db.Brands.Where(q => (q.Name.Contains(b.Name) || b.Name == null) && (q.Manufacturer.Contains(b.Manufacturer) || b.Manufacturer == null)).ToList();
            return brandList;
        }
        public static Brand GetBrandByID(int ID)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Brands.Where(q => q.ID == ID).FirstOrDefault();
        }
        public static Brand GetBrandByName(string Name)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Brands.Where(q => q.Name == Name).FirstOrDefault();
        }
        public static void SaveBrand(Brand b)
        {
            using (var dbCtx = new DatabaseContext())
            {
                if (b.ID == 0)
                {
                    dbCtx.Entry(b).State = EntityState.Added;
                    //Add Student object into Students DBset
                    dbCtx.Brands.Add(b);

                }
                else
                {
                    dbCtx.Entry(b).State = EntityState.Modified;
                }

                // call SaveChanges method to save student into database
                dbCtx.SaveChanges();
            }
        }
        #endregion
        #region Users
        public static Users GetUserFromCreadentials(Users u)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Users.Where(q => q.UserName == u.UserName && q.Password == u.Password).FirstOrDefault();
        }
        public static List<Users> GetUsers(Users u)
        {
            DatabaseContext db = new DatabaseContext();
            List<Users> userList = null;
            if (u == null)
                userList = db.Users.ToList<Users>();
            else
                userList = db.Users.Where(q => (q.Name.Contains(u.Name) || u.Name == null) && (q.UserName.Contains(u.UserName) || u.UserName == null) && (q.Email.Contains(u.Email) || u.Email == null)).ToList();
            return userList;
        }
        public static void SaveUser(Users u)
        {
            using (var dbCtx = new DatabaseContext())
            {
                if (u.ID == 0)
                {
                    dbCtx.Entry(u).State = EntityState.Added;
                    //Add Student object into Students DBset
                    dbCtx.Users.Add(u);

                }
                else
                {
                    dbCtx.Entry(u).State = EntityState.Modified;
                }

                // call SaveChanges method to save student into database
                dbCtx.SaveChanges();
            }
        }
        public static Users GetUserByID(int ID)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Users.Where(q => q.ID == ID).FirstOrDefault();
        }
        #endregion
        #region SKU
        public static List<SKUs> GetSKU(SKUs ss)
        {
            List<SKUs> Data2 = null;
            DatabaseContext db = new DatabaseContext();
            if (ss == null)
            {
                var Data = (from bs in db.Brands
                            join s in db.SKUs on bs.ID equals s.BrandID
                            select new { ID = s.ID, Brand = bs.Name, BrandID = bs.ID, Price = s.Price, SKU = s.SKU }).ToList();
                Data2 = (from d in Data select new SKUs { ID = d.ID, Brand = d.Brand, BrandID = d.BrandID, Price = d.Price, SKU = d.SKU }).ToList();
            }
            else
            {
                var Data = (from bs in db.Brands
                            join s in db.SKUs on bs.ID equals s.BrandID
                            where ((bs.Name.Contains(ss.Brand) || ss.Brand == null) && (s.SKU.Contains(ss.SKU) || ss.SKU == null))
                            select new { ID = s.ID, Brand = bs.Name, BrandID = bs.ID, Price = s.Price, SKU = s.SKU }).ToList();
                Data2 = (from d in Data select new SKUs { ID = d.ID, Brand = d.Brand, BrandID = d.BrandID, Price = d.Price, SKU = d.SKU }).ToList();
            }

            return Data2;
            //return db.BrandsL
            //return db.SKUs.ToList<SKUs>();
        }
        public static SKUs GetSKUByID(int ID)
        {
            DatabaseContext db = new DatabaseContext();
            SKUs s = null;
            if (ID > 0)
                s = db.SKUs.AsNoTracking().Where(q => q.ID == ID).FirstOrDefault();
            else
                s = new SKUs();
            s.Brands = new System.Web.Mvc.SelectList(db.Brands, "ID", "Name");
            return s;
        }
        public static void SaveSKU(SKUs s)
        {
            using (var dbCtx = new DatabaseContext())
            {
                if (s.ID == 0)
                {
                    dbCtx.Entry(s).State = EntityState.Added;
                    dbCtx.SKUs.Add(s);
                }
                else
                {
                    dbCtx.Entry(s).State = EntityState.Modified;
                }
                dbCtx.SaveChanges();
            }
        }
        public static List<object> GetSKUs(string Qry)
        {
            DatabaseContext db = new DatabaseContext();
            return (from s in db.SKUs join b in db.Brands on s.BrandID equals b.ID where s.SKU.Contains(Qry) select new { sku = s.SKU, price = "$" + s.Price, brand = b.Name, brandId = b.ID }).ToList<object>();
        }
        #endregion
        #region Purchase Order
        public static List<Brand> GetNestedPOs(PurchaseOrder p)
        {
            DatabaseContext db = new DatabaseContext();
            List<Brand> data2 = null;
            if (p == null)
            {
                var data = (from d in db.Brands select new { ID = d.ID, Name = d.Name, PurchaseOrderSummaryDetails = 0 }).ToList();
                data2 = (from k in data select new Brand { ID = k.ID, Name = k.Name, PurchaseOrderSummaryDetails = GetPOs(k.ID, "") }).ToList();
            }
            else
            {
                var data = (from d in db.Brands where (d.Name == p.Brand || p.Brand == "" || p.Brand == null) select new { ID = d.ID, Name = d.Name, PurchaseOrderSummaryDetails = 0 }).ToList();
                data2 = (from k in data select new Brand { ID = k.ID, Name = k.Name, PurchaseOrderSummaryDetails = GetPOs(k.ID, p.SKU) }).ToList();
            }
            data2.RemoveAll(q => q.PurchaseOrderSummaryDetails.Count == 0);
            return data2;
        }
        public static List<PurchaseOrderSummary> GetPOs(int BrandID, string SKU)
        {
            DatabaseContext db = new DatabaseContext();
            var Data = (from p in db.PurchaseOrderSummary join s in db.SKUs on p.SKU equals s.SKU where s.BrandID == BrandID && (SKU == "" || SKU == null || s.SKU == SKU) select new { ID = p.ID, PerOrderCost = p.PerOrderCost, OrderAmount = p.OrderAmount, PurchaseOrderCount = p.PurchaseOrderCount, SKU = p.SKU, LastUpdatedTimeStamp = p.LastUpdatedTimeStamp, PurchaseOrderDetails = 0 }).ToList();
            return (from k in Data select new PurchaseOrderSummary { ID = k.ID, PerOrderCost = k.PerOrderCost, OrderAmount = k.OrderAmount, PurchaseOrderCount = k.PurchaseOrderCount, SKU = k.SKU, LastUpdatedTimeStamp = k.LastUpdatedTimeStamp, PurchaseOrderDetails = GetPurchaseOrderDetailsBySKU(k.SKU) }).ToList();
        }
        public static List<PurchaseOrder> GetPurchaseOrderDetailsBySKU(string SKU)
        {
            DatabaseContext db = new DatabaseContext();
            return db.PurchaseOrders.AsNoTracking().Where(q => q.SKU == SKU).ToList<PurchaseOrder>();
        }
        public static PurchaseOrder GetPOByID(int ID)
        {
            DatabaseContext db = new DatabaseContext();
            PurchaseOrder p = null;
            if (ID > 0)
                p = db.PurchaseOrders.AsNoTracking().Where(q => q.ID == ID).FirstOrDefault();
            else
                p = new PurchaseOrder();
            return p;
        }
        public static void SavePO(PurchaseOrder p)
        {
            PurchaseOrder PrevPO = null;
            using (var dbCtx1 = new DatabaseContext())
            {
                if (p.ID == 0)
                {
                    p.UserID = sessionHelper.UserID;
                    dbCtx1.Entry(p).State = EntityState.Added;
                    dbCtx1.PurchaseOrders.Add(p);
                    dbCtx1.SaveChanges();

                }
                else
                {
                    PrevPO = GetPOByID(p.ID);
                    p.UserID = sessionHelper.UserID;
                    dbCtx1.Entry(p).State = EntityState.Modified;
                    dbCtx1.SaveChanges();
                }
                UpdatePOSummary(p.SKU, p.Quantity, p.TotalCost);
                if (p.ID > 0 && PrevPO != null && p.SKU != PrevPO.SKU)
                {
                    UpdatePOSummary(PrevPO.SKU, PrevPO.Quantity, PrevPO.TotalCost);
                }
            }
        }
        private static void UpdatePOSummary(string SKU, int OrderCount, decimal OrderAmount)
        {
            using (var dbCtx = new DatabaseContext())
            {
                PurchaseOrderSummary ps = GetPOSummaryBySKU(SKU);
                if (ps == null)
                {
                    ps = new PurchaseOrderSummary();
                    ps.SKU = SKU;
                    ps.PurchaseOrderCount = OrderCount;
                    ps.PerOrderCost = OrderAmount / OrderCount;
                    ps.OrderAmount = OrderAmount;
                    ps.LastUpdatedBy = sessionHelper.UserID;

                    dbCtx.Entry(ps).State = EntityState.Added;
                    dbCtx.PurchaseOrderSummary.Add(ps);
                }
                else
                {
                    List<PurchaseOrder> po = GetPurchaseOrderDetailsBySKU(SKU);
                    ps.PurchaseOrderCount = po.Sum(q => q.Quantity);
                    ps.OrderAmount = po.Sum(q => q.TotalCost);
                    if (ps.OrderAmount == 0)
                    {
                        dbCtx.Entry(ps).State = EntityState.Deleted;
                    }
                    else
                    {
                        ps.PerOrderCost = ps.OrderAmount / ps.PurchaseOrderCount;
                        ps.LastUpdatedBy = sessionHelper.UserID;
                        dbCtx.Entry(ps).State = EntityState.Modified;
                    }
                }
                dbCtx.SaveChanges();
            }
        }
        public static PurchaseOrderSummary GetPOSummaryBySKU(string SKU)
        {
            DatabaseContext db = new DatabaseContext();
            return db.PurchaseOrderSummary.AsNoTracking().Where(q => q.SKU == SKU).FirstOrDefault();
        }
        public static void DeletePO(PurchaseOrder p)
        {
            Delete<PurchaseOrder>(p);
            UpdatePOSummary(p.SKU, 0, 0);
            //using (var dbCtx = new DatabaseContext())
            //{
            //    dbCtx.Entry(p).State = EntityState.Deleted;
            //    dbCtx.SaveChanges();

            //}
        }
        #endregion
        #region MISC
        public static void Delete<T>(T t) where T : class
        {
            using (var dbCtx = new DatabaseContext())
            {
                dbCtx.Entry(t).State = EntityState.Deleted;
                dbCtx.SaveChanges();
            }
        }
        #endregion
    }
}