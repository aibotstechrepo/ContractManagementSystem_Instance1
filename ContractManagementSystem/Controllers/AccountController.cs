using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.Windows.Forms;
using ContractManagementSystem.Models;
using System.Web.Security;
using NLog;

namespace ContractManagementSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
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
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        // GET: Account
        public ActionResult Login()
        {
            Logger.Info("Accessing Login Page");
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            Logger.Info("Attempt UID Login");
            try
            {
                using (var context = new ContractManagementSystemDBEntities())
                {

                    Logger.Info("Accessing DB for Login");
                    var LoginCheck = context.tblUserLogins.Where(x => x.UserName == UserName).Select(x => new {x.EmployeeID, x.Password }); /*select tblUserLogin;*/

                        Logger.Info("Accesed DB, Checking User Details: User Found");
                    if (LoginCheck.ToArray().Length > 0)
                    {
                        foreach (var l in LoginCheck)
                        {
                            if (l.Password == Password)
                            {

                                Logger.Info("Accesed DB, Checking User Details: Password Match");
                                var AccountStatus = context.tblUserMasters.Where(x => x.UserEmployeeID == l.EmployeeID).Select(x => new { x.UserEmployeeName, x.UserEmployeeID, x.UserRoleApprover, x.UserRoleAdmin, x.UserRoleFinance, x.UserRoleInitiator, x.UserRoleLegal, x.UserStatus }); /*select tblUserMaster;*/
                                foreach (var userData in AccountStatus)
                                {

                                    Logger.Info("Accesed DB, Checking User Details: Checking Status");
                                    if (userData.UserStatus.Trim().ToLower().Contains("active"))
                                    {
                                        Logger.Info("Accesed DB, Checking User Details: User is Active");
                                        //FormsAuthentication.SetAuthCookie(r.EmployeeID.ToString(), false);
                                        FormsAuthentication.SetAuthCookie(userData.UserEmployeeName + "|" + userData.UserEmployeeID, true);

                                        if ((userData.UserRoleAdmin) || (userData.UserRoleApprover) || (userData.UserRoleFinance) || (userData.UserRoleInitiator) || (userData.UserRoleLegal))
                                        {
                                            return Json(Url.Action("Dashboard", "Home"));
                                        }
                                        else
                                        {
                                            return Json(Url.Action("Repository", "Contract"));
                                        }
                                        
                                    }
                                    else
                                    {
                                        Logger.Info("Accesed DB, Checking User Details: User is Not Active");
                                        return Json("Invalid");
                                    }
                                }
                            }
                            else
                            {

                                Logger.Info("Accesed DB, Checking User Details: Password Not Matched");

                                return Json("Invalid");
                            }
                        }
                    }
                    else
                    {

                        Logger.Info("Accesed DB, Account Not Found");
                        //Account Not Found

                        return Json("Invalid");
                    }
                }
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Account' Controller , 'Login 'Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace"+ Ex.StackTrace);
                return Json("Invalid");
            }
            return Json("Invalid");
        }

        [HttpPost]
        public ActionResult Logout()
        {

            Logger.Info("Attempt UID Logout");

            try
            {
                Logger.Info("Accessing DB for Logout");
                FormsAuthentication.SignOut();
                Logger.Info("Setting Auth Cookie and Redericting to user");
                return Json(Url.Action("Login", "Account"));
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Account' Controller , 'Logout 'Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                // return Json("Invalid");
            }

            return Json("Invalid");
        }


        //        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        //var LoginCheckq = from tblUserLogin in db.tblUserLogins.Where(x => x.UserName == UserName) select tblUserLogin;
        //foreach (var l in LoginCheck)
        //{
        //    if(l.Password == Password)
        //    {

        //        var UserStatusCheck = from tblUserLogin in db.tblUserLogins.Where(x => x.UserName == UserName) select tblUserLogin;

        //        if ()
        //        FormsAuthentication.SetAuthCookie(r.EmployeeID.ToString(), false);
        //        FormsAuthentication.SetAuthCookie(UserName + "|" + UserId, true);

        //        return Json(Url.Action("Dashboard", "Home"));
        //        // Json("Valid");
        //    }
        //}



        public ActionResult SSO()
        {
            Logger.Info("Attempt UID SSO");
            try
            {
                Logger.Info("Accessing DB for SSO");
                var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                //MessageBox.Show(userName.ToString());
                //string userName = HttpContext.Current.User.Identity.Name.Split('\\')[1].ToString();
                //string displayName = GetAllADUsers().Where(x =>
                //  x.UserName == userName).Select(x => x.DisplayName).First();
                //return displayName;
            }
            catch (Exception Ex)
            { //Exception logic here 

                Logger.Error(Ex, "'Account' Controller , 'SSO 'Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
            }
            return View();
        }

        //List
        public ActionResult UsersAuthentication()
        {
            Logger.Info("Attempt UID UsersAuthentication");
            Logger.Info("Accesed DB, Checking User Details: Checking Status");
            return View();
        }

        public ActionResult CreateLogin()
        {
            Logger.Info("Attempt UID UsersAuthentication");
            Logger.Info("Accesed DB, Checking User Details: Checking Status");
            return View();
        }

        public ActionResult ErrorPage()
        {
            Logger.Info("Accessing 404 Error Page");
            return View();
        }

    }
}