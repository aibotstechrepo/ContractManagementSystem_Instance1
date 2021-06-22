using ContractManagementSystem.Models;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ContractManagementSystem.Controllers
{
    public class AlertsConfiguration : IJob
    {

        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        readonly string ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];
        Task IJob.Execute(IJobExecutionContext context)
        {

            ContractTimelineExpiry();
            TemplateApprovalTimeline();
            ContractApprovalTimeline();

            return null;
        }

        //===============Begin Contract Expiry TimeLine===============//
        void ContractExpiryTimeLine(int ID, int ContractID, string ContractName, string ContractType, string Category, string SubCategory, string DurationFrom, string DurationTo, string ReminderType, int Remainder, string Link)
        {
            try
            {

                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Expiry Timeline");


                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, ID + ".csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                    System.IO.File.AppendAllText(file, "Contract ID" + " , " + "Contract Name" + " , " + "Contract Type" + " , " + "Cluster" + " , " + "Function" + " , " + "Contract Effect From " + " , " + "Contract Effect To" + " , " + "Reminder Type" + " , " + "Remaining # days to expire" + " , " + "Contract Link" + Environment.NewLine);

                }
                //System.IO.File.WriteAllText(file, "");
                System.IO.File.AppendAllText(file, ContractID + " , " + ContractName + " , " + ContractType + " , " + Category + " , " + SubCategory + " , " + DurationFrom + " , " + DurationTo + " , " + ReminderType + " , " + Remainder + " , " + Link + Environment.NewLine);
            }
            catch { }
        }

        void ClearContractExpiryTimeLine()
        {
            try
            {
                string path = "";
                path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Expiry Timeline");
                FileDelete(path);

                path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Contract Approval Timeline");
                FileDelete(path);
                path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Contract Escalation Alert");
                FileDelete(path);
                path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Template Approval Timeline");
                FileDelete(path);


                //System.IO.File.WriteAllText(file, "");

            }
            catch { }
        }

        void FileDelete(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string[] files = Directory.GetFiles(path);
            foreach (var item in files)
            {
                System.IO.File.Delete(item);
            }

        }
        void ContractTimelineExpiry()
        {
            try
            {
                ClearContractExpiryTimeLine();


                //EscalationAlerts();
                //TemplateApprovalTimeline();
                //ContractApprovalTimeline();
                var Remainder1Date = "";
                Remainder1Date = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder1Date.ToString()).First();
                var Remainder1Duration = "";
                Remainder1Duration = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder1Duration.ToString()).First();

                var Remainder2Date = "";
                Remainder2Date = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder2Date.ToString()).First();
                var Remainder2Duration = "";
                Remainder2Duration = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder2Duration.ToString()).First();

                var Remainder3Date = "";
                Remainder3Date = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder3Date.ToString()).First();
                var Remainder3Duration = "";
                Remainder3Duration = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder3Duration.ToString()).First();

                var Remainder4Date = "";
                Remainder4Date = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder4Date.ToString()).First();
                var Remainder4Duration = "";
                Remainder4Duration = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder4Duration.ToString()).First();

                var Remainder5Date = "";
                Remainder5Date = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder5Date.ToString()).First();
                var Remainder5Duration = "";
                Remainder5Duration = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractExpiryReminder5Duration.ToString()).First();


                int Rem1 = 0;
                int Rem2 = 0;
                int Rem3 = 0;
                int Rem4 = 0;
                int Rem5 = 0;

                if (Remainder1Duration == "Week")
                {
                    Rem1 = Convert.ToInt32(Remainder1Date) * 7;
                }
                else if (Remainder1Duration == "Month")
                {
                    Rem1 = Convert.ToInt32(Remainder1Date) * 30;
                }
                else if (Remainder1Duration == "Day")
                {
                    Rem1 = Convert.ToInt32(Remainder1Date);
                }
                if (Remainder2Duration == "Week")
                {
                    Rem2 = Convert.ToInt32(Remainder2Date) * 7;
                }
                else if (Remainder2Duration == "Month")
                {
                    Rem2 = Convert.ToInt32(Remainder2Date) * 30;
                }
                else if (Remainder2Duration == "Day")
                {
                    Rem2 = Convert.ToInt32(Remainder2Date);
                }
                if (Remainder3Duration == "Week")
                {
                    Rem3 = Convert.ToInt32(Remainder3Date) * 7;
                }
                else if (Remainder3Duration == "Month")
                {
                    Rem3 = Convert.ToInt32(Remainder3Date) * 30;
                }
                else if (Remainder3Duration == "Day")
                {
                    Rem3 = Convert.ToInt32(Remainder3Date);
                }
                if (Remainder4Duration == "Week")
                {
                    Rem4 = Convert.ToInt32(Remainder4Date) * 7;
                }
                else if (Remainder4Duration == "Month")
                {
                    Rem4 = Convert.ToInt32(Remainder4Date) * 30;
                }
                else if (Remainder4Duration == "Day")
                {
                    Rem4 = Convert.ToInt32(Remainder4Date);
                }
                if (Remainder5Duration == "Week")
                {
                    Rem5 = Convert.ToInt32(Remainder5Date) * 7;
                }
                else if (Remainder5Duration == "Month")
                {
                    Rem5 = Convert.ToInt32(Remainder5Date) * 30;
                }
                else if (Remainder5Duration == "Day")
                {
                    Rem5 = Convert.ToInt32(Remainder5Date);
                }



                var ContractTable = from tblContractMaster in db.tblContractMasters.Where(x => x.Status == "In Effect") select tblContractMaster;
                foreach (var eachContract in ContractTable)
                {
                    var ContractID = eachContract.ContractID;
                    var ContractName = eachContract.ContractName;
                    var DurationFrom = eachContract.InEffectFrom;
                    var DurationTo = eachContract.InEffectTo;
                    var ContractType = eachContract.ContractType;
                    var Category = eachContract.ContractCategory;
                    var SubCategory = eachContract.ContractSubCategory;

                    int Initiator = 0; int Approver1ID = 0; int Approver2ID = 0; int Approver3ID = 0; int Approver4ID = 0; int Approver5ID = 0; int Approver6ID = 0; int Approver7ID = 0; int Approver8ID = 0; int Approver9ID = 0; int Approver10ID = 0;
                    try { Initiator = Convert.ToInt32(eachContract.Initiator); } catch { }
                    try { Approver1ID = Convert.ToInt32(eachContract.Approver1ID); } catch { }
                    try { Approver2ID = Convert.ToInt32(eachContract.Approver2ID); } catch { }
                    try { Approver3ID = Convert.ToInt32(eachContract.Approver3ID); } catch { }
                    try { Approver4ID = Convert.ToInt32(eachContract.Approver4ID); } catch { }
                    try { Approver5ID = Convert.ToInt32(eachContract.Approver5ID); } catch { }
                    try { Approver6ID = Convert.ToInt32(eachContract.Approver6ID); } catch { }
                    try { Approver7ID = Convert.ToInt32(eachContract.Approver7ID); } catch { }
                    try { Approver8ID = Convert.ToInt32(eachContract.Approver8ID); } catch { }
                    try { Approver9ID = Convert.ToInt32(eachContract.Approver9ID); } catch { }
                    try { Approver10ID = Convert.ToInt32(eachContract.Approver10ID); } catch { }

                    string Link = ApplicationLink + "/Contract/Details/" + ContractID;

                    if (eachContract.Initiator.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Initiator, ContractID, ContractName, ContractType, Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver1ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver1ID, ContractID, ContractName, ContractType, Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver2ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver2ID, ContractID, ContractName, ContractType, Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver3ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver3ID, ContractID, ContractName, ContractType, Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver4ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver4ID, ContractID, ContractName, ContractType, Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver5ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver5ID, ContractID, ContractName, ContractType, Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver6ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver6ID, ContractID, ContractName, ContractType, Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver7ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver7ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver8ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver8ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver9ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver9ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver10ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver10ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                }

                var ContractModificationTable = from tblContractModification in db.tblContractModifications.Where(x => x.Status == "In Effect") select tblContractModification;

                foreach (var eachContract in ContractModificationTable)
                {

                    var ContractID = eachContract.ContractID;
                    var ContractName = eachContract.ContractName;
                    var DurationFrom = eachContract.InEffectFrom;
                    var DurationTo = eachContract.InEffectTo;
                    var ContractType = eachContract.ContractType;
                    var Category = eachContract.ContractCategory;
                    var SubCategory = eachContract.ContractSubCategory;

                    int Initiator = 0; int Approver1ID = 0; int Approver2ID = 0; int Approver3ID = 0; int Approver4ID = 0; int Approver5ID = 0; int Approver6ID = 0; int Approver7ID = 0; int Approver8ID = 0; int Approver9ID = 0; int Approver10ID = 0;
                    try { Initiator = Convert.ToInt32(eachContract.Initiator); } catch { }
                    try { Approver1ID = Convert.ToInt32(eachContract.Approver1ID); } catch { }
                    try { Approver2ID = Convert.ToInt32(eachContract.Approver2ID); } catch { }
                    try { Approver3ID = Convert.ToInt32(eachContract.Approver3ID); } catch { }
                    try { Approver4ID = Convert.ToInt32(eachContract.Approver4ID); } catch { }
                    try { Approver5ID = Convert.ToInt32(eachContract.Approver5ID); } catch { }
                    try { Approver6ID = Convert.ToInt32(eachContract.Approver6ID); } catch { }
                    try { Approver7ID = Convert.ToInt32(eachContract.Approver7ID); } catch { }
                    try { Approver8ID = Convert.ToInt32(eachContract.Approver8ID); } catch { }
                    try { Approver9ID = Convert.ToInt32(eachContract.Approver9ID); } catch { }
                    try { Approver10ID = Convert.ToInt32(eachContract.Approver10ID); } catch { }

                    string Link = ApplicationLink + "/Contract/Details/" + eachContract.OriginalContractID;


                    if (eachContract.Initiator.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Initiator, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver1ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver1ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver2ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver2ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver3ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver3ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver4ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver4ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver5ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver5ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver6ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver6ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver7ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver7ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver8ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver8ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver9ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver9ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                    if (eachContract.Approver10ID.ToString().Length > 1)
                    {
                        ContractTimelineExpiry(Rem1, Rem2, Rem3, Rem4, Rem5, DurationTo, Approver10ID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, Link);
                    }
                }


                AlertEmailForContractExpiry();
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                    
            }
        }

        void ContractTimelineExpiry(int Rem1, int Rem2, int Rem3, int Rem4, int Rem5, string InEffectTo, int ApproverID, int ContractID, string ContractName, string ContractType, string Category, string SubCategory, string DurationFrom, string DurationTo, string Link)
        {
            try
            {
                int Found = 1;
                if (!string.IsNullOrWhiteSpace(InEffectTo))
                {
                    DateTime ExpieryDate = DateTime.ParseExact(InEffectTo, "dd/MM/yyyy", null);

                    Found = DateTime.Compare(ExpieryDate, DateTime.Now);

                    int ExpiryDays = 0;
                    ExpiryDays = Convert.ToInt32((ExpieryDate - DateTime.Now).TotalDays);

                    if (ExpiryDays > 0 && ExpiryDays <= Rem1)
                    {
                        ContractExpiryTimeLine(ApproverID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, "5th Reminder", Rem1, Link);
                    }
                    else if (ExpiryDays >= Rem1 && ExpiryDays <= Rem2)
                    {
                        ContractExpiryTimeLine(ApproverID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, "4th Reminder", Rem2, Link);
                    }
                    else if (ExpiryDays >= Rem2 && ExpiryDays <= Rem3)
                    {
                        ContractExpiryTimeLine(ApproverID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, "3rd Reminder", Rem3, Link);

                    }
                    else if (ExpiryDays >= Rem3 && ExpiryDays <= Rem4)
                    {
                        ContractExpiryTimeLine(ApproverID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, "2nd Reminder", Rem4, Link);

                    }
                    else if (ExpiryDays >= Rem4 && ExpiryDays <= Rem5)
                    {
                        ContractExpiryTimeLine(ApproverID, ContractID, ContractName, ContractType,  Category, SubCategory, DurationFrom, DurationTo, "1st Reminder", Rem5, Link);
                    }
                }
            }
            catch (Exception Ex) {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }


        }

        void AlertEmailForContractExpiry()
        {
            try
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Expiry Timeline");
                string[] Files = Directory.GetFiles(path);

                //int i = 0;
                for (int i = 0; i < Files.Length; i++)
                {
                    FileInfo fi = new FileInfo(Files[i]);
                    string ID = fi.Name;

                    string EmpID = ID.Replace(".csv", "");


                    var EmployeeName = "";
                    EmployeeName = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeName).First();

                    var EmployeeEmail = "";
                    EmployeeEmail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeEmail).First();

                    string[] TO = { EmployeeEmail };
                    string[] Attachments = { Files[i] };

                    string Subject = "Contract Expiry reminder";
                    string Paragraph = "Kindly find the attached document contains list of contracts expire soon. ";
                    string Body = "<html><body>Dear " + EmployeeName + ",<br/><br/>" + Paragraph + "<br/><br/>Regards,<br/><b>System Admin</b><br/><b>Contract Management System</b></body></html>";
                    SMTP.Send2(TO, Subject, Body, Attachments);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }
        //===============End Contract Expiry TimeLine===============//


        //===============Begin Template Approval TimeLine===============//
        void TemplateApprovalTimeline()
        {
            try
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Template Approval Timeline");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string[] files = Directory.GetFiles(path);
                foreach (var item in files)
                {
                    System.IO.File.Delete(item);
                }

                var TemplateTable = from tblTemplateMaster in db.tblTemplateMasters.Where(x => x.Status == "Pending Approval") select tblTemplateMaster;
                foreach (var eachTemplate in TemplateTable)
                {
                    var DaysToApprove = "";
                    DaysToApprove = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.TemplateLegal.ToString()).First();

                    string NextApprover = eachTemplate.NextApprover;
                    var TemplateID = eachTemplate.TemplateID;
                    var TemplateName = eachTemplate.Name;
                    var TemplateType = eachTemplate.Type;
                    if (NextApprover.Length > 1)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var file = Path.Combine(path, NextApprover + ".csv");

                        if (!(System.IO.File.Exists(file)))
                        {
                            System.IO.File.Create(file).Close();
                            System.IO.File.AppendAllText(file, "Template ID" + " , " + "Template Name" + " , " + "Template Type" + " , " + "Approve before # days" + " , " + "Template Link" + Environment.NewLine);
                        }

                        System.IO.File.AppendAllText(file, TemplateID + " , " + TemplateName + " , " + TemplateType + " , " + DaysToApprove + " , " + ApplicationLink + "/Template/Details/" + TemplateID + Environment.NewLine);

                    }
                }

                EmailForTemplateTimeLine();
            }
            catch(Exception Ex) {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }

        void EmailForTemplateTimeLine()
        {
            try
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Template Approval Timeline");
                string[] Files = Directory.GetFiles(path);

                //int i = 0;
                for (int i = 0; i < Files.Length; i++)
                {
                    FileInfo fi = new FileInfo(Files[i]);
                    string ID = fi.Name;

                    string EmpID = ID.Replace(".csv", "");


                    var EmployeeName = "";
                    EmployeeName = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeName).First();

                    var EmployeeEmail = "";
                    EmployeeEmail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeEmail).First();

                    string[] TO = { EmployeeEmail };
                    string[] Attachments = { Files[i] };

                    string Subject = "Template Approval reminder";
                    string Paragraph = "Kindly find the attached document contains list of Templates required your review and approval. ";
                    string Body = "<html><body>Dear " + EmployeeName + ",<br/><br/>" + Paragraph + "<br/><br/>Regards,<br/><b>System Admin</b><br/><b>Contract Management System</b></body></html>";
                    SMTP.Send2(TO, Subject, Body, Attachments);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }
        //===============End Template Approval Timeline===============//

        //===============Begin Contract Approval TimeLine===============//


        int MaxTimeByUserID(int UserID)
        {

            string UserRole = "";
            var UserTable = from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == UserID) select tblUserMaster;

            foreach (var item in UserTable)
            {
                if (item.UserRoleApprover == true)
                {
                    UserRole = UserRole + "Approver ";
                }
                if (item.UserRoleFinance == true)
                {
                    UserRole = UserRole + " Finance";

                }
                if (item.UserRoleLegal == true)
                {
                    UserRole = UserRole + " Legal";
                }
            }

            string DaysToApprove_Approver = "";
            string DaysToApprove_Finance = "";
            string DaysToApprove_Legal = "";
            DaysToApprove_Approver = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractHod.ToString()).First();
            DaysToApprove_Finance = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractFinance.ToString()).First();
            DaysToApprove_Legal = (from tblAlertSystem in db.tblAlertSystems select tblAlertSystem.ContractLegal.ToString()).First();

            int Approver_Days = 0;
            int Finance_Days = 0;
            int Legal_Days = 0;

            try { Approver_Days = Convert.ToInt32(DaysToApprove_Approver); } catch { }
            try { Finance_Days = Convert.ToInt32(DaysToApprove_Finance); } catch { }
            try { Legal_Days = Convert.ToInt32(DaysToApprove_Legal); } catch { }

            int highest = 0;
            if (UserRole.Contains("Approver") && UserRole.Contains("Finance") && UserRole.Contains("Legal"))
            {
                highest = Math.Max(Approver_Days, Math.Max(Finance_Days, Legal_Days));
            }
            else if (UserRole.Contains("Approver") && UserRole.Contains("Finance"))
            {
                highest = Math.Max(Approver_Days, Finance_Days);
            }
            else if (UserRole.Contains("Approver") && UserRole.Contains("Legal"))
            {
                highest = Math.Max(Approver_Days, Legal_Days);
            }
            else if (UserRole.Contains("Finance") && UserRole.Contains("Legal"))
            {
                highest = Math.Max(Finance_Days, Legal_Days);
            }
            else if (UserRole.Contains("Approver"))
            {
                highest = Approver_Days;
            }
            else if (UserRole.Contains("Finance"))
            {
                highest = Finance_Days;
            }
            else if (UserRole.Contains("Legal"))
            {
                highest = Legal_Days;
            }

            return highest;
        }

        string GetUserIDNameEMailByEmpID(int EmpID)
        {
            tblUserMaster UserData = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID == EmpID) select tblUserMaster).FirstOrDefault();
            return UserData.UserEmployeeID + " - " + UserData.UserEmployeeName + " - " + UserData.UserEmployeeEmail;

        }

        void ContractApprovalTimeline()
        {
            try

            {

                var ContractTable = from tblContractMaster in db.tblContractMasters.Where(x => x.Status == "Pending Approval") select tblContractMaster;
                foreach (var eachContract in ContractTable)
                {
                    int NextApprover = 0;
                    try { NextApprover = Convert.ToInt32(eachContract.NextApprover); } catch { }
                    int MaxTime = MaxTimeByUserID(NextApprover);



                    if (NextApprover == eachContract.Approver1ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver2ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver2ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver1ReceivedOn.ToString(), "Approver 1", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver2ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver3ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver3ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver2ReceivedOn.ToString(), "Approver 2", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver3ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver3ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver4ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver3ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver3ReceivedOn.ToString(), "Approver 3", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver4ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver4ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver5ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver4ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver4ReceivedOn.ToString(), "Approver 4", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver5ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver5ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver6ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver6ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver5ReceivedOn.ToString(), "Approver 5", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver6ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver6ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver7ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver7ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver6ReceivedOn.ToString(), "Approver 6", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver7ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver7ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver8ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver8ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver7ReceivedOn.ToString(), "Approver 7", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver8ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver8ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver9ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver9ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver8ReceivedOn.ToString(), "Approver 8", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver9ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver9ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver10ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver10ID), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver9ReceivedOn.ToString(), "Approver 9", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver10ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver10ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.ContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                    }


                }

                ContractModificationApprovalTimeline();


                EmailContractApprovalTimeline();
                EmailContractEscalation();
            }
            catch(Exception Ex) {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }
        void ContractModificationApprovalTimeline()
        {
            try
            {

                var ContractTable = from tblContractModification in db.tblContractModifications.Where(x => x.Status == "Pending Approval") select tblContractModification;
                foreach (var eachContract in ContractTable)
                {
                    int NextApprover = 0;
                    try { NextApprover = Convert.ToInt32(eachContract.NextApprover); } catch { }
                    int MaxTime = MaxTimeByUserID(NextApprover);



                    if (NextApprover == eachContract.Approver1ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver1ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver2ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver2ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver1ReceivedOn.ToString(), "Approver 1", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver2ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver2ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver3ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver3ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver2ReceivedOn.ToString(), "Approver 2", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver3ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver3ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver4ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver3ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver3ReceivedOn.ToString(), "Approver 3", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver4ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver4ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver5ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver4ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver4ReceivedOn.ToString(), "Approver 4", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver5ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver5ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver6ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver6ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver5ReceivedOn.ToString(), "Approver 5", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver6ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver6ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver7ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver7ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver6ReceivedOn.ToString(), "Approver 6", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver7ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver7ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver8ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver8ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver7ReceivedOn.ToString(), "Approver 7", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver8ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver8ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver9ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver9ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver8ReceivedOn.ToString(), "Approver 8", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver9ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver9ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                        if (RemainingDays > MaxTime)
                        {
                            if (eachContract.Approver10ID > 0)
                            {
                                //send Esclation email 
                                string PendingFrom = GetUserIDNameEMailByEmpID(Convert.ToInt32(NextApprover));
                                ContractEscalationTimeline(Convert.ToInt32(eachContract.Approver10ID), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, PendingFrom, eachContract.Approver9ReceivedOn.ToString(), "Approver 9", eachContract.InEffectFrom, eachContract.InEffectTo);


                            }
                        }

                    }
                    else if (NextApprover == eachContract.Approver10ID)
                    {
                        DateTime ReceivedDate = Convert.ToDateTime(eachContract.Approver10ReceivedOn);
                        int RemainingDays = Convert.ToInt32((DateTime.Now - ReceivedDate).TotalDays);
                        string RemaingDate = ReceivedDate.AddDays(MaxTime).ToString();

                        ContractApprovalTimeline(Convert.ToInt32(NextApprover), eachContract.OriginalContractID.ToString(), eachContract.ContractName, eachContract.ContractType,  eachContract.ContractCategory, eachContract.ContractSubCategory, RemaingDate, eachContract.InEffectFrom, eachContract.InEffectTo);

                       
                    }


                }



            }
            catch(Exception Ex) {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }

        void ContractApprovalTimeline(int NextApprover, string ContractID, string ContractName, string ContractType, string Category, string SubCategory, string DaysToApprove, string InEffectFrom, string InEffectTo)
        {
            try
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Contract Approval Timeline");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, NextApprover + ".csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                    System.IO.File.AppendAllText(file, "Contract ID" + " , " + "Contract Name" + " , " + "Contract Type" + " , " + "Cluster" + " , " + "Function" + " , " + "Approve on or before # Date" + " , " + "Contract InEffect From" + " , " + "Contract InEffect To" + " ," + "Contract Link" + Environment.NewLine);
                }

                System.IO.File.AppendAllText(file, ContractID + " , " + ContractName + " , " + ContractType + " , " + Category + " , " + SubCategory + " , " + DaysToApprove + " , " + InEffectFrom + " , " + InEffectTo + " , " + ApplicationLink + "/Contract/Details/" + ContractID + Environment.NewLine);
            }
            catch(Exception Ex) {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }


        void ContractEscalationTimeline(int NextApprover, string ContractID, string ContractName, string ContractType, string Category, string SubCategory, string PendingFrom, string ReceivedOn, string ApprovalLevel, string InEffectFrom, string InEffectTo)
        {
            try
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Contract Escalation Alert");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var file = Path.Combine(path, NextApprover + ".csv");

                if (!(System.IO.File.Exists(file)))
                {
                    System.IO.File.Create(file).Close();
                    System.IO.File.AppendAllText(file, "Contract ID" + " , " + "Contract Name" + " , " + "Contract Type" + " , " + "Cluster" + " , " + "Function" + " , " + "Approval Pending From" + " , " + "Received for approval on" + " , " + "Approval Level" + ", " + "Contract InEffect From" + " , " + "Contract InEffect To" + Environment.NewLine);
                }

                System.IO.File.AppendAllText(file, ContractID + " , " + ContractName + " , " + ContractType + " , " + Category + " , " + SubCategory + " , " + PendingFrom + " , " + ReceivedOn + " , " + ApprovalLevel + " , " + InEffectFrom + " , " + InEffectTo + Environment.NewLine);
            }
            catch(Exception Ex) {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }


        void EmailContractApprovalTimeline()
        {
            try
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Contract Approval Timeline");
                string[] Files = Directory.GetFiles(path);

                //int i = 0;
                for (int i = 0; i < Files.Length; i++)
                {
                    FileInfo fi = new FileInfo(Files[i]);
                    string ID = fi.Name;

                    string EmpID = ID.Replace(".csv", "");


                    var EmployeeName = "";
                    EmployeeName = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeName).First();

                    var EmployeeEmail = "";
                    EmployeeEmail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeEmail).First();

                    string[] TO = { EmployeeEmail };
                    string[] Attachments = { Files[i] };

                    string Subject = "Contract Approval reminder";
                    string Paragraph = "Kindly find the attached document contains list of Contracts required your review and approval. ";
                    string Body = "<html><body>Dear " + EmployeeName + ",<br/><br/>" + Paragraph + "<br/><br/>Regards,<br/><b>System Admin</b><br/><b>Contract Management System</b></body></html>";
                    SMTP.Send2(TO, Subject, Body, Attachments);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }

        void EmailContractEscalation()
        {
            try
            {
                string path = WebConfigurationManager.AppSettings["ABTWorkingDirectory"]; path = Path.Combine(path, "Alerts", "Contract Escalation Alert");
                string[] Files = Directory.GetFiles(path);

                //int i = 0;
                for (int i = 0; i < Files.Length; i++)
                {
                    FileInfo fi = new FileInfo(Files[i]);
                    string ID = fi.Name;

                    string EmpID = ID.Replace(".csv", "");


                    var EmployeeName = "";
                    EmployeeName = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeName).First();

                    var EmployeeEmail = "";
                    EmployeeEmail = (from tblUserMaster in db.tblUserMasters.Where(x => x.UserEmployeeID.ToString() == EmpID) select tblUserMaster.UserEmployeeEmail).First();

                    string[] TO = { EmployeeEmail };
                    string[] Attachments = { Files[i] };

                    string Subject = "Contract Escalation Alert";
                    string Paragraph = "Kindly find the attached document contains list of Contracts has been escalated. ";
                    string Body = "<html><body>Dear " + EmployeeName + ",<br/><br/>" + Paragraph + "<br/><br/>Regards,<br/><b>System Admin</b><br/><b>Contract Management System</b></body></html>";
                    SMTP.Send2(TO, Subject, Body, Attachments);
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'AlertConfiguration' Controller , 'ContractTimelineExpiry' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
        }


        //===============End Contract Approval Timeline===============//
    }
}