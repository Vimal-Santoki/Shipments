using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shipments.Utils;
using Shipments.Models;
using Shipments.Filters;
using System.Data;
using System.IO;

namespace Shipments.Controllers
{
    [CustomAuthorizeFilter]
    [CustomeErrorHandler]
    public class SKUController : Controller
    {
        //
        // GET: /SKU/

        public ActionResult Index()
        {
            List<SKUs> s = ShipmentsHelper.GetSKU(null);
            return View(s);
        }
        public ActionResult Add()
        {
            SKUs s = ShipmentsHelper.GetSKUByID(0);
            return View(s);
        }
        public ActionResult Save(SKUs b)
        {
            ShipmentsHelper.SaveSKU(b);
            return RedirectToAction("Index");
        }
        public int EditSKU(int ID)
        {
            SKUs skus = ShipmentsHelper.GetSKUByID(ID);
            Session[Constants.SESSION_KEY_SKU_OBJ] = skus;
            return 1;
        }
        public ActionResult Edit()
        {
            return View((SKUs)Session[Constants.SESSION_KEY_SKU_OBJ]);
        }
        public int Delete(int ID)
        {
            SKUs sku = ShipmentsHelper.GetSKUByID(ID);
            //ShipmentsHelper.DeleteSKU(shipment);
            ShipmentsHelper.Delete<SKUs>(sku);
            return 1;
        }
        public ActionResult Filter(SKUs b)
        {
            List<SKUs> Ss = ShipmentsHelper.GetSKU(b);
            return View("Index", Ss);
        }
        public ActionResult Download(SKUs s, bool IsFiltered)
        {
            List<SKUs> Ss = ShipmentsHelper.GetSKU(IsFiltered ? s : null);
            DownloadManager.DownloadExcel(string.Format(Constants.APPLICATION_KEY_SKU_FILE_NAME, DateTime.Now.ToString("dd-MMM-yyyy hhmmss")), DownloadManager.SKUsData(Ss));
            return View("Index", Ss);
        }
        public ActionResult Upload()
        {
            return View();
        }
        public ActionResult GetUploadFormat()
        {
            string path = Server.MapPath(Constants.APPLICATION_KEY_SKU_UPLOAD_FORMAT_FILE_NAME);
            return File(path, "application/force-download", Path.GetFileName(path));
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase SKUFile)
        {
            //Users u = (Users)Session["LoginUser"];
            string path = Constants.APPLICATION_KEY_UPLOAD_PATH;

            if (SKUFile != null)
            {
                path = Server.MapPath(path + SKUFile.FileName);
                SKUFile.SaveAs(path);
                DataTable dt = DownloadManager.GetDataTableFromCSV(path, true);
                foreach (DataRow dRow in dt.Rows)
                {
                    SKUs s = new SKUs();
                    s.BrandID=ShipmentsHelper.GetBrandByName(dRow["Brand"].ToString()).ID;
                    s.SKU= dRow["SKU"].ToString();
                    s.Price = Convert.ToDecimal(dRow["Price"]);
                    s.ID = 0;
                    ShipmentsHelper.SaveSKU(s);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
