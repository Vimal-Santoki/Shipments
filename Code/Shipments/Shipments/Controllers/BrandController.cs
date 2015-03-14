using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shipments.Utils;
using Shipments.Models;
using Shipments.Filters;
using System.IO;
using System.Data;

namespace Shipments.Controllers
{
    [CustomAuthorizeFilter]
    [CustomeErrorHandler]
    public class BrandController : Controller
    {
        //
        // GET: /Brand/

        public ActionResult Index()
        {
            List<Brand> b = ShipmentsHelper.GetBrand(null);
            return View(b);
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Save(Brand b)
        {
            ShipmentsHelper.SaveBrand(b);
            return RedirectToAction("Index");
        }
        public int EditBrand(int ID)
        {
            Brand brand = ShipmentsHelper.GetBrandByID(ID);
            Session[Constants.SESSION_KEY_BRAND_OBJ] = brand;
            return 1;
        }
        public ActionResult Edit()
        {
            return View((Brand)Session[Constants.SESSION_KEY_BRAND_OBJ]);
        }
        public int Delete(int ID)
        {
            Brand brand = ShipmentsHelper.GetBrandByID(ID);
            ShipmentsHelper.Delete<Brand>(brand);
           // ShipmentsHelper.DeleteBrand(brand);
            return 1;
        }
        public ActionResult Filter(Brand b)
        {
            List<Brand> Bs = ShipmentsHelper.GetBrand(b);
            return View("Index", Bs);
        }
        public ActionResult Download(Brand b, bool IsFiltered)
        {
            List<Brand> Bs = ShipmentsHelper.GetBrand(IsFiltered ? b : null);
            DownloadManager.DownloadExcel(string.Format(Constants.APPLICATION_KEY_BRAND_FILE_NAME, DateTime.Now.ToString("dd-MMM-yyyy hhmmss")), DownloadManager.BrandsData(Bs));
            return View("Index", Bs);
        }
        public ActionResult Upload()
        {
            return View();
        }
        public ActionResult GetUploadFormat()
        {
            string path = Server.MapPath(Constants.APPLICATION_KEY_BRAND_UPLOAD_FORMAT_FILE_NAME);
            return File(path, "application/force-download", Path.GetFileName(path));
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase BrandFile)
        {
            //Users u = (Users)Session["LoginUser"];
            string path = Constants.APPLICATION_KEY_UPLOAD_PATH;

            if (BrandFile != null)
            {
                path=Server.MapPath(path + BrandFile.FileName);
                BrandFile.SaveAs(path);
                DataTable dt = DownloadManager.GetDataTableFromCSV(path,true);
                foreach (DataRow dRow in dt.Rows)
                {
                    Brand b = new Brand();
                    b.Name = dRow["Brand"].ToString();
                    b.Manufacturer = dRow["Manufacturer"].ToString();
                    b.ID = 0;
                    ShipmentsHelper.SaveBrand(b);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
