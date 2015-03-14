using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shipments.Utils
{
    public class Constants
    {
        //Session Variables
        public static string SESSION_KEY_USER_NAME = "UserName";
        public static string SESSION_KEY_USER_LOGIN_NAME = "UserLoginName";
        public static string SESSION_KEY_IS_ADMIN = "IsAdmin";
        public static string SESSION_KEY_USER_ID = "UserID";
        public static string SESSION_KEY_BRAND_OBJ = "BrandObject";
        public static string SESSION_KEY_SHIPMENT_OBJ = "ShipmentObject";
        public static string SESSION_KEY_PURCHASE_ORDER_OBJ = "POObject";
        public static string SESSION_KEY_SKU_OBJ = "SKUObject";
        public static string SESSION_KEY_USER_OBJ = "UserObject";

        //Application Variables
        public static int APPLICATION_KEY_BULK_UPLOAD_COUNT = 500;
        public static string APPLICATION_KEY_UPLOAD_PATH = @"\Content\Upload\";
        public static string APPLICATION_KEY_DOWNLOAD_WRAPPER = @"<div><table cellspacing=""0"" rules=""all"" border=""1"" style=""border-collapse:collapse;"">{0}</table></div>";
        public static string APPLICATION_KEY_ROW_WRAPPER = @"<tr>{0}</tr>";
        #region Shipment Download Template
        public static string APPLICATION_KEY_SHIPMENT_SUMMARY_HEADER_FORMAT = @"<th scope=""col"" style=""background-color:#c4bd97"" >Brand</th><th scope=""col"" style=""background-color:#c4bd97"">SKU</th><th scope=""col"" style=""background-color:#c4bd97"">Total Shipped</th><th scope=""col"" style=""background-color:#c4bd97"">Avg Cost Per Shipment</th><th scope=""col"" style=""background-color:#c4bd97"">Total Cost</th>";
        public static string APPLICATION_KEY_SHIPMENT_HEADER_FORMAT = @"<th scope=""col"" style=""background-color:green;width:20px""></th><th scope=""col"" style=""background-color:yellow"">ShipmentID</th><th scope=""col"" style=""background-color:yellow"">Shipment Date</th><th scope=""col"" style=""background-color:yellow"">Received Date At FBA</th><th scope=""col"" style=""background-color:yellow"">Quantity</th><th scope=""col"" style=""background-color:yellow"">Cost Per Shipment</th><th scope=""col"" style=""background-color:yellow"">Total Cost</th>";
        public static string APPLICATION_KEY_SHIPMENT_SUMMARY_DATA_FORMAT = @"<td style=""background-color:#c4bd97"">{0}</td><td style=""background-color:#c4bd97"">{1}</td><td style=""background-color:#c4bd97"">{2}</td><td style=""background-color:#c4bd97"">{3}</td><td style=""background-color:#c4bd97"">{4}</td>";
        public static string APPLICATION_KEY_SHIPMENT_DATA_FORMAT = @"<td style=""background-color:green;width:20px""></td><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>";
        public static string APPLICATION_KEY_SHIPMENT_FILE_NAME = "Shipments_{0}.xls";
        public static string APPLICATION_KEY_SHIPMENT_UPLOAD_FORMAT = @"<th scope=""col"" style=""background-color:yellow"">Brand</th><th scope=""col"" style=""background-color:yellow"">SKU</th><th scope=""col"" style=""background-color:yellow"">ShipmentID</th><th scope=""col"" style=""background-color:yellow"">ShipmentDate</th><th scope=""col"" style=""background-color:yellow"">FBAReceivedDate</th><th scope=""col"" style=""background-color:yellow"">Quantity</th><th scope=""col"" style=""background-color:yellow"">CostPerShipment</th><th scope=""col"" style=""background-color:yellow"">TotalCost</th>";
        public static string APPLICATION_KEY_SHIPMENT_UPLOAD_FORMAT_FILE_NAME = "/Content/Download/Formats/Shipments_Upload_Format.csv";
        #endregion
        #region Brand Download Template
        public static string APPLICATION_KEY_BRAND_FILE_NAME = "Brands_{0}.xls";
        public static string APPLICATION_KEY_BRANDS_HEADER_FORMAT = @"<th scope=""col"" style=""background-color:yellow"" >Brand Name</th><th scope=""col"" style=""background-color:yellow"">Manufacturer</th>";
        public static string APPLICATION_KEY_BRANDS_DATA_FORMAT = @"<td>{0}</td><td>{1}</td>";
        public static string APPLICATION_KEY_BRAND_UPLOAD_FORMAT_FILE_NAME = "/Content/Download/Formats/Brands_Upload_Format.csv";
        #endregion
        #region SKU Download Template
        public static string APPLICATION_KEY_SKU_FILE_NAME = "SKUs_{0}.xls";
        public static string APPLICATION_KEY_SKUS_HEADER_FORMAT = @"<th scope=""col"" style=""background-color:yellow"" >Brand Name</th><th scope=""col"" style=""background-color:yellow"">SKU</th><th scope=""col"" style=""background-color:yellow"">Price</th>";
        public static string APPLICATION_KEY_SKUS_DATA_FORMAT = @"<td>{0}</td><td>{1}</td><td>{2}</td>";
        public static string APPLICATION_KEY_SKU_UPLOAD_FORMAT_FILE_NAME = "/Content/Download/Formats/SKUs_Upload_Format.csv";
        #endregion
        #region User Download Template
        public static string APPLICATION_KEY_USER_FILE_NAME = "Users_{0}.xls";
        public static string APPLICATION_KEY_USERS_HEADER_FORMAT = @"<th scope=""col"" style=""background-color:yellow"" >User </th><th scope=""col"" style=""background-color:yellow"">User Name</th><th scope=""col"" style=""background-color:yellow"">Email</th><th scope=""col"" style=""background-color:yellow"">Contact#</th><th scope=""col"" style=""background-color:yellow"">Is Admin</th>";
        public static string APPLICATION_KEY_USERS_DATA_FORMAT = @"<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>";
        #endregion

    }
}