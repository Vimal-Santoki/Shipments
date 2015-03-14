using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shipments.Models;
using Shipments.Utils;
using Shipments.Filters;

namespace Shipments.Controllers
{
    [CustomAuthorizeFilter]
    [CustomeErrorHandler]
    public class PurchaseOrderController : Controller
    {
        //
        // GET: /PurchaseOrder/

        public ActionResult Index()
        {
            List<Brand> POs = ShipmentsHelper.GetNestedPOs(null);
            return View(POs);
        }
        public ActionResult Add()
        {
            PurchaseOrder s = ShipmentsHelper.GetPOByID(0);
            return View(s);
        }
        public int EditPO(int ID)
        {
            PurchaseOrder po = ShipmentsHelper.GetPOByID(ID);
            Session[Constants.SESSION_KEY_PURCHASE_ORDER_OBJ] = po;
            return 1;
        }
        public ActionResult Edit()
        {
            return View((PurchaseOrder)Session[Constants.SESSION_KEY_PURCHASE_ORDER_OBJ]);
        }
        public ActionResult Save(PurchaseOrder p)
        {
            ShipmentsHelper.SavePO(p);
            return RedirectToAction("Index");
        }
        public ActionResult Filter(PurchaseOrder p)
        {
            List<Brand> POs = ShipmentsHelper.GetNestedPOs(p);
            return View("Index", POs);
        }
        public int Delete(int ID)
        {
            PurchaseOrder po = ShipmentsHelper.GetPOByID(ID);
            ShipmentsHelper.DeletePO(po);
            return 1;
        }
    }
}
