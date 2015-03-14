using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shipments.Models;
using Shipments.Utils;
using Shipments.Filters;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Data;

namespace Shipments.Controllers
{
    [CustomAuthorizeFilter]
    [CustomeErrorHandler]
    public class ShipmentsController : Controller
    {
        //
        // GET: /Shipments/

        public ActionResult Index()
        {
            List<Brand> SMs = ShipmentsHelper.GetNestedShipments(null);
            //int i = SMs.Max(q => (q.ShipmentSummaryDetails.Max(x=>x.ShipmentDetails.Count)));
            //sm.ShipmentDetails = ShipmentsHelper.GetShipmentDetails();
            return View(SMs);
        }
        public ActionResult Add()
        {
            Shipment s = ShipmentsHelper.GetShipmentByID(0);
            return View(s);
        }
        public int EditShipment(int ID)
        {
            Shipment shipment = ShipmentsHelper.GetShipmentByID(ID);
            Session[Constants.SESSION_KEY_SHIPMENT_OBJ] = shipment;
            return 1;
        }
        public ActionResult Edit()
        {
            return View((Shipment)Session[Constants.SESSION_KEY_SHIPMENT_OBJ]);
        }
        public ActionResult Save(Shipment sm)
        {
            ShipmentsHelper.SaveShipment(sm);
            return RedirectToAction("Index");
        }
        public ActionResult Filter(Shipment sm)
        {

            List<Brand> SMs = ShipmentsHelper.GetNestedShipments(sm);
            return View("Index", SMs);

        }
        public ActionResult Download(Shipment sm, bool IsFiltered)
        {
            List<Brand> SMs = ShipmentsHelper.GetNestedShipments(IsFiltered ? sm : null);
            DownloadManager.DownloadExcel(string.Format(Constants.APPLICATION_KEY_SHIPMENT_FILE_NAME, DateTime.Now.ToString("dd-MMM-yyyy hhmmss")), DownloadManager.ShipmentsData(SMs));
            return View("Index", SMs);
        }
        public int Delete(int ID)
        {
            Shipment shipment = ShipmentsHelper.GetShipmentByID(ID);
            ShipmentsHelper.DeleteShipment(shipment);
            return 1;
        }
        [HttpGet]
        public ActionResult GetSKUs(string Query)
        {
            List<object> obj = ShipmentsHelper.GetSKUs(Query);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Upload()
        {
            return View();
        }
        public ActionResult GetUploadFormat()
        {
            string path = Server.MapPath(Constants.APPLICATION_KEY_SHIPMENT_UPLOAD_FORMAT_FILE_NAME);
            return File(path, "application/force-download", Path.GetFileName(path));
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase ShipmentFile)
        {
            Users u = (Users)Session["LoginUser"];
            string path = Constants.APPLICATION_KEY_UPLOAD_PATH;

            if (ShipmentFile != null)
            {
                path = Server.MapPath(path + ShipmentFile.FileName);
                ShipmentFile.SaveAs(path);
                DataTable dt = DownloadManager.GetDataTableFromCSV(path, true);
                foreach (DataRow dRow in dt.Rows)
                {
                    Shipment sm = new Shipment();
                    sm.BrandID = ShipmentsHelper.GetBrandByName(dRow["Brand"].ToString()).ID;
                    sm.ID = 0;
                    sm.SKU = dRow["SKU"].ToString();
                    sm.ShipmentID = dRow["ShipmentID"].ToString();
                    System.IO.File.AppendAllText(Server.MapPath("~/Log.txt"), "ShipmentDate:" + dRow["ShipmentDate"].ToString() + Environment.NewLine);
                    sm.ShipmentDate = Convert.ToDateTime(dRow["ShipmentDate"]);
                    System.IO.File.AppendAllText(Server.MapPath("~/Log.txt"), "FBAReceivedDate:" + dRow["FBAReceivedDate"].ToString() + Environment.NewLine);
                    sm.FBAReceivedDate = Convert.ToDateTime(dRow["FBAReceivedDate"]);
                    sm.Quantity = Convert.ToInt16(dRow["Quantity"]);
                    sm.ShipmentCost = Convert.ToDecimal(dRow["CostPerShipment"]);
                    sm.TotalCost = Convert.ToDecimal(dRow["TotalCost"]);
                    System.IO.File.AppendAllText(Server.MapPath("~/Log.txt"), "EntryTimeStamp:" + sm.EntryTimeStamp.ToString() + Environment.NewLine);
                    ShipmentsHelper.SaveShipment(sm);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
