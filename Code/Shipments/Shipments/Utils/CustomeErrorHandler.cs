using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shipments.Utils
{
    public class CustomeErrorHandler:HandleErrorAttribute
    {
        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        public override void OnException(ExceptionContext filterContext)
        {

            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }
            // if the request is AJAX return JSON else view.
            if (IsAjax(filterContext))
            { }
            else
            {
                if ((filterContext.Exception).Message != "Cannot redirect after HTTP headers have been sent.")
                {
                }//Normal Exception
                //So let it handle by its default ways.
                base.OnException(filterContext);


            }
        }
    }
}