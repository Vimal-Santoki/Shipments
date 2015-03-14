using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shipments.Models;
using Shipments.Utils;
using Shipments.Filters;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web.Mvc;
using System.Data;
//using System.Data.OleDb;

namespace Shipments.Utils
{
    public class DownloadManager
    {
        public static string ShipmentsData(List<Brand> SMs)
        {
            StringBuilder sb = new StringBuilder(), sbRow = new StringBuilder();
            int colCount = SMs.Max(q => (q.ShipmentSummaryDetails.Max(x => x.ShipmentDetails.Count)));
            sb.Append(Constants.APPLICATION_KEY_SHIPMENT_SUMMARY_HEADER_FORMAT);
            for (int i = 0; i < colCount; i++)
            {
                sb.Append(Constants.APPLICATION_KEY_SHIPMENT_HEADER_FORMAT);
            }

            string Header = string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, sb.ToString());
            sb.Clear();
            int ColsFound = 0;
            foreach (Brand b in SMs)
            {
                foreach (ShipmentSummary ss in b.ShipmentSummaryDetails)
                {
                    ColsFound = 0;
                    sb.Append(string.Format(Constants.APPLICATION_KEY_SHIPMENT_SUMMARY_DATA_FORMAT, b.Name, ss.SKU, ss.ShipmentCount, ss.PerShipmentCost, ss.ShipmentAmount));
                    foreach (Shipment s in ss.ShipmentDetails)
                    {
                        ColsFound++;
                        sb.Append(string.Format(Constants.APPLICATION_KEY_SHIPMENT_DATA_FORMAT, s.ShipmentID, s.ShipmentDate, s.FBAReceivedDate, s.Quantity, s.ShipmentCost, s.TotalCost));
                    }
                    for (int i = 0; i < (colCount - ColsFound); i++)
                    {
                        sb.Append(string.Format(Constants.APPLICATION_KEY_SHIPMENT_DATA_FORMAT, "", "", "", "", "", ""));
                    }
                    sbRow.Append(string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, sb.ToString()));
                    sb.Clear();
                }
            }
            sb.Append(string.Format(Constants.APPLICATION_KEY_DOWNLOAD_WRAPPER, Header + sbRow.ToString()));
            sbRow.Clear();
            return sb.ToString();
        }
        public static string GetShipmentUploadFormat()
        {
            string Header = string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, Constants.APPLICATION_KEY_SHIPMENT_UPLOAD_FORMAT);
            return string.Format(Constants.APPLICATION_KEY_DOWNLOAD_WRAPPER, Header);
        }
        public static string BrandsData(List<Brand> SMs)
        {
            StringBuilder sb = new StringBuilder(), sbRow = new StringBuilder();

            string Header = string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, Constants.APPLICATION_KEY_BRANDS_HEADER_FORMAT);
            foreach (Brand b in SMs)
            {
                sb.Append(string.Format(Constants.APPLICATION_KEY_BRANDS_DATA_FORMAT, b.Name, b.Manufacturer));
                sbRow.Append(string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, sb.ToString()));
                sb.Clear();
            }
            sb.Append(string.Format(Constants.APPLICATION_KEY_DOWNLOAD_WRAPPER, Header + sbRow.ToString()));
            sbRow.Clear();
            return sb.ToString();
        }
        public static string SKUsData(List<SKUs> SKUs)
        {
            StringBuilder sb = new StringBuilder(), sbRow = new StringBuilder();

            string Header = string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, Constants.APPLICATION_KEY_SKUS_HEADER_FORMAT);
            foreach (SKUs s in SKUs)
            {
                sb.Append(string.Format(Constants.APPLICATION_KEY_SKUS_DATA_FORMAT, s.Brand,s.SKU,s.Price));
                sbRow.Append(string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, sb.ToString()));
                sb.Clear();
            }
            sb.Append(string.Format(Constants.APPLICATION_KEY_DOWNLOAD_WRAPPER, Header + sbRow.ToString()));
            sbRow.Clear();
            return sb.ToString();
        }
        public static string UsersData(List<Users> Us)
        {
            StringBuilder sb = new StringBuilder(), sbRow = new StringBuilder();

            string Header = string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, Constants.APPLICATION_KEY_USERS_HEADER_FORMAT);
            foreach (Users u in Us)
            {
                sb.Append(string.Format(Constants.APPLICATION_KEY_USERS_DATA_FORMAT, u.Name, u.UserName,u.Email,u.ContactNo,u.IsAdmin));
                sbRow.Append(string.Format(Constants.APPLICATION_KEY_ROW_WRAPPER, sb.ToString()));
                sb.Clear();
            }
            sb.Append(string.Format(Constants.APPLICATION_KEY_DOWNLOAD_WRAPPER, Header + sbRow.ToString()));
            sbRow.Clear();
            return sb.ToString();
        }
        public static void DownloadExcel(string FileName, string Data)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=\"" + FileName + "\"");
            //HttpContext.Current.Response.ContentType = "application/ms-excel";
            //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            sw.Write(Data);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        public static void DownloadExcel2(string FileName, string Data)
        {

        }
        public static void WriteToExcel(string flPath)
        {
            Excel.Application oXL = null;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            oXL = new Excel.Application();
            oWB = oXL.Workbooks.Add(Missing.Value);

            // Get the Active sheet 
            oSheet = (Excel.Worksheet)oWB.ActiveSheet;
            oSheet.Name = "Data";
            oSheet.Cells[1, 1] = "Brand";
            oSheet.Cells[1, 2] = "SKU";

            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.Buffer = true;
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=\"Test.xlsx\"");
            //HttpContext.Current.Response.ContentType = "application/ms-excel";
            //HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //HttpContext.Current.Response.ContentType = "application/octet-stream";
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //HttpContext.Current.Response.Charset = "";
            oSheet = null;

            oWB.SaveAs(flPath, Excel.XlFileFormat.xlWorkbookDefault,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Excel.XlSaveAsAccessMode.xlExclusive,
                Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value);
            oWB.Close(Missing.Value, Missing.Value, Missing.Value);
            oWB = null;
            oXL.Quit();

            //context.HttpContext.Response.AddHeader("content-disposition","attachment; filename=\"Test.xlsx\"");
            //HttpContext.Current.Response.TransmitFile(HttpContext.Current.Server.MapPath("/Content/Test.xlsx"));
        }

        
        //public static DataTable GetDataTableFromExcel(string path)
        //{
        //    DataTable  dtResult = new DataTable();
        //    string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
        //    // if you don't want to show the header row (first row) in the grid
        //    // use 'HDR=NO' in the string

        //    string strSQL = "SELECT * FROM [Data$]";
        //    OleDbConnection excelConnection = new OleDbConnection(connectionString);
        //    excelConnection.Open(); // this will open an Excel file
        //    OleDbCommand dbCommand = new OleDbCommand(strSQL, excelConnection);
        //    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(dbCommand);


        //    dataAdapter.Fill(dtResult);

        //    //dtLuxoScrapped.Columns.Add("GooglePatern");

        //    // bind the datasource
        //    dataAdapter.Dispose();
        //    dbCommand.Dispose();
        //    excelConnection.Close();
        //    excelConnection.Dispose();
        //   // Result = CreateLuxotticaAmazonInventoryLoader(savePath);
        //    return dtResult;
        //}

        public static DataTable GetDataTableFromCSV(string FileName, bool IsHeader)
        {
            bool Header = IsHeader;
            DataTable dt = new DataTable();
            //string[] SplitValue = { "\r\n" };
            System.IO.StreamReader sr = new System.IO.StreamReader(FileName);
            //System.Collections.Generic.List<string> TotalLines = new System.Collections.Generic.List<string>();
            int Columns = 0;
            string Result = "";
            DataRow dtr = null;
            char c = (char)sr.Read();

            try
            {
                while (sr.Peek() != -1)
                {
                    switch (c)
                    {
                        case ',':
                            if (Header)
                            {
                                dt.Columns.Add(Result.Trim());
                                //Header = false;
                            }
                            else
                            {
                                dtr[Columns] = Result.Trim();
                                Columns += 1;
                            }
                            Result = "";
                            c = (char)sr.Read();
                            break;
                        case '\"':
                            //Result+=c;
                            char q = c;
                            c = (char)sr.Read();
                            bool IsInQuates = true;
                            while (IsInQuates && sr.Peek() != -1)
                            {
                                //Result += c;
                                if (c == q)
                                {
                                    //Result += c;
                                    c = (char)sr.Read();
                                    if (c != q)
                                    {
                                        //Result += c;
                                        IsInQuates = false;
                                    }
                                }

                                if (IsInQuates)
                                {
                                    Result += c;
                                    c = (char)sr.Read();
                                }
                            }
                            break;
                        case '\n':
                            Result += c;
                            c = (char)sr.Read();

                            if (Header)
                            {
                                dt.Columns.Add(Result.Trim('\n').Trim('\r').Trim());
                                Header = false;
                            }
                            else
                            {
                                dtr[Columns] = Result.Trim();
                                Columns = 0;
                                dt.Rows.Add(dtr);
                            }

                            dtr = dt.NewRow();
                            Result = "";
                            break;
                        //case '\r':
                        //    TotalLines.Add(Result);
                        //    break;
                        default:
                            Result += c;
                            c = (char)sr.Read();
                            if (sr.Peek() == -1)
                            {
                                Result += c;
                                if (Header)
                                {
                                    dt.Columns.Add(Result.Trim());

                                    Header = false;
                                }
                                else
                                {
                                    dtr[Columns] = Result.Trim();
                                    //Columns += 1;
                                }
                                dt.Rows.Add(dtr);
                                Result = "";
                            }
                            break;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                sr.Close();
            }
            return dt;
        }
    }
}