using ContractManagementSystem.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace ContractManagementSystem.Controllers
{
    //[Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: Users
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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult New()
        {
            if (TempData["newstatus"] != null)
            {
                ViewBag.Status = "NewSuccess";
                TempData.Remove("newstatus");
            }
            //if (Request.IsAjaxRequest())
            //    return View(); // Return view with no master.
            //else
            //    return View("New", "_HomeLayout"); // Return view with master.
            Logger.Info("Accessing User New Page");
            return View();
        }

        //*****************************Integrated (Pooja) on 14/3/20***********************
        [HttpPost]
        
        public ActionResult New(tblUserMaster user, string UserRoleApprover, string UserRoleInitiator, string UserRoleLegal, string UserRoleFinance, string UserRoleAdmin, string UserRoleReviewer, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt User New");
            try
            {
                string EmployeeID = user.UserEmployeeID.ToString();
                string HODEmployeeID = user.UserHodEmployeeID.ToString();
                EmployeeID = HttpUtility.HtmlEncode(user.UserEmployeeID);
                user.UserEmployeeName = HttpUtility.HtmlEncode(user.UserEmployeeName);
                user.UserEmployeeEmail = HttpUtility.HtmlEncode(user.UserEmployeeEmail);
                user.UserEmployeeDesignation = HttpUtility.HtmlEncode(user.UserEmployeeDesignation);
                user.UserCategory = HttpUtility.HtmlEncode(user.UserCategory);
                user.UserSubCategory = HttpUtility.HtmlEncode(user.UserSubCategory);
                user.UserStatus = HttpUtility.HtmlEncode(user.UserStatus);
                HODEmployeeID = HttpUtility.HtmlEncode(user.UserHodEmployeeID);
                user.UserHodEmployeeName = HttpUtility.HtmlEncode(user.UserHodEmployeeName);
                user.UserHodEmployeeEmailAddress = HttpUtility.HtmlEncode(user.UserHodEmployeeEmailAddress);
                UserRoleApprover = HttpUtility.HtmlEncode(UserRoleApprover);
                UserRoleInitiator = HttpUtility.HtmlEncode(UserRoleInitiator);
                UserRoleLegal = HttpUtility.HtmlEncode(UserRoleLegal);
                UserRoleFinance = HttpUtility.HtmlEncode(UserRoleFinance);
                UserRoleAdmin = HttpUtility.HtmlEncode(UserRoleAdmin);
                UserRoleReviewer = HttpUtility.HtmlEncode(UserRoleReviewer);


                Logger.Info("Accessing DB for Saving the User Records");
                //tblUserMaster user = new tblUserMaster();
                if (!String.IsNullOrWhiteSpace(UserRoleApprover))
                {
                    user.UserRoleApprover = true;
                }
                else
                {
                    user.UserRoleApprover = false;
                }
                if (!String.IsNullOrWhiteSpace(UserRoleInitiator))
                {
                    user.UserRoleInitiator = true;
                }
                else
                {
                    user.UserRoleInitiator = false;
                }
                if (!String.IsNullOrWhiteSpace(UserRoleLegal))
                {
                    user.UserRoleLegal = true;
                }
                else
                {
                    user.UserRoleLegal = false;
                }
                if (!String.IsNullOrWhiteSpace(UserRoleFinance))
                {
                    user.UserRoleFinance = true;
                }
                else
                {
                   
                    user.UserRoleFinance = false;
                }
                if (!String.IsNullOrWhiteSpace(UserRoleAdmin))
                {
                    user.UserRoleAdmin = true;
                }
                else
                {
                    user.UserRoleAdmin = false;
                }
                if (!String.IsNullOrWhiteSpace(UserRoleReviewer))
                {
                    user.UserRoleReviewer = true;
                }
                else
                {
                    user.UserRoleReviewer = false;
                }


                //user.UserEmployeeID = model.UserEmployeeID;
                //user.UserEmployeeName = model.UserEmployeeName;
                //user.UserEmployeeEmail = model.UserEmployeeEmail;
                //user.UserEmployeeDesignation = model.UserEmployeeDesignation;
                //user.UserCategory = model.UserCategory;
                //user.UserSubCategory = model.UserSubCategory;
                //user.UserStatus = model.UserStatus;
                //user.UserHodEmployeeID = model.UserHodEmployeeID;
                //user.UserHodEmployeeName = model.UserHodEmployeeName;
                //user.UserHodEmployeeEmailAddress = model.UserHodEmployeeEmailAddress;


                db.tblUserMasters.Add(user);
                
                db.SaveChanges();
                Logger.Info("Accessed DB, User Record Saved");

                Logger.Info("Accessing DB for Saving the User Log Details");
                tblUserLog log = new tblUserLog
                {
                    LogUserUID = user.UserID,
                    ModifiedBy = CurrentUser.ToString()+ " - "+CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };


                //tblLog log = new tblLog();
                //log.LogUserID = UserEmployeeID;
                ////log.DateandTime = DateTime.Now;
                db.tblUserLogs.Add(log);
            
                db.SaveChanges();
                Logger.Info("Accessed DB, User Log Details Saved");

                TempData["newstatus"] = "NewSuccess";
                return RedirectToAction("New");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'New' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                // status = Ex.InnerException.Message;
                // MessageBox.Show(ex.ToString());
            }

            return View(user);

        }
        //****************************************************
        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Users/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            Logger.Info("Accessing DB for User Details");
            tblUserMaster tblUserMaster = db.tblUserMasters.Find(id);
            Logger.Info("Accessed DB, Checking User Details: Checking Status");
            if (tblUserMaster == null)
            {
                Logger.Info("Accessed DB, Checking User Details: Details Not Found");
                return HttpNotFound();
            }
            Logger.Info("Accessed DB, Checking User Details: Details Found");

            
            Logger.Info("Redirecting to User Details Page");
            return View(tblUserMaster);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UserEdit(tblUserMaster model, string UserRoleApprover, string UserRoleInitiator, string UserRoleLegal, string UserRoleFinance, string UserRoleAdmin,string UserRoleReviewer, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt User UserEdit");
            try
            {
                Logger.Info("Accessing DB for Updating the User Records");
                tblUserMaster user = db.tblUserMasters.Find(UserID);
                //user.UserID = UserID;
                string OldValues = "";
                string NewValues = "";
                if (user.UserEmployeeID != model.UserEmployeeID)
                {
                    OldValues = OldValues + "Employee ID : " + user.UserEmployeeID + " , ";
                    NewValues = NewValues + "Employee ID : " + model.UserEmployeeID + " , ";
                }
                if (user.UserEmployeeName != model.UserEmployeeName)
                {
                    OldValues = OldValues + "Employee Name : " + user.UserEmployeeName + " , ";
                    NewValues = NewValues + "Employee Name : " + model.UserEmployeeName + " , ";
                }
                if (user.UserHodEmployeeEmailAddress != model.UserHodEmployeeEmailAddress)
                {
                    OldValues = OldValues + "Email Address : " + user.UserHodEmployeeEmailAddress + " , ";
                    NewValues = NewValues + "Email Address : " + model.UserHodEmployeeEmailAddress + " , ";
                }
                if (user.UserEmployeeDesignation != model.UserEmployeeDesignation)
                {
                    OldValues = OldValues + "Designation : " + user.UserEmployeeDesignation + " , ";
                    NewValues = NewValues + "Designation : " + model.UserEmployeeDesignation + " , ";
                }
              
                if (user.UserHodEmployeeID.ToString() != model.UserHodEmployeeID.ToString())
                {
                    OldValues = OldValues + "Reporting Manager ID : " + user.UserHodEmployeeID.ToString() + " , ";
                    NewValues = NewValues + "Reporting Manager ID : " + model.UserHodEmployeeID + " , ";
                }
                if (user.UserHodEmployeeName != model.UserHodEmployeeName)
                {
                    OldValues = OldValues + "Reporting Manager Name : " + user.UserHodEmployeeName.ToString() + " , ";
                    NewValues = NewValues + "Reporting Manager Name : " + model.UserHodEmployeeName + " , ";
                }
                if (user.UserHodEmployeeEmailAddress != model.UserHodEmployeeEmailAddress)
                {
                    OldValues = OldValues + "Reporting Manager Email : " + user.UserHodEmployeeEmailAddress + " , ";
                    NewValues = NewValues + "Reporting Manager Email : " + model.UserHodEmployeeEmailAddress + " , ";
                }
                if (user.UserCategory != model.UserCategory)
                {
                    OldValues = OldValues + "Cluster : " + user.UserCategory + " , ";
                    NewValues = NewValues + "Cluster : " + model.UserCategory + " , ";
                }
                if (user.UserSubCategory != model.UserSubCategory)
                {
                    OldValues = OldValues + "Function : " + user.UserSubCategory + " , ";
                    NewValues = NewValues + "Function : " + model.UserSubCategory + " , ";
                }
                if (user.UserStatus != model.UserStatus)
                {
                    OldValues = OldValues + "Status : " + user.UserStatus + " , ";
                    NewValues = NewValues + "Status : " + model.UserStatus + " , ";
                }

              
                //string EmployeeID = user.UserEmployeeID.ToString();
                //string HODEmployeeID = user.UserHodEmployeeID.ToString();
                user.UserEmployeeID = model.UserEmployeeID;
                user.UserEmployeeName = HttpUtility.HtmlEncode(model.UserEmployeeName);
                user.UserEmployeeEmail = HttpUtility.HtmlEncode(model.UserEmployeeEmail);
                user.UserEmployeeDesignation = HttpUtility.HtmlEncode(model.UserEmployeeDesignation);
                user.UserCategory = HttpUtility.HtmlEncode(model.UserCategory);
                user.UserSubCategory = HttpUtility.HtmlEncode(model.UserSubCategory);
                user.UserStatus = HttpUtility.HtmlEncode(model.UserStatus);
                user.UserHodEmployeeID = model.UserHodEmployeeID;
                user.UserHodEmployeeName = HttpUtility.HtmlEncode(model.UserHodEmployeeName);
                user.UserHodEmployeeEmailAddress = HttpUtility.HtmlEncode(model.UserHodEmployeeEmailAddress);
                UserRoleApprover = HttpUtility.HtmlEncode(UserRoleApprover);
                UserRoleInitiator = HttpUtility.HtmlEncode(UserRoleInitiator);
                UserRoleLegal = HttpUtility.HtmlEncode(UserRoleLegal);
                UserRoleFinance = HttpUtility.HtmlEncode(UserRoleFinance);
                UserRoleAdmin = HttpUtility.HtmlEncode(UserRoleAdmin);
                UserRoleReviewer = HttpUtility.HtmlEncode(UserRoleReviewer);

                if (!String.IsNullOrWhiteSpace(UserRoleApprover))
            {
                    if (user.UserRoleApprover == false)
                    {
                        OldValues = OldValues + "Approver : " + user.UserRoleApprover.ToString() + " , ";
                        NewValues = NewValues + "Approver : " + "true" + " , ";
                    }
                    user.UserRoleApprover = true;
            }
            else
            {
                    if (user.UserRoleApprover == true)
                    {
                        OldValues = OldValues + "Approver : " + user.UserRoleApprover.ToString() + " , ";
                        NewValues = NewValues + "Approver : " + "false" + " , ";
                    }
                    user.UserRoleApprover = false;
            }
            if (!String.IsNullOrWhiteSpace(UserRoleInitiator))
            {
                    if (user.UserRoleInitiator == false)
                    {
                        OldValues = OldValues + "Initiator : " + user.UserRoleInitiator.ToString() + " , ";
                        NewValues = NewValues + "Initiator : " + "true" + " , ";

                    }

                    user.UserRoleInitiator = true;
            }
            else
            {
                    if (user.UserRoleInitiator == true)
                    {
                        OldValues = OldValues + "Initiator : " + user.UserRoleInitiator.ToString() + " , ";
                        NewValues = NewValues + "Initiator : " + "false" + " , ";

                    }
                    user.UserRoleInitiator = false;
            }
            if (!String.IsNullOrWhiteSpace(UserRoleLegal))
            {
                    if (user.UserRoleLegal == false)
                    {
                        OldValues = OldValues + "Legal : " + user.UserRoleLegal.ToString() + " , ";
                        NewValues = NewValues + "Legal : " + "true" + " , ";
                    }
                    user.UserRoleLegal = true;
            }
            else
            {
                    if (user.UserRoleLegal == true)
                    {
                        OldValues = OldValues + "Legal : " + user.UserRoleLegal.ToString() + " , ";
                        NewValues = NewValues + "Legal : " + "false" + " , ";
                    }
                    user.UserRoleLegal = false;
            }
            if (!String.IsNullOrWhiteSpace(UserRoleFinance))
            {
                    if (user.UserRoleFinance == false)
                    {
                        OldValues = OldValues + "Finance : " + user.UserRoleFinance.ToString() + " , ";
                        NewValues = NewValues + "Finance : " + "true" + " , ";

                    }
                    user.UserRoleFinance = true;
            }
            else
            {
                    if (user.UserRoleFinance == true)
                    {
                        OldValues = OldValues + "Finance : " + user.UserRoleFinance.ToString() + " , ";
                        NewValues = NewValues + "Finance : " + "false" + " , ";
                    }
                    user.UserRoleFinance = false;
            }
            if (!String.IsNullOrWhiteSpace(UserRoleAdmin))
            {
                    if (user.UserRoleAdmin == false)
                    {
                        OldValues = OldValues + "Admin : " + user.UserRoleAdmin.ToString() + " , ";
                        NewValues = NewValues + "Admin : " + "true" + " , ";
                    }
                    user.UserRoleAdmin = true;
            }
            else
            {
                    if (user.UserRoleAdmin == true)
                    {
                        OldValues = OldValues + "Admin : " + user.UserRoleAdmin.ToString() + " , ";
                        NewValues = NewValues + "Admin : " + "false" + " , ";
                    }
                    user.UserRoleAdmin = false;
            }
                if (!String.IsNullOrWhiteSpace(UserRoleReviewer))
                {
                    if (user.UserRoleReviewer == false)
                    {
                        OldValues = OldValues + "Reviewer : " + user.UserRoleReviewer.ToString() + " , ";
                        NewValues = NewValues + "Reviewer : " + "true" + " , ";
                    }
                    user.UserRoleReviewer = true;
                }
                else
                {
                    if (user.UserRoleReviewer == true)
                    {
                        OldValues = OldValues + "Reviewer : " + user.UserRoleReviewer.ToString() + " , ";
                        NewValues = NewValues + "Reviewer : " + "false" + " , ";
                    }
                    user.UserRoleReviewer = false;
                }


                //user.UserEmployeeID = UserEmployeeID;
                //user.UserEmployeeName = UserEmployeeName;
                //user.UserEmployeeEmail = UserEmployeeEmail;

                //user.UserEmployeeDesignation = UserEmployeeDesignation;
                //user.UserCategory = UserCategory;
                //user.UserSubCategory = UserSubCategory;
                //user.UserStatus = UserStatus;

                //user.UserHodEmployeeID = UserHodEmployeeID;
                //user.UserHodEmployeeName = UserHodEmployeeName;
                //user.UserHodEmployeeEmailAddress = UserHodEmployeeEmailAddress;



                //    Logger.Info("Checking for User ModelState Validation");
                //    if (ModelState.IsValid)
                //{

                db.Entry(user).State = EntityState.Modified;

                if (OldValues.Length > 0)
                {
                    Logger.Info("Accessing DB for Saving the UserLog Details ");
                    tblUserLog log = new tblUserLog();
                    log.LogUserUID = user.UserID;
                    log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    log.LogActivity = "Modified";
                    log.ChangedFrom = OldValues;
                    log.ChangedTo = NewValues;
                    log.DateandTime = DateTime.Now.ToString();
                    db.tblUserLogs.Add(log);
                }
                    db.SaveChanges();
                    //ViewData["students"] = 1;
                    Logger.Info("Accessed DB, User Updated Record Saved");
                
                Logger.Info("Redirecting to User Repository Page");
                TempData["status"] = "Success";
                return RedirectToAction("Repository");

                //}
                // return View(user);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'UserEdit' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //status = Ex.InnerException.Message;
            }
            return RedirectToAction("Details");
        }


        [Authorize(Roles = "admin")]
        // [HttpPost, ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int UserID)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt User DeleteConfirmed");
            try
            {
                Logger.Info("Accessing DB for Deleting the User Records");
                tblUserMaster tblUserMaster = db.tblUserMasters.Find(UserID);
                db.tblUserMasters.Remove(tblUserMaster);
                db.SaveChanges();
                Logger.Info("Accessed DB, User Record Deleted");

                Logger.Info("Accessing DB for Saving the User Log Details");
                tblUserLog log = new tblUserLog
                {
                    LogUserUID = UserID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deletion",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblUserLogs.Add(log);
            
            db.SaveChanges();
                Logger.Info("Accessed DB, User Log Record Saved");
               // return RedirectToAction("Repository");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'DeleteConfirmed' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            TempData["deletestatus"] = "DeleteSuccess";
            Logger.Info("Redirecting to Users Repository Page");
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

            Logger.Info("Accessing Users Repository Page");
            Logger.Info("Accessing DB for Repository");


            IQueryable<UsersRepositoryModel> UsersList = null;
            //if (User.IsInRole("admin"))
            //{
            UsersList = (from x in db.tblUserMasters
                         select new UsersRepositoryModel()
                         {
                             UserID = x.UserID,
                             UserEmployeeID = x.UserEmployeeID,
                             UserEmployeeName = x.UserEmployeeName,
                             UserEmployeeEmail = x.UserEmployeeEmail,
                             UserCategory = x.UserCategory,
                             UserSubCategory = x.UserSubCategory,
                             UserRoleAdmin = x.UserRoleAdmin,
                             UserRoleApprover = x.UserRoleApprover,
                             UserRoleFinance = x.UserRoleFinance,
                             UserRoleInitiator = x.UserRoleInitiator,
                             UserRoleLegal = x.UserRoleLegal,
                             UserRoleReviewer = x.UserRoleReviewer,
                             UserStatus = x.UserStatus
                         }).AsQueryable();


            //}

            //List<tblUserMaster> UserMaster = db.tblUserMasters.ToList();
            //UserMaster.Reverse();
            return View(UsersList);

        }
        public ActionResult Index()
        {
            return RedirectToAction("Repository");
        }

        [HttpPost]
        public JsonResult user_category_list()
        {
            Logger.Info("Attempt Users user_category_list");
            try
            {

                Logger.Info("Accessing DB for Category List");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category List: Category List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'user_category_list' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult user_subcategory_list(string user_category_id)
        {
            Logger.Info("Attempt Users user_subcategory_list");
            try
            {
                Logger.Info("Accessing DB for SubCategory List");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == user_category_id) select tblSubCategory.SubCategoryName;
                foreach (var r in result)
                {
                    Console.WriteLine(r);
                }
                Logger.Info("Accessed DB, Checking SubCategory List: SubCategory List Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'user_subcategory_list' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getHodDetails(string EmployeeDetails, string OptionToSearch)
        {
            OptionToSearch = OptionToSearch.Trim();
            if (OptionToSearch == "Employee ID")
            {
                var result = db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail,
                    x.UserStatus
                });  /*select tblUserMaster*/
                return Json(result);
            }
            else if (OptionToSearch == "Employee Name")
            {
                var result = db.tblUserMasters.Where(x => x.UserEmployeeName.Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail,
                    x.UserStatus
                });  /*select tblUserMaster*/;
                return Json(result);
            }
            else if (OptionToSearch == "Employee Email Address")
            {
                var result = db.tblUserMasters.Where(x => x.UserEmployeeEmail.Contains(EmployeeDetails)).Select(x => new {
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserEmployeeDesignation,
                    x.UserCategory,
                    x.UserSubCategory,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserEmployeeEmail,
                    x.UserStatus
                });  /*select tblUserMaster*/;
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult getHodDetails2(string employeename)
        {
            Logger.Info("Attempt Users getHodDetails2");
            try
            {
                if (!string.IsNullOrWhiteSpace(employeename))
                {
                    Logger.Info("Accessing DB for Users Details : Employee Name match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeName.Contains(employeename)) select tblUserMaster;
                    string[] IDname = new string[7];

                    foreach (var r in result)
                    {
                        string UserRole = "";
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleAdmin == true)
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                            UserRole = UserRole + "Admin";
                        }
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleInitiator == true)
                        {

                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Initiator";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Initiator";
                            }
                        }
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleApprover == true)
                        {
                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Approver";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Approver";
                            }
                        }
                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleFinance == true)
                        {
                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Finance Approver";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Finance Approver";
                            }
                        }

                        Logger.Info("Accessed DB, Checking User Details: Checking Status");
                        if (r.UserRoleLegal == true)
                        {
                            if (UserRole.Length > 0)
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                                UserRole = UserRole + ", Legal Approver";
                            }
                            else
                            {
                                Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                                UserRole = UserRole + "Legal Approver";
                            }
                        }
                        IDname[0] = "success";
                        IDname[1] = r.UserEmployeeID.ToString();
                        IDname[2] = r.UserEmployeeEmail;
                        IDname[3] = r.UserEmployeeDesignation;
                        IDname[4] = UserRole;
                        IDname[5] = r.UserSubCategory;
                        IDname[6] = r.UserCategory;
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(IDname);
                    }
                }
            }
                //Logger.Info("Accessed DB, Checking Users Details : Details Found");
                //return Json("");
            
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'getHodDetails2' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [HttpPost]
        public JsonResult getHodDetails3(string employeeemail)
        {
            Logger.Info("Attempt Users getHodDetails2");
            try
            {
                if (!string.IsNullOrWhiteSpace(employeeemail))
                    Logger.Info("Accessing DB for Users Details : Employee Email match");
                var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeEmail.Contains(employeeemail)) select tblUserMaster;
                string[] IDEmail = new string[7];
                foreach (var r in result)
                {
                    string UserRole = "";
                    Logger.Info("Accessed DB, Checking User Details: Checking Status");
                    if (r.UserRoleAdmin == true)
                    {
                        Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                        UserRole = UserRole + "Admin";
                    }
                    Logger.Info("Accessed DB, Checking User Details: Checking Status");
                    if (r.UserRoleInitiator == true)
                    {

                        if (UserRole.Length > 0)
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                            UserRole = UserRole + ", Initiator";
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                            UserRole = UserRole + "Initiator";
                        }
                    }
                    Logger.Info("Accessed DB, Checking User Details: Checking Status");
                    if (r.UserRoleApprover == true)
                    {
                        if (UserRole.Length > 0)
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                            UserRole = UserRole + ", Approver";
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                            UserRole = UserRole + "Approver";
                        }
                    }
                    Logger.Info("Accessed DB, Checking User Details: Checking Status");
                    if (r.UserRoleFinance == true)
                    {
                        if (UserRole.Length > 0)
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                            UserRole = UserRole + ", Finance Approver";
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                            UserRole = UserRole + "Finance Approver";
                        }
                    }

                    Logger.Info("Accessed DB, Checking User Details: Checking Status");
                    if (r.UserRoleLegal == true)
                    {
                        if (UserRole.Length > 0)
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Active");
                            UserRole = UserRole + ", Legal Approver";
                        }
                        else
                        {
                            Logger.Info("Accessed DB, Checking User Details: UserRole is Not Active");
                            UserRole = UserRole + "Legal Approver";
                        }
                    }
                    IDEmail[0] = "success";
                    IDEmail[1] = r.UserEmployeeID.ToString();
                    IDEmail[2] = r.UserEmployeeName;
                    IDEmail[3] = r.UserEmployeeDesignation;
                    IDEmail[4] = UserRole;
                    IDEmail[5] = r.UserSubCategory;
                    IDEmail[6] = r.UserCategory;
                    Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                    return Json(IDEmail);
                }
            }
               // Logger.Info("Accessed DB, Checking Users Details : Details Found");
               // return Json("");
            
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'getHodDetails3' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
    Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
}

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt Users getLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking Users Log Details: LogID match");
                var result = from tblUserLog in db.tblUserLogs.Where(x => x.LogUserUID == ID) select tblUserLog;
                Logger.Info("Accessed DB, Checking Users Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'getLogDetail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SaveLog(string initialvalue, string details, int ID, string UserID)
        {
            Logger.Info("Attempt Users SaveLog");
            try
            {
                Logger.Info("Accessing DB for Saving the Users Log Details");
                tblUserLog log = new tblUserLog
                {
                    LogUserUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblUserLogs.Add(log);
               
                db.SaveChanges();
                Logger.Info("Accessed DB, Users Log Record Saved");

                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'SaveLog 'Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }

        [HttpPost]
        
        public ActionResult employeeIDVerification(int EmployeeID)
        {
            Logger.Info("Attempt Users employeeIDVerification");

            try
            {
                Logger.Info("Accessed DB, Checking Users Details: EmployeeID match");
                var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == EmployeeID) select tblUserMaster.UserEmployeeID;
                Logger.Info("Accessed DB, Checking Users Details: EmployeeID Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'employeeIDVerification' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }
        [HttpPost]

        public ActionResult employeeIDVerification2(int EmployeeID, int UserTableID)
        {
            Logger.Info("Attempt Users employeeIDVerification");

            try
            {
                Logger.Info("Accessed DB, Checking Users Details: EmployeeID match");
                var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserID != UserTableID).Where(x => x.UserEmployeeID == EmployeeID) select tblUserMaster.UserEmployeeID;
                Logger.Info("Accessed DB, Checking Users Details: EmployeeID Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Users' Controller , 'employeeIDVerification' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }
    }
}