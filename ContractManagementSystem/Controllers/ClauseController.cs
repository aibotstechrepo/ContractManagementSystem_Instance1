using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using ContractManagementSystem.Models;
using NLog;
using System.Diagnostics;
using System.IO;
using System.Web.Configuration;

namespace ContractManagementSystem.Controllers
{
    //[Authorize(Roles = "admin")]
    public class ClauseController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue // Use this value to set your maximum size for all of your Requests
            };
        }
        // GET: Clause
        [Authorize(Roles = "admin,legal")]
        public ActionResult New()
        {
            Logger.Info("Accessing Clause New Page");
            return View();
        }


        //*********************Integrated 13/3/20(Pooja)******************//


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(tblClauseMaster emp,string editor1, string Enable, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt ClauseMaster New");
            string status = "";
            try
            {
                Logger.Info("Accessing DB for Saving the ClauseMaster Records");
                //tblClauseMaster emp = new tblClauseMaster();

                emp.ClauseClauseTitle = HttpUtility.HtmlEncode(emp.ClauseClauseTitle);
                emp.ClauseClauseDescription = HttpUtility.HtmlEncode(emp.ClauseClauseDescription);
                if (!String.IsNullOrWhiteSpace(Enable))
                {
                    emp.Enable = "Active";
                }
                else
                {
                    emp.Enable = "InActive";
                }
                //emp.ClauseClauseTitle = model.ClauseClauseTitle;
                //emp.ClauseClauseDescription = model.ClauseClauseDescription;
                //emp.ClauseClauseType = model.ClauseClauseType;

                emp.ClauseClauseText = editor1;
                emp.ClauseLastModified = DateTime.Now;


                db.tblClauseMasters.Add(emp);
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Record Saved");

                Logger.Info("Accessing DB for Saving the ClauseMaster Log Details");
                tblClauseLog log = new tblClauseLog
                {
                    LogClauseUID = emp.ClauseClauseID,
                    ModifiedBy = CurrentUser.ToString() +" - "+ CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblClauseLogs.Add(log);
               
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Log Details Saved");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'New' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                status = Ex.InnerException.Message;
                // MessageBox.Show(ex.ToString());
            }

            //TempData["Success"] = true;
            return View(emp);

        }
        //*******************************************************

        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Clause/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            Logger.Info("Accessing DB for ClauseMaster Details");
            tblClauseMaster tblClauseMaster = db.tblClauseMasters.Find(id);

            Logger.Info("Accessed DB, Checking ClauseMaster Details: Checking Status");
            if (tblClauseMaster == null)
            {
                Logger.Info("Accessed DB, Checking ClauseMaster Details: Details Not Found");
                return HttpNotFound();
            }

            Logger.Info("Accessed DB, Checking ClauseMaster Details: Details Found");

            Logger.Info("Redirecting to ClauseMaster Details Page");
            return View(tblClauseMaster);
        }

        [Authorize(Roles = "admin, legal")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClauseEdit(tblClauseMaster model, int ClauseClauseID, string editor1, string Enable)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            Logger.Info("Attempt ClauseMaster ClauseEdit");
            try
            {
                Logger.Info("Accessing DB for Updating the ClauseMaster Records");
                tblClauseMaster tblClauseMaster = db.tblClauseMasters.Find(ClauseClauseID);
                string OldValues = "";
                string NewValues = "";
                if (tblClauseMaster.ClauseClauseTitle != model.ClauseClauseTitle)
                {
                    OldValues = OldValues + "Clause Title : " + tblClauseMaster.ClauseClauseTitle + " , ";
                    NewValues = NewValues + "Clause Title : " + model.ClauseClauseTitle + " , ";
                }
                if (tblClauseMaster.ClauseClauseDescription != model.ClauseClauseDescription)
                {
                    OldValues = OldValues + "Clause Description : " + tblClauseMaster.ClauseClauseDescription + " , ";
                    NewValues = NewValues + "Clause Description : " + model.ClauseClauseDescription + " , ";
                }
               
                if (tblClauseMaster.ClauseClauseText != editor1)
                {
                    OldValues = OldValues + "Clause Text : " + tblClauseMaster.ClauseClauseText + " , ";
                    NewValues = NewValues + "Clause Text : " + editor1 + " , ";
                }
                if (!String.IsNullOrWhiteSpace(Enable))
                {
                    if (tblClauseMaster.Enable == "InActive")
                    {
                        OldValues = OldValues + "Enable : " + tblClauseMaster.Enable + " , ";
                        NewValues = NewValues + "Enable : " + "Active" + " , ";
                    }
                    tblClauseMaster.Enable = "Active";
                }
                else
                {
                    if (tblClauseMaster.Enable == "Active")
                    {
                        OldValues = OldValues + "Enable : " + tblClauseMaster.Enable + " , ";
                        NewValues = NewValues + "Enable : " + "InActive" + " , ";
                    }
                    tblClauseMaster.Enable = "InActive";
                }

                tblClauseMaster.ClauseClauseID = ClauseClauseID;

                tblClauseMaster.ClauseClauseTitle = HttpUtility.HtmlEncode(model.ClauseClauseTitle);
                tblClauseMaster.ClauseClauseDescription = HttpUtility.HtmlEncode(model.ClauseClauseDescription);
                //tblClauseMaster.ClauseClauseTitle = tblClauseMaster.ClauseClauseTitle;
                //tblClauseMaster.ClauseClauseDescription = tblClauseMaster.ClauseClauseDescription;
                //tblClauseMaster.ClauseClauseType = ClauseClauseType;

                tblClauseMaster.ClauseClauseText = editor1;
                tblClauseMaster.ClauseLastModified = DateTime.Now;

                Logger.Info("Checking for ClauseMaster ModelState Validation");
                if (ModelState.IsValid)
                {
                   
                    db.Entry(tblClauseMaster).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        Logger.Info("Accessing DB for Saving the ClauseLog Details ");
                        tblClauseLog log = new tblClauseLog();
                        log.LogClauseUID = tblClauseMaster.ClauseClauseID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();
                        db.tblClauseLogs.Add(log);
                    }
                    db.SaveChanges();
                    Logger.Info("Accessed DB, ClauseMaster Updated Record Saved");

                    Logger.Info("Redirecting to ClauseMaster Repository Page");
                    TempData["status"] = "Success";
                    return RedirectToAction("Repository");
                }
               
                
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'ClauseEdit' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //status = Ex.InnerException.Message;
            }
            return View();
        }

        [Authorize(Roles = "admin, legal")]
        // [HttpPost, ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed( int ClauseClauseID,int UserID)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt ClauseMaster DeleteConfirmed");
            try
            {
                Logger.Info("Accessing DB for Deleting the ClauseMaster Records");
                tblClauseMaster tblClauseMaster = db.tblClauseMasters.Find(ClauseClauseID);
                db.tblClauseMasters.Remove(tblClauseMaster);
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Record Deleted");

                Logger.Info("Accessing DB for Saving the ClauseMaster Log Details");
                tblClauseLog log = new tblClauseLog
                {
                    LogClauseUID = ClauseClauseID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deletion",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblClauseLogs.Add(log);
               
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Log Record Saved");


            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'DeleteConfirmed' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            TempData["deletestatus"] = "DeleteSuccess";
            Logger.Info("Redirecting to ClauseMaster Repository Page");
            return RedirectToAction("Repository");

        }
        public ActionResult Repository()
        {
            if (TempData["status"] != null)
            {
                ViewBag.Status = "Success";
                TempData.Remove("status");
            }
            if (TempData["deletestatus"] != null)
            {
                ViewBag.Status = "DeleteSuccess";
                TempData.Remove("deletestatus");
            }
            Logger.Info("Accessing ClauseMaster Repository Page");
            Logger.Info("Accessing DB for Repository");

            IQueryable<ClauseRepositoryModel> ClauseList = null;

            ClauseList = (from x in db.tblClauseMasters
                          select new ClauseRepositoryModel()
                          {
                              ClauseClauseID = x.ClauseClauseID,
                              ClauseClauseTitle = x.ClauseClauseTitle,
                              ClauseLastModified = x.ClauseLastModified
                          }).AsQueryable();


            return View(ClauseList);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Repository");
        }

        [HttpPost]
        public JsonResult category_list()
        {
            Logger.Info("Attempt ClauseMaster category_list");
            try
            {

                Logger.Info("Accessing DB for Category List");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category List: Category List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'category_list' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult subcategory_list(string category_id)
        {
            Logger.Info("Attempt ClauseMaster subcategory_list");
            try
            {
                Logger.Info("Accessing DB for SubCategory List");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == category_id) select tblSubCategory.SubCategoryName;
                foreach (var r in result)
                {
                    Console.WriteLine(r);
                }
                Logger.Info("Accessed DB, Checking SubCategory List: SubCategory List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'subcategory_list' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt ClauseMaster SaveLog");
            try
            {
                Logger.Info("Accessing DB for Saving the ClauseMaster Log Details");
                tblClauseLog log = new tblClauseLog
                {
                    LogClauseUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };
                db.tblClauseLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, ClauseMaster Log Record Saved");


                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'SaveLog' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt ClauseMaster getLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking ClauseMaster Log Details: LogID match");
                var result = from tblClauseLog in db.tblClauseLogs.Where(x => x.LogClauseUID == ID) select tblClauseLog;
                Logger.Info("Accessed DB, Checking ClauseMaster Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'getLogDetail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClonningClause(int ClauseID)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Clause ClonningCluase");
            try
            {
                Logger.Info("Accessing DB for Updating the Clause Cloning Details: Checking Status");
                tblClauseMaster OriginalClause = db.tblClauseMasters.Find(ClauseID);

                tblClauseMaster ModifiedClause = new tblClauseMaster();


                ModifiedClause.ClauseClauseTitle = OriginalClause.ClauseClauseTitle;
                ModifiedClause.ClauseClauseDescription = OriginalClause.ClauseClauseDescription;
                ModifiedClause.ClauseClauseText = OriginalClause.ClauseClauseText;
                ModifiedClause.ClauseLastModified = DateTime.Now;
                ModifiedClause.ClauseModificationType = "Cloning";
                ModifiedClause.OriginalClauseID = OriginalClause.ClauseClauseID;

                db.tblClauseMasters.Add(ModifiedClause);


                db.SaveChanges();
                
                tblClauseLog log = new tblClauseLog();
                log.LogClauseUID = ModifiedClause.ClauseClauseID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Clause Cloning";
                log.ChangedFrom = "Clause cloned from " + OriginalClause.ClauseClauseID;
                log.ChangedTo = "To " + ModifiedClause.ClauseClauseID;
                log.DateandTime = DateTime.Now.ToString();
                db.tblClauseLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, Clause Log Record Saved");


                string[] response = new string[2];
                response[0] = "sucess";
                response[1] = "" + ModifiedClause.ClauseClauseID;

                return Json(response);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Clause' Controller , 'ClonningCluase' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //status = Ex.InnerException.Message;
                return Json("error");
            }
        }

        //----------upload Word Document-------//

        [HttpPost]
        public JsonResult UploadWordDocument(HttpPostedFileBase UploadWordDocument)
        {
            try
            {



                if (UploadWordDocument != null)
                {
                    string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"];
                    string Folder = DateTime.Now.ToString("yyyyddM-HHmmss");
                    path = Path.Combine(path, Folder);
                    path = path.Replace(" ", "");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string FilePath = Path.Combine(path, Path.GetFileName(UploadWordDocument.FileName));
                    FilePath = FilePath.Replace(" ", "");
                    UploadWordDocument.SaveAs(FilePath);

                    string HTMLContent = WordHandler.ConvertToHtml(FilePath, path);
                    if (!string.IsNullOrWhiteSpace(HTMLContent))
                    {
                        return Json(HTMLContent);
                    }
                }
            }
            catch (Exception Ex)
            {

                Logger.Error("Ex.Message: " + Ex.Message + "StackTrace : " + Ex.StackTrace);
            }
            return Json("");

        }

        string DOCtoHTML(string FilePath)
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"];
            string EXEfile = Path.Combine(path, "Converter.exe");
            ProcessStartInfo info = new ProcessStartInfo(EXEfile);
            info.Arguments = @"HTML " + FilePath;
            info.UseShellExecute = false;

            info.RedirectStandardInput = true;

            info.RedirectStandardError = true;

            info.RedirectStandardOutput = true;
            //info.WorkingDirectory = @"D:\Temp\WCFTEST";
            //info.UserName = @".\Elitebook1"; // see the link mentioned at the top

            //info.Password = new NetworkCredential("", "").SecurePassword;

            using (Process install = Process.Start(info))

            {

                string output = install.StandardOutput.ReadToEnd();

                install.WaitForExit();

                // Do something with you output data

                Console.WriteLine(output);

            }
            string fileContents = "";
            try
            {
                string OuputFile = Path.GetDirectoryName(FilePath);
                OuputFile = Path.Combine(OuputFile, Path.GetFileNameWithoutExtension(FilePath) + ".html");
                fileContents = System.IO.File.ReadAllText(OuputFile);
            }
            catch
            {

            }
            return fileContents;
        }


    }
}