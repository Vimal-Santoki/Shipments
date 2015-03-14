using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Shipments.Filters
{
    public class CustomAuthorizeFilter:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            Utils.SessionHelper sessionHelper = new Utils.SessionHelper();
            filterContext.Controller.ViewBag.AutherizationMessage = "Please Autheticate using your credentials.";
            if (sessionHelper.UserID == null)
            {
                filterContext.Controller.ControllerContext.HttpContext.Response.Redirect("/SignIn/Index");
            }
        }
    }
}