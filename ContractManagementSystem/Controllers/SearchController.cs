using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{

    public class SearchController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();

        readonly string ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];

        // GET: Search
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

        public ActionResult Index()
        {
            Logger.Info("Accessing the search New Page");
            CommonModel model = new CommonModel
            {
                Users = db.tblUserMasters,
                Category = db.tblCategories,
                SubCategory = db.tblSubCategories,
                Clause = db.tblClauseMasters,
                Vendor = db.tblVendorMasters,
                Alert = db.tblAlertSystems,
                Approval = db.tblApprovalMasters,
                Contract = db.tblContractMasters,
                Template = db.tblTemplateMasters
            };
            return View(model);
        }

        //public ActionResult Index()
        //{
        //    return RedirectToAction("search");
        //}

        [HttpPost]
        public JsonResult getCategoryList()
        {
            Logger.Info("Attempt Search getCategoryList");
            try
            {
                Logger.Info("Accessing DB for Category List: checking Status ");
                var categorylist = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checked Category:Category List Found ");
                return Json(categorylist);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'getCategoryList' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getSubCategoryList()
        {
            Logger.Info("Attempt Search getSubCategoryList");
            try
            {
                Logger.Info("Accessing DB for SubCategory List: checking Status ");
                var subcategorylist = from tblSubCategory in db.tblSubCategories select tblSubCategory.SubCategoryName;
                Logger.Info("Accessed DB, Checked SubCategory:SubCategory List Found ");
                return Json(subcategorylist);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'getSubCategoryList' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

      

        [HttpPost]
        public JsonResult getUserCategory()
        {
            Logger.Info("Attempt Search getUserCategory");
            try
            {
                Logger.Info("Accessing DB for Category List for User: checking Status ");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checked Category:Category List for User Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'getUserCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getUserSubCategory(string user_category_id)
        {
            Logger.Info("Attempt Search getUserCategory");
            try
            {
                Logger.Info("Accessing DB for SubCategory List : SubCategoryUserID Match");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == user_category_id) select tblSubCategory.SubCategoryName;

                foreach (var r in result)
                {
                    Console.WriteLine(r);
                }
                Logger.Info("Accessed DB, Checking SubCategory List: SubCategory List Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'getUserSubCategory' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getUserDesignation()
        {
            Logger.Info("Attempt Search getUserDesignation");
            try
            {
                Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                var userdesignation = from tblUserMaster in db.tblUserMasters select tblUserMaster.UserEmployeeDesignation;
                var r = userdesignation.Distinct();
                Logger.Info("Accessed DB, Checked UserMaster Details:User Designation Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'getUserDesignation' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }
        [HttpPost]
        public JsonResult getVendorEntity()
        {
            Logger.Info("Attempt Search getVendorEntity");
            try
            {
                Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                var vendorentity = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorTypeofEntity;
                var r = vendorentity.Distinct();
                Logger.Info("Accessed DB, Checked VendorMaster Details:Vendor Type Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'getVendorEntity' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        

        [HttpPost]
        public JsonResult getContractVendorList()
        {
            Logger.Info("Attempt Search getContractVendorList");
            try
            {
                Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                var vendorName = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorVendorName;
                var r = vendorName.Distinct();
                Logger.Info("Accessed DB, Checked VendorMaster Details:Vendor Name List Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'getContractVendorList' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public ActionResult gettblClause(string clausetitle, string clausedescription)
        {
            Logger.Info("Attempt Search gettblClause");
            try
            {
                if (clausetitle.Length > 0)
                {
                    Logger.Info("Accessing DB for ClauseMaster Details: checking Status ");
                    var result = from tblClauseMaster in db.tblClauseMasters.Where(x => x.ClauseClauseTitle.Contains(clausetitle))
                                 select tblClauseMaster;
                    Logger.Info("Accessed DB, Checked ClauseMaster Details:ClauseMaster Details Found ");
                    return Json(result);
                }
                else if (clausedescription.Length > 0)
                {
                    Logger.Info("Accessing DB for ClauseMaster Details: checking Status ");
                    var result = from tblClauseMaster in db.tblClauseMasters.Where(x => x.ClauseClauseDescription.Contains(clausedescription))
                                 select tblClauseMaster;
                    Logger.Info("Accessed DB, Checked ClauseMaster Details:ClauseMaster Details Found ");
                    return Json(result);
                }
                else
                {
                    Logger.Info("Accessing DB for ClauseMaster Details: checking Status ");
                    var result = from tblClauseMaster in db.tblClauseMasters.Where(x => x.ClauseClauseTitle.Contains(clausetitle)).
                            Where(x => x.ClauseClauseDescription.Contains(clausedescription))
                                 select tblClauseMaster;
                    Logger.Info("Accessed DB, Checked ClauseMaster Details:ClauseMaster Details Found ");
                    return Json(result);
                }

                
                
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'gettblClause' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }

        }
        [HttpPost]
        public ActionResult gettblUserBasic(string basicuserid, string basicusername, string basicuseremail, string basicuserstatus, string basicuserrole)
        {
            basicuserstatus = basicuserstatus.Trim();
            basicuserrole = basicuserrole.Trim();
            if (basicuserid == "")
            {
                basicuserid = null;
            }
            if (basicusername == "")
            {
                basicusername = null;
            }
            if (basicuseremail == "")
            {
                basicuseremail = null;
            }
            if (basicuserstatus == "")
            {
                basicuserstatus = null;
            }
            //if (basicuserrole == "")
            //{
            //    basicuserrole = null;
            //}

            Logger.Info("Attempt Search gettblUserBasic");
            try
            {

               
                if (basicuserrole == "Initiator")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(basicuserid)
                                || x.UserEmployeeName.Contains(basicusername) || x.UserEmployeeEmail.Contains(basicuseremail) ||
                               x.UserStatus.Contains(basicuserstatus) || x.UserRoleInitiator == true)
                         select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if(basicuserrole == "Approver")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(basicuserid)
                             || x.UserEmployeeName.Contains(basicusername) || x.UserEmployeeEmail.Contains(basicuseremail)
                             || x.UserStatus.Contains(basicuserstatus) || x.UserRoleApprover == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (basicuserrole == "Admin")
                {
                   
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(basicuserid)
                             || x.UserEmployeeName.Contains(basicusername) || x.UserEmployeeEmail.Contains(basicuseremail)
                             || x.UserStatus.Contains(basicuserstatus) || x.UserRoleAdmin == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (basicuserrole == "Finance")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(basicuserid)
                             || x.UserEmployeeName.Contains(basicusername) || x.UserEmployeeEmail.Contains(basicuseremail)
                             || x.UserStatus.Contains(basicuserstatus) || x.UserRoleFinance == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (basicuserrole == "Legal")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(basicuserid)
                             || x.UserEmployeeName.Contains(basicusername) || x.UserEmployeeEmail.Contains(basicuseremail)
                             || x.UserStatus.Contains(basicuserstatus) || x.UserRoleLegal == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (basicuserrole == "Reviewer")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(basicuserid)
                             || x.UserEmployeeName.Contains(basicusername) || x.UserEmployeeEmail.Contains(basicuseremail)
                             || x.UserStatus.Contains(basicuserstatus) || x.UserRoleReviewer == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(basicuserid)
                             || x.UserEmployeeName.Contains(basicusername) || x.UserEmployeeEmail.Contains(basicuseremail)
                             || x.UserStatus.Contains(basicuserstatus))
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
               

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'gettblUserBasic' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }

        }

        [HttpPost]
        public ActionResult gettblUserAdvance(string advanceuserid, string advanceusername, string advanceuseremail, string advancecategory, string advanceusersubcategory, string advanceuserdesignation, string advanceuserrole, string advanceuserstatus)
        {
            advanceusersubcategory = advanceusersubcategory.Trim();
            advanceuserdesignation = advanceuserdesignation.Trim();
            advanceuserrole = advanceuserrole.Trim();
            advanceuserstatus = advanceuserstatus.Trim();
            advanceuserrole = advanceuserrole.Trim();
            if (advanceuserid == "")
            {
                advanceuserid = null;
            }
            if (advanceusername == "")
            {
                advanceusername = null;
            }
            if (advanceuseremail == "")
            {
                advanceuseremail = null;
            }
            if (advancecategory == "")
            {
                advancecategory = null;
            }
            if (advanceusersubcategory == "")
            {
                advanceusersubcategory = null;
            }
            if (advanceuserdesignation == "")
            {
                advanceuserdesignation = null;
            }
            //if (advanceuserrole == "")
            //{
            //    advanceuserrole = null;
            //}
            if (advanceuserstatus == "")
            {
                advanceuserstatus = null;
            }
            Logger.Info("Attempt Search gettblUserAdvance");
            try
            {
                if (advanceuserrole == "Initiator")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(advanceuserid)
                             || x.UserEmployeeName.Contains(advanceusername) || x.UserEmployeeEmail.Contains(advanceuseremail) ||
                             x.UserCategory.Contains(advancecategory) || x.UserSubCategory.Contains(advanceusersubcategory) ||
                             x.UserEmployeeDesignation.Contains(advanceuserdesignation) || x.UserStatus.Contains(advanceuserstatus)
                             || x.UserRoleInitiator == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (advanceuserrole == "Approver")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(advanceuserid)
                             || x.UserEmployeeName.Contains(advanceusername) || x.UserEmployeeEmail.Contains(advanceuseremail) ||
                             x.UserCategory.Contains(advancecategory) || x.UserSubCategory.Contains(advanceusersubcategory) ||
                             x.UserEmployeeDesignation.Contains(advanceuserdesignation) || x.UserStatus.Contains(advanceuserstatus)
                             || x.UserRoleApprover == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (advanceuserrole == "Admin")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(advanceuserid)
                             || x.UserEmployeeName.Contains(advanceusername) || x.UserEmployeeEmail.Contains(advanceuseremail) ||
                             x.UserCategory.Contains(advancecategory) || x.UserSubCategory.Contains(advanceusersubcategory) ||
                             x.UserEmployeeDesignation.Contains(advanceuserdesignation) || x.UserStatus.Contains(advanceuserstatus)
                             || x.UserRoleAdmin == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (advanceuserrole == "Finance")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(advanceuserid)
                             || x.UserEmployeeName.Contains(advanceusername) || x.UserEmployeeEmail.Contains(advanceuseremail) ||
                             x.UserCategory.Contains(advancecategory) || x.UserSubCategory.Contains(advanceusersubcategory) ||
                             x.UserEmployeeDesignation.Contains(advanceuserdesignation) || x.UserStatus.Contains(advanceuserstatus)
                             || x.UserRoleFinance == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (advanceuserrole == "Legal")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(advanceuserid)
                             || x.UserEmployeeName.Contains(advanceusername) || x.UserEmployeeEmail.Contains(advanceuseremail) ||
                             x.UserCategory.Contains(advancecategory) || x.UserSubCategory.Contains(advanceusersubcategory) ||
                             x.UserEmployeeDesignation.Contains(advanceuserdesignation) || x.UserStatus.Contains(advanceuserstatus)
                             || x.UserRoleLegal == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else if (advanceuserrole == "Reviewer")
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(advanceuserid)
                             || x.UserEmployeeName.Contains(advanceusername) || x.UserEmployeeEmail.Contains(advanceuseremail) ||
                             x.UserCategory.Contains(advancecategory) || x.UserSubCategory.Contains(advanceusersubcategory) ||
                             x.UserEmployeeDesignation.Contains(advanceuserdesignation) || x.UserStatus.Contains(advanceuserstatus)
                             || x.UserRoleReviewer == true)
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }
                else
                {
                    Logger.Info("Accessing DB for UserMaster Details: checking Status ");
                    var result = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString().Contains(advanceuserid)
                             || x.UserEmployeeName.Contains(advanceusername) || x.UserEmployeeEmail.Contains(advanceuseremail) ||
                             x.UserCategory.Contains(advancecategory) || x.UserSubCategory.Contains(advanceusersubcategory) ||
                             x.UserEmployeeDesignation.Contains(advanceuserdesignation) || x.UserStatus.Contains(advanceuserstatus)
                             )
                                 select tblUserMaster;
                    Logger.Info("Accessed DB, Checked UserMaster Details:UserMaster Details Found ");
                    return Json(result);
                }

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'gettblUserAdvance' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }

        }

        [HttpPost] 
        public ActionResult gettblVendorBasic(string basicvendorname, string basicvendorvendorcin, string basicvendorentity,
            string basicvendorpan, string basicvendorLLPIN, string basicvendorAuthSign)
        {
            if (basicvendorname == "")
            {
                basicvendorname = null;
            }
            if (basicvendorvendorcin == "")
            {
                basicvendorvendorcin = null;
            }
            if (basicvendorpan == "")
            {
                basicvendorpan = null;
            }
            if (basicvendorLLPIN == "")
            {
                basicvendorLLPIN = null;
            }
            if (basicvendorAuthSign == "")
            {
                basicvendorAuthSign = null;
            }

            Logger.Info("Attempt Search gettblVendorBasic");
            try
            {
               if(basicvendorentity == "Company")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity == basicvendorentity).Where(x => x.VendorVendorName == basicvendorname
                    || x.VendorCorporateIdentificationNumber.ToString().Contains(basicvendorvendorcin))
                           .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });

                    //return Json(result.ToList()[0]);
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if(basicvendorentity == "Partnership Firm")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.
                           Where(x => x.VendorTypeofEntity == basicvendorentity).Where(x => x.VendorVendorName ==  basicvendorname
                           || x.VendorAuthorisedSignatory.Contains(basicvendorAuthSign))
                           .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });

                    //return Json(result.ToList()[0]);
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if (basicvendorentity == "Sole Proprietorship")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity ==  basicvendorentity).Where(x => x.VendorVendorName == basicvendorname
                    || x.PAN.Contains(basicvendorpan))
                           .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });

                    //return Json(result.ToList()[0]);
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if (basicvendorentity == "Limited Liability Partnership Firm")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity == basicvendorentity).Where(x => x.VendorVendorName == basicvendorname
                    || x.LLPIN.Contains(basicvendorLLPIN))
                           .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });

                    //return Json(result.ToList()[0]);
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if (basicvendorentity == "Others")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorVendorName == basicvendorname).
                           Where(x => x.VendorTypeofEntity == basicvendorentity).Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });

                    //return Json(result.ToList()[0]);
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'gettblVendorBasic' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult gettblVendorAdvanced(string advancevendorname, string advancevendorcin, string AdvanceAuthorisedSignatory, string advancevendorpan, string advancevendorentity,
            string AdvanceVendorAdhaar, string advancevendorLLPIN, string AdvanceVendorAddressLLPIN, string advancevendorAuthSign, string AdvanceVendorRegisteredAddress,
            string AdvanceVendorAddress, string AdvanceVendorRegisteredAddress_Company, string AdvanceVendorCorporateAddress_Company,string AdvanceVendorName_Sole,
            string AdvanceVendorAddress_Sole, string AdvanceVendorName1P, string AdvanceVendorFather1P, string AdvanceVendorAddress1P, int AdvanceVendorAge1P = 0)
        {
           if(advancevendorname == "")
            {
                advancevendorname = null;
            }
            if (advancevendorcin == "")
            {
                advancevendorcin = null;
            }
            if (AdvanceAuthorisedSignatory == "")
            {
                AdvanceAuthorisedSignatory = null;
            }
            if (advancevendorpan == "")
            {
                advancevendorpan = null;
            }
            if (AdvanceVendorAdhaar == "")
            {
                AdvanceVendorAdhaar = null;
            }

            if (advancevendorLLPIN == "")
            {
                advancevendorLLPIN = null;
            }
            if (AdvanceVendorAddressLLPIN == "")
            {
                AdvanceVendorAddressLLPIN = null;
            }
            if (advancevendorAuthSign == "")
            {
                advancevendorAuthSign = null;
            }
            if (AdvanceVendorRegisteredAddress == "")
            {
                AdvanceVendorRegisteredAddress = null;
            }
            if (AdvanceVendorAddress == "")
            {
                AdvanceVendorAddress = null;
            }

            if (AdvanceVendorRegisteredAddress_Company == "")
            {
                AdvanceVendorRegisteredAddress_Company = null;
            }
            if (AdvanceVendorCorporateAddress_Company == "")
            {
                AdvanceVendorCorporateAddress_Company = null;
            }
            if (AdvanceVendorName_Sole == "")
            {
                AdvanceVendorName_Sole = null;
            }
            if (AdvanceVendorAddress_Sole == "")
            {
                AdvanceVendorAddress_Sole = null;
            }
            if (AdvanceVendorName1P == "")
            {
                AdvanceVendorName1P = null;
            }

            if (AdvanceVendorAddress1P == "")
            {
                AdvanceVendorAddress1P = null;
            }
            if (AdvanceVendorFather1P == "")
            {
                AdvanceVendorFather1P = null;
            }
           
            Logger.Info("Attempt Search gettblVendorAdvanced");
            try
            {
                if(advancevendorentity == "Company")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity.Contains(advancevendorentity)).Where(x => x.VendorVendorName.Contains(advancevendorname) || x.VendorCorporateIdentificationNumber.ToString().Contains(advancevendorcin)
                    || x.VendorAuthorisedSignatory.Contains(AdvanceAuthorisedSignatory)
                    || x.VendorRegisteredAddress.Contains(AdvanceVendorRegisteredAddress_Company) || x.CorporateAddress.Contains(AdvanceVendorCorporateAddress_Company))

                                 .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if (advancevendorentity == "Sole Proprietorship")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity.Contains(advancevendorentity)).Where(x => x.VendorVendorName.Contains(advancevendorname)
                    || x.PAN.Contains(advancevendorpan) || x.AdhaarCardNo.Contains(AdvanceVendorAdhaar) || x.NameoftheProprietor.Contains(AdvanceVendorName_Sole) ||
                    x.VendorRegisteredAddress.Contains(AdvanceVendorAddress_Sole)).Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if (advancevendorentity == "Limited Liability Partnership Firm")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity.Contains(advancevendorentity)).Where(x => x.VendorVendorName.Contains(advancevendorname)
                    || x.LLPIN.Contains(advancevendorLLPIN) || x.VendorRegisteredAddress.Contains(AdvanceVendorAddressLLPIN))
                           .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if (advancevendorentity == "Others")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity.Contains(advancevendorentity)).Where(x => x.VendorVendorName.Contains(advancevendorname)
                    || x.VendorRegisteredAddress.Contains(AdvanceVendorAddress))
                                 .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }
                else if (advancevendorentity == "Partnership Firm")
                {
                    Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                    var result = /*from tblVendorMaster in */db.tblVendorMasters.Where(x => x.VendorTypeofEntity.Contains(advancevendorentity)).Where(x => x.VendorVendorName.Contains(advancevendorname)
                    || x.VendorAuthorisedSignatory.Contains(advancevendorAuthSign) || x.VendorRegisteredAddress.Contains(AdvanceVendorRegisteredAddress)
                    || x.Partner1Name.Contains(AdvanceVendorName1P) || x.Partner1Age == AdvanceVendorAge1P || x.Partner1FatherName.Contains(AdvanceVendorFather1P)
                    || x.Partner1Address.Contains(AdvanceVendorAddress1P))
                                 .Select(x => new { x.VendorVendorID, x.VendorVendorName, x.VendorTypeofEntity, x.VendorRegisteredAddress });
                    Logger.Info("Accessed DB, Checked VendorMaster Details:VendorMaster Details Found ");
                    return Json(result);
                }

                return Json("");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'gettblVendorAdvanced' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult gettblClusterFunction(string cluster)
        {
            Logger.Info("Attempt Search gettblClusterFunction");
            try
            {
                cluster = cluster.Trim();
                Logger.Info("Accessing DB for SubCategory List: checking Status ");
                var result = db.tblSubCategories.Where(x => x.CategoryName.Contains(cluster)).Select(x => new {x.SubCategoryID, x.CategoryName, x.SubCategoryName });
                // Where(x => x.VendorCorporateIdentificationNumber.ToString().Contains(advancevendorcin))
               // select tblSubCategory;
                //  return Json(result.ToList()[0]);
                Logger.Info("Accessed DB, Checked SubCategory List:SubCategory List Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'gettblClusterFunction' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }
        [HttpPost]
        public ActionResult gettblFunctionCluster(string cluster)
        {
            Logger.Info("Attempt Search gettblClusterFunction");
            try
            {
                cluster = cluster.Trim();
                Logger.Info("Accessing DB for SubCategory List: checking Status ");
                var result = db.tblSubCategories.Where(x => x.SubCategoryName.Contains(cluster)).Select(x => new {x.SubCategoryID, x.CategoryName, x.SubCategoryName });
                                 // Where(x => x.VendorCorporateIdentificationNumber.ToString().Contains(advancevendorcin))
                                 //select tblSubCategory;
                                 //  return Json(result.ToList()[0]);
                Logger.Info("Accessed DB, Checked SubCategory List:SubCategory List Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'gettblClusterFunction' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        //----------------------------Contract search------------------------------------//
        [HttpPost]
        public JsonResult GetContractType()
        {
            Logger.Info("Attempt Search GetContractType");
            try
            {
                Logger.Info("Accessing DB for ContractMaster Details: checking Status ");
                var contracttype = from tblContractMaster in db.tblContractMasters select tblContractMaster.ContractType;
                var r = contracttype.Distinct();
                Logger.Info("Accessed DB, Checked ContractMaster Details:Contract Type Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GetContractType' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult GetContractCluster()
        {

            Logger.Info("Attempt Search GetContractCluster");
            try
            {
                Logger.Info("Accessing DB for ContractMaster Details: checking Status ");
                var ContractCategory = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                var r = ContractCategory.Distinct();
                Logger.Info("Accessed DB, Checked ContractMaster Details:Category Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GetContractCluster' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult GetContractFunction()
        {
            Logger.Info("Attempt Search GetContractFunction");
            try
            {
                Logger.Info("Accessing DB for ContractMaster Details: checking Status ");
                var ContractSubCategory = from tblSubCategory in db.tblSubCategories select tblSubCategory.SubCategoryName;
                var r = ContractSubCategory.Distinct();
                Logger.Info("Accessed DB, Checked ContractMaster Details:SubCategory Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GetContractFunction' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public ActionResult GettblContractBasic(string contractname, string contracttype, string contractUID, string contractcluster, string contractfunction, string contractdescription)
        {
            contracttype = contracttype.Trim();
            contractcluster = contractcluster.Trim();
            contractfunction = contractfunction.Trim();

            if (contractname == "")
            {
                contractname = null;
            }
            if (contracttype == "")
            {
                contracttype = null;
            }
            if (contractUID == "")
            {
                contractUID = null;
            }
            if (contractcluster == "")
            {
                contractcluster = null;
            }
            if (contractfunction == "")
            {
                contractfunction = null;
            }
            if (contractdescription == "")
            {
                contractdescription = null;
            }

            Logger.Info("Attempt Search GettblContractBasic");
            try
            {

                Logger.Info("Accessing DB for ContractMaster Details: checking Status ");
                var result = from tblContractMaster in db.tblContractMasters.Where(x => x.ContractName.Contains(contractname)
                             || x.ContractType.Contains(contracttype) || x.ContractCategory.Contains(contractcluster)
                             || x.ContractSubCategory.Contains(contractfunction) || x.ContractID.ToString() == contractUID ||
                             x.Description.Contains(contractdescription))
                             select tblContractMaster;
                Logger.Info("Accessed DB, Checked ContractMaster Details:ContractMaster Details Found ");
                return Json(result);

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GettblContractBasic' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }

        }

        [HttpPost]
        public ActionResult GettblContractAdvanced(string contractname, string contracttype, string contractUID, string contractcluster, string contractfunction, string contractdescription, string contractvendor, string DurationDate, string DurationDateTo)
        {
            contracttype = contracttype.Trim();
            contractcluster = contractcluster.Trim();
            contractfunction = contractfunction.Trim();
            contractvendor = contractvendor.Trim();
            if (contractname == "")
            {
                contractname = null;
            }
            if (contracttype == "")
            {
                contracttype = null;
            }
            if (contractUID == "")
            {
                contractUID = null;
            }
            if (contractcluster == "")
            {
                contractcluster = null;
            }
            if (contractfunction == "")
            {
                contractfunction = null;
            }
            if (contractdescription == "")
            {
                contractdescription = null;
            }

            //if (contractvendor == "")
            //{
            //    contractvendor = null;
            //}
            //if (DurationDate == "")
            //{
            //    DurationDate = null;
            //}
            //if (DurationDateTo == "")
            //{
            //    DurationDateTo = null;
            //}
            Logger.Info("Attempt Search GettblContractAdvanced");
            try
            {


                if (contractvendor.Length > 0)
                {
                    try
                    {

                        //string vendor = "";
                        var vendor = db.tblVariableDatas.Where(x => x.Value == contractvendor).Where(x => x.Type == "Contract").
                                  Where(x => x.Version == "Contract").Select(x => new { x.TypeID });
                        foreach (var item in vendor)
                        {
                            Logger.Info("Accessing DB for ContractMaster Details: checking Status ");
                            var result = from tblContractMaster in db.tblContractMasters.Where(x => x.ContractName.Contains(contractname)
                                         || x.ContractType.Contains(contracttype) || x.ContractCategory.Contains(contractcluster) ||
                                         x.ContractSubCategory.Contains(contractfunction) || x.ContractID.ToString() == contractUID ||
                                         x.Description.Contains(contractdescription) || x.ContractID == item.TypeID)
                                         select tblContractMaster;
                            Logger.Info("Accessed DB, Checked ContractMaster Details:ContractMaster Details Found ");

                            return Json(result);
                        }

                    }
                    catch { return Json(""); }
                }
                if (DurationDate.Length > 0 || DurationDateTo.Length > 0)
                {
                    //string fromdate = "";
                    //fromdate = (from tblVariableData in db.tblVariableDatas.Where(x => x.Value == DurationDate) select tblVariableData.TypeID.ToString()).First();

                    //string fromto = "";
                    //fromto = (from tblVariableData in db.tblVariableDatas.Where(x => x.Value == DurationDateTo) select tblVariableData.TypeID.ToString()).First();

                    Logger.Info("Accessing DB for ContractMaster Details: checking Status ");
                    var result = from tblContractMaster in db.tblContractMasters.Where(x => x.InEffectFrom.Contains(DurationDate)).
                            Where(x => x.InEffectTo.Contains(DurationDateTo))
                                 select tblContractMaster;
                    Logger.Info("Accessed DB, Checked ContractMaster Details:ContractMaster Details Found ");
                    return Json(result);
                }

                else
                {
                    Logger.Info("Accessing DB for ContractMaster Details: checking Status ");
                    var result = from tblContractMaster in db.tblContractMasters.Where(x => x.ContractName.Contains(contractname)
                                 || x.ContractType.Contains(contracttype) || x.ContractCategory.Contains(contractcluster) ||
                                 x.ContractSubCategory.Contains(contractfunction) || x.ContractID.ToString() == contractUID
                                 || x.Description.Contains(contractdescription))
                                 select tblContractMaster;
                    Logger.Info("Accessed DB, Checked ContractMaster Details:ContractMaster Details Found ");
                    return Json(result);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GettblContractAdvanced' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        //-------------------------Template-------------------------//
        [HttpPost]
        public JsonResult GetTemplateType()
        {
            Logger.Info("Attempt Search GetTemplateType");
            try
            {
                Logger.Info("Accessing DB for TemplateMaster Details: checking Status ");
                var templatetype = from tblTemplateMaster in db.tblTemplateMasters select tblTemplateMaster.Type;
                var r = templatetype.Distinct();
                Logger.Info("Accessed DB, Checked TemplateMaster Details:Template type Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GetTemplateType' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult GetTemplateCluster()
        {
            Logger.Info("Attempt Search GetTemplateCluster");
            try
            {
                Logger.Info("Accessing DB for TemplateMaster Details: checking Status ");
                var templateCategory = from tblTemplateMaster in db.tblTemplateMasters select tblTemplateMaster.Category;
                var r = templateCategory.Distinct();
                Logger.Info("Accessed DB, Checked TemplateMaster Details:Category Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GetTemplateCluster' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult GetTemplateFunction()
        {
            Logger.Info("Attempt Search GetTemplateFunction");
            try
            {
                Logger.Info("Accessing DB for TemplateMaster Details: checking Status ");
                var templateSubCategory = from tblTemplateMaster in db.tblTemplateMasters select tblTemplateMaster.SubCategory;
                var r = templateSubCategory.Distinct();
                Logger.Info("Accessed DB, Checked TemplateMaster Details:SubCategory Details Found ");
                return Json(r);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GetTemplateFunction' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult GettblTemplateBasic(string templatename, string templatetype)
        {
            Logger.Info("Attempt Search GettblTemplateBasic");
            try
            {

                templatetype = templatetype.Trim();
                Logger.Info("Accessing DB for TemplateMaster Details: checking Status ");
                var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.Name.Contains(templatename)).
                        Where(x => x.Type.Contains(templatetype))
                             select tblTemplateMaster;
                Logger.Info("Accessed DB, Checked TemplateMaster Details:TemplateMaster Details Found ");
                return Json(result);

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GettblTemplateBasic' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }

        }
       
        [HttpPost]
        public ActionResult GettblTemplateAdvanced(string templatename, string templatetype)
        {
            Logger.Info("Attempt Search GettblTemplateAdvanced");
            try
            {

                templatetype = templatetype.Trim();
                
                Logger.Info("Accessing DB for TemplateMaster Details: checking Status ");
                var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.Name.Contains(templatename)).
                        Where(x => x.Type.Contains(templatetype))
                             select tblTemplateMaster;
                Logger.Info("Accessed DB, Checked TemplateMaster Details:TemplateMaster Details Found ");
                return Json(result);

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Search' Controller , 'GettblTemplateAdvanced' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }

        }

        [HttpPost]
        public ActionResult TemplateList(string[] TemplateID)
        {
            if (TemplateID.Length > 0)
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, "Template.csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                }
                System.IO.File.WriteAllText(file, "");
                System.IO.File.AppendAllText(file, "Template ID" + " , " + "Template Name" + " , " + "Template Type"
                                + " , " + "Initiated By" + " , " + "Template Link" + Environment.NewLine);

                for (int i = 0; i < TemplateID.Length; i++)
                {
                    string ID = TemplateID[i];
                    var TemplateTable = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID.ToString() == ID) select tblTemplateMaster;
                    if (TemplateTable.ToList().Count > 0)
                    {
                        foreach (var item in TemplateTable)
                        {
                            System.IO.File.AppendAllText(file, item.TemplateID + " , " + item.Name + " , " + item.Type
                                + " , " + item.Initiator + " , " + ApplicationLink + "/Template/Details/" + item.TemplateID + Environment.NewLine);
                        }
                    }
                }



            }
            return null;
        }


        public ActionResult TemplateList()
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"];
            path = Path.Combine(path, "Reports");

            var file = Path.Combine(path, "Template.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CMS Template Report.csv");

        }
        [HttpPost]
        public ActionResult ContractList(string[] ContractID)
        {
            if (ContractID.Length > 0)
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, "Contract.csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                }

                System.IO.File.WriteAllText(file, "");
                System.IO.File.AppendAllText(file, "Contract ID" + " , " + "Contract Name" + " , " + "Contract Type"
                                + " , " + "Cluster" + " , " + "Function" + " , " + "Initiated By"
                                + " , " + "Duration From" + " , " + "Duration To" + " , " + "Contract Link" + Environment.NewLine);

                for (int i = 0; i < ContractID.Length; i++)
                {
                    string ID = ContractID[i];
                    var ContractTable = from tblContractMaster in db.tblContractMasters.Where(x => x.ContractID.ToString() == ID) select tblContractMaster;
                    if (ContractTable.ToList().Count > 0)
                    {
                        foreach (var item in ContractTable)
                        {
                            System.IO.File.AppendAllText(file, item.ContractID + " , " + item.ContractName + " , " + item.ContractType
                                + " , " + item.ContractCategory + " , " + item.ContractSubCategory + " , " + item.Initiator
                                + " , " + item.InEffectFrom + " , " + item.InEffectTo + " , " + ApplicationLink + "/Contract/Details/" + item.ContractID + Environment.NewLine);
                        }
                    }
                }

            }
            return null;
        }


        public ActionResult ContractList()
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");
            var file = Path.Combine(path, "Contract.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CMS Contract Report.csv");

        }

        [HttpPost]
        public ActionResult VendorList(string[] VendorID)
        {
            if (VendorID.Length > 0)
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, "Vendor.csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                }
                System.IO.File.WriteAllText(file, "");
                System.IO.File.AppendAllText(file, "Vendor ID" + " , " + "Vendor Name" + " , " + "Type of Entity" + " , " + "Corporate Identification Number"
                                    + " , " + "Authorized Signatory" + " , " + "Registered Address" + " , " + "Branch 1 Address" + " , " + "Branch 2 Address"
                                    + " , " + "Branch 3 Address" + " , " + "Branch 4 Address" + " , " + "Branch 5 Address" + Environment.NewLine);

                for (int i = 0; i < VendorID.Length; i++)
                {
                    string ID = VendorID[i];
                    var VendorTable = from tblVendorMaster in db.tblVendorMasters.Where(x => x.VendorVendorID.ToString() == ID) select tblVendorMaster;
                    if (VendorTable.ToList().Count > 0)
                    {
                        foreach (var item in VendorTable)
                        {

                            string VednorAddress = item.VendorRegisteredAddress;
                            if (!string.IsNullOrWhiteSpace(VednorAddress))
                            {
                                VednorAddress = VednorAddress.Replace(",", " ");
                            }
                            string vendorname = item.VendorVendorName;
                            if (!string.IsNullOrWhiteSpace(vendorname))
                            {
                                vendorname = vendorname.Replace(",", " ");
                            }
                            string typeofentity = item.VendorTypeofEntity;
                            if (!string.IsNullOrWhiteSpace(typeofentity))
                            {
                                typeofentity = typeofentity.Replace(",", " ");
                            }
                            string authorisedsign = item.VendorAuthorisedSignatory;
                            if (!string.IsNullOrWhiteSpace(authorisedsign))
                            {
                                authorisedsign = authorisedsign.Replace(",", " ");
                            }
                            string address1 = item.VendorBranchOffice1;
                            if (!string.IsNullOrWhiteSpace(address1))
                            {
                                address1 = address1.Replace(",", " ");
                            }
                            string address2 = item.VendorBranchOffice2;
                            if (!string.IsNullOrWhiteSpace(address2))
                            {
                                address2 = address2.Replace(",", " ");
                            }
                            string address3 = item.VendorBranchOffice3;
                            if (!string.IsNullOrWhiteSpace(address3))
                            {
                                address3 = address3.Replace(",", " ");
                            }
                            string address4 = item.VendorBranchOffice4;
                            if (!string.IsNullOrWhiteSpace(address4))
                            {
                                address4 = address4.Replace(",", " ");
                            }
                            string address5 = item.VendorBranchOffice5;
                            if (!string.IsNullOrWhiteSpace(address5))
                            {
                                address5 = address5.Replace(",", " ");
                            }

                            System.IO.File.AppendAllText(file, item.VendorVendorID + " , " + vendorname + " , " + typeofentity + " , " + item.VendorCorporateIdentificationNumber
                                    + " , " + authorisedsign + " , " + VednorAddress + " , " + address1 + " , " + address2
                                    + " , " + address3 + " , " + address4 + " , " + address5 + Environment.NewLine);
                        }
                    }
                }

            }
            return null;
        }


        public ActionResult VendorList()
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");
            var file = Path.Combine(path, "Vendor.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CMS Vendor Report.csv");

        }

        [HttpPost]
        public ActionResult UserList(string[] UserEmployeeID)
        {
            if (UserEmployeeID.Length > 0)
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, "Users.csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                }
                System.IO.File.WriteAllText(file, "");
                System.IO.File.AppendAllText(file, "Employee ID" + " , " + "Employee Name" + " , " + "Employee Email Address" + " , " + "Employee Designation"
                                + " , " + "Cluster" + " , " + "Function" + " , " + "Status"
                                + " , " + "Hod Employee ID" + " , " + "Hod Employee Name" + " , " + "Hod Employee Email Address" + " , " + "Admin Role" + " , " + "Initiator Role"
                                + " , " + "Approver Role" + " , " + "Legal Role" + " , " + "Finance Role" + Environment.NewLine);

                for (int i = 0; i < UserEmployeeID.Length; i++)
                {
                    string ID = UserEmployeeID[i];
                    var UsersTable = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == ID) select tblUserMaster;
                    if (UsersTable.ToList().Count > 0)
                    {
                        foreach (var item in UsersTable)
                        {
                            string Admin = "";
                            string Initiator = "";
                            string Approver = "";
                            string Legal = "";
                            string Finance = "";
                            if (item.UserRoleAdmin == true)
                            {
                                Admin = Admin + "Yes";
                            }
                            else
                            {
                                Admin = Admin + "No";
                            }
                            if (item.UserRoleInitiator == true)
                            {
                                Initiator = Initiator + "Yes";
                            }
                            else
                            {
                                Initiator = Initiator + "No";
                            }
                            if (item.UserRoleApprover == true)
                            {
                                Approver = Approver + "Yes";
                            }
                            else
                            {
                                Approver = Approver + "No";
                            }
                            if (item.UserRoleLegal == true)
                            {
                                Legal = Legal + "Yes";
                            }
                            else
                            {
                                Legal = Legal + "No";
                            }
                            if (item.UserRoleFinance == true)
                            {
                                Finance = Finance + "Yes";
                            }
                            else
                            {
                                Finance = Finance + "No";
                            }


                            System.IO.File.AppendAllText(file, item.UserEmployeeID + " , " + item.UserEmployeeName + " , " + item.UserEmployeeEmail + " , " + item.UserEmployeeDesignation
                                + " , " + item.UserCategory + " , " + item.UserSubCategory + " , " + item.UserStatus
                                + " , " + item.UserHodEmployeeID + " , " + item.UserHodEmployeeName + " , " + item.UserHodEmployeeEmailAddress + " , " + Admin + " , " + Initiator
                                + " , " + Approver + " , " + Legal + " , " + Finance + Environment.NewLine);
                        }
                    }
                }


            }
            return null;
        }

        public ActionResult UserList()
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");
            var file = Path.Combine(path, "Users.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CMS Users Report.csv");

        }

        [HttpPost]
        public ActionResult SubCategoryList(string[] SubCategoryID)
        {
            if (SubCategoryID.Length > 0)
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, "Cluster & Function.csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                }
                System.IO.File.WriteAllText(file, "");
                System.IO.File.AppendAllText(file, "ID" + " , " + "Cluster Name" + " , " + "Function Name" + Environment.NewLine);


                for (int i = 0; i < SubCategoryID.Length; i++)
                {
                    string ID = SubCategoryID[i];
                    var SubCategoryTable = from tblSubCategory in db.tblSubCategories.Where(x => x.SubCategoryID.ToString() == ID) select tblSubCategory;
                    if (SubCategoryTable.ToList().Count > 0)
                    {
                        foreach (var item in SubCategoryTable)
                        {
                            System.IO.File.AppendAllText(file, item.SubCategoryID + " , " + item.CategoryName + " , " + item.SubCategoryName + Environment.NewLine);
                        }
                    }
                }

            }

            return null;
        }

        public ActionResult SubCategoryList()
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");
            var file = Path.Combine(path, "Cluster & Function.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CMS Cluster & Function Report.csv");

        }

        [HttpPost]
        public ActionResult ClauseList(string[] ClauseID)
        {
            if (ClauseID.Length > 0)
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, "Clause.csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                }
                System.IO.File.WriteAllText(file, "");
                System.IO.File.AppendAllText(file, "ID" + " , " + "Clause Title" + " , " + "Description" + Environment.NewLine);


                for (int i = 0; i < ClauseID.Length; i++)
                {
                    string ID = ClauseID[i];
                    var ClauseTable = from tblClauseMaster in db.tblClauseMasters.Where(x => x.ClauseClauseID.ToString() == ID) select tblClauseMaster;
                    if (ClauseTable.ToList().Count > 0)
                    {
                        foreach (var item in ClauseTable)
                        {
                            string ClauseTitle = item.ClauseClauseTitle;
                            if (!string.IsNullOrWhiteSpace(ClauseTitle))
                            {
                                ClauseTitle = ClauseTitle.Replace(",", " ");
                            }
                            string ClauseDesc = item.ClauseClauseDescription;
                            if (!string.IsNullOrWhiteSpace(ClauseDesc))
                            {
                                ClauseDesc = ClauseDesc.Replace(",", " ");
                            }
                            System.IO.File.AppendAllText(file, item.ClauseClauseID + " , " + ClauseTitle + " , " + ClauseDesc + Environment.NewLine);
                        }
                    }
                }

            }

            return null;
        }

        public ActionResult ClauseList()
        {
            string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Reports");
            var file = Path.Combine(path, "Clause.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CMS Clause Report.csv");

        }
    }
}