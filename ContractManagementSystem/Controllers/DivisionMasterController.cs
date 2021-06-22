using ContractManagementSystem.Models;
using NLog;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContractManagementSystem.Controllers
{
    //[Authorize(Roles = "admin")]
    public class DivisionMasterController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: Division

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

        public ActionResult Cluster()
        {
            if (TempData["newstatus"] != null)
            {
                ViewBag.Status = "NewSuccess";
                TempData.Remove("newstatus");
            }
            if (TempData["categorystatus"] != null)
            {
                ViewBag.Status = "CategorySuccess";
                TempData.Remove("categorystatus");
            }
            Logger.Info("Attempt DivisionMaster Category");
            Logger.Info("Accessing DB for Category Page");
            IQueryable<ClusterRepositoryModel> ClusterList = null;

            ClusterList = (from x in db.tblCategories
                           select new ClusterRepositoryModel()
                           {
                               CategoryID = x.CategoryID,
                               CategoryName = x.CategoryName,
                               LastModified = x.LastModified
                           }).AsQueryable();


            return View(ClusterList);
        }

        public ActionResult Index()
        {
            return RedirectToAction("Cluster");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecord1(tblCategory model, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DivisionMaster SaveRecord1");
            try
            {

                Logger.Info("Accessing DB for saving the Category Records");
                //tblCategory category = new tblCategory();
                //category.CategoryName = model.CategoryName;
                model.CategoryName = HttpUtility.HtmlEncode(model.CategoryName);

                model.LastModified = DateTime.Now;
                db.tblCategories.Add(model);
                db.SaveChanges();
                Logger.Info("Accessed DB, Category Details Saved");

                Logger.Info("Accessing DB for Saving the CategoryLog Details");

                tblCategoryLog log = new tblCategoryLog
                {
                    LogCategoryUID = model.CategoryID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, Category Log Details Saved");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SaveRecord' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            TempData["newstatus"] = "NewSuccess";
            Logger.Info("Setting Auth Cookie and Redirecting to Category");
            return RedirectToAction("Cluster");
        }
        public ActionResult Function()
        {
            if (TempData["newstatus"] != null)
            {
                ViewBag.Status = "NewSuccess";
                TempData.Remove("newstatus");
            }
            if (TempData["subcategorystatus"] != null)
            {
                ViewBag.Status = "SubCategorySuccess";
                TempData.Remove("subcategorystatus");
            }
            Logger.Info("Attempt DivisionMaster SubCategory");
            Logger.Info("Accessing DB for SubCategory Page");
            IQueryable<FunctionRepositoryModel> FunctionList = null;

            FunctionList = (from x in db.tblSubCategories
                            select new FunctionRepositoryModel()
                            {
                                SubCategoryID = x.SubCategoryID,
                                SubCategoryName = x.SubCategoryName,
                                LastModified = x.LastModified,
                                CategoryName = x.CategoryName
                            }).AsQueryable();


            return View(FunctionList);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecord3(tblSubCategory model, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DivisionMaster SaveRecord3");
            try
            {
                Logger.Info("Accessing DB for saving the SubCategory Records");
                // tblSubCategory subcategory = new tblSubCategory();

                //model.SubCategoryID = model.SubCategoryID;
                model.CategoryName = HttpUtility.HtmlEncode(model.CategoryName);
                model.SubCategoryName = HttpUtility.HtmlEncode(model.SubCategoryName);
                //subcategory.CategoryName = model.CategoryName;
                //subcategory.SubCategoryName = model.SubCategoryName;
                model.LastModified = DateTime.Now;

                db.tblSubCategories.Add(model);

                db.SaveChanges();
                Logger.Info("Accessed DB, SubCategory Details Saved");

                Logger.Info("Accessing DB for Saving the SubCategoryLog Details");
                tblSubCategoryLog log = new tblSubCategoryLog
                {
                    LogSubCategoryUID = model.SubCategoryID,
                    ModifiedBy = CurrentUser.ToString()+ " - " +CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblSubCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, SubCategoryLog Details Saved");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SaveRecord3' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            TempData["newstatus"] = "NewSuccess";
            return RedirectToAction("Function");
        }


        [HttpPost]
        public JsonResult CategoryList()
        {
            Logger.Info("Attempt DivisionMaster CategoryList");
            try
            {
                Logger.Info("Accessing DB for Category List: Checking Status ");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accesed DB, Checked Category List : Category List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'CategoryList' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult EditCategory(string CategoryID)
        {
            Logger.Info("Attempt DivisionMaster EditCategory");
            try
            {

                int IDName = Convert.ToInt32(CategoryID);
                Logger.Info("Accessing DB for Updating the Category List");
                var result = from tblCategory in db.tblCategories.Where(x => x.CategoryID == IDName) select tblCategory;
                foreach (var r in result)
                {
                    Logger.Info("Accesed DB, Category Updating List Saved");
                    return Json(r);
                }
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'EditCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecordCategory(tblCategory model)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            Logger.Info("Attempt DivisionMaster SaveRecordCategory");

            try
            {
                Logger.Info("Accessing DB for saving Category Records");

                tblCategory Category = db.tblCategories.Find(model.CategoryID);
                
                    string OldValues = "";
                    string NewValues = "";
                    if (Category.CategoryName != model.CategoryName)
                    {
                        OldValues = OldValues + "Cluster Name : " + Category.CategoryName + " , ";
                        NewValues = NewValues + "Cluster Name : " + model.CategoryName + " , ";
                    }

                //Category.CategoryName = model.CategoryName;
                Category.CategoryName = HttpUtility.HtmlEncode(model.CategoryName);
                //CategoryID = model.CategoryID,
                Category.LastModified = DateTime.Now;
                
                Logger.Info("Accessed DB: Checking ModelState Validation");
                if (ModelState.IsValid)
                {
                    db.Entry(Category).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        Logger.Info("Accessing DB for Saving the CategoryLog Details ");
                        tblCategoryLog log = new tblCategoryLog();
                        log.LogCategoryUID = Category.CategoryID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();
                        db.tblCategoryLogs.Add(log);
                    }
                    db.SaveChanges();
                    Logger.Info("Accesed DB, Category Details Saved");
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SaveRecord' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            TempData["categorystatus"] = "CategorySuccess";
            Logger.Info("Redirecting to Category");
            return RedirectToAction("Cluster");
        }
       
        [HttpPost]
        public JsonResult EditSubCategory(string SubCategoryID)
        {
            Logger.Info("Attempt DivisionMaster EditCategory");
            try
            {
                int IDName = Convert.ToInt32(SubCategoryID);
                Logger.Info("Accessing DB for Udating the SubCategory Record ");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.SubCategoryID == IDName) select tblSubCategory;

                foreach (var r in result)
                {

                    return Json(r);
                }
                Logger.Info("Accessed DB, SubCategory Record Updated");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'EditSubCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult SaveRecordSubCategory(tblSubCategory model)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }

            Logger.Info("Attempt DivisionMaster SaveRecordSubCategory");
            try
            {

                Logger.Info("Accessing DB for Saving SubCategory Records");
                //tblSubCategory SubCategory = new tblSubCategory();
                //SubCategory.SubCategoryName = model.SubCategoryName;
                //SubCategory.CategoryName = model.CategoryName;
                tblSubCategory SubCategory = db.tblSubCategories.Find(model.SubCategoryID);

                string OldValues = "";
                string NewValues = "";
                if (SubCategory.CategoryName != model.CategoryName)
                {
                    OldValues = OldValues + "Cluster Name : " + SubCategory.CategoryName + " , ";
                    NewValues = NewValues + "Cluster Name : " + model.CategoryName + " , ";
                }
                if (SubCategory.SubCategoryName != model.SubCategoryName)
                {
                    OldValues = OldValues + "Function Name : " + SubCategory.SubCategoryName + " , ";
                    NewValues = NewValues + "Function Name : " + model.SubCategoryName + " , ";
                }

                SubCategory.CategoryName = HttpUtility.HtmlEncode(model.CategoryName);
                SubCategory.SubCategoryName = HttpUtility.HtmlEncode(model.SubCategoryName);

                //model.SubCategoryID = model.SubCategoryID;
                SubCategory.LastModified = DateTime.Now;
                Logger.Info("Checking model State validation");
                if (ModelState.IsValid)
                {
                    db.Entry(SubCategory).State = EntityState.Modified;
                    if (OldValues.Length > 0)
                    {
                        Logger.Info("Accessing DB for Saving the SubCategoryLog Details ");
                        tblSubCategoryLog log = new tblSubCategoryLog();
                        log.LogSubCategoryUID = SubCategory.SubCategoryID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();
                        db.tblSubCategoryLogs.Add(log);
                    }
                    db.SaveChanges();
                    Logger.Info("Accessed DB, SubCategory Details Saved");
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SaveRecordSubCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            TempData["subcategorystatus"] = "SubCategorySuccess";
            Logger.Info("Redirecting to SubCategory");
            return RedirectToAction("Function");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteConfirmedSubCategory(int SubCategoryID, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DivisionMaster DeleteConfirmedSubCategory");
            try
            {
                Logger.Info("Accessing DB for Deleting the SubCategory List ");
                tblSubCategory tblSubCategory = db.tblSubCategories.Find(SubCategoryID);
                db.tblSubCategories.Remove(tblSubCategory);
                db.SaveChanges();
                Logger.Info("Accesed DB, SubCategory List Deleted");

                Logger.Info("Accessing DB for Saving the SubCategory Log Details");
                tblSubCategoryLog log = new tblSubCategoryLog
                {
                    LogSubCategoryUID = SubCategoryID,
                    ModifiedBy = CurrentUser.ToString()+ " - " +CurrentUserName,
                    LogActivity = "Deletion",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblSubCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, SubCategory Log Details Saved");

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'DeleteConfirmedSubCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult DeleteConfirmedCategory(int CategoryID, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt DivisionMaster DeleteConfirmedCategory");
            try
            {
                Logger.Info("Accessing DB for Deleting the Category List ");
                tblCategory tblCategory = db.tblCategories.Find(CategoryID);
                db.tblCategories.Remove(tblCategory);

                db.SaveChanges();
                Logger.Info("Accesed DB, Category Details Deleted");

                Logger.Info("Accessing DB for Saving Category Log Details");
                tblCategoryLog log = new tblCategoryLog
                {
                    LogCategoryUID = CategoryID,
                    ModifiedBy = CurrentUser.ToString()+" - "+CurrentUserName,
                    LogActivity = "Deletion",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, Category Log Details Saved");

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'DeleteConfirmedCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
            }

        }

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt DivisionMaster getLogDetail");
            try
            {
                Logger.Info("Accessing DB for CategoryLog Details : CategoryLogID Match");
                var result = from tblCategoryLog in db.tblCategoryLogs.Where(x => x.LogCategoryUID == ID) select tblCategoryLog;
                Logger.Info("Accessed DB, Checking CategoryLog Deatils : CategoryLog Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'getLogDetail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

       
        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {

            Logger.Info("Attempt DivisionMaster SaveLog");
            try
            {
                Logger.Info("Accessing DB for saving CategoryLog Details");
                tblCategoryLog log = new tblCategoryLog
                {
                    LogCategoryUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, CategoryLog Details Saved");

                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SaveLog' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public ActionResult getLog2Detail(int ID)
        {
            Logger.Info("Attempt DivisionMaster getLog2Detail");
            try
            {
                Logger.Info("Accessing DB for SubCategoryLog Details: SubCategoryLogID Match");
                var result = from tblSubCategoryLog in db.tblSubCategoryLogs.Where(x => x.LogSubCategoryUID == ID) select tblSubCategoryLog;
                Logger.Info("Accessed DB, Checked SubCategoryLog : SubCategoryLog Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'getLog2Detail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SaveLog2(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt DivisionMaster SaveLog2");
            try
            {
                Logger.Info("Accessing DB for saving SubCategoryLog Details");
                tblSubCategoryLog log = new tblSubCategoryLog
                {
                    LogSubCategoryUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblSubCategoryLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, SubCategory Log Details Saved");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SaveLog2' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CategoryNameVerification(string Category)
        {
            Logger.Info("Attempt DivisionMaster CategoryNameVerification");
            Category = Category.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking Category Details: CategoryName match");
                var result = from tblCategory in db.tblCategories.Where(x => x.CategoryName == Category) select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category Details: CategoryName Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'CategoryNameVerification' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult CategoryNameVerification2(string Category,int ID)
        {
            Logger.Info("Attempt DivisionMaster CategoryNameVerification");
            Category = Category.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking Category Details: CategoryName match");
                var result = from tblCategory in db.tblCategories.Where(x => x.CategoryID != ID).Where(x => x.CategoryName == Category) select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category Details: CategoryName Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'CategoryNameVerification' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SubCategoryNameVerification(string Category, string SubCategory)
        {
            Logger.Info("Attempt DivisionMaster CategoryNameVerification");
            Category = Category.Trim();
            SubCategory = SubCategory.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking SubCategory Details: SubCategory match");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == Category).Where(x => x.SubCategoryName == SubCategory) select tblSubCategory;
                string[] name = new string[2];
                foreach (var r in result)
                {
                    name[0] = r.CategoryName;
                    name[1] = r.SubCategoryName;
                }
                Logger.Info("Accessed DB, Checking SubCategory Details: SubCategory Found");
                return Json(name);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SubCategoryNameVerification' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SubCategoryNameVerification2(string Category, string SubCategory, int ID)
        {
            Logger.Info("Attempt DivisionMaster CategoryNameVerification");
            Category = Category.Trim();
            SubCategory = SubCategory.Trim();
            try
            {
                Logger.Info("Accessed DB, Checking SubCategory Details: SubCategory match");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.SubCategoryID != ID).Where(x => x.CategoryName == Category).Where(x => x.SubCategoryName == SubCategory) select tblSubCategory;
                string[] name = new string[2];
                foreach (var r in result)
                {
                    name[0] = r.CategoryName;
                    name[1] = r.SubCategoryName;
                }
                Logger.Info("Accessed DB, Checking SubCategory Details: SubCategory Found");
                return Json(name);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'DivisionMaster' Controller , 'SubCategoryNameVerification' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }
    }
}


