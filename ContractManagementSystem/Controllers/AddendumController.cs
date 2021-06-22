using ContractManagementSystem.Models;
using NLog;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace ContractManagementSystem.Controllers
{
    public class AddendumController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        readonly string ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];
        // GET: Addendum
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
        public ActionResult Repository()
        {

            int CurrentUserLoggedID = 0;
            try
            {
                CurrentUserLoggedID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            }
            catch { }

            //db.tblContractModifications.ToList()
            Logger.Info("Accessing Contract Repository Page");
            Logger.Info("Accessing DB for Repository");
            IQueryable<AddendumRepositoryModel> Addendumlist = null;

            if (User.IsInRole("reviewer"))
            {
                Addendumlist = (from x in db.tblContractModifications
                                where x.Status == "In Effect" || x.Status == "Expired"
                                select new AddendumRepositoryModel()
                                {
                                    ContractID = x.ContractID,
                                    ContractName = x.ContractName,
                                    ContractType = x.ContractType,
                                    ContractCategory = x.ContractCategory,
                                    ContractSubCategory = x.ContractSubCategory,
                                    Status = x.Status,
                                    DateofInitiated = x.DateofInitiated,
                                    NextApprover = x.NextApprover,
                                    Initiator = x.Initiator,
                                    RejectedBy = x.RejectedBy,
                                    InEffectTo = x.InEffectTo,
                                    ContractModificationType = x.ContractModificationType,
                                    OriginalContractID = x.OriginalContractID,
                                    PhysicalCopyLocation = x.PhysicalCopyLocation
                                }).AsQueryable();


            }
            else
            {
                Addendumlist = (from x in db.tblContractModifications
                                where
                                    (((x.Status == "Draft") && ((x.Initiator) == CurrentUserLoggedID)) ||
                                    (x.Status == "Approved") && (((x.Initiator) == CurrentUserLoggedID) || ((x.Approver1ID) == CurrentUserLoggedID) || ((x.Approver2ID) == CurrentUserLoggedID) || ((x.Approver3ID) == CurrentUserLoggedID) || ((x.Approver4ID) == CurrentUserLoggedID) || ((x.Approver5ID) == CurrentUserLoggedID) || ((x.Approver6ID) == CurrentUserLoggedID) || ((x.Approver7ID) == CurrentUserLoggedID) || ((x.Approver8ID) == CurrentUserLoggedID) || ((x.Approver9ID) == CurrentUserLoggedID) || ((x.Approver10ID) == CurrentUserLoggedID)) ||
                                    ((x.Status == "Pending Approval") && ((x.Initiator == CurrentUserLoggedID) || (x.NextApprover == CurrentUserLoggedID + ""))) ||
                                    ((x.Status == "Rejected") && (x.NextApprover == CurrentUserLoggedID + "")) ||
                                    ((x.Status == "In Effect") && (((x.Initiator) == CurrentUserLoggedID) || ((x.Approver1ID) == CurrentUserLoggedID) || ((x.Approver2ID) == CurrentUserLoggedID) || ((x.Approver3ID) == CurrentUserLoggedID) || ((x.Approver4ID) == CurrentUserLoggedID) || ((x.Approver5ID) == CurrentUserLoggedID) || ((x.Approver6ID) == CurrentUserLoggedID) || ((x.Approver7ID) == CurrentUserLoggedID) || ((x.Approver8ID) == CurrentUserLoggedID) || ((x.Approver9ID) == CurrentUserLoggedID) || ((x.Approver10ID) == CurrentUserLoggedID)) ||
                                    ((((x.Status == "Expired"))) && (((x.Initiator) == CurrentUserLoggedID) || ((x.Approver1ID) == CurrentUserLoggedID) || ((x.Approver2ID) == CurrentUserLoggedID) || ((x.Approver3ID) == CurrentUserLoggedID) || ((x.Approver4ID) == CurrentUserLoggedID) || ((x.Approver5ID) == CurrentUserLoggedID) || ((x.Approver6ID) == CurrentUserLoggedID) || ((x.Approver7ID) == CurrentUserLoggedID) || ((x.Approver8ID) == CurrentUserLoggedID) || ((x.Approver9ID) == CurrentUserLoggedID) || ((x.Approver10ID) == CurrentUserLoggedID)))
                                    ))

                                select new AddendumRepositoryModel()
                                {
                                    ContractID = x.ContractID,
                                    ContractName = x.ContractName,
                                    ContractType = x.ContractType,
                                    ContractCategory = x.ContractCategory,
                                    ContractSubCategory = x.ContractSubCategory,
                                    Status = x.Status,
                                    DateofInitiated = x.DateofInitiated,
                                    NextApprover = x.NextApprover,
                                    Initiator = x.Initiator,
                                    RejectedBy = x.RejectedBy,
                                    InEffectTo = x.InEffectTo,
                                    ContractModificationType = x.ContractModificationType,
                                    OriginalContractID = x.OriginalContractID,
                                    PhysicalCopyLocation = x.PhysicalCopyLocation
                                }).AsQueryable();


            }


            return View(Addendumlist);
        }
        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Addendum/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            int CurrentUser = 0;
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
            }
            catch { }
            Logger.Info("Accessing DB for Contract Details");
            tblContractModification tblContractModification = db.tblContractModifications.Find(id);

            Logger.Info("Accessed DB, Checking Contract Details: Checking Status");
            if (tblContractModification == null)
            {
                Logger.Info("Unauthorizrd access for draft contract");
                return HttpNotFound();
            }
            if (tblContractModification.Status == "Draft" && tblContractModification.Initiator == CurrentUser)
            {

                Logger.Info("Accessed DB, Checking Contract Details: Details Found");

                Logger.Info("Redirecting to Contract Details Page");
                return View(tblContractModification);
            }
            else if (((tblContractModification.Status == "Pending Approval") || (tblContractModification.Status == "Rejected")) && ((tblContractModification.Initiator == CurrentUser) || (tblContractModification.Approver1ID == CurrentUser) || (tblContractModification.Approver2ID == CurrentUser) || (tblContractModification.Approver3ID == CurrentUser) || (tblContractModification.Approver4ID == CurrentUser) || (tblContractModification.Approver5ID == CurrentUser) || (tblContractModification.Approver6ID == CurrentUser) || (tblContractModification.Approver7ID == CurrentUser) || (tblContractModification.Approver8ID == CurrentUser) || (tblContractModification.Approver9ID == CurrentUser) || (tblContractModification.Approver10ID == CurrentUser)))
            {

                Logger.Info("Accessed DB, Checking Contract Details: Details Found");

                Logger.Info("Redirecting to Contract Details Page");
                return View(tblContractModification);
            }

            else if (((tblContractModification.Status == "Approved") && ((tblContractModification.Initiator == CurrentUser) || (tblContractModification.Approver1ID == CurrentUser) || (tblContractModification.Approver2ID == CurrentUser) || (tblContractModification.Approver3ID == CurrentUser) || (tblContractModification.Approver4ID == CurrentUser) || (tblContractModification.Approver5ID == CurrentUser) || (tblContractModification.Approver6ID == CurrentUser) || (tblContractModification.Approver7ID == CurrentUser) || (tblContractModification.Approver8ID == CurrentUser) || (tblContractModification.Approver9ID == CurrentUser) || (tblContractModification.Approver10ID == CurrentUser))))
            {

                Logger.Info("Accessed DB, Checking Contract Details: Details Found");

                Logger.Info("Redirecting to Contract Details Page");
                return View(tblContractModification);
            }
            else if (((tblContractModification.Status == "In Effect") && ((tblContractModification.Initiator == CurrentUser) || (tblContractModification.Approver1ID == CurrentUser) || (tblContractModification.Approver2ID == CurrentUser) || (tblContractModification.Approver3ID == CurrentUser) || (tblContractModification.Approver4ID == CurrentUser) || (tblContractModification.Approver5ID == CurrentUser) || (tblContractModification.Approver6ID == CurrentUser) || (tblContractModification.Approver7ID == CurrentUser) || (tblContractModification.Approver8ID == CurrentUser) || (tblContractModification.Approver9ID == CurrentUser) || (tblContractModification.Approver10ID == CurrentUser))))
            {

                Logger.Info("Accessed DB, Checking Contract Details: Details Found");

                Logger.Info("Redirecting to Contract Details Page");
                return View(tblContractModification);
            }
            else if (((tblContractModification.Status == "Expired") && ((tblContractModification.Initiator == CurrentUser) || (tblContractModification.Approver1ID == CurrentUser) || (tblContractModification.Approver2ID == CurrentUser) || (tblContractModification.Approver3ID == CurrentUser) || (tblContractModification.Approver4ID == CurrentUser) || (tblContractModification.Approver5ID == CurrentUser) || (tblContractModification.Approver6ID == CurrentUser) || (tblContractModification.Approver7ID == CurrentUser) || (tblContractModification.Approver8ID == CurrentUser) || (tblContractModification.Approver9ID == CurrentUser) || (tblContractModification.Approver10ID == CurrentUser))))
            {

                Logger.Info("Accessed DB, Checking Contract Details: Details Found");

                Logger.Info("Redirecting to Contract Details Page");
                return View(tblContractModification);
            }
            else if (User.IsInRole("reviewer"))
            {
                Logger.Info("Redirecting to Contract Details Page");
                return View(tblContractModification);
            }
            else
            {
                Logger.Info("Unauthorizrd access of contract");
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteContractFromDB(int ContractID, int CurrentUserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Contract DeleteContractFromDB");
            try
            {
                Logger.Info("Accessing DB for Deleting the Contract Records");
                try
                {
                    var ContractModificationTable = from tblContractModification in db.tblContractModifications.Where(x => x.ContractID == ContractID) select tblContractModification;


                    foreach (var item in ContractModificationTable)
                    {
                        tblContractMaster eachcontract = db.tblContractMasters.Find(item.OriginalContractID);

                        eachcontract.ContractModificationType = "";
                        db.Entry(eachcontract).State = EntityState.Modified;
                    }
                }
                catch { }

                tblContractModification contract = db.tblContractModifications.Find(ContractID);

                db.tblContractModifications.Remove(contract);

                db.SaveChanges();
                Logger.Info("Accessed DB, Contract Record Deleted");

                Logger.Info("Accessing DB for Saving the Contract Log Details");
                tblContractLog log = new tblContractLog
                {
                    LogContractUID = ContractID,
                    ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName,
                    LogActivity = "Deletion",
                    ChangedFrom = "-",
                    ChangedTo = "-",
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblContractLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, Contract Log Record Saved");
                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Contract' Controller , 'DeleteContractFromDB' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateContractInDB(int ContractId, int[] UID, string ContractName, string ContractType, string ContractDescription, string ContractCluster, string ContractFunction, string ContractTemplateType, int CuurrentUserID = 0, int ContractIDforTemplate = 0)
        {
            int CurrentUserID = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUserID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Contract UpdateContractInDB");
            try
            {
                Logger.Info("Accessing DB for Updating the Contract Records");
                tblContractModification eachContract = db.tblContractModifications.Find(ContractId);

                string OldValues = "";
                string NewValues = "";


                if (eachContract.ContractName != ContractName)
                {
                    OldValues = OldValues + "Contract Name : " + eachContract.ContractName + " , ";
                    NewValues = NewValues + "Contract Name : " + ContractName + " , ";
                }
                if (eachContract.ContractType != ContractType)
                {
                    OldValues = OldValues + "Contract Type : " + eachContract.ContractType + " , ";
                    NewValues = NewValues + "Contract Type : " + ContractType + " , ";
                }
                if (eachContract.ContractCategory != ContractCluster)
                {
                    OldValues = OldValues + "Cluster : " + eachContract.ContractCategory + " , ";
                    NewValues = NewValues + "Cluster : " + ContractCluster + " , ";
                }
                if (eachContract.ContractSubCategory != ContractFunction)
                {
                    OldValues = OldValues + "Function : " + eachContract.ContractSubCategory + " , ";
                    NewValues = NewValues + "Function : " + ContractFunction + " , ";
                }
                if (eachContract.Description != ContractDescription)
                {
                    OldValues = OldValues + "Description : " + eachContract.Description + " , ";
                    NewValues = NewValues + "Description : " + ContractDescription + " , ";
                }
                if (eachContract.TemplateType != ContractTemplateType)
                {
                    OldValues = OldValues + "Create Contract Using : " + eachContract.TemplateType + " , ";
                    NewValues = NewValues + "Create Contract Using : " + ContractTemplateType + " , ";
                }

                try
                {

                    if (eachContract.Approver1ID != UID[0])
                    {
                        OldValues = OldValues + "Approver1 ID : " + eachContract.Approver1ID + " , ";
                        NewValues = NewValues + "Approver1 ID : " + UID[0] + " , ";
                    }
                    if (eachContract.Approver2ID != UID[1])
                    {
                        OldValues = OldValues + "Approver2 ID : " + eachContract.Approver2ID + " , ";
                        NewValues = NewValues + "Approver2 ID : " + UID[1] + " , ";
                    }

                    if (eachContract.Approver3ID != UID[2])
                    {
                        OldValues = OldValues + "Approver3 ID : " + eachContract.Approver3ID + " , ";
                        NewValues = NewValues + "Approver3 ID : " + UID[2] + " , ";
                    }

                    if (eachContract.Approver4ID != UID[3])
                    {
                        OldValues = OldValues + "Approver4 ID : " + eachContract.Approver4ID + " , ";
                        NewValues = NewValues + "Approver4 ID : " + UID[3] + " , ";
                    }

                    if (eachContract.Approver5ID != UID[4])
                    {
                        OldValues = OldValues + "Approver5 ID : " + eachContract.Approver5ID + " , ";
                        NewValues = NewValues + "Approver5 ID : " + UID[4] + " , ";
                    }

                    if (eachContract.Approver6ID != UID[5])
                    {
                        OldValues = OldValues + "Approver6 ID : " + eachContract.Approver6ID + " , ";
                        NewValues = NewValues + "Approver6 ID : " + UID[5] + " , ";
                    }

                    if (eachContract.Approver7ID != UID[6])
                    {
                        OldValues = OldValues + "Approver7 ID : " + eachContract.Approver7ID + " , ";
                        NewValues = NewValues + "Approver7 ID : " + UID[6] + " , ";
                    }

                    if (eachContract.Approver8ID != UID[7])
                    {
                        OldValues = OldValues + "Approver8 ID : " + eachContract.Approver8ID + " , ";
                        NewValues = NewValues + "Approver8 ID : " + UID[7] + " , ";
                    }

                    if (eachContract.Approver9ID != UID[8])
                    {
                        OldValues = OldValues + "Approver9 ID : " + eachContract.Approver9ID + " , ";
                        NewValues = NewValues + "Approver9 ID : " + UID[8] + " , ";
                    }

                    if (eachContract.Approver10ID != UID[9])
                    {
                        OldValues = OldValues + "Approver10 ID : " + eachContract.Approver10ID + " , ";
                        NewValues = NewValues + "Approver10 ID : " + UID[9] + " , ";
                    }
                }
                catch { }



                if (UID.Length > 0)
                {
                    eachContract.Approver1ID = UID[0];
                }
                else
                {
                    if (eachContract.Approver1ID != 0)
                    {
                        OldValues = OldValues + "Approver1 ID : " + eachContract.Approver1ID + " , ";
                        NewValues = NewValues + "Approver1 ID : " + "0" + " , ";
                    }
                    eachContract.Approver1ID = 0;

                }
                if (UID.Length > 1)
                {
                    eachContract.Approver2ID = UID[1];
                }
                else
                {
                    if (eachContract.Approver2ID != 0)
                    {
                        OldValues = OldValues + "Approver2 ID : " + eachContract.Approver2ID + " , ";
                        NewValues = NewValues + "Approver2 ID : " + "0" + " , ";
                    }
                    eachContract.Approver2ID = 0;

                }
                if (UID.Length > 2)
                {
                    eachContract.Approver3ID = UID[2];
                }
                else
                {
                    if (eachContract.Approver3ID != 0)
                    {
                        OldValues = OldValues + "Approver3 ID : " + eachContract.Approver3ID + " , ";
                        NewValues = NewValues + "Approver3 ID : " + "0" + " , ";
                    }
                    eachContract.Approver3ID = 0;

                }
                if (UID.Length > 3)
                {
                    eachContract.Approver4ID = UID[3];
                }
                else
                {
                    if (eachContract.Approver4ID != 0)
                    {
                        OldValues = OldValues + "Approver4 ID : " + eachContract.Approver4ID + " , ";
                        NewValues = NewValues + "Approver4 ID : " + "0" + " , ";
                    }
                    eachContract.Approver4ID = 0;

                }
                if (UID.Length > 4)
                {
                    eachContract.Approver5ID = UID[4];
                }
                else
                {
                    if (eachContract.Approver5ID != 0)
                    {
                        OldValues = OldValues + "Approver5 ID : " + eachContract.Approver5ID + " , ";
                        NewValues = NewValues + "Approver5 ID : " + "0" + " , ";
                    }
                    eachContract.Approver5ID = 0;

                }
                if (UID.Length > 5)
                {
                    eachContract.Approver6ID = UID[5];
                }
                else
                {
                    if (eachContract.Approver6ID != 0)
                    {
                        OldValues = OldValues + "Approver6 ID : " + eachContract.Approver6ID + " , ";
                        NewValues = NewValues + "Approver6 ID : " + "0" + " , ";
                    }
                    eachContract.Approver6ID = 0;

                }
                if (UID.Length > 6)
                {
                    eachContract.Approver7ID = UID[6];
                }
                else
                {
                    if (eachContract.Approver7ID != 0)
                    {
                        OldValues = OldValues + "Approver7 ID : " + eachContract.Approver7ID + " , ";
                        NewValues = NewValues + "Approver7 ID : " + "0" + " , ";
                    }
                    eachContract.Approver7ID = 0;

                }
                if (UID.Length > 7)
                {
                    eachContract.Approver8ID = UID[7];
                }
                else
                {
                    if (eachContract.Approver8ID != 0)
                    {
                        OldValues = OldValues + "Approver8 ID : " + eachContract.Approver8ID + " , ";
                        NewValues = NewValues + "Approver8 ID : " + "0" + " , ";
                    }
                    eachContract.Approver8ID = 0;

                }
                if (UID.Length > 8)
                {
                    eachContract.Approver9ID = UID[8];
                }
                else
                {
                    if (eachContract.Approver9ID != 0)
                    {
                        OldValues = OldValues + "Approver9 ID : " + eachContract.Approver9ID + " , ";
                        NewValues = NewValues + "Approver9 ID : " + "0" + " , ";
                    }
                    eachContract.Approver9ID = 0;

                }
                if (UID.Length > 9)
                {
                    eachContract.Approver10ID = UID[9];
                }
                else
                {
                    if (eachContract.Approver10ID != 0)
                    {
                        OldValues = OldValues + "Approver10 ID : " + eachContract.Approver10ID + " , ";
                        NewValues = NewValues + "Approver10 ID : " + "0" + " , ";
                    }
                    eachContract.Approver10ID = 0;

                }

                string TemplateContent = "";
                try
                {
                    Logger.Info("Accessed DB, Checking for Template Details : TemplateID match");
                    TemplateContent = (from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.TemplateID == ContractIDforTemplate) select tblTemplateMaster.Template).First();
                    Logger.Info("Accessed DB, Checking for Template Details : Template Found");
                }
                catch
                {

                }

                eachContract.ContractName = HttpUtility.HtmlEncode(ContractName);
                eachContract.ContractType = HttpUtility.HtmlEncode(ContractType);
                eachContract.ContractCategory = HttpUtility.HtmlEncode(ContractCluster);
                eachContract.ContractSubCategory = HttpUtility.HtmlEncode(ContractFunction);
                eachContract.Description = HttpUtility.HtmlEncode(ContractDescription);
                eachContract.TemplateType = HttpUtility.HtmlEncode(ContractTemplateType);
                //eachContract.OriginalTemplateID = ContractIDforTemplate;

                //if(ContractTemplateType == "Choose From Template")
                //{
                //    eachContract.ContractDraft = TemplateContent;
                //}

                // eachContract.ContractModificationType = "FreshContract";
                // eachContract.Status = "Draft";
                db.Entry(eachContract).State = EntityState.Modified;

                if (OldValues.Length > 0)
                {
                    tblContractLog log = new tblContractLog();
                    log.LogContractUID = eachContract.ContractID;
                    log.ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName;
                    log.LogActivity = "Modified";
                    log.ChangedFrom = OldValues;
                    log.ChangedTo = NewValues;
                    log.DateandTime = DateTime.Now.ToString();

                    db.tblContractLogs.Add(log);
                }

                db.SaveChanges();
                Logger.Info("Accessed DB, Contract Record Updated");


                string[] response = new string[2];
                response[0] = "success";
                response[1] = "" + eachContract.ContractID;

                return Json(response);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Contract' Controller , 'UpdateContractInDB' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ModificationContract(string ModificationType, int ContractID, string[] arrVariableNames, string[] arrVariableValues, int CurrentUser)
        {
            int CurrentUserID = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUserID = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Contract ModificationContract");
            try
            {
                Logger.Info("Accessing DB for Updating the Contract Extension/Modification Details: Checking Status");
                tblContractModification contract = db.tblContractModifications.Find(ContractID);

                tblContractLog log = new tblContractLog();
                log.LogContractUID = contract.ContractID;
                log.ModifiedBy = CurrentUserID.ToString() + " - " + CurrentUserName;
                log.LogActivity = "contract modified";
                log.ChangedFrom = "Current Contract changed to";
                log.ChangedTo = ModificationType;
                log.DateandTime = DateTime.Now.ToString();
                db.tblContractLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accessed DB, contract Log Record Saved");



                tblContractModification modifiedContract = new tblContractModification
                {
                    ContractName = HttpUtility.HtmlEncode(contract.ContractName),
                    ContractType = HttpUtility.HtmlEncode(contract.ContractType),
                    ContractCategory = HttpUtility.HtmlEncode(contract.ContractCategory),
                    ContractSubCategory = HttpUtility.HtmlEncode(contract.ContractSubCategory),
                    Description = HttpUtility.HtmlEncode(contract.Description),
                    TemplateType = HttpUtility.HtmlEncode(contract.TemplateType),
                    OriginalContractID = ContractID,
                    Initiator = contract.Initiator,
                    OriginalTemplateID = contract.OriginalTemplateID,

                    // modifiedContract.ContractDraft = contract.ContractDraft;
                    UploadExistingFile = contract.UploadExistingFile,
                    UploadExistingContractFileName = contract.UploadExistingContractFileName,
                    UploadExistingContractFileType = contract.UploadExistingContractFileType,

                    ContractModificationType = ModificationType,
                    Status = "Draft",

                    Approver1ID = contract.Approver1ID,
                    Approver2ID = contract.Approver2ID,
                    Approver3ID = contract.Approver3ID,
                    Approver4ID = contract.Approver4ID,
                    Approver5ID = contract.Approver5ID,
                    Approver6ID = contract.Approver6ID,
                    Approver7ID = contract.Approver7ID,
                    Approver8ID = contract.Approver8ID,
                    Approver9ID = contract.Approver9ID,
                    Approver10ID = contract.Approver10ID,

                    InEffectFrom = contract.InEffectFrom,
                    InEffectTo = contract.InEffectTo
                };




                db.tblContractModifications.Add(modifiedContract);
                db.SaveChanges();
                Logger.Info("Accessed DB, Contract Modifcation/Extension Details Saved");

                if (modifiedContract.ContractModificationType == "Termination")
                {
                    string Termination = "";
                    Termination = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == contract.Initiator) select tblUserMaster.UserEmployeeName).First();
                    string Terminationemail = "";
                    Terminationemail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == contract.Initiator) select tblUserMaster.UserEmployeeEmail).First();
                    var TableVariable = /*from tblVariableData in*/ db.tblVariableDatas.Where(x => x.Version == "Contract").Where(x => x.TypeID == ContractID).Select(x => new { x.Value, x.Variable });
                    string[] temp = new string[5];

                    string EntityType = "";
                    foreach (var r in TableVariable)
                    {
                        temp[0] = r.Variable;
                        temp[1] = r.Value;

                        if (temp[0] == "Entity")
                        {
                            //temp[3] = "Vendor Name";
                            EntityType = EntityType + temp[1];
                        }
                        if (EntityType == "Company")
                        {
                            if (temp[0] == "Name of the Company")
                            {
                                temp[4] = temp[1];
                            }
                        }
                        if (EntityType == "Partnership Firm")
                        {
                            if (temp[0] == "Name of the Partnership Firm")
                            {
                                temp[4] = temp[1];
                            }
                        }
                        if (EntityType == "Sole Proprietorship")
                        {
                            if (temp[0] == "Name of the Proprietorship Concern")
                            {
                                temp[4] = temp[1];
                            }
                        }
                        if (EntityType == "Limited Liability Partnership Firm")
                        {
                            if (temp[0] == "Limited Liability Partnership Firm")
                            {
                                temp[4] = temp[1];
                            }
                        }
                        if (EntityType == "Others")
                        {
                            if (temp[0] == "Name")
                            {
                                temp[4] = temp[1];
                            }
                        }

                    }
                    string employeename = Termination;
                    string VendorName = temp[4];
                    string[] TO = { Terminationemail };

                    string Subject = contract.ContractName + " is Terminated ";
                    string UrL = ApplicationLink + "/Contract/Details/" + ContractID;
                    string Paragraph = "The Contract details as mentioned below is  requested for Termiation and has been sent for Approval. <br/><br/>";
                    string Body = "Dear " + Termination + ",<br/><br/>" + Paragraph + "<html><head><style>table,td{border:1px solid black;},table>td{width:70%;} table{border-collapse:collapse;}</style></head><body><table style='width:70%;' cellpadding='5'><tr><td><b> Template Name </b></td><td>" + contract.ContractName + "</td></tr><tr><td><b> Template Unique ID </b></td><td>" + contract.ContractID + "</td></tr><tr><td><b> Vendor</b></td><td>" + VendorName + "</td></tr><tr><td><b> Template Link </b></td><td><a href=" + UrL + ">Review</a></td></tr></table><br/><br/>Regards,<br/><b>System Admin</b><br/><b>Contract Management System</b></body></html>";
                    SMTP.Send(TO, Subject, Body);
                }

                for (int i = 0; i < arrVariableNames.Length; i++)
                {
                    Logger.Info("Attempt Contract Variables");
                    try
                    {

                        string TempVariable = arrVariableNames[i];

                        Logger.Info("Accessing DB for Contract Variable Details");
                        var Variable = from tblVariableData in db.tblVariableDatas.Where(x => x.TypeID == modifiedContract.ContractID).Where(x => x.Type == "Contract").Where(x => x.Version == "Contract").Where(x => x.Variable == TempVariable) select tblVariableData;

                        Logger.Info("Accessed DB, Checking Contract Variable Details: Details Found");
                        if (Variable.ToList().Count > 0)
                        {
                            //tblVariableData Variable = new tblVariableData();
                            // tblVariableData Variable = db.tblVariableDatas.Find(ID);
                            foreach (var item in Variable)
                            {
                                //item.Type = "Template";
                                //item.TypeID = ID;
                                //Variable.Variable = arrVariableNames[i];
                                //item.Variable = arrVariableNames[i];
                                Logger.Info("Accessing DB for Updating the Contract Variables");
                                tblVariableData variable = new tblVariableData();
                                variable = item;

                                variable.Value = arrVariableValues[i];
                                if (ModelState.IsValid)
                                {
                                    db.Entry(variable).State = EntityState.Modified;

                                    //  db.SaveChanges();

                                }
                            }
                            //db.SaveChanges();

                        }

                        else
                        {
                            int TemplateID = 0;
                            try
                            {
                                TemplateID = (int)db.tblContractModifications.Find(modifiedContract.ContractID).OriginalTemplateID;
                            }
                            catch { }
                            System.Collections.Generic.List<string> list = db.tblVariableDatas.Where(x => x.Type == "Template").Where(x => x.Version == "Template").Where(x => x.TypeID == TemplateID).Select(x => x.Variable).ToList();

                            Logger.Info("Accessed DB, Checking Contract Variable Details: Details Not Found");
                            Logger.Info("Accessing DB for Saving the Contract Variables");

                            tblVariableData newVariable = new tblVariableData();

                            newVariable.Type = "Contract";
                            newVariable.TypeID = modifiedContract.ContractID;
                            newVariable.Variable = arrVariableNames[i];
                            newVariable.Value = arrVariableValues[i];
                            newVariable.Version = "Contract";

                            int a = list.Where(t => t == arrVariableNames[i]).Count();

                            if (a > 0)
                            { newVariable.VariableFrom = "Template"; }



                            db.tblVariableDatas.Add(newVariable);
                        }


                        Logger.Info("Accessed DB, Contract Variables Saved");
                    }
                    catch (Exception Ex)
                    {
                        Logger.Error(Ex, "'Contract' Controller , 'ModificationContract' Action HTTP POST Main exception  Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                        return Json("error");
                    }
                    db.SaveChanges();
                }

                //contract.ContractModificationType = modifiedContract.ContractModificationType;
                //db.Entry(contract).State = EntityState.Modified;
                db.SaveChanges();

                string[] response = new string[2];
                response[0] = "sucess";
                response[1] = "" + modifiedContract.ContractID;
                //if (ModelState.IsValid)
                //{
                //    Contract.ContractModificationType = ModificationType;
                //    db.Entry(Contract).State = EntityState.Modified;
                //    db.SaveChanges();
                //    return RedirectToAction("ContractDraftExtension", "Contract", new { id = ModifiedContract.ContractID });
                //}
                return Json(response);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Contract' Controller , 'ModificationContract' Action HTTP POST Main exception  Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //status = Ex.InnerException.Message;
                return Json("error");
            }

        }

    }
}