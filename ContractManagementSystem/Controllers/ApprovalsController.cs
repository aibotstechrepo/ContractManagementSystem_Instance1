using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{
    public class ApprovalsController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: Approvals
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
        public ActionResult New()
        {
            Logger.Info("Accessing ApprovalsMaster New Page");
            return View(db.tblUserMasters.ToList());
        }

        public ActionResult Repository()
        {
            Logger.Info("Accessing ApprovalsMaster Repository Page");
            Logger.Info("Accessing DB for Repository");


            IQueryable<ApprovalsRepositoryModel> ApprovalsList = null;

            ApprovalsList = (from x in db.tblApprovalMasters
                             select new ApprovalsRepositoryModel()
                             {
                                 ApprovalID = x.ApprovalID,
                                 Category = x.Category,
                                 SubCategory = x.SubCategory,
                                 LastModified = x.LastModified,
                                 WorkflowType = x.WorkflowType
                             }).AsQueryable();


            return View(ApprovalsList);

        }

        public ActionResult Index()
        {
            return RedirectToAction("Repository");
        }

        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Approvals/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            Logger.Info("Accessing DB for ApprovalMaster Details");
            tblApprovalMaster tblApprovalMaster = db.tblApprovalMasters.Find(id);

            Logger.Info("Accessed DB, Checking ApprovalMaster Details: Checking Status");
            if (tblApprovalMaster == null)
            {
                Logger.Info("Accessed DB, Checking ApprovalMaster Details: Details Not Found");
                return HttpNotFound();

            }
            Logger.Info("Accessed DB, Checking ApprovalMaster Details: Details Found");

            Logger.Info("Redirecting to ApprovalMaster Details Page");
            return View(tblApprovalMaster);
        }

       
       
        [HttpPost]
        public JsonResult getCategory()
        {
            Logger.Info("Attempt ApproverMaster getCategory");
            try
            {
                Logger.Info("Accessing DB for Category List");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category List: Category Found");
                return Json(result);
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'getCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getSubCategory(string user_category_id)
        {
            Logger.Info("Attempt ApproverMaster getSubCategory");
            try
            {
                Logger.Info("Accessing DB for SubCategory List");
                
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == user_category_id) select tblSubCategory.SubCategoryName;
                //foreach (var r in result)
                //{
                //    Console.WriteLine(r);
                //}
                Logger.Info("Accessed DB, Checking SubCategory List: SubCategory Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'getSubCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }



      

        [HttpPost]
        public ActionResult SubmitApproversToDB(string WorkflowType, string ApproverCluster, string ApproverFunction, int[] UID, int CurrentUserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt ApproverMaster SubmitApproversToDB");
            try
            {
                Logger.Info("Accessing DB for Saving the ApprovalMaster Records");
                tblApprovalMaster approver = new tblApprovalMaster
                {

                    //approver.WorkflowType = WorkflowType;
                    //approver.Category = ApproverCluster;
                    //approver.SubCategory = ApproverFunction;

                    WorkflowType = HttpUtility.HtmlEncode(WorkflowType),
                    Category = HttpUtility.HtmlEncode(ApproverCluster),
                    SubCategory = HttpUtility.HtmlEncode(ApproverFunction)
                };


                if (UID.Length > 0)
                {
                    approver.Approver1ID = UID[0];
                }
                if (UID.Length > 1)
                {
                    approver.Approver2ID = UID[1];
                }
                if (UID.Length > 2)
                {
                    approver.Approver3ID = UID[2];
                }
                if (UID.Length > 3)
                {
                    approver.Approver4ID = UID[3];
                }
                if (UID.Length > 4)
                {
                    approver.Approver5ID = UID[4];
                }
                if (UID.Length > 5)
                {
                    approver.Approver6ID = UID[5];
                }
                if (UID.Length > 6)
                {
                    approver.Approver7ID = UID[6];
                }
                if (UID.Length > 7)
                {
                    approver.Approver8ID = UID[7];
                }
                if (UID.Length > 8)
                {
                    approver.Approver9ID = UID[8];
                }
                if (UID.Length > 9)
                {
                    approver.Approver10ID = UID[9];
                }
                approver.LastModified = DateTime.Now;

                db.tblApprovalMasters.Add(approver);
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Record Saved");

                Logger.Info("Accessing DB for Saving the ApprovalMaster Log Details");
                tblApprovalLog log = new tblApprovalLog
                {
                    LogApprovalUID = approver.ApprovalID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Created",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblApprovalLogs.Add(log);
                db.SaveChanges();

                Logger.Info("Accessed DB, ApprovalMaster Log Record Saved");
                return Json("success");
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'SubmitApproversToDB' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
           
        }



            [HttpPost]
        public ActionResult UpdateApproversToDB(int ApprovalID, string WorkflowType, string ApproverCluster, string ApproverFunction, int[] UID, int CurrentUserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUserID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt ApproverMaster UpdateApproversToDB");
            try
            {
                Logger.Info("Accessing DB for Updating the ApprovalMaster Records");
                tblApprovalMaster approver = db.tblApprovalMasters.Find(ApprovalID);

                string OldValues = "";
                string NewValues = "";


                if (approver.WorkflowType != WorkflowType)
                {
                    OldValues = OldValues + "Workflow Type : " + approver.WorkflowType + " , ";
                    NewValues = NewValues + "Workflow Type : " + WorkflowType + " , ";
                }
                if (approver.Category != ApproverCluster)
                {
                    OldValues = OldValues + "Cluster : " + approver.Category + " , ";
                    NewValues = NewValues + "Cluster : " + ApproverCluster + " , ";
                }
                if (approver.SubCategory != ApproverFunction)
                {
                    OldValues = OldValues + "Function : " + approver.SubCategory + " , ";
                    NewValues = NewValues + "Function : " + ApproverFunction + " , ";
                }


                try
                {

                    if (approver.Approver1ID != UID[0])
                    {
                        OldValues = OldValues + "Approver1 ID : " + approver.Approver1ID + " , ";
                        NewValues = NewValues + "Approver1 ID : " + UID[0] + " , ";
                    }
                    if (approver.Approver2ID != UID[1])
                    {
                        OldValues = OldValues + "Approver2 ID : " + approver.Approver2ID + " , ";
                        NewValues = NewValues + "Approver2 ID : " + UID[1] + " , ";
                    }

                    if (approver.Approver3ID != UID[2])
                    {
                        OldValues = OldValues + "Approver3 ID : " + approver.Approver3ID + " , ";
                        NewValues = NewValues + "Approver3 ID : " + UID[2] + " , ";
                    }

                    if (approver.Approver4ID != UID[3])
                    {
                        OldValues = OldValues + "Approver4 ID : " + approver.Approver4ID + " , ";
                        NewValues = NewValues + "Approver4 ID : " + UID[3] + " , ";
                    }

                    if (approver.Approver5ID != UID[4])
                    {
                        OldValues = OldValues + "Approver5 ID : " + approver.Approver5ID + " , ";
                        NewValues = NewValues + "Approver5 ID : " + UID[4] + " , ";
                    }

                    if (approver.Approver6ID != UID[5])
                    {
                        OldValues = OldValues + "Approver6 ID : " + approver.Approver6ID + " , ";
                        NewValues = NewValues + "Approver6 ID : " + UID[5] + " , ";
                    }

                    if (approver.Approver7ID != UID[6])
                    {
                        OldValues = OldValues + "Approver7 ID : " + approver.Approver7ID + " , ";
                        NewValues = NewValues + "Approver7 ID : " + UID[6] + " , ";
                    }

                    if (approver.Approver8ID != UID[7])
                    {
                        OldValues = OldValues + "Approver8 ID : " + approver.Approver8ID + " , ";
                        NewValues = NewValues + "Approver8 ID : " + UID[7] + " , ";
                    }

                    if (approver.Approver9ID != UID[8])
                    {
                        OldValues = OldValues + "Approver9 ID : " + approver.Approver9ID + " , ";
                        NewValues = NewValues + "Approver9 ID : " + UID[8] + " , ";
                    }

                    if (approver.Approver10ID != UID[9])
                    {
                        OldValues = OldValues + "Approver10 ID : " + approver.Approver10ID + " , ";
                        NewValues = NewValues + "Approver10 ID : " + UID[9] + " , ";
                    }
                }
                catch { }



                approver.WorkflowType = HttpUtility.HtmlEncode(WorkflowType);
                approver.Category = HttpUtility.HtmlEncode(ApproverCluster);
                approver.SubCategory = HttpUtility.HtmlEncode(ApproverFunction);
                if (UID.Length > 0)
                {
                    approver.Approver1ID = UID[0];
                }
                else
                {
                    approver.Approver1ID = 0;
                }
                if (UID.Length > 1)
                {
                    approver.Approver2ID = UID[1];
                }
                else
                {
                    approver.Approver2ID = 0;
                }
                if (UID.Length > 2)
                {
                    approver.Approver3ID = UID[2];
                }
                else
                {
                    approver.Approver3ID = 0;
                }
                if (UID.Length > 3)
                {
                    approver.Approver4ID = UID[3];
                }
                else
                {
                    approver.Approver4ID = 0;
                }
                if (UID.Length > 4)
                {
                    approver.Approver5ID = UID[4];
                }
                else
                {
                    approver.Approver5ID = 0;
                }
                if (UID.Length > 5)
                {
                    approver.Approver6ID = UID[5];
                }
                else
                {
                    approver.Approver6ID = 0;
                }
                if (UID.Length > 6)
                {
                    approver.Approver7ID = UID[6];
                }
                else
                {
                    approver.Approver7ID = 0;
                }
                if (UID.Length > 7)
                {
                    approver.Approver8ID = UID[7];
                }
                else
                {
                    approver.Approver8ID = 0;
                }
                if (UID.Length > 8)
                {
                    approver.Approver9ID = UID[8];
                }
                else
                {
                    approver.Approver9ID = 0;
                }
                if (UID.Length > 9)
                {
                    approver.Approver10ID = UID[9];
                }
                else
                {
                    approver.Approver10ID = 0;
                }
                approver.LastModified = DateTime.Now;

                db.Entry(approver).State = EntityState.Modified;

                if (OldValues.Length > 0)
                {
                    tblApprovalLog log = new tblApprovalLog();
                    log.LogApprovalUID = approver.ApprovalID;
                    log.ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName;
                    log.LogActivity = "Modified";
                    log.ChangedFrom = OldValues;
                    log.ChangedTo = NewValues;
                    log.DateandTime = DateTime.Now.ToString();

                    db.tblApprovalLogs.Add(log);
                }
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Updated Record Saved");

               
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'UpdateApproversToDB' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

     
         [HttpPost]

        public ActionResult DeleteApproverFromDB(int ApprovalID, int CurrentUserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt ApproverMaster DeleteApproverFromDB");
            try
            {
                Logger.Info("Accessing DB for Deleting the ApprovalMaster Records");
                tblApprovalMaster approval = db.tblApprovalMasters.Find(ApprovalID);
                db.tblApprovalMasters.Remove(approval);
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Record Deleted");

                Logger.Info("Accessing DB for Saving the ApprovalMaster Log Details");
                tblApprovalLog log = new tblApprovalLog
                {
                    LogApprovalUID = ApprovalID,
                    ModifiedBy = CurrentUser.ToString() +" - "+ CurrentUserName,
                    LogActivity = "Deletion",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblApprovalLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Log Record Saved");
                return Json("success");
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'DeleteApproverFromDB' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }



    [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt ApprovalMaster getLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking ApprovalMaster Log Details: LogID match");
                var result = from tblApprovalLog in db.tblApprovalLogs.Where(x => x.LogApprovalUID == ID) select tblApprovalLog;
                Logger.Info("Accessed DB, Checking ApprovalMaster Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'getLogDetail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult GetApproversBasedOnClusterAndFunction(string ApproverCluster, string ApproverFunction)
        {
            Logger.Info("Attempt ApprovalMaster GetApproversBasedOnClusterAndFunction");
            try
            {
                Logger.Info("Accessed DB, Checking UserMaster Details: Category and SubCategory match");
                var ApproverList = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserCategory.Contains(ApproverCluster)).
                            Where(x => x.UserSubCategory.Contains(ApproverFunction)).
                            Select(x => new
                            {
                                x.UserCategory,
                                x.UserEmployeeDesignation,
                                x.UserEmployeeEmail,
                                x.UserEmployeeID,
                                x.UserEmployeeName,
                                x.UserRoleAdmin,
                                x.UserRoleApprover,
                                x.UserRoleFinance,
                                x.UserRoleInitiator,
                                x.UserRoleLegal,
                                x.UserRoleReviewer,
                                x.UserSubCategory
                            });
                                   //select tblUserMaster;
                Logger.Info("Accessed DB, Checking UserMaster Details: Category and SubCategory Found");
                return Json(ApproverList);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'GetApproversBasedOnClusterAndFunction' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt ApprovalMaster SaveLog");


            try
            {
                Logger.Info("Accessing DB for Saving the ApprovalMaster Log Details");
                tblApprovalLog log = new tblApprovalLog
                {
                    LogApprovalUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblApprovalLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, ApprovalMaster Log Record Saved");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'SaveLog' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }
        [HttpPost]
        public JsonResult GetUserById(string EmployeeID)
        {
            Logger.Info("Attempt ApprovalMaster GetUserById");

            try
            {
                string[] UserInfo = { "", "User Not Found", "User Not Found", "User Not Found", "User Not Found", "", "", "" };

                if (!string.IsNullOrWhiteSpace(EmployeeID))
                {
                    EmployeeID = EmployeeID.Trim();
                    int EMPID = Convert.ToInt32(EmployeeID);
                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeID match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == EMPID) select tblUserMaster;
                   
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
                        UserInfo[0] = "success";
                        UserInfo[1] = r.UserEmployeeName;
                        UserInfo[2] = r.UserEmployeeEmail;
                        UserInfo[3] = r.UserEmployeeDesignation;
                        UserInfo[4] = UserRole;
                        UserInfo[5] = r.UserSubCategory;
                        UserInfo[6] = r.UserCategory;
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(UserInfo);
                    }
                    return Json(UserInfo);
                }
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'GetUserById' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [HttpPost]
        public JsonResult GetUserByName(string EmployeeName)
        {
            Logger.Info("Attempt ApprovalMaster GetUserByName");

            try
            {

                if (!string.IsNullOrWhiteSpace(EmployeeName))
                {
                    EmployeeName = EmployeeName.Trim();
                    //int EMPID = Convert.ToInt32(EmployeeID);
                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeName match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeName == EmployeeName) select tblUserMaster;
                    string[] UserInfo = new string[7];

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
                        UserInfo[0] = "success";
                        UserInfo[1] = r.UserEmployeeID.ToString();
                        UserInfo[2] = r.UserEmployeeEmail;
                        UserInfo[3] = r.UserEmployeeDesignation;
                        UserInfo[4] = UserRole;
                        UserInfo[5] = r.UserSubCategory;
                        UserInfo[6] = r.UserCategory;
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(UserInfo);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'GetUserByName' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [HttpPost]
        public JsonResult GetUserByEmail(string EmployeeEmail)
        {
            Logger.Info("Attempt ApprovalMaster GetUserByEmail");

            try
            {

                if (!string.IsNullOrWhiteSpace(EmployeeEmail))
                {
                    EmployeeEmail = EmployeeEmail.Trim();
                    //int EMPID = Convert.ToInt32(EmployeeID);
                    Logger.Info("Accessed DB, Checking UserMaster Details: EmployeeEmail match");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeEmail == EmployeeEmail) select tblUserMaster;
                    string[] UserInfo = new string[7];

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
                        UserInfo[0] = "success";
                        UserInfo[1] = r.UserEmployeeID.ToString();
                        UserInfo[2] = r.UserEmployeeName;
                        UserInfo[3] = r.UserEmployeeDesignation;
                        UserInfo[4] = UserRole;
                        UserInfo[5] = r.UserSubCategory;
                        UserInfo[6] = r.UserCategory;
                        Logger.Info("Accessed DB, Checking UserMaster Details: User Details Found");
                        return Json(UserInfo);
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'GetUserByEmail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                string[] errors = { "error" };
                return Json(errors);
            }
            Logger.Info("Accessed DB, Checking UserMaster Details: User Details Not Found");
            string[] failures = { "failure" };
            return Json(failures);
        }

        [HttpPost]
        public ActionResult CheckApprovalWorkflowExist(string ApprovalCluster,string ApprovalFunction)
        {
            Logger.Info("Attempt ApprovalMaster CheckApprovalWorkflowExist");
            try
            {
                Logger.Info("Accessed DB, Checking ApprovalMaster Details: Checking Status");
                var result = /*from tblApprovalMaster in*/ db.tblApprovalMasters.Select(x => new { x.Category, x.SubCategory }); //select tblApprovalMaster;
                string[] ApprovalInfo = new string[2];
                foreach (var item in result)
                {
                    Logger.Info("Accessed DB, Checking ApprovalMaster Details: Category and SubCategory match");
                    if ((item.Category == ApprovalCluster) && (item.SubCategory == ApprovalFunction))
                    {
                        Logger.Info("Accessed DB, Checking ApprovalMaster Details: Details Found");
                        return Json("success");
                    }
                    else
                    {
                        Logger.Info("Accessed DB, Checking ApprovalMaster Details: Details Not Found");
                        return Json("failure");
                    }
                }
                return Json("error");
            }
            catch(Exception Ex)
            {
                Logger.Error(Ex, "'Approvals' Controller , 'CheckApprovalWorkflowExist' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("Error");
            }
        }

        [HttpPost]
        public JsonResult getHodDetails(string EmployeeDetails, string OptionToSearch)
        {
            OptionToSearch = OptionToSearch.Trim();
            if (OptionToSearch == "Employee ID")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(EmployeeDetails)).Select(x => new
                {
                    x.UserCategory,
                    x.UserEmployeeDesignation,
                    x.UserEmployeeEmail,
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserStatus,
                }); //select tblUserMaster;
                return Json(result);
            }
            else if (OptionToSearch == "Employee Name")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeName.Contains(EmployeeDetails)).Select(x => new
                {
                    x.UserCategory,
                    x.UserEmployeeDesignation,
                    x.UserEmployeeEmail,
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserStatus,
                }); //select tblUserMaster; //select tblUserMaster;
                return Json(result);
            }
            else if (OptionToSearch == "Employee Email Address")
            {
                var result = /*from tblUserMaster in*/ db.tblUserMasters.Where(x => x.UserEmployeeEmail.Contains(EmployeeDetails)).Select(x => new
                {
                    x.UserCategory,
                    x.UserEmployeeDesignation,
                    x.UserEmployeeEmail,
                    x.UserEmployeeID,
                    x.UserEmployeeName,
                    x.UserRoleAdmin,
                    x.UserRoleApprover,
                    x.UserRoleFinance,
                    x.UserRoleInitiator,
                    x.UserRoleLegal,
                    x.UserRoleReviewer,
                    x.UserSubCategory,
                    x.UserStatus,
                }); //select tblUserMaster; //select tblUserMaster;
                return Json(result);
            }
            else
            {
                return Json("");
            }
        }

    }
}