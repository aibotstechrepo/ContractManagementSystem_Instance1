using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Windows.Forms;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{

    //[Authorize(Roles = "admin")]
    public class VendorController : Controller
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

        // GET: Vendor
        [Authorize(Roles = "admin")]
        public ActionResult New()
        {
            Logger.Info("Accessing Vendor New Page");
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult VendorTemplate()
        {
            Logger.Info("Accessing VendorTemplate Page");
            return View();
        }


        [HttpPost]
        public ActionResult GetVendorTemplate(string select)
        {
            Logger.Info("Attempt Vendor GetVendorTemplate");
            try
            {
                Logger.Info("Accessing DB for Vendor Details");
                if (select == "Company")
                {
                    Logger.Info("Accessed DB, Checking Vendor Details: VendorTemplateID match");
                    var result = from tblVendorTemplate in db.tblVendorTemplates select tblVendorTemplate.VendorCompany;
                    Logger.Info("Accessed DB, Checking Vendor Details: VendorTemplate Found");
                    return Json(result);
                }

                if (select == "Partnership Firm")
                {
                    Logger.Info("Accessed DB, Checking Vendor Details: VendorTemplateID match");

                    var result = from tblVendorTemplate in db.tblVendorTemplates select tblVendorTemplate.VendorPartnershipFirm;
                    Logger.Info("Accessed DB, Checking Vendor Details: VendorTemplate Found");
                    return Json(result);
                }

                if (select == "Sole Proprietorship")
                {
                    Logger.Info("Accessed DB, Checking Vendor Details: TemplateID match");
                    var result = from tblVendorTemplate in db.tblVendorTemplates select tblVendorTemplate.VendorSoleProprietorship;
                    Logger.Info("Accessed DB, Checking Vendor Details: VendorTemplate Found");
                    return Json(result);
                }
                if (select == "Limited Liability Partnership Firm")
                {
                    Logger.Info("Accessed DB, Checking Vendor Details: TemplateID match");
                    var result = from tblVendorTemplate in db.tblVendorTemplates select tblVendorTemplate.VendorLLPF;
                    Logger.Info("Accessed DB, Checking Vendor Details: VendorTemplate Found");
                    return Json(result);
                }
                if (select == "Others")
                {
                    Logger.Info("Accessed DB, Checking Vendor Details: TemplateID match");
                    var result = from tblVendorTemplate in db.tblVendorTemplates select tblVendorTemplate.VendorOthers;
                    Logger.Info("Accessed DB, Checking Vendor Details: VendorTemplate Found");
                    return Json(result);
                }


                Logger.Info("Accessing DB for Vendor Details: Details Found");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'GetVendorTemplate' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);

                return Json("errors");
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveVendorTemplate(string content, string Select)
        {
            int ID = 0;
            string OldValues = "";
            string NewValues = "";
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            try
            {
                Logger.Info("Accessing DB for VendorTemplate Details");
                var vendor = from tblVendorTemplate in db.tblVendorTemplates select tblVendorTemplate;

                if (vendor.ToList().Count > 0) {
                    foreach (var eachItem in vendor)
                    {
                        ID = eachItem.ID;
                        if (Select == "Company")
                        {
                            if (eachItem.VendorCompany != content)
                            {
                                OldValues = OldValues + "Company : " + eachItem.VendorCompany + " , ";
                                NewValues = NewValues + "Company : " + content + " , ";
                            }
                            eachItem.VendorCompany = content;
                        }
                        else if (Select == "Partnership Firm")
                        {
                            if (eachItem.VendorPartnershipFirm != content)
                            {
                                OldValues = OldValues + "Partnership Firm : " + eachItem.VendorPartnershipFirm + " , ";
                                NewValues = NewValues + "Partnership Firm : " + content + " , ";
                            }
                            eachItem.VendorPartnershipFirm = content;
                        }
                        else if (Select == "Sole Proprietorship")
                        {
                            if (eachItem.VendorSoleProprietorship != content)
                            {
                                OldValues = OldValues + "Sole Proprietorship : " + eachItem.VendorSoleProprietorship + " , ";
                                NewValues = NewValues + "Sole Proprietorship : " + content + " , ";
                            }
                            eachItem.VendorSoleProprietorship = content;
                        }
                        else if (Select == "Limited Liability Partnership Firm")
                        {
                            if (eachItem.VendorLLPF != content)
                            {
                                OldValues = OldValues + "Limited Liability Partnership Firm : " + eachItem.VendorLLPF + " , ";
                                NewValues = NewValues + "Limited Liability Partnership Firm : " + content + " , ";
                            }
                            eachItem.VendorLLPF = content;
                        }
                        else if (Select == "Others")
                        {
                            if (eachItem.VendorOthers != content)
                            {
                                OldValues = OldValues + "Others : " + eachItem.VendorOthers + " , ";
                                NewValues = NewValues + "Others : " + content + " , ";
                            }
                            eachItem.VendorOthers = content;
                        }

                        db.Entry(eachItem).State = EntityState.Modified;
                    }

                    if (OldValues.Length > 0)
                    {
                        tblVendorTemplateLog log = new tblVendorTemplateLog();
                        log.LogVendorTemplateUID = ID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblVendorTemplateLogs.Add(log);
                    }
                    db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }
                else
                {
                    tblVendorTemplate template = new tblVendorTemplate();
                    if (Select == "Company")
                    {
                        template.VendorCompany = content;
                    }
                    else if (Select == "Partnership Firm")
                    {
                        template.VendorPartnershipFirm = content;
                    }
                    else if (Select == "Sole Proprietorship")
                    {
                        template.VendorSoleProprietorship = content;
                    }
                    else if (Select == "Limited Liability Partnership Firm")
                    {
                        template.VendorLLPF = content;
                    }
                    else if (Select == "Others")
                    {
                        template.VendorOthers = content;
                    }
                    db.tblVendorTemplates.Add(template);

                    
                    tblVendorTemplateLog log = new tblVendorTemplateLog();
                    log.LogVendorTemplateUID = template.ID;
                    log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    log.LogActivity = "Created";
                    log.ChangedFrom = "-";
                    log.ChangedTo = "-";
                    log.DateandTime = DateTime.Now.ToString();

                    db.tblVendorTemplateLogs.Add(log);

                    db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'SaveVendorTemplate' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);

                return Json("errors");
            }
        }


        [HttpPost]
        public ActionResult GetVendorTemplateLogDetail(int ID)
        {
            Logger.Info("Attempt Vendor GetVendorTemplateLogDetail");

            try
            {
                Logger.Info("Accessed DB, Checking VendorTemplate Log Details: LogID match");

                var result = from tblVendorTemplateLog in db.tblVendorTemplateLogs select tblVendorTemplateLog;
                Logger.Info("Accessed DB, Checking VendorTemplate Log Details: LogDetails Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'GetVendorTemplateLogDetail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }


        //*****************************Integrated (Pooja) on 14/3/20***************
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitCompanyVendor(string TypeOfEntity, string VendorName, string CIN, string AuthSign, string RegAddress,string CorporateAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor SubmitCompanyVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = new tblVendorMaster();
                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);
                
                vendor.VendorCorporateIdentificationNumber = HttpUtility.HtmlEncode(CIN);
                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(RegAddress);
                vendor.VendorAuthorisedSignatory = HttpUtility.HtmlEncode(AuthSign);
                vendor.CorporateAddress = HttpUtility.HtmlEncode(CorporateAddress);
                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                db.tblVendorMasters.Add(vendor);
                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                Logger.Info("Accessing DB for Saving VendorLog Details");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Created";
                log.ChangedFrom = "-";
                log.ChangedTo = "-";
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);
               
                db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Details Saved");
                return Json("success");
                
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'SubmitCompanyVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
            
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitSoleProprietorshipVendor(string TypeOfEntity, string VendorName, string NameoftheProprietor, string PAN, string AdhaarCardNo, string ProprietorshipAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor SubmitSoleProprietorshipVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = new tblVendorMaster();
                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.NameoftheProprietor = HttpUtility.HtmlEncode(NameoftheProprietor);
                vendor.PAN = HttpUtility.HtmlEncode(PAN);
                vendor.AdhaarCardNo = HttpUtility.HtmlEncode(AdhaarCardNo);
                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(ProprietorshipAddress);

                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);


                db.tblVendorMasters.Add(vendor);
                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                Logger.Info("Accessing DB for Saving VendorLog Details");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Created";
                log.ChangedFrom = "-";
                log.ChangedTo = "-";
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Details Saved");
                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'SubmitSoleProprietorshipVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitLLPFirmVendor(string TypeOfEntity, string VendorName, string LLPIN, string LLPAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor SubmitLLPFirmVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = new tblVendorMaster();
                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.LLPIN = HttpUtility.HtmlEncode(LLPIN);
                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(LLPAddress);

                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                db.tblVendorMasters.Add(vendor);
                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                Logger.Info("Accessing DB for Saving VendorLog Details");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Created";
                log.ChangedFrom = "-";
                log.ChangedTo = "-";
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Details Saved");
                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'SubmitLLPFirmVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitOtherVendor(string TypeOfEntity, string VendorName, string OthersAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor SubmitOtherVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = new tblVendorMaster();
                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(OthersAddress);

                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                db.tblVendorMasters.Add(vendor);
                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                Logger.Info("Accessing DB for Saving VendorLog Details");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Created";
                log.ChangedFrom = "-";
                log.ChangedTo = "-";
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Details Saved");
                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'SubmitOtherVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitPartnershipVendor(string TypeOfEntity, string VendorName, string PartnershipAuthSign, string PartnershipAddress,
                                                    string Partner1Name, int Partner1Age, string Partner1FatherName, string Partner1Address,
                                                    string Partner2Name = null, int Partner2Age = 0,  string Partner2FatherName = null,  string Partner2Address = null, 
                                                    string Partner3Name = null,  int Partner3Age = 0,  string Partner3FatherName = null,  string Partner3Address = null, 
                                                    string Partner4Name = null,  int Partner4Age = 0,  string Partner4FatherName = null,  string Partner4Address = null, 
                                                    string Partner5Name = null,  int Partner5Age = 0,  string Partner5FatherName = null,  string Partner5Address = null, 
                                                    string Partner6Name = null,  int Partner6Age = 0,  string Partner6FatherName = null,  string Partner6Address = null, 
                                                    string Partner7Name = null,  int Partner7Age = 0,  string Partner7FatherName = null,  string Partner7Address = null, 
                                                    string Partner8Name = null,  int Partner8Age = 0,  string Partner8FatherName = null,  string Partner8Address = null, 
                                                    string Partner9Name = null,  int Partner9Age = 0,  string Partner9FatherName = null,  string Partner9Address = null, 
                                                    string Partner10Name = null,  int Partner10Age = 0,  string Partner10FatherName = null,  string Partner10Address = null, 
                                                    string Branch1 = null,  string Branch2 = null,  string Branch3 = null,  string Branch4 = null,  string Branch5 = null)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor New");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = new tblVendorMaster();
                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(PartnershipAddress);
                vendor.VendorAuthorisedSignatory = HttpUtility.HtmlEncode(PartnershipAuthSign);

                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                vendor.Partner1Name = HttpUtility.HtmlEncode(Partner1Name);
                //string Partner1_Age = vendor.Partner1Age.ToString();
                //Partner1_Age = HttpUtility.HtmlEncode(Partner1Age);

                vendor.Partner1Age = Partner1Age;

                vendor.Partner1FatherName = HttpUtility.HtmlEncode(Partner1FatherName);
                vendor.Partner1Address = HttpUtility.HtmlEncode(Partner1Address);

                vendor.Partner2Name = HttpUtility.HtmlEncode(Partner2Name);
                //string Partner2_Age = vendor.Partner2Age.ToString();
                //Partner2_Age = HttpUtility.HtmlEncode(Partner2Age);
                vendor.Partner2Age = Partner2Age;
                vendor.Partner2FatherName = HttpUtility.HtmlEncode(Partner2FatherName);
                vendor.Partner2Address = HttpUtility.HtmlEncode(Partner2Address);

                vendor.Partner3Name = HttpUtility.HtmlEncode(Partner3Name);
                //string Partner3_Age = vendor.Partner3Age.ToString();
                //Partner3_Age = HttpUtility.HtmlEncode(Partner3Age);
                vendor.Partner3Age = Partner3Age;
                vendor.Partner3FatherName = HttpUtility.HtmlEncode(Partner3FatherName);
                vendor.Partner3Address = HttpUtility.HtmlEncode(Partner3Address);

                vendor.Partner4Name = HttpUtility.HtmlEncode(Partner4Name);
                //string Partner4_Age = vendor.Partner4Age.ToString();
                //Partner4_Age = HttpUtility.HtmlEncode(Partner4Age);
                vendor.Partner4Age = Partner4Age;
                vendor.Partner4FatherName = HttpUtility.HtmlEncode(Partner4FatherName);
                vendor.Partner4Address = HttpUtility.HtmlEncode(Partner4Address);

                vendor.Partner5Name = HttpUtility.HtmlEncode(Partner5Name);
                //string Partner5_Age = vendor.Partner5Age.ToString();
                //Partner5_Age = HttpUtility.HtmlEncode(Partner5Age);
                vendor.Partner5Age = Partner5Age;
                vendor.Partner5FatherName = HttpUtility.HtmlEncode(Partner5FatherName);
                vendor.Partner5Address = HttpUtility.HtmlEncode(Partner5Address);

                vendor.Partner6Name = HttpUtility.HtmlEncode(Partner6Name);
                //string Partner6_Age = vendor.Partner6Age.ToString();
                //Partner6_Age = HttpUtility.HtmlEncode(Partner6Age);
                vendor.Partner6Age = Partner6Age;
                vendor.Partner6FatherName = HttpUtility.HtmlEncode(Partner6FatherName);
                vendor.Partner6Address = HttpUtility.HtmlEncode(Partner6Address);

                vendor.Partner7Name = HttpUtility.HtmlEncode(Partner7Name);
                //string Partner7_Age = vendor.Partner7Age.ToString();
                //Partner7_Age = HttpUtility.HtmlEncode(Partner7Age);
                vendor.Partner7Age = Partner7Age;
                vendor.Partner7FatherName = HttpUtility.HtmlEncode(Partner7FatherName);
                vendor.Partner7Address = HttpUtility.HtmlEncode(Partner7Address);

                vendor.Partner8Name = HttpUtility.HtmlEncode(Partner8Name);
                //string Partner8_Age = vendor.Partner8Age.ToString();
                //Partner8_Age = HttpUtility.HtmlEncode(Partner8Age);
                vendor.Partner8Age = Partner8Age;
                vendor.Partner8FatherName = HttpUtility.HtmlEncode(Partner8FatherName);
                vendor.Partner8Address = HttpUtility.HtmlEncode(Partner8Address);

                vendor.Partner9Name = HttpUtility.HtmlEncode(Partner9Name);
                //string Partner9_Age = vendor.Partner9Age.ToString();
                //Partner9_Age = HttpUtility.HtmlEncode(Partner9Age);
                vendor.Partner9Age = Partner9Age;
                vendor.Partner9FatherName = HttpUtility.HtmlEncode(Partner9FatherName);
                vendor.Partner9Address = HttpUtility.HtmlEncode(Partner9Address);

                vendor.Partner10Name = HttpUtility.HtmlEncode(Partner10Name);
                //string Partner10_Age = vendor.Partner10Age.ToString();
                //Partner10_Age = HttpUtility.HtmlEncode(Partner10Age);
                vendor.Partner10Age = Partner10Age;
                vendor.Partner10FatherName = HttpUtility.HtmlEncode(Partner10FatherName);
                vendor.Partner10Address = HttpUtility.HtmlEncode(Partner10Address);

                db.tblVendorMasters.Add(vendor);
                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                Logger.Info("Accessing DB for Saving VendorLog Details");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Created";
                log.ChangedFrom = "-";
                log.ChangedTo = "-";
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Details Saved");
                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'SubmitPartnershipVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }




        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateCompanyVendor(int ID, string TypeOfEntity, string VendorName, string CIN, string AuthSign, string RegAddress, string CorporateAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor UpdateCompanyVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = db.tblVendorMasters.Find(ID);
                string OldValues = "";
                string NewValues = "";
                if (vendor.VendorTypeofEntity != TypeOfEntity)
                {
                    OldValues = OldValues + "Type Of Entity : " + vendor.VendorTypeofEntity + " , ";
                    NewValues = NewValues + "Type Of Entity : " + TypeOfEntity + " , ";
                }
                if (vendor.VendorVendorName != VendorName)
                {
                    OldValues = OldValues + "Name of the Company : " + vendor.VendorVendorName + " , ";
                    NewValues = NewValues + "Name of the Company : " + VendorName + " , ";
                }
                if (vendor.VendorCorporateIdentificationNumber != CIN)
                {
                    OldValues = OldValues + "CIN : " + vendor.VendorCorporateIdentificationNumber + " , ";
                    NewValues = NewValues + "CIN : " + CIN + " , ";
                }
                if (vendor.VendorAuthorisedSignatory != AuthSign)
                {
                    OldValues = OldValues + "Authorised Signatory : " + vendor.VendorAuthorisedSignatory + " , ";
                    NewValues = NewValues + "Authorised Signatory : " + AuthSign + " , ";
                }
                if (vendor.VendorRegisteredAddress != RegAddress)
                {
                    OldValues = OldValues + "Registered Address : " + vendor.VendorRegisteredAddress + " , ";
                    NewValues = NewValues + "Registered Address : " + RegAddress + " , ";
                }
                if (vendor.CorporateAddress != CorporateAddress)
                {
                    OldValues = OldValues + "Corporate Address : " + vendor.CorporateAddress + " , ";
                    NewValues = NewValues + "Corporate Address : " + CorporateAddress + " , ";
                }
                if (vendor.VendorBranchOffice1 != Branch1)
                {
                    OldValues = OldValues + "Branch 1 Address : " + vendor.VendorBranchOffice1 + " , ";
                    NewValues = NewValues + "Branch 1 Address : " + Branch1 + " , ";
                }
                if (vendor.VendorBranchOffice2 != Branch2)
                {
                    OldValues = OldValues + "Branch 2 Address : " + vendor.VendorBranchOffice2 + " , ";
                    NewValues = NewValues + "Branch 2 Address : " + Branch2 + " , ";
                }
                if (vendor.VendorBranchOffice3 != Branch3)
                {
                    OldValues = OldValues + "Branch 3 Address : " + vendor.VendorBranchOffice3 + " , ";
                    NewValues = NewValues + "Branch 3 Address : " + Branch3 + " , ";
                }
                if (vendor.VendorBranchOffice4 != Branch4)
                {
                    OldValues = OldValues + "Branch 4 Address : " + vendor.VendorBranchOffice4 + " , ";
                    NewValues = NewValues + "Branch 4 Address : " + Branch4 + " , ";
                }
                if (vendor.VendorBranchOffice5 != Branch5)
                {
                    OldValues = OldValues + "Branch 5 Address : " + vendor.VendorBranchOffice5 + " , ";
                    NewValues = NewValues + "Branch 5 Address : " + Branch5 + " , ";
                }



                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.VendorCorporateIdentificationNumber = HttpUtility.HtmlEncode(CIN);
                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(RegAddress);
                vendor.VendorAuthorisedSignatory = HttpUtility.HtmlEncode(AuthSign);
                vendor.CorporateAddress = HttpUtility.HtmlEncode(CorporateAddress);
                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                vendor.NameoftheProprietor = null;
                vendor.PAN = null;
                vendor.AdhaarCardNo = null;
                vendor.LLPIN = null;
                vendor.Partner1Name = null;
                vendor.Partner1Age = null;
                vendor.Partner1FatherName = null;
                vendor.Partner1Address = null;
                vendor.Partner2Name = null;
                vendor.Partner2Age = null;
                vendor.Partner2FatherName = null;
                vendor.Partner2Address = null;
                vendor.Partner3Name = null;
                vendor.Partner3Age = null;
                vendor.Partner3FatherName = null;
                vendor.Partner3Address = null;
                vendor.Partner4Name = null;
                vendor.Partner4Age = null;
                vendor.Partner4FatherName = null;
                vendor.Partner4Address = null;

                vendor.Partner5Name = null;
                vendor.Partner5Age = null;
                vendor.Partner5FatherName = null;
                vendor.Partner5Address = null;

                vendor.Partner6Name = null;
                vendor.Partner6Age = null;
                vendor.Partner6FatherName = null;
                vendor.Partner6Address = null;

                vendor.Partner7Name = null;
                vendor.Partner7Age = null; 
                vendor.Partner7FatherName = null;
                vendor.Partner7Address = null;

                vendor.Partner8Name = null;
                vendor.Partner8Age = null;
                vendor.Partner8FatherName = null;
                vendor.Partner8Address = null;

                vendor.Partner9Name = null;
                vendor.Partner9Age = null;
                vendor.Partner9FatherName = null;
                vendor.Partner9Address = null;

                vendor.Partner10Name = null;
                vendor.Partner10Age = null;
                vendor.Partner10FatherName = null;
                vendor.Partner10Address = null;

                db.Entry(vendor).State = EntityState.Modified;


                Logger.Info("Accessing DB for Saving the VendorLog Details ");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Modified";
                log.ChangedFrom = OldValues;
                log.ChangedTo = NewValues;
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'UpdateCompanyVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateSoleProprietorshipVendor(int ID, string TypeOfEntity, string VendorName, string NameoftheProprietor, string PAN, string AdhaarCardNo, string ProprietorshipAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor UpdateSoleProprietorshipVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = db.tblVendorMasters.Find(ID);

                string OldValues = "";
                string NewValues = "";
                if (vendor.VendorTypeofEntity != TypeOfEntity)
                {
                    OldValues = OldValues + "Type Of Entity : " + vendor.VendorTypeofEntity + " , ";
                    NewValues = NewValues + "Type Of Entity : " + TypeOfEntity + " , ";
                }
                if (vendor.VendorVendorName != VendorName)
                {
                    OldValues = OldValues + "Name of the Proprietorship Concern: " + vendor.VendorVendorName + " , ";
                    NewValues = NewValues + "Name of the Proprietorship Concern: " + VendorName + " , ";
                }
                if (vendor.NameoftheProprietor != NameoftheProprietor)
                {
                    OldValues = OldValues + "Name of the Proprietor : " + vendor.NameoftheProprietor + " , ";
                    NewValues = NewValues + "Name of the Proprietor : " + NameoftheProprietor + " , ";
                }
                if (vendor.PAN != PAN)
                {
                    OldValues = OldValues + "PAN : " + vendor.PAN + " , ";
                    NewValues = NewValues + "PAN : " + PAN + " , ";
                }
                if (vendor.AdhaarCardNo != AdhaarCardNo)
                {
                    OldValues = OldValues + "Adhaar Card No. : " + vendor.AdhaarCardNo + " , ";
                    NewValues = NewValues + "Adhaar Card No. : " + AdhaarCardNo + " , ";
                }
                if (vendor.VendorRegisteredAddress != ProprietorshipAddress)
                {
                    OldValues = OldValues + "Address : " + vendor.VendorRegisteredAddress + " , ";
                    NewValues = NewValues + "Address : " + ProprietorshipAddress + " , ";
                }
                if (vendor.VendorBranchOffice1 != Branch1)
                {
                    OldValues = OldValues + "Branch 1 Address : " + vendor.VendorBranchOffice1 + " , ";
                    NewValues = NewValues + "Branch 1 Address : " + Branch1 + " , ";
                }
                if (vendor.VendorBranchOffice2 != Branch2)
                {
                    OldValues = OldValues + "Branch 2 Address : " + vendor.VendorBranchOffice2 + " , ";
                    NewValues = NewValues + "Branch 2 Address : " + Branch2 + " , ";
                }
                if (vendor.VendorBranchOffice3 != Branch3)
                {
                    OldValues = OldValues + "Branch 3 Address : " + vendor.VendorBranchOffice3 + " , ";
                    NewValues = NewValues + "Branch 3 Address : " + Branch3 + " , ";
                }
                if (vendor.VendorBranchOffice4 != Branch4)
                {
                    OldValues = OldValues + "Branch 4 Address : " + vendor.VendorBranchOffice4 + " , ";
                    NewValues = NewValues + "Branch 4 Address : " + Branch4 + " , ";
                }
                if (vendor.VendorBranchOffice5 != Branch5)
                {
                    OldValues = OldValues + "Branch 5 Address : " + vendor.VendorBranchOffice5 + " , ";
                    NewValues = NewValues + "Branch 5 Address : " + Branch5 + " , ";
                }


                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.NameoftheProprietor = HttpUtility.HtmlEncode(NameoftheProprietor);
                vendor.PAN = HttpUtility.HtmlEncode(PAN);
                vendor.AdhaarCardNo = HttpUtility.HtmlEncode(AdhaarCardNo);
                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(ProprietorshipAddress);
                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);


                vendor.VendorCorporateIdentificationNumber = null;
                vendor.VendorAuthorisedSignatory = null;
                vendor.CorporateAddress = null;
                vendor.LLPIN = null;
                vendor.Partner1Name = null;
                vendor.Partner1Age = null;
                vendor.Partner1FatherName = null;
                vendor.Partner1Address = null;
                vendor.Partner2Name = null;
                vendor.Partner2Age = null;
                vendor.Partner2FatherName = null;
                vendor.Partner2Address = null;
                vendor.Partner3Name = null;
                vendor.Partner3Age = null;
                vendor.Partner3FatherName = null;
                vendor.Partner3Address = null;
                vendor.Partner4Name = null;
                vendor.Partner4Age = null;
                vendor.Partner4FatherName = null;
                vendor.Partner4Address = null;

                vendor.Partner5Name = null;
                vendor.Partner5Age = null;
                vendor.Partner5FatherName = null;
                vendor.Partner5Address = null;

                vendor.Partner6Name = null;
                vendor.Partner6Age = null;
                vendor.Partner6FatherName = null;
                vendor.Partner6Address = null;

                vendor.Partner7Name = null;
                vendor.Partner7Age = null;
                vendor.Partner7FatherName = null;
                vendor.Partner7Address = null;

                vendor.Partner8Name = null;
                vendor.Partner8Age = null;
                vendor.Partner8FatherName = null;
                vendor.Partner8Address = null;

                vendor.Partner9Name = null;
                vendor.Partner9Age = null;
                vendor.Partner9FatherName = null;
                vendor.Partner9Address = null;

                vendor.Partner10Name = null;
                vendor.Partner10Age = null;
                vendor.Partner10FatherName = null;
                vendor.Partner10Address = null;

              
                db.Entry(vendor).State = EntityState.Modified;

                Logger.Info("Accessing DB for Saving the VendorLog Details ");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Modified";
                log.ChangedFrom = OldValues;
                log.ChangedTo = NewValues;
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'UpdateSoleProprietorshipVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateLLPFirmVendor(int ID, string TypeOfEntity, string VendorName, string LLPIN, string LLPAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor New");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = db.tblVendorMasters.Find(ID);

                string OldValues = "";
                string NewValues = "";
                if (vendor.VendorTypeofEntity != TypeOfEntity)
                {
                    OldValues = OldValues + "Type Of Entity : " + vendor.VendorTypeofEntity + " , ";
                    NewValues = NewValues + "Type Of Entity : " + TypeOfEntity + " , ";
                }
                if (vendor.VendorVendorName != VendorName)
                {
                    OldValues = OldValues + "Name of the Limited Liability Partnership: " + vendor.VendorVendorName + " , ";
                    NewValues = NewValues + "Name of the Limited Liability Partnership: " + VendorName + " , ";
                }

                if (vendor.LLPIN != LLPIN)
                {
                    OldValues = OldValues + "LLP IN: " + vendor.LLPIN + " , ";
                    NewValues = NewValues + "LLP IN: " + LLPIN + " , ";
                }
                if (vendor.VendorRegisteredAddress != LLPAddress)
                {
                    OldValues = OldValues + "Registered Address of the LLP: " + vendor.VendorRegisteredAddress + " , ";
                    NewValues = NewValues + "Registered Address of the LLP: " + LLPAddress + " , ";
                }
                if (vendor.VendorBranchOffice1 != Branch1)
                {
                    OldValues = OldValues + "Branch 1 Address : " + vendor.VendorBranchOffice1 + " , ";
                    NewValues = NewValues + "Branch 1 Address : " + Branch1 + " , ";
                }
                if (vendor.VendorBranchOffice2 != Branch2)
                {
                    OldValues = OldValues + "Branch 2 Address : " + vendor.VendorBranchOffice2 + " , ";
                    NewValues = NewValues + "Branch 2 Address : " + Branch2 + " , ";
                }
                if (vendor.VendorBranchOffice3 != Branch3)
                {
                    OldValues = OldValues + "Branch 3 Address : " + vendor.VendorBranchOffice3 + " , ";
                    NewValues = NewValues + "Branch 3 Address : " + Branch3 + " , ";
                }
                if (vendor.VendorBranchOffice4 != Branch4)
                {
                    OldValues = OldValues + "Branch 4 Address : " + vendor.VendorBranchOffice4 + " , ";
                    NewValues = NewValues + "Branch 4 Address : " + Branch4 + " , ";
                }
                if (vendor.VendorBranchOffice5 != Branch5)
                {
                    OldValues = OldValues + "Branch 5 Address : " + vendor.VendorBranchOffice5 + " , ";
                    NewValues = NewValues + "Branch 5 Address : " + Branch5 + " , ";
                }

                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.LLPIN = HttpUtility.HtmlEncode(LLPIN);
                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(LLPAddress);

                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                vendor.NameoftheProprietor = null;
                vendor.PAN = null;
                vendor.AdhaarCardNo = null;
                vendor.VendorCorporateIdentificationNumber = null;
                vendor.VendorAuthorisedSignatory = null;
                vendor.CorporateAddress = null;
                vendor.Partner1Name = null;
                vendor.Partner1Age = null;
                vendor.Partner1FatherName = null;
                vendor.Partner1Address = null;
                vendor.Partner2Name = null;
                vendor.Partner2Age = null;
                vendor.Partner2FatherName = null;
                vendor.Partner2Address = null;
                vendor.Partner3Name = null;
                vendor.Partner3Age = null;
                vendor.Partner3FatherName = null;
                vendor.Partner3Address = null;
                vendor.Partner4Name = null;
                vendor.Partner4Age = null;
                vendor.Partner4FatherName = null;
                vendor.Partner4Address = null;

                vendor.Partner5Name = null;
                vendor.Partner5Age = null;
                vendor.Partner5FatherName = null;
                vendor.Partner5Address = null;

                vendor.Partner6Name = null;
                vendor.Partner6Age = null;
                vendor.Partner6FatherName = null;
                vendor.Partner6Address = null;

                vendor.Partner7Name = null;
                vendor.Partner7Age = null;
                vendor.Partner7FatherName = null;
                vendor.Partner7Address = null;

                vendor.Partner8Name = null;
                vendor.Partner8Age = null;
                vendor.Partner8FatherName = null;
                vendor.Partner8Address = null;

                vendor.Partner9Name = null;
                vendor.Partner9Age = null;
                vendor.Partner9FatherName = null;
                vendor.Partner9Address = null;

                vendor.Partner10Name = null;
                vendor.Partner10Age = null;
                vendor.Partner10FatherName = null;
                vendor.Partner10Address = null;

                db.Entry(vendor).State = EntityState.Modified;

                Logger.Info("Accessing DB for Saving the VendorLog Details ");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Modified";
                log.ChangedFrom = OldValues;
                log.ChangedTo = NewValues;
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");

                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'New' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateOtherVendor(int ID, string TypeOfEntity, string VendorName, string OthersAddress, string Branch1, string Branch2, string Branch3, string Branch4, string Branch5)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor UpdateOtherVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = db.tblVendorMasters.Find(ID);
                string OldValues = "";
                string NewValues = "";
                if(vendor.VendorTypeofEntity != TypeOfEntity)
                {
                    OldValues = OldValues + "Type Of Entity : " + vendor.VendorTypeofEntity + " , ";
                    NewValues = NewValues + "Type Of Entity : " + TypeOfEntity + " , ";
                }
                if (vendor.VendorVendorName != VendorName)
                {
                    OldValues = OldValues + "Name : " + vendor.VendorVendorName + " , ";
                    NewValues = NewValues + "Name : " + VendorName + " , ";
                }
                if (vendor.VendorRegisteredAddress != OthersAddress)
                {
                    OldValues = OldValues + "Address : " + vendor.VendorRegisteredAddress + " , ";
                    NewValues = NewValues + "Address : " + OthersAddress + " , ";
                }
                if (vendor.VendorBranchOffice1 != Branch1)
                {
                    OldValues = OldValues + "Branch 1 Address : " + vendor.VendorBranchOffice1 + " , ";
                    NewValues = NewValues + "Branch 1 Address : " + Branch1 + " , ";
                }
                if (vendor.VendorBranchOffice2 != Branch2)
                {
                    OldValues = OldValues + "Branch 2 Address : " + vendor.VendorBranchOffice2 + " , ";
                    NewValues = NewValues + "Branch 2 Address : " + Branch2 + " , ";
                }
                if (vendor.VendorBranchOffice3 != Branch3)
                {
                    OldValues = OldValues + "Branch 3 Address : " + vendor.VendorBranchOffice3 + " , ";
                    NewValues = NewValues + "Branch 3 Address : " + Branch3 + " , ";
                }
                if (vendor.VendorBranchOffice4 != Branch4)
                {
                    OldValues = OldValues + "Branch 4 Address : " + vendor.VendorBranchOffice4 + " , ";
                    NewValues = NewValues + "Branch 4 Address : " + Branch4 + " , ";
                }
                if (vendor.VendorBranchOffice5 != Branch5)
                {
                    OldValues = OldValues + "Branch 5 Address : " + vendor.VendorBranchOffice5 + " , ";
                    NewValues = NewValues + "Branch 5 Address : " + Branch5 + " , ";
                }


                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(OthersAddress);

                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                vendor.NameoftheProprietor = null;
                vendor.PAN = null;
                vendor.AdhaarCardNo = null;
                vendor.VendorCorporateIdentificationNumber = null;
                vendor.VendorAuthorisedSignatory = null;
                vendor.CorporateAddress = null;
                vendor.LLPIN = null;
                vendor.Partner1Name = null;
                vendor.Partner1Age = null;
                vendor.Partner1FatherName = null;
                vendor.Partner1Address = null;
                vendor.Partner2Name = null;
                vendor.Partner2Age = null;
                vendor.Partner2FatherName = null;
                vendor.Partner2Address = null;
                vendor.Partner3Name = null;
                vendor.Partner3Age = null;
                vendor.Partner3FatherName = null;
                vendor.Partner3Address = null;
                vendor.Partner4Name = null;
                vendor.Partner4Age = null;
                vendor.Partner4FatherName = null;
                vendor.Partner4Address = null;

                vendor.Partner5Name = null;
                vendor.Partner5Age = null;
                vendor.Partner5FatherName = null;
                vendor.Partner5Address = null;

                vendor.Partner6Name = null;
                vendor.Partner6Age = null;
                vendor.Partner6FatherName = null;
                vendor.Partner6Address = null;

                vendor.Partner7Name = null;
                vendor.Partner7Age = null;
                vendor.Partner7FatherName = null;
                vendor.Partner7Address = null;

                vendor.Partner8Name = null;
                vendor.Partner8Age = null;
                vendor.Partner8FatherName = null;
                vendor.Partner8Address = null;

                vendor.Partner9Name = null;
                vendor.Partner9Age = null;
                vendor.Partner9FatherName = null;
                vendor.Partner9Address = null;

                vendor.Partner10Name = null;
                vendor.Partner10Age = null;
                vendor.Partner10FatherName = null;
                vendor.Partner10Address = null;

                db.Entry(vendor).State = EntityState.Modified;

                Logger.Info("Accessing DB for Saving the VendorLog Details ");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Modified";
                log.ChangedFrom = OldValues;
                log.ChangedTo = NewValues;
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);


                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");
                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'UpdateOtherVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdatePartnershipVendor(int ID, string TypeOfEntity, string VendorName, string PartnershipAuthSign, string PartnershipAddress,
                                                    string Partner1Name, int Partner1Age, string Partner1FatherName, string Partner1Address,
                                                    string Partner2Name = null, int Partner2Age = 0, string Partner2FatherName = null, string Partner2Address = null,
                                                    string Partner3Name = null, int Partner3Age = 0, string Partner3FatherName = null, string Partner3Address = null,
                                                    string Partner4Name = null, int Partner4Age = 0, string Partner4FatherName = null, string Partner4Address = null,
                                                    string Partner5Name = null, int Partner5Age = 0, string Partner5FatherName = null, string Partner5Address = null,
                                                    string Partner6Name = null, int Partner6Age = 0, string Partner6FatherName = null, string Partner6Address = null,
                                                    string Partner7Name = null, int Partner7Age = 0, string Partner7FatherName = null, string Partner7Address = null,
                                                    string Partner8Name = null, int Partner8Age = 0, string Partner8FatherName = null, string Partner8Address = null,
                                                    string Partner9Name = null, int Partner9Age = 0, string Partner9FatherName = null, string Partner9Address = null,
                                                    string Partner10Name = null, int Partner10Age = 0, string Partner10FatherName = null, string Partner10Address = null,
                                                    string Branch1 = null, string Branch2 = null, string Branch3 = null, string Branch4 = null, string Branch5 = null)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor UpdatePartnershipVendor");
            try
            {
                Logger.Info("Accessing DB for Saving VendorMaster Details");
                tblVendorMaster vendor = db.tblVendorMasters.Find(ID);

                string OldValues = "";
                string NewValues = "";
                if (vendor.VendorTypeofEntity != TypeOfEntity)
                {
                    OldValues = OldValues + "Type Of Entity : " + vendor.VendorTypeofEntity + " , ";
                    NewValues = NewValues + "Type Of Entity : " + TypeOfEntity + " , ";
                }
                if (vendor.VendorVendorName != VendorName)
                {
                    OldValues = OldValues + "Name of the Partnership Firm : " + vendor.VendorVendorName + " , ";
                    NewValues = NewValues + "Name of the Partnership Firm : " + VendorName + " , ";
                }
                if (vendor.VendorAuthorisedSignatory != PartnershipAuthSign)
                {
                    OldValues = OldValues + "Authorised Signatory : " + vendor.VendorAuthorisedSignatory + " , ";
                    NewValues = NewValues + "Authorised Signatory : " + PartnershipAuthSign + " , ";
                }
                if (vendor.VendorRegisteredAddress != PartnershipAddress)
                {
                    OldValues = OldValues + "Registered Address : " + vendor.VendorRegisteredAddress + " , ";
                    NewValues = NewValues + "Registered Address : " + PartnershipAddress + " , ";
                }

                if (vendor.VendorBranchOffice1 != Branch1)
                {
                    OldValues = OldValues + "Branch 1 Address : " + vendor.VendorBranchOffice1 + " , ";
                    NewValues = NewValues + "Branch 1 Address : " + Branch1 + " , ";
                }
                if (vendor.VendorBranchOffice2 != Branch2)
                {
                    OldValues = OldValues + "Branch 2 Address : " + vendor.VendorBranchOffice2 + " , ";
                    NewValues = NewValues + "Branch 2 Address : " + Branch2 + " , ";
                }
                if (vendor.VendorBranchOffice3 != Branch3)
                {
                    OldValues = OldValues + "Branch 3 Address : " + vendor.VendorBranchOffice3 + " , ";
                    NewValues = NewValues + "Branch 3 Address : " + Branch3 + " , ";
                }
                if (vendor.VendorBranchOffice4 != Branch4)
                {
                    OldValues = OldValues + "Branch 4 Address : " + vendor.VendorBranchOffice4 + " , ";
                    NewValues = NewValues + "Branch 4 Address : " + Branch4 + " , ";
                }
                if (vendor.VendorBranchOffice5 != Branch5)
                {
                    OldValues = OldValues + "Branch 5 Address : " + vendor.VendorBranchOffice5 + " , ";
                    NewValues = NewValues + "Branch 5 Address : " + Branch5 + " , ";
                }
                if (vendor.Partner1Name != Partner1Name)
                {
                    OldValues = OldValues + "Partner 1 Name : " + vendor.Partner1Name + " , ";
                    NewValues = NewValues + "Partner 1 Name : " + Partner1Name + " , ";
                }
                if (vendor.Partner1Age != Partner1Age)
                {
                    OldValues = OldValues + "Partner 1 Age : " + vendor.Partner1Age + " , ";
                    NewValues = NewValues + "Partner 1 Age : " + Partner1Age + " , ";
                }
                if (vendor.Partner1FatherName != Partner1FatherName)
                {
                    OldValues = OldValues + "Partner 1 Father Name : " + vendor.Partner1FatherName + " , ";
                    NewValues = NewValues + "Partner 1 Father Name : " + Partner1FatherName + " , ";
                }
                if (vendor.Partner1Address != Partner1Address)
                {
                    OldValues = OldValues + "Partner 1 Address : " + vendor.Partner1Address + " , ";
                    NewValues = NewValues + "Partner 1 Address : " + Partner1Address + " , ";
                }


                if (vendor.Partner2Name != Partner2Name)
                {
                    OldValues = OldValues + "Partner 2 Name : " + vendor.Partner2Name + " , ";
                    NewValues = NewValues + "Partner 2 Name : " + Partner2Name + " , ";
                }
                if (vendor.Partner2Age != Partner2Age)
                {
                    OldValues = OldValues + "Partner 2 Age : " + vendor.Partner2Age + " , ";
                    NewValues = NewValues + "Partner 2 Age : " + Partner2Age + " , ";
                }
                if (vendor.Partner2FatherName != Partner2FatherName)
                {
                    OldValues = OldValues + "Partner 2 Father Name : " + vendor.Partner2FatherName + " , ";
                    NewValues = NewValues + "Partner 2 Father Name : " + Partner2FatherName + " , ";
                }
                if (vendor.Partner2Address != Partner2Address)
                {
                    OldValues = OldValues + "Partner 2 Address : " + vendor.Partner2Address + " , ";
                    NewValues = NewValues + "Partner 2 Address : " + Partner2Address + " , ";
                }


                if (vendor.Partner3Name != Partner3Name)
                {
                    OldValues = OldValues + "Partner 3 Name : " + vendor.Partner3Name + " , ";
                    NewValues = NewValues + "Partner 3 Name : " + Partner3Name + " , ";
                }
                if (vendor.Partner3Age != Partner3Age)
                {
                    OldValues = OldValues + "Partner 3 Age : " + vendor.Partner3Age + " , ";
                    NewValues = NewValues + "Partner 3 Age : " + Partner3Age + " , ";
                }
                if (vendor.Partner3FatherName != Partner3FatherName)
                {
                    OldValues = OldValues + "Partner 3 Father Name : " + vendor.Partner3FatherName + " , ";
                    NewValues = NewValues + "Partner 3 Father Name : " + Partner3FatherName + " , ";
                }
                if (vendor.Partner3Address != Partner3Address)
                {
                    OldValues = OldValues + "Partner 3 Address : " + vendor.Partner3Address + " , ";
                    NewValues = NewValues + "Partner 3 Address : " + Partner3Address + " , ";
                }


                if (vendor.Partner4Name != Partner4Name)
                {
                    OldValues = OldValues + "Partner 4 Name : " + vendor.Partner4Name + " , ";
                    NewValues = NewValues + "Partner 4 Name : " + Partner4Name + " , ";
                }
                if (vendor.Partner4Age != Partner4Age)
                {
                    OldValues = OldValues + "Partner 4 Age : " + vendor.Partner4Age + " , ";
                    NewValues = NewValues + "Partner 4 Age : " + Partner4Age + " , ";
                }
                if (vendor.Partner4FatherName != Partner4FatherName)
                {
                    OldValues = OldValues + "Partner 4 Father Name : " + vendor.Partner4FatherName + " , ";
                    NewValues = NewValues + "Partner 4 Father Name : " + Partner4FatherName + " , ";
                }
                if (vendor.Partner4Address != Partner4Address)
                {
                    OldValues = OldValues + "Partner 4 Address : " + vendor.Partner4Address + " , ";
                    NewValues = NewValues + "Partner 4 Address : " + Partner4Address + " , ";
                }


                if (vendor.Partner5Name != Partner5Name)
                {
                    OldValues = OldValues + "Partner 5 Name : " + vendor.Partner5Name + " , ";
                    NewValues = NewValues + "Partner 5 Name : " + Partner5Name + " , ";
                }
                if (vendor.Partner5Age != Partner5Age)
                {
                    OldValues = OldValues + "Partner 5 Age : " + vendor.Partner5Age + " , ";
                    NewValues = NewValues + "Partner 5 Age : " + Partner5Age + " , ";
                }
                if (vendor.Partner5FatherName != Partner5FatherName)
                {
                    OldValues = OldValues + "Partner 5 Father Name : " + vendor.Partner5FatherName + " , ";
                    NewValues = NewValues + "Partner 5 Father Name : " + Partner5FatherName + " , ";
                }
                if (vendor.Partner5Address != Partner5Address)
                {
                    OldValues = OldValues + "Partner 5 Address : " + vendor.Partner5Address + " , ";
                    NewValues = NewValues + "Partner 5 Address : " + Partner5Address + " , ";
                }


                if (vendor.Partner6Name != Partner6Name)
                {
                    OldValues = OldValues + "Partner 6 Name : " + vendor.Partner6Name + " , ";
                    NewValues = NewValues + "Partner 6 Name : " + Partner6Name + " , ";
                }
                if (vendor.Partner6Age != Partner6Age)
                {
                    OldValues = OldValues + "Partner 6 Age : " + vendor.Partner6Age + " , ";
                    NewValues = NewValues + "Partner 6 Age : " + Partner6Age + " , ";
                }
                if (vendor.Partner6FatherName != Partner6FatherName)
                {
                    OldValues = OldValues + "Partner 6 Father Name : " + vendor.Partner6FatherName + " , ";
                    NewValues = NewValues + "Partner 6 Father Name : " + Partner6FatherName + " , ";
                }
                if (vendor.Partner6Address != Partner6Address)
                {
                    OldValues = OldValues + "Partner 6 Address : " + vendor.Partner6Address + " , ";
                    NewValues = NewValues + "Partner 6 Address : " + Partner6Address + " , ";
                }


                if (vendor.Partner7Name != Partner7Name)
                {
                    OldValues = OldValues + "Partner 7 Name : " + vendor.Partner7Name + " , ";
                    NewValues = NewValues + "Partner 7 Name : " + Partner7Name + " , ";
                }
                if (vendor.Partner7Age != Partner7Age)
                {
                    OldValues = OldValues + "Partner 7 Age : " + vendor.Partner7Age + " , ";
                    NewValues = NewValues + "Partner 7 Age : " + Partner7Age + " , ";
                }
                if (vendor.Partner7FatherName != Partner7FatherName)
                {
                    OldValues = OldValues + "Partner 7 Father Name : " + vendor.Partner7FatherName + " , ";
                    NewValues = NewValues + "Partner 7 Father Name : " + Partner7FatherName + " , ";
                }
                if (vendor.Partner7Address != Partner7Address)
                {
                    OldValues = OldValues + "Partner 7 Address : " + vendor.Partner7Address + " , ";
                    NewValues = NewValues + "Partner 7 Address : " + Partner7Address + " , ";
                }

                if (vendor.Partner8Name != Partner8Name)
                {
                    OldValues = OldValues + "Partner 8 Name : " + vendor.Partner8Name + " , ";
                    NewValues = NewValues + "Partner 8 Name : " + Partner8Name + " , ";
                }
                if (vendor.Partner8Age != Partner8Age)
                {
                    OldValues = OldValues + "Partner 8 Age : " + vendor.Partner8Age + " , ";
                    NewValues = NewValues + "Partner 8 Age : " + Partner8Age + " , ";
                }
                if (vendor.Partner8FatherName != Partner8FatherName)
                {
                    OldValues = OldValues + "Partner 8 Father Name : " + vendor.Partner8FatherName + " , ";
                    NewValues = NewValues + "Partner 8 Father Name : " + Partner8FatherName + " , ";
                }
                if (vendor.Partner8Address != Partner8Address)
                {
                    OldValues = OldValues + "Partner 8 Address : " + vendor.Partner8Address + " , ";
                    NewValues = NewValues + "Partner 8 Address : " + Partner8Address + " , ";
                }


                if (vendor.Partner9Name != Partner9Name)
                {
                    OldValues = OldValues + "Partner 9 Name : " + vendor.Partner9Name + " , ";
                    NewValues = NewValues + "Partner 9 Name : " + Partner9Name + " , ";
                }
                if (vendor.Partner9Age != Partner9Age)
                {
                    OldValues = OldValues + "Partner 9 Age : " + vendor.Partner9Age + " , ";
                    NewValues = NewValues + "Partner 9 Age : " + Partner9Age + " , ";
                }
                if (vendor.Partner9FatherName != Partner9FatherName)
                {
                    OldValues = OldValues + "Partner 9 Father Name : " + vendor.Partner9FatherName + " , ";
                    NewValues = NewValues + "Partner 9 Father Name : " + Partner9FatherName + " , ";
                }
                if (vendor.Partner9Address != Partner9Address)
                {
                    OldValues = OldValues + "Partner 9 Address : " + vendor.Partner9Address + " , ";
                    NewValues = NewValues + "Partner 9 Address : " + Partner9Address + " , ";
                }

                if (vendor.Partner10Name != Partner10Name)
                {
                    OldValues = OldValues + "Partner 10 Name : " + vendor.Partner10Name + " , ";
                    NewValues = NewValues + "Partner 10 Name : " + Partner10Name + " , ";
                }
                if (vendor.Partner10Age != Partner10Age)
                {
                    OldValues = OldValues + "Partner 10 Age : " + vendor.Partner10Age + " , ";
                    NewValues = NewValues + "Partner 10 Age : " + Partner10Age + " , ";
                }
                if (vendor.Partner10FatherName != Partner10FatherName)
                {
                    OldValues = OldValues + "Partner 10 Father Name : " + vendor.Partner10FatherName + " , ";
                    NewValues = NewValues + "Partner 10 Father Name : " + Partner10FatherName + " , ";
                }
                if (vendor.Partner10Address != Partner10Address)
                {
                    OldValues = OldValues + "Partner 10 Address : " + vendor.Partner10Address + " , ";
                    NewValues = NewValues + "Partner 10 Address : " + Partner10Address + " , ";
                }

                vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(TypeOfEntity);
                vendor.VendorVendorName = HttpUtility.HtmlEncode(VendorName);

                vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(PartnershipAddress);
                vendor.VendorAuthorisedSignatory = HttpUtility.HtmlEncode(PartnershipAuthSign);

                vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(Branch1);
                vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(Branch2);
                vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(Branch3);
                vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(Branch4);
                vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(Branch5);

                vendor.Partner1Name = HttpUtility.HtmlEncode(Partner1Name);
                //string Partner1_Age = vendor.Partner1Age.ToString();
                //Partner1_Age = HttpUtility.HtmlEncode(Partner1Age);

                vendor.Partner1Age = Partner1Age;

                vendor.Partner1FatherName = HttpUtility.HtmlEncode(Partner1FatherName);
                vendor.Partner1Address = HttpUtility.HtmlEncode(Partner1Address);

                vendor.Partner2Name = HttpUtility.HtmlEncode(Partner2Name);
                //string Partner2_Age = vendor.Partner2Age.ToString();
                //Partner2_Age = HttpUtility.HtmlEncode(Partner2Age);
                vendor.Partner2Age = Partner2Age;
                vendor.Partner2FatherName = HttpUtility.HtmlEncode(Partner2FatherName);
                vendor.Partner2Address = HttpUtility.HtmlEncode(Partner2Address);

                vendor.Partner3Name = HttpUtility.HtmlEncode(Partner3Name);
                //string Partner3_Age = vendor.Partner3Age.ToString();
                //Partner3_Age = HttpUtility.HtmlEncode(Partner3Age);
                vendor.Partner3Age = Partner3Age;
                vendor.Partner3FatherName = HttpUtility.HtmlEncode(Partner3FatherName);
                vendor.Partner3Address = HttpUtility.HtmlEncode(Partner3Address);

                vendor.Partner4Name = HttpUtility.HtmlEncode(Partner4Name);
                //string Partner4_Age = vendor.Partner4Age.ToString();
                //Partner4_Age = HttpUtility.HtmlEncode(Partner4Age);
                vendor.Partner4Age = Partner4Age;
                vendor.Partner4FatherName = HttpUtility.HtmlEncode(Partner4FatherName);
                vendor.Partner4Address = HttpUtility.HtmlEncode(Partner4Address);

                vendor.Partner5Name = HttpUtility.HtmlEncode(Partner5Name);
                //string Partner5_Age = vendor.Partner5Age.ToString();
                //Partner5_Age = HttpUtility.HtmlEncode(Partner5Age);
                vendor.Partner5Age = Partner5Age;
                vendor.Partner5FatherName = HttpUtility.HtmlEncode(Partner5FatherName);
                vendor.Partner5Address = HttpUtility.HtmlEncode(Partner5Address);

                vendor.Partner6Name = HttpUtility.HtmlEncode(Partner6Name);
                //string Partner6_Age = vendor.Partner6Age.ToString();
                //Partner6_Age = HttpUtility.HtmlEncode(Partner6Age);
                vendor.Partner6Age = Partner6Age;
                vendor.Partner6FatherName = HttpUtility.HtmlEncode(Partner6FatherName);
                vendor.Partner6Address = HttpUtility.HtmlEncode(Partner6Address);

                vendor.Partner7Name = HttpUtility.HtmlEncode(Partner7Name);
                //string Partner7_Age = vendor.Partner7Age.ToString();
                //Partner7_Age = HttpUtility.HtmlEncode(Partner7Age);
                vendor.Partner7Age = Partner7Age;
                vendor.Partner7FatherName = HttpUtility.HtmlEncode(Partner7FatherName);
                vendor.Partner7Address = HttpUtility.HtmlEncode(Partner7Address);

                vendor.Partner8Name = HttpUtility.HtmlEncode(Partner8Name);
                //string Partner8_Age = vendor.Partner8Age.ToString();
                //Partner8_Age = HttpUtility.HtmlEncode(Partner8Age);
                vendor.Partner8Age = Partner8Age;
                vendor.Partner8FatherName = HttpUtility.HtmlEncode(Partner8FatherName);
                vendor.Partner8Address = HttpUtility.HtmlEncode(Partner8Address);

                vendor.Partner9Name = HttpUtility.HtmlEncode(Partner9Name);
                //string Partner9_Age = vendor.Partner9Age.ToString();
                //Partner9_Age = HttpUtility.HtmlEncode(Partner9Age);
                vendor.Partner9Age = Partner9Age;
                vendor.Partner9FatherName = HttpUtility.HtmlEncode(Partner9FatherName);
                vendor.Partner9Address = HttpUtility.HtmlEncode(Partner9Address);

                vendor.Partner10Name = HttpUtility.HtmlEncode(Partner10Name);
                //string Partner10_Age = vendor.Partner10Age.ToString();
                //Partner10_Age = HttpUtility.HtmlEncode(Partner10Age);
                vendor.Partner10Age = Partner10Age;
                vendor.Partner10FatherName = HttpUtility.HtmlEncode(Partner10FatherName);
                vendor.Partner10Address = HttpUtility.HtmlEncode(Partner10Address);

                vendor.NameoftheProprietor = null;
                vendor.PAN = null;
                vendor.AdhaarCardNo = null;
                vendor.VendorCorporateIdentificationNumber = null;
                vendor.CorporateAddress = null;
                vendor.LLPIN = null;

                db.Entry(vendor).State = EntityState.Modified;

                Logger.Info("Accessing DB for Saving the VendorLog Details ");
                tblVendorLog log = new tblVendorLog();
                log.LogVendorUID = vendor.VendorVendorID;
                log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                log.LogActivity = "Modified";
                log.ChangedFrom = OldValues;
                log.ChangedTo = NewValues;
                log.DateandTime = DateTime.Now.ToString();

                db.tblVendorLogs.Add(log);

                db.SaveChanges();
                Logger.Info("Accessed DB, VendorMaster Details Saved");
                return Json("success");

            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'UpdatePartnershipVendor' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }




        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult VendorEdit(tblVendorMaster vendor,int VendorVendorID)
        {
            Logger.Info("Attempt Vendor VendorEdit");
            try
            {
                Logger.Info("Accessing DB for Updating the VendorMaster Details ");
              //  tblVendorMaster vendor = new tblVendorMaster();
            vendor.VendorVendorID = VendorVendorID;
            vendor.VendorVendorName = HttpUtility.HtmlEncode(vendor.VendorVendorName);
            vendor.VendorTypeofEntity = HttpUtility.HtmlEncode(vendor.VendorTypeofEntity);
            vendor.VendorCorporateIdentificationNumber = HttpUtility.HtmlEncode(vendor.VendorCorporateIdentificationNumber);
            vendor.VendorRegisteredAddress = HttpUtility.HtmlEncode(vendor.VendorRegisteredAddress);
            vendor.VendorAuthorisedSignatory = HttpUtility.HtmlEncode(vendor.VendorAuthorisedSignatory);
            vendor.VendorBranchOffice1 = HttpUtility.HtmlEncode(vendor.VendorBranchOffice1);
            vendor.VendorBranchOffice2 = HttpUtility.HtmlEncode(vendor.VendorBranchOffice2);
            vendor.VendorBranchOffice3 = HttpUtility.HtmlEncode(vendor.VendorBranchOffice3);
            vendor.VendorBranchOffice4 = HttpUtility.HtmlEncode(vendor.VendorBranchOffice4);
            vendor.VendorBranchOffice5 = HttpUtility.HtmlEncode(vendor.VendorBranchOffice5);
                Logger.Info("Checking model state validation");
                if (ModelState.IsValid)
            {
               
                db.Entry(vendor).State = EntityState.Modified;
               
                db.SaveChanges();
                    Logger.Info("Accesed DB, VendorMaster Details Updated");
                    Logger.Info("Redirecting to Repository");
                    TempData["status"] = "Success";
                    return RedirectToAction("Repository");
                    
            }
            
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'VendorEdit' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);

            }
            return View();
        }


        // GET: tblVendorMasters/Details/5
        public ActionResult Details()
        {
            return RedirectToAction("Repository");
        }

        [Route("Vendor/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            Logger.Info("Attempt Vendor Details");

            Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
            tblVendorMaster tblVendorMaster = db.tblVendorMasters.Find(id);
            if (tblVendorMaster == null)
            {
                Logger.Info("Accesed DB, Checking VendorMaster Details: Details not Found");
                return HttpNotFound();
            }
            Logger.Info("Accessed DB, checking  VendorMaster Details: Details Found");

            Logger.Info("Redirecting to VendorMaster Details Page");
            return View(tblVendorMaster);
        }


        [Authorize(Roles = "admin")]
        //[HttpPost, ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int VendorVendorID, int UserID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Vendor DeleteConfirmed");
            try
            {
                Logger.Info("Accessing DB for Deleting the VendorMaster Details ");

                tblVendorMaster tblVendorMaster = db.tblVendorMasters.Find(VendorVendorID);
            db.tblVendorMasters.Remove(tblVendorMaster);
            Logger.Info("Accesed DB, Checking User Details: User Saved");
            db.SaveChanges();

                Logger.Info("Accesed DB, VendorMaster Record Deleted");

                Logger.Info("Accessing DB for Saving the VendorLog Details ");
                tblVendorLog log = new tblVendorLog();
            log.LogVendorUID = VendorVendorID;
            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
            log.LogActivity = "Deletion";
            log.ChangedFrom = "-";
            log.ChangedTo = "-";
            log.DateandTime = DateTime.Now.ToString();

            db.tblVendorLogs.Add(log);
           
            db.SaveChanges();
                Logger.Info("Accesed DB, VendorLog Records Saved");

                
                
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'DeleteConfirmed' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //throw Ex;
            }
            TempData["deletestatus"] = "DeleteSuccess";
            Logger.Info("Redirecting to Repository");
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
            Logger.Info("Attempt Vendor Repository");
            Logger.Info("Accessing DB for Vendor Repository");

            IQueryable<VendorRepositoryModel> VendorList = null;

            VendorList = (from x in db.tblVendorMasters
                          select new VendorRepositoryModel()
                          {
                              VendorVendorID = x.VendorVendorID,
                              VendorVendorName = x.VendorVendorName,
                              VendorTypeofEntity = x.VendorTypeofEntity,
                              VendorRegisteredAddress = x.VendorRegisteredAddress
                          }).AsQueryable();


            //List<tblVendorMaster> VendorList = db.tblVendorMasters.ToList();
            //VendorList.Reverse();
            return View(VendorList);

        }

        public ActionResult Index()
        {
            return RedirectToAction("Repository");
        }

        [HttpPost]
        public JsonResult entity_list()
        {
            Logger.Info("Attempt Vendor entity_list");
            try
            {
                Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                var result = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorTypeofEntity;
                result = result.Distinct();
                Logger.Info("Accessed DB, Checked VendorMaster Details:Type Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'entity_list' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                throw Ex;
            }
        }

        [HttpPost]
        public ActionResult getLogDetail(int ID)
        {
            Logger.Info("Attempt Vendor getLogDetail");
            try
            {
                Logger.Info("Accessing DB for VendorLog Details: VendorLogID Match");
                var result = from tblVendorLog in db.tblVendorLogs.Where(x => x.LogVendorUID == ID) select tblVendorLog;
                Logger.Info("Accessed DB, Checked VendorLog Details : Details  Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'getLogDetail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                throw Ex;
            }
        }
        

        [HttpPost]
        public JsonResult entitylist()
        {
            Logger.Info("Attempt Vendor entitylist");
            try
            {
                Logger.Info("Accessing DB for VendorMaster Details: checking Status ");
                var result = from tblVendorMaster in db.tblVendorMasters select tblVendorMaster.VendorTypeofEntity;
                result = result.Distinct();
                Logger.Info("Accessed DB, Checked VendorMaster Details:Vendor Type Details Found ");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Vendor' Controller , 'entitylist' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                throw Ex;
            }
        }



        //----------upload Word Document-------//

        [HttpPost]
        public JsonResult UploadWordDocument(HttpPostedFileBase UploadWordDocument)
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

                string HTMLContent = DOCtoHTML(FilePath);
                if (!string.IsNullOrWhiteSpace(HTMLContent))
                {
                    return Json(HTMLContent);
                }
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