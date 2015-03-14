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
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            List<Users> us = ShipmentsHelper.GetUsers(null);
            return View(us);
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Save(Users u)
        {
            ShipmentsHelper.SaveUser(u);
            return RedirectToAction("Index");
        }
        public int EditUser(int ID)
        {
            Users user= ShipmentsHelper.GetUserByID(ID);
            Session[Constants.SESSION_KEY_USER_OBJ] = user;
            return 1;
        }
        public ActionResult Edit()
        {
            return View((Users)Session[Constants.SESSION_KEY_USER_OBJ]);
        }
        public int Delete(int ID)
        {
            Users user = ShipmentsHelper.GetUserByID(ID);
            ShipmentsHelper.Delete<Users>(user);
            return 1;
        }
        public ActionResult Filter(Users u)
        {
            List<Users> Us = ShipmentsHelper.GetUsers(u);
            return View("Index", Us);
        }
        public ActionResult Download(Users u, bool IsFiltered)
        {
            List<Users> Us = ShipmentsHelper.GetUsers(IsFiltered ? u : null);
            DownloadManager.DownloadExcel(string.Format(Constants.APPLICATION_KEY_USER_FILE_NAME, DateTime.Now.ToString("dd-MMM-yyyy hhmmss")), DownloadManager.UsersData(Us));
            return View("Index", Us);
        }
    }
}
