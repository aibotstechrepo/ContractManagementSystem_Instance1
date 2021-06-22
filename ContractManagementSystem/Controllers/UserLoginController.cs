using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{
    public class UserLoginController : Controller
    {
        public readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        // GET: UserLogin
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
        public ActionResult Login()
        {
            Logger.Info("Accessing UID Login Page");
            return View();
        }

        [HttpPost]
        public ActionResult LoginDetails(string username, string password)
        {
            Logger.Info("Attempt UID Login");
            try
            {
                Logger.Info("Accesed DB, Checking User Details: password and username match");
                var result = db.tblUserLogins.Where(x => x.UserName == username).Where(x => x.Password == password);
                Logger.Info("Accesed DB, Checking User Details: Login");
                return Json(result);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'UserLogin' Controller , 'LoginDetails' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                return Json("error");
            }

        }
    }
}