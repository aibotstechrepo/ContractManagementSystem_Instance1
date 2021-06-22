using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Configuration;
using NLog;

namespace ContractManagementSystem.Controllers
{
    public static class SMTP
    {
        public static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static bool Send(string[] To, string Subject, string Body)
        {

            try
            {
                string HostName = ConfigurationManager.AppSettings["SMTPHostName"];// "mail.aibotstech.com";
                string HostPort = ConfigurationManager.AppSettings["SMTPHostPort"]; 
                string UserName = ConfigurationManager.AppSettings["SMTPUserName"]; 
                string Password = ConfigurationManager.AppSettings["SMTPPassword"]; 
                
                string SenderEmailID = UserName;

                string[] CC = { SenderEmailID };

                //LogHandeler.ModuleLogFile("SMTP", "Send", "SMTP Started", "N", "39", null, null, "started");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(HostName, Convert.ToInt32(HostPort));
                mail.From = new MailAddress(SenderEmailID);
                mail.IsBodyHtml = true;

                if (To.Length > 0)
                {

                    foreach (var eachTo in To)
                    {
                        if (!String.IsNullOrWhiteSpace(eachTo))
                        {
                            mail.To.Add(eachTo);
                        }
                    }
                    // LogHandeler.ModuleLogFile("SMTP", "Send", "To List added", "N", "54", null, null, null);
                }
                if (CC.Length > 0)
                {
                    foreach (var eachCC in CC)
                    {
                        if (!String.IsNullOrWhiteSpace(eachCC))
                        {
                            mail.CC.Add(eachCC);
                        }
                    }
                    // LogHandeler.ModuleLogFile("SMTP", "Send", "CC List added", "N", "65", null, null, null);
                }
                string[] BCC = new string[0];
                if (BCC.Length > 0)
                {
                    foreach (var eachBCC in BCC)
                    {
                        if (!String.IsNullOrWhiteSpace(eachBCC))
                        {
                            mail.Bcc.Add(eachBCC);
                        }
                    }
                    //  LogHandeler.ModuleLogFile("SMTP", "Send", "BCC List added", "N", "76", null, null, null);
                }
                if (!String.IsNullOrWhiteSpace(Subject))
                {
                    mail.Subject = Subject;
                }
                if (!String.IsNullOrWhiteSpace(Body))
                {
                    mail.Body = Body;
                }
                string[] Attachments = { };
                if (Attachments.Length > 0)
                {
                    // LogHandeler.ModuleLogFile("SMTP", "Send", "Attachment available", "N", "89", null, null, null);
                    foreach (var eachFile in Attachments)
                    {
                        if (!String.IsNullOrWhiteSpace(eachFile))
                        {
                            mail.Attachments.Add(new Attachment(eachFile));
                        }

                    }
                }
                SmtpServer.Port = Convert.ToInt32(HostPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
                SmtpServer.Send(mail);
                //  LogHandeler.ModuleLogFile("SMTP", "Send", "Email Sent", "N", "89", null, null, null);
                return true;
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "SMTP Error" + " Exception : " + Ex.ToString() + " Stack Trace : " + Ex.StackTrace );
            }
            return false;
        }

        public static bool Send2(string[] To, string Subject, string Body, string[] Attachments)
        {

            try
            {
                string HostName = ConfigurationManager.AppSettings["SMTPHostName"];// "mail.aibotstech.com";
                string HostPort = ConfigurationManager.AppSettings["SMTPHostPort"];
                string UserName = ConfigurationManager.AppSettings["SMTPUserName"];
                string Password = ConfigurationManager.AppSettings["SMTPPassword"];

                string SenderEmailID = UserName;

                string[] CC = { SenderEmailID };

                //LogHandeler.ModuleLogFile("SMTP", "Send", "SMTP Started", "N", "39", null, null, "started");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(HostName, Convert.ToInt32(HostPort));
                mail.From = new MailAddress(SenderEmailID);
                mail.IsBodyHtml = true;

                if (To.Length > 0)
                {

                    foreach (var eachTo in To)
                    {
                        if (!String.IsNullOrWhiteSpace(eachTo))
                        {
                            mail.To.Add(eachTo);
                        }
                    }
                    // LogHandeler.ModuleLogFile("SMTP", "Send", "To List added", "N", "54", null, null, null);
                }
                if (CC.Length > 0)
                {
                    foreach (var eachCC in CC)
                    {
                        if (!String.IsNullOrWhiteSpace(eachCC))
                        {
                            mail.CC.Add(eachCC);
                        }
                    }
                    // LogHandeler.ModuleLogFile("SMTP", "Send", "CC List added", "N", "65", null, null, null);
                }
                string[] BCC = new string[0];
                if (BCC.Length > 0)
                {
                    foreach (var eachBCC in BCC)
                    {
                        if (!String.IsNullOrWhiteSpace(eachBCC))
                        {
                            mail.Bcc.Add(eachBCC);
                        }
                    }
                    //  LogHandeler.ModuleLogFile("SMTP", "Send", "BCC List added", "N", "76", null, null, null);
                }
                if (!String.IsNullOrWhiteSpace(Subject))
                {
                    mail.Subject = Subject;
                }
                if (!String.IsNullOrWhiteSpace(Body))
                {
                    mail.Body = Body;
                }
                //string[] Attachments = { };
                if (Attachments.Length > 0)
                {
                    // LogHandeler.ModuleLogFile("SMTP", "Send", "Attachment available", "N", "89", null, null, null);
                    foreach (var eachFile in Attachments)
                    {
                        if (!String.IsNullOrWhiteSpace(eachFile))
                        {
                            mail.Attachments.Add(new Attachment(eachFile));
                        }

                    }
                }
                SmtpServer.Port = Convert.ToInt32(HostPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
                SmtpServer.Send(mail);
                //  LogHandeler.ModuleLogFile("SMTP", "Send", "Email Sent", "N", "89", null, null, null);
                return true;
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "SMTP Error" + " Exception : " + Ex.ToString() + " Stack Trace : " + Ex.StackTrace);
            }
            return false;
        }

    }
}