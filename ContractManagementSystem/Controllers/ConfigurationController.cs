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
    [Authorize(Roles = "admin")]
    public class ConfigurationController : Controller
    {
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();

        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: AlertsAndNotification
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

        //public ActionResult AlertsNotification()
        //{
        //    return View();
        //}
        public ActionResult Index()
        {
            
            Logger.Info("Redirecting to AlertSystem Details Page");
            return View();
        }


        [HttpPost]
        public ActionResult SaveContractExpiryAlert(int Rem1Date, int Rem2Date, int Rem3Date, int Rem4Date, int Rem5Date, string Rem1Duration, string Rem2Duration,
           string Rem3Duration, string Rem4Duration, string Rem5Duration, int ID = 0)
        {
            int CurrentUser = 0;
            string CurrentUserName = "";
            try
            {
                CurrentUser = Convert.ToInt32(User.Identity.Name.Split('|')[1]);
                CurrentUserName = User.Identity.Name.Split('|')[0];
            }
            catch { }
            Logger.Info("Attempt Alerts And Notification SaveAlertRecord");
            try
            {
                Logger.Info("Accessing DB for saving  AlertSystem Record");
                tblAlertSystem m = db.tblAlertSystems.Find(ID);

                if (m == null)
                {
                    tblAlertSystem alert = new tblAlertSystem();

                    alert.ContractExpiryReminder1Date = Rem1Date;
                    alert.ContractExpiryReminder1Duration = HttpUtility.HtmlEncode(Rem1Duration);
                    alert.ContractExpiryReminder2Date = Rem2Date;
                    alert.ContractExpiryReminder2Duration = HttpUtility.HtmlEncode(Rem2Duration);
                    alert.ContractExpiryReminder3Date = Rem3Date;
                    alert.ContractExpiryReminder3Duration = HttpUtility.HtmlEncode(Rem3Duration);
                    alert.ContractExpiryReminder4Date = Rem4Date;
                    alert.ContractExpiryReminder4Duration = HttpUtility.HtmlEncode(Rem4Duration);
                    alert.ContractExpiryReminder5Date = Rem5Date;
                    alert.ContractExpiryReminder5Duration = HttpUtility.HtmlEncode(Rem5Duration);


                    alert.AuditTemplateDuration = "Day";
                    alert.AuditContractDuration = "Day";
                    alert.AuditClauseDuration = "Day";

                    db.tblAlertSystems.Add(alert);
                    db.SaveChanges();

                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                else
                {
                    string OldValues = "";
                    string NewValues = "";


                    if (m.ContractExpiryReminder1Date != Rem1Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder5 Date : " + m.ContractExpiryReminder1Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder5 Date : " + Rem1Date + " , ";
                    }
                    if (m.ContractExpiryReminder1Duration != Rem1Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder5 Duration : " + m.ContractExpiryReminder1Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder5 Duration : " + Rem1Duration + " , ";
                    }
                    if (m.ContractExpiryReminder2Date != Rem2Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder4 Date : " + m.ContractExpiryReminder2Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder4 Date : " + Rem2Date + " , ";
                    }
                    if (m.ContractExpiryReminder2Duration != Rem2Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder4 Duration : " + m.ContractExpiryReminder2Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder4 Duration : " + Rem2Duration + " , ";
                    }
                    if (m.ContractExpiryReminder3Date != Rem3Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder3 Date : " + m.ContractExpiryReminder3Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder3 Date : " + Rem3Date + " , ";
                    }
                    if (m.ContractExpiryReminder3Duration != Rem3Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder3 Duration : " + m.ContractExpiryReminder3Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder3 Duration : " + Rem3Duration + " , ";
                    }
                    if (m.ContractExpiryReminder4Date != Rem4Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder2 Date : " + m.ContractExpiryReminder4Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder2 Date : " + Rem4Date + " , ";
                    }
                    if (m.ContractExpiryReminder4Duration != Rem4Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder2 Duration : " + m.ContractExpiryReminder4Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder2 Duration : " + Rem4Duration + " , ";
                    }
                    if (m.ContractExpiryReminder5Date != Rem5Date)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder1 Date : " + m.ContractExpiryReminder5Date + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder1 Date : " + Rem5Date + " , ";
                    }
                    if (m.ContractExpiryReminder5Duration != Rem5Duration)
                    {
                        OldValues = OldValues + "Contract Expiry Reminder1 Duration : " + m.ContractExpiryReminder5Duration + " , ";
                        NewValues = NewValues + "Contract Expiry Reminder1 Duration : " + Rem5Duration + " , ";
                    }




                    //if (m.OldContractDate != model.OldContractDate)
                    //{
                    //    OldValues = OldValues + "Old Contract Date : " + m.OldContractDate + " , ";
                    //    NewValues = NewValues + "Old Contract Date : " + model.OldContractDate + " , ";
                    //}
                    //if (m.OldContractDuration != model.OldContractDuration)
                    //{
                    //    OldValues = OldValues + "Old Contract Duration : " + m.OldContractDuration + " , ";
                    //    NewValues = NewValues + "Old Contract Duration : " + model.OldContractDuration + " , ";
                    //}

                    m.ContractExpiryReminder1Date = Rem1Date;
                    m.ContractExpiryReminder1Duration = HttpUtility.HtmlEncode(Rem1Duration);
                    m.ContractExpiryReminder2Date = Rem2Date;
                    m.ContractExpiryReminder2Duration = HttpUtility.HtmlEncode(Rem2Duration);
                    m.ContractExpiryReminder3Date = Rem3Date;
                    m.ContractExpiryReminder3Duration = HttpUtility.HtmlEncode(Rem3Duration);
                    m.ContractExpiryReminder4Date = Rem4Date;
                    m.ContractExpiryReminder4Duration = HttpUtility.HtmlEncode(Rem4Duration);
                    m.ContractExpiryReminder5Date = Rem5Date;
                    m.ContractExpiryReminder5Duration = HttpUtility.HtmlEncode(Rem5Duration);

                    //m.OldContractDate = model.OldContractDate;
                    //m.OldContractDuration = HttpUtility.HtmlEncode(model.OldContractDuration);

                    db.Entry(m).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        tblAlertLog log = new tblAlertLog();
                        log.LogAlertUID = m.AlertID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblAlertLogs.Add(log);
                    }

                    db.SaveChanges();
                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                return Json("success");
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SaveAlertRecord' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return View(Ex.Message);

            }
        }

         [HttpPost]
        public ActionResult SaveApprovalAlerts(string TemLegal, string ContractHod, string ContractLegal, string ContractFinance, int ID = 0)
        {
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
                Logger.Info("Accessing DB for saving  AlertSystem Record");
                tblAlertSystem m = db.tblAlertSystems.Find(ID);

                if (m == null)
                {
                    tblAlertSystem alert = new tblAlertSystem();

                    alert.TemplateLegal = HttpUtility.HtmlEncode(TemLegal);
                    alert.ContractHod = HttpUtility.HtmlEncode(ContractHod);
                    alert.ContractFinance = HttpUtility.HtmlEncode(ContractFinance);
                    alert.ContractLegal = HttpUtility.HtmlEncode(ContractLegal);

                    alert.ContractExpiryReminder1Duration = "Day";
                    alert.ContractExpiryReminder2Duration = "Day";
                    alert.ContractExpiryReminder3Duration = "Day";
                    alert.ContractExpiryReminder4Duration = "Day";
                    alert.ContractExpiryReminder5Duration = "Day";
                    alert.AuditTemplateDuration = "Month";
                    alert.AuditContractDuration = "Month";
                    alert.AuditClauseDuration = "Month";

                    db.tblAlertSystems.Add(alert);
                    db.SaveChanges();

                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                else
                {
                    string OldValues = "";
                    string NewValues = "";


                   

                    if (m.TemplateLegal != TemLegal)
                    {
                        OldValues = OldValues + "Template Legal : " + m.TemplateLegal + " , ";
                        NewValues = NewValues + "Template Legal : " + TemLegal + " , ";
                    }
                    if (m.ContractHod != ContractHod)
                    {
                        OldValues = OldValues + "Contract Hod : " + m.ContractHod + " , ";
                        NewValues = NewValues + "Contract Hod : " + ContractHod + " , ";
                    }
                    if (m.ContractFinance != ContractFinance)
                    {
                        OldValues = OldValues + "Contract Finance : " + m.ContractFinance + " , ";
                        NewValues = NewValues + "Contract Finance : " + ContractFinance + " , ";
                    }
                    if (m.ContractLegal != ContractLegal)
                    {
                        OldValues = OldValues + "Contract Legal : " + m.ContractLegal + " , ";
                        NewValues = NewValues + "Contract Legal : " + ContractLegal + " , ";
                    }



                    //if (m.OldContractDate != model.OldContractDate)
                    //{
                    //    OldValues = OldValues + "Old Contract Date : " + m.OldContractDate + " , ";
                    //    NewValues = NewValues + "Old Contract Date : " + model.OldContractDate + " , ";
                    //}
                    //if (m.OldContractDuration != model.OldContractDuration)
                    //{
                    //    OldValues = OldValues + "Old Contract Duration : " + m.OldContractDuration + " , ";
                    //    NewValues = NewValues + "Old Contract Duration : " + model.OldContractDuration + " , ";
                    //}

                    m.TemplateLegal = HttpUtility.HtmlEncode(TemLegal);
                    m.ContractHod = HttpUtility.HtmlEncode(ContractHod);
                    m.ContractFinance = HttpUtility.HtmlEncode(ContractFinance);
                    m.ContractLegal = HttpUtility.HtmlEncode(ContractLegal);

                    //m.OldContractDate = model.OldContractDate;
                    //m.OldContractDuration = HttpUtility.HtmlEncode(model.OldContractDuration);

                    db.Entry(m).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        tblAlertLog log = new tblAlertLog();
                        log.LogAlertUID = m.AlertID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblAlertLogs.Add(log);
                    }

                    db.SaveChanges();
                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                return Json("success");
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SaveApprovalAlerts' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return View(Ex.Message);

            }
        }

        
        [HttpPost]
        public ActionResult SaveReviewAlerts(int TempDate, int ConDate, int ClaDate, string TempDuration, string ConDuration, string ClaDuration, int ID = 0)
        {
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
                Logger.Info("Accessing DB for saving  AlertSystem Record");
                tblAlertSystem m = db.tblAlertSystems.Find(ID);
                if (m == null)
                {
                    tblAlertSystem alert = new tblAlertSystem();

                    alert.AuditTemplateDate = TempDate;
                    alert.AuditTemplateDuration = HttpUtility.HtmlEncode(TempDuration);
                    alert.AuditContractDate = ConDate;
                    alert.AuditContractDuration = HttpUtility.HtmlEncode(ConDuration);
                    alert.AuditClauseDate = ClaDate;
                    alert.AuditClauseDuration = HttpUtility.HtmlEncode(ClaDuration);

                    alert.ContractExpiryReminder1Duration = "Day";
                    alert.ContractExpiryReminder2Duration = "Day";
                    alert.ContractExpiryReminder3Duration = "Day";
                    alert.ContractExpiryReminder4Duration = "Day";
                    alert.ContractExpiryReminder5Duration = "Day";

                    db.tblAlertSystems.Add(alert);
                    db.SaveChanges();

                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                else
                {
                    string OldValues = "";
                    string NewValues = "";


                    if (m.AuditTemplateDate != TempDate)
                    {
                        OldValues = OldValues + "Review Alert Template Date : " + m.AuditTemplateDate + " , ";
                        NewValues = NewValues + "Review Alert Template Date : " + TempDate + " , ";
                    }
                    if (m.AuditTemplateDuration != TempDuration)
                    {
                        OldValues = OldValues + "Review Alert Template Duration : " + m.AuditTemplateDuration + " , ";
                        NewValues = NewValues + "Review Alert Template Duration : " + TempDuration + " , ";
                    }
                    if (m.AuditContractDate != ConDate)
                    {
                        OldValues = OldValues + "Review Alert Contract Date : " + m.AuditContractDate + " , ";
                        NewValues = NewValues + "Review Alert Contract Date : " + ConDate + " , ";
                    }
                    if (m.AuditContractDuration != ConDuration)
                    {
                        OldValues = OldValues + "Review Alert Contract Duration : " + m.AuditContractDuration + " , ";
                        NewValues = NewValues + "Review Alert Contract Duration : " + ConDuration + " , ";
                    }
                    if (m.AuditClauseDate != ClaDate)
                    {
                        OldValues = OldValues + "Review Alert Clause Date : " + m.AuditClauseDate + " , ";
                        NewValues = NewValues + "Review Alert Clause Date : " + ClaDate + " , ";
                    }
                    if (m.AuditClauseDuration != ClaDuration)
                    {
                        OldValues = OldValues + "Review Alert Clause Duration : " + m.AuditClauseDuration + " , ";
                        NewValues = NewValues + "Review Alert Clause Duration : " + ClaDuration + " , ";
                    }

                    m.AuditTemplateDate = TempDate;
                    m.AuditTemplateDuration = HttpUtility.HtmlEncode(TempDuration);
                    m.AuditContractDate = ConDate;
                    m.AuditContractDuration = HttpUtility.HtmlEncode(ConDuration);
                    m.AuditClauseDate = ClaDate;
                    m.AuditClauseDuration = HttpUtility.HtmlEncode(ClaDuration);


                    db.Entry(m).State = EntityState.Modified;

                    if (OldValues.Length > 0)
                    {
                        tblAlertLog log = new tblAlertLog();
                        log.LogAlertUID = m.AlertID;
                        log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                        log.LogActivity = "Modified";
                        log.ChangedFrom = OldValues;
                        log.ChangedTo = NewValues;
                        log.DateandTime = DateTime.Now.ToString();

                        db.tblAlertLogs.Add(log);
                    }

                    db.SaveChanges();
                    Logger.Info("Accesed DB, AlertSystem Details Saved");
                }
                return Json("success");
            }

            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Configuration' Controller , 'SaveReviewAlerts' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return View(Ex.Message);

            }
        }


        [HttpPost]
        public ActionResult getLogDetail(int ID = 0)
        {
            Logger.Info("Attempt Alerts And Notification getLogDetail");
            try
            {
                Logger.Info("Accessing DB for LogDetails");
                var result = from tblAlertLog in db.tblAlertLogs select tblAlertLog;
                Logger.Info("Accesed DB, LogDetails  Found");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertsAndNotification' Controller , 'getLogDetail' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return View(Ex.Message);

            }
        }

        [HttpPost]
        public ActionResult SaveLog(string details, int ID, string initialvalue, string UserID)
        {
            Logger.Info("Attempt AlertsAndNotification SaveLog");
           
            try
            {
                Logger.Info("Accessing DB for saving AlertLog Details");

                tblAlertLog log = new tblAlertLog
                {
                    LogAlertUID = ID,
                    ModifiedBy = UserID.ToString(),
                    LogActivity = "Modified",
                    ChangedFrom = initialvalue,
                    ChangedTo = details,
                    DateandTime = DateTime.Now.ToString()
                };

                db.tblAlertLogs.Add(log);
                db.SaveChanges();
                Logger.Info("Accesed DB, AlertLog  Records Saved");
                return Json("");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertsAndNotification' Controller , 'SaveLog' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json(Ex.Message);
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitPageSetUp(string PaperTypeValue, string PaperSizeValue, string TopValue, string LeftValue, string BottomValue, string RightValue)
        {
            //int ID = 0;

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
                Logger.Info("Accessing DB for Configuration Details");
                var pagesetupDetails = from tblConfiguration in db.tblConfigurations.Where(x => x.PaperType == PaperTypeValue)
                                       .Where(x => x.PaperSize == PaperSizeValue)
                                       select tblConfiguration;


                if (pagesetupDetails.ToList().Count > 0)
                {

                    foreach (var eachItem in pagesetupDetails)
                    {
                        string OldValues = "";
                        string NewValues = "";
                        if (eachItem.MarginTop != TopValue)
                        {
                            OldValues = OldValues + "Margin Top : " + eachItem.MarginTop + " , ";
                            NewValues = NewValues + "Margin Top : " + TopValue + " , ";
                        }
                        if (eachItem.MarginLeft != LeftValue)
                        {
                            OldValues = OldValues + "Margin Left : " + eachItem.MarginLeft + " , ";
                            NewValues = NewValues + "Margin Left : " + LeftValue + " , ";
                        }
                        if (eachItem.MarginBottom != BottomValue)
                        {
                            OldValues = OldValues + "Margin Bottom : " + eachItem.MarginBottom + " , ";
                            NewValues = NewValues + "Margin Bottom : " + BottomValue + " , ";
                        }
                        if (eachItem.MarginRight != RightValue)
                        {
                            OldValues = OldValues + "Margin Right : " + eachItem.MarginRight + " , ";
                            NewValues = NewValues + "Margin Right : " + RightValue + " , ";
                        }
                        eachItem.MarginTop = TopValue;
                        eachItem.MarginLeft = LeftValue;
                        eachItem.MarginBottom = BottomValue;
                        eachItem.MarginRight = RightValue;

                        db.Entry(eachItem).State = EntityState.Modified;

                        if (OldValues.Length > 0)
                        {
                            tblAlertLog log = new tblAlertLog();
                            log.LogAlertUID = eachItem.ID;
                            log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                            log.LogActivity = "Modified";
                            log.ChangedFrom = OldValues;
                            log.ChangedTo = NewValues;
                            log.DateandTime = DateTime.Now.ToString();

                            db.tblAlertLogs.Add(log);
                        }

                    }

                    db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }
                else
                {

                    tblConfiguration pagesetup = new tblConfiguration();
                    pagesetup.PaperType = PaperTypeValue;
                    pagesetup.PaperSize = PaperSizeValue;
                    pagesetup.MarginTop = TopValue;
                    pagesetup.MarginLeft = LeftValue;
                    pagesetup.MarginBottom = BottomValue;
                    pagesetup.MarginRight = RightValue;

                    db.tblConfigurations.Add(pagesetup);
                    db.SaveChanges();

                    //tblAlertLog log = new tblAlertLog();
                    //    log.LogAlertUID = pagesetup.ID;
                    //    log.ModifiedBy = CurrentUser.ToString() + " - " + CurrentUserName;
                    //    log.LogActivity = "Creation";
                    //    log.ChangedFrom = "-";
                    //    log.ChangedTo = "-";
                    //    log.DateandTime = DateTime.Now.ToString();

                    //    db.tblAlertLogs.Add(log);

                    //db.SaveChanges();
                    Logger.Info("Accessed DB, Record Saved");
                }

                return Json("success");
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Alerts and  Notification' Controller , 'SubmitPageSetUp' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);

                return Json("errors");
            }
        }


        [HttpPost]
        public ActionResult GetConfigurationTable(string PaperType, string PaperSize)
        {
            Logger.Info("Attempt AlertsAndNotification GetConfigurationTable");
            try
            {
                Logger.Info("Accessing DB for Configuration Details : Configuration match");
                var TableConfig = db.tblConfigurations.Where(x => x.PaperType == PaperType).Where(x => x.PaperSize == PaperSize).Select(x => new { x.PaperType, x.PaperSize, x.MarginTop, x.MarginRight, x.MarginLeft, x.MarginBottom });

                string[] eachItem = new string[9];
                foreach (var r in TableConfig)
                {
                    eachItem[0] = r.PaperType;
                    eachItem[1] = r.PaperSize;
                    eachItem[2] = r.MarginTop.ToString();
                    eachItem[3] = r.MarginLeft.ToString();
                    eachItem[4] = r.MarginBottom.ToString();
                    eachItem[5] = r.MarginRight.ToString();
                    eachItem[6] = "success";

                }
                Logger.Info("Accessed DB, Checking Configuration Details : Details Found");
                return Json(eachItem);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertsAndNotification' Controller , 'GetConfigurationTable' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult GetAlertSystemDetails()
        {
            var result = from tblAlertSystem in db.tblAlertSystems select tblAlertSystem;
            return Json(result);
        }

    }
}