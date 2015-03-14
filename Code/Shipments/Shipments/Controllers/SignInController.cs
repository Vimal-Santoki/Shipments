using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shipments.Models;
using Shipments.Utils;

namespace Shipments.Controllers
{
    public class SignInController : Controller
    {
        SessionHelper sessionHelper = new SessionHelper();
        //
        // GET: /SignIn/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogIn(Users u)
        {
            Users us = ShipmentsHelper.GetUserFromCreadentials(u);
            if (us != null)
            {
                
                sessionHelper.UserLoginName = us.UserName;
                sessionHelper.UserName = us.Name;
                sessionHelper.IsAdmin = us.IsAdmin;
                sessionHelper.UserID = us.ID;
                return RedirectToAction("Index", "Shipments");
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Index");
        }
    }
}
