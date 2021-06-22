using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{
    public class AuditTrailController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: AuditTrail

        private int id { get; set; }
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

        public ActionResult ClauseAudit()
        {
            if (User.IsInRole("reviewer"))
            {
                return RedirectToAction("Repository", "Contract");
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
           
            //Logger.Info("Accessing the ClauseAudit New Page");
            //CommonModel model = new CommonModel
            //{
            //    Clause = db.tblClauseMasters,
            //    Template = db.tblTemplateMasters,

            //    ClauseAudit = db.tblClauseAudits,
            //    TemplateAudit = db.tblTemplateAudits,
            //    ContractAudit = db.tblContractAudits,
            //    Contract = db.tblContractMasters
            //};
            //Logger.Info("Accesed DB, Checking User Details: User Found");
            //return View(model);
        }

        //public ActionResult Index()
        //{
        //    return RedirectToAction("ClauseAudit");
        //}

        //[HttpPost]
        //public ActionResult getReviewClauseType()
        //{
        //    Logger.Info("Attempt UID Audit ReviewClausetype");

        //    Logger.Info("Accessing DB for Audit ReviewClausetype");
        //    var result = from tblClauseMaster in db.tblClauseMasters select tblClauseMaster.ClauseClauseType;
        //    var type = result.Distinct();
        //    Logger.Info("Accesed DB, Checking User Details: User Found");
        //    return Json(type);
        //}



        [HttpPost]
        public ActionResult getClauseTableDetails(int clauseID)
        {
            Logger.Info("Attempt AuditTrail getClauseTableDetails");
            try
            {
                Logger.Info("Accesing DB for Clause Master Details");
                var result = from tblClauseMaster in db.tblClauseMasters.Where(x => x.ClauseClauseID == clauseID) select tblClauseMaster;

                Logger.Info("Accessed DB, Checking Clause Master Details: Details Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getClauseTableDetails' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ClauseReviewEditSave(int clauseid, int AuditClauseID, string clausetitle,string clausetext, string clausedescription, string AuditComments)
        {
            Logger.Info("Attempt AuditTrail ClauseReviewEditSave");
            try
            {
                Logger.Info("Accessing DB for Updating the ClauseMaster Records ");
                tblClauseMaster m = new tblClauseMaster
                {
                    ClauseClauseID = clauseid,
                    ClauseClauseTitle = clausetitle,
                    ClauseClauseDescription = clausedescription,
                    //m.ClauseClauseType = clausetype;
                    ClauseClauseText = clausetext,
                    ClauseLastModified = DateTime.Now,
                    ClauseStatus = "Verified"
                };

                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                Logger.Info("Accesed DB, Clause Master Records Updated");

                Logger.Info("Accessing DB for Saving the ClauseAudit Log Details");
                tblClauseAudit audit = new tblClauseAudit
                {
                    AuditClauseComments = AuditComments,
                    AuditClauseID = AuditClauseID,
                    AuditLastReviewed = DateTime.Now,
                    AuditStatus = "Verified"
                };

                db.tblClauseAudits.Add(audit);
                db.SaveChanges();

                Logger.Info("Accessed DB, ClauseAudit Log Details Saved");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'ClauseReviewEditSave' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            return Json("success");
        }

        [HttpPost]

        public ActionResult DeleteReviewClause(int DeleteClauseID)
        {
            Logger.Info("Attempt AuditTrail DeleteReviewClause");
            try
            {
                Logger.Info("Accessing DB for Deleting the ClauseMaster Records ");
                tblClauseMaster tblClauseMaster = db.tblClauseMasters.Find(DeleteClauseID);
                db.tblClauseMasters.Remove(tblClauseMaster);
                db.SaveChanges();
                Logger.Info("Accesed DB, ClauseMaster Record Deleted");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'DeleteReviewClause' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
            }

        }

        //-------------------------TemplateAudit-----------------//
        [HttpPost]
        public ActionResult getReviewtemplateType()
        {
            Logger.Info("Attempt AuditTrail getReviewtemplateType");
            try
            {
                Logger.Info("Accessing DB for Template Details: checking Status ");
                var result = from tblTemplateMaster in db.tblTemplateMasters select tblTemplateMaster.Type;
           
            var type = result.Distinct();
                Logger.Info("Accessed DB, Checking Template Details:Template Type Found ");
                return Json(type);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getReviewtemplateType' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getTemplateCluster()
        {
            Logger.Info("Attempt AuditTrail getTemplateCluster");
            try
            {
                Logger.Info("Accessing DB for Category List:  checking Status ");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category List: Category List Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getTemplateCluster' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getTemplateFunction(string user_category_id)
        {
            Logger.Info("Attempt AuditTrail getTemplateFunction");
            try
            {
                Logger.Info("Accessing DB for SubCategory List: Checking Status ");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == user_category_id) select tblSubCategory.SubCategoryName;
            foreach (var r in result)
            {
               
                Console.WriteLine(r);
            }
                Logger.Info("Accessing DB, Checking Category List : Category List Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getTemplateFunction 'Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public ActionResult getTemplateTableDetails(int templateID)
        {
            Logger.Info("Attempt AuditTrail getTemplateTableDetails");
            try
            {
                Logger.Info("Accessing DB for Template details : templateID Match");
                var result = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == templateID) select tblTemplateMaster;
                Logger.Info("Accesed DB, Checked Template details : Details Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getTemplateTableDetails' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TemplateReviewEditSave(int templateid, int AuditTemplateID, string templatename, string templatetype, string templatecluster, string templatefunction, string templatedescription, string AuditComments)
        {
            Logger.Info("Attempt AuditTrail TemplateReviewEditSave");
            try
            {
                Logger.Info("Accessing DB for Saving Template Review Details");
                tblTemplateMaster m = new tblTemplateMaster
                {
                TemplateID = templateid,
                Name = templatename,
                Type = templatetype,
                Category = templatecluster,
                SubCategory = templatefunction,
                Description = templatedescription,
                LastReviewed = DateTime.Now,
                Status = "Verified"
                };

                db.Entry(m).State = EntityState.Modified;
               
                db.SaveChanges();
                Logger.Info("Accesed DB, Template Review Details Saved");

                Logger.Info("Accessing DB for Saving the TemplateAudit Log Details");
                tblTemplateAudit template = new tblTemplateAudit
                {
                    AuditTemplateComments = AuditComments,
                    AuditTemplateID = AuditTemplateID,
                    AuditLastReviewed = DateTime.Now,
                    AuditStatus = "Verified"
                };

                db.tblTemplateAudits.Add(template);
               
                db.SaveChanges();
                Logger.Info("Accessed DB, Template Audit Log Details Saved");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'TemplateReviewEditSave' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }

        [HttpPost]

        public ActionResult DeleteReviewTemplate(int DeleteTemplateID)
        {
            Logger.Info("Attempt AuditTrail DeleteReviewTemplate");
            try
            {
                Logger.Info("Accessing DB for Deleting the Template Records");
                tblTemplateMaster tblTemplateMaster = db.tblTemplateMasters.Find(DeleteTemplateID);
                db.tblTemplateMasters.Remove(tblTemplateMaster);
                db.SaveChanges();

                Logger.Info("Accessed DB, Template Record Deleted");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'DeleteReviewTemplate' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
            }
        }


        //-------------------------Contract Audit-----------------//
        [HttpPost]
        public ActionResult getReviewContractType()
        {
            Logger.Info("Attempt AuditTrail getReviewContractType");
            try
            {
                Logger.Info("Accessing DB for Contract Details:  checking Status ");
                var result = from tblContractMaster in db.tblContractMasters select tblContractMaster.ContractType;
            var type = result.Distinct();
           
                Logger.Info("Accessed DB, Checking Contract Details : Details Found ");
                return Json(type);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getReviewContractType' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult getContractCluster()
        {
            Logger.Info("Attempt AuditTrail getContractCluster");
            try
            {
                Logger.Info("Accessing DB for Category List:  checking Status ");
                var result = from tblCategory in db.tblCategories select tblCategory.CategoryName;
                Logger.Info("Accessed DB, Checking Category List : Category List Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getContractCluster' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public JsonResult getContractFunction(string user_category_id)
        {
            Logger.Info("Attempt AuditTrail getContractFunction");
            try
            {
                Logger.Info("Accessing DB for SubCategory List: Checking Status ");
                var result = from tblSubCategory in db.tblSubCategories.Where(x => x.CategoryName == user_category_id) select tblSubCategory.SubCategoryName;
            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

                Logger.Info("Accessing DB, Checking SubCategory List : SubCategory List Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getContractFunction' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public ActionResult getContractTableDetails(int ContractID)
        {
            Logger.Info("Attempt AuditTrail getContractTableDetails");
            try
            {
                Logger.Info("Accessing DB for Contract details :ContractID Match");
                var result = from tblContractMaster in db.tblContractMasters.Where(x => x.ContractID == ContractID) select tblContractMaster;
                Logger.Info("Accesed DB, Checking Contract details : Details Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'getContractTableDetails' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ContractReviewEditSave(int Contractid, int AuditContractID, string Contractname, string Contracttype, string Contractcluster, string Contractfunction, string Contractdescription, string AuditComments)
        {
            Logger.Info("Attempt AuditTrail ContractReviewEditSave");
            try
            {
                Logger.Info("Accessing DB for Saving the ContractMaster Details");
                tblContractMaster m = new tblContractMaster
                {
                    ContractID = Contractid,
                    ContractName = Contractname,
                    ContractType = Contracttype,
                    ContractCategory = Contractcluster,
                    ContractSubCategory = Contractfunction,
                    Description = Contractdescription,
                    LastReviewed = DateTime.Now,
                    Status = "Verified"
                };

                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                Logger.Info("Accessed DB, Contract Details Saved");

                Logger.Info("Accessing DB for Saving the ContractAudit Log Details");
                tblContractAudit contract = new tblContractAudit
                {
                    AuditContractComments = AuditComments,
                    AuditContractD = AuditContractID,
                    AuditLastReviewed = DateTime.Now,
                    AuditStatus = "Verified"
                };

                db.tblContractAudits.Add(contract);
                db.SaveChanges();
                Logger.Info("Accessed DB, ContractAudit Log Details Saved");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'ContractReviewEditSave' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }

        [HttpPost]

        public ActionResult DeleteReviewContract(int DeleteContractID)
        {
            Logger.Info("Attempt AuditTrail DeleteReviewContract");
            try
            {
                Logger.Info("Accessing DB for Deleting the ContractMaster Records ");
                tblContractMaster tblContractMaster = db.tblContractMasters.Find(DeleteContractID);
                db.tblContractMasters.Remove(tblContractMaster);
                db.SaveChanges();
                Logger.Info("Accesed DB, Contract Records Deleted Records");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AuditTrail' Controller , 'DeleteReviewContract' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json(Ex.Message);
            }

        }
    }
}