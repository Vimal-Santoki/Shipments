using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shipments.Models;

namespace Shipments.Utils
{
    public class SessionHelper
    {
        public string UserName
        {
            get
            {
                return HttpContext.Current.Session[Constants.SESSION_KEY_USER_NAME].ToString();
            }
            set
            {
                HttpContext.Current.Session[Constants.SESSION_KEY_USER_NAME] = value;
            }
        }
        public string UserLoginName
        {
            get
            {
                return HttpContext.Current.Session[Constants.SESSION_KEY_USER_LOGIN_NAME].ToString();
            }
            set
            {
                HttpContext.Current.Session[Constants.SESSION_KEY_USER_LOGIN_NAME] = value;
            }
        }
        public bool IsAdmin
        {
            get
            {
                return Convert.ToBoolean(HttpContext.Current.Session[Constants.SESSION_KEY_IS_ADMIN]);
            }
            set
            {
                HttpContext.Current.Session[Constants.SESSION_KEY_IS_ADMIN] = value;
            }
        }
        public int UserID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session[Constants.SESSION_KEY_USER_ID]);
            }
            set
            {
                HttpContext.Current.Session[Constants.SESSION_KEY_USER_ID] = value;
            }
        }
        public Brand BrandObject
        {
            get
            {
                return (Brand)(HttpContext.Current.Session[Constants.SESSION_KEY_BRAND_OBJ]);
            }
            set
            {
                HttpContext.Current.Session[Constants.SESSION_KEY_BRAND_OBJ] = value;
            }
        }
        public Shipment ShipmentObject
        {
            get
            {
                return (Shipment)(HttpContext.Current.Session[Constants.SESSION_KEY_SHIPMENT_OBJ]);
            }
            set
            {
                HttpContext.Current.Session[Constants.SESSION_KEY_SHIPMENT_OBJ] = value;
            }
        }
    }
}