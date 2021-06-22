using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContractManagementSystem.Models;
using NLog;

namespace ContractManagementSystem.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }
        // GET: Home
        public ActionResult Dashboard()
        {
            Logger.Info("Accessing Home Dashboard Page");
            Logger.Info("Accessing DB for Dashboard");
            //string username = Request.ServerVariables["LOGON_USER"];
            // int EmployeeId = Convert.ToInt32(username.Split('|')[1]);
            CommonModel model = new CommonModel
            {
                Template = db.tblTemplateMasters,
                Contract = db.tblContractMasters,
                ContractModification = db.tblContractModifications,
                
            };
            return View(model);
            // return View();
        }

        public ActionResult Repository()
        {
            Logger.Info("Accessing Home Repository Page");
            Logger.Info("Accessing DB for Home Repository");
            //return View(db.tblContractMasters.ToList());
            CommonModel model = new CommonModel
            {
                Template = db.tblTemplateMasters,
                Contract = db.tblContractMasters,
                ContractModification = db.tblContractModifications,
               
            };
            return View(model);

            //List<tblContractMaster> ContractMasters = db.tblContractMasters.ToList();
            //ContractMasters.Reverse();
            //return View(ContractMasters);

        }

        [HttpPost]
        public JsonResult templatestatus(int EmpID)
        {
            Logger.Info("Attempt Home templatestatus");
            try
            {
                Logger.Info("Accessing DB for ContractMaster Details: ID match");
                var templatestatus = db.tblContractMasters.Where(x => x.Initiator == EmpID).Where(x => x.Status == "Draft").Count();
                Logger.Info("Accessed DB, Checking ContractMaster Details : Status Found");
                return Json(templatestatus);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Home' Controller , 'templatestatus' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult ContractStatistics(int EmployeeID)
        {
            Logger.Info("Attempt Home ContractStatistics");
            try
            {
                Logger.Info("Accessing DB for Contract Statistic Details : Checking Status");
                int[] DataPoints = new int[4];
                var Contract_data = db.tblContractMasters.Where(x => x.Initiator == EmployeeID || x.NextApprover == "" + EmployeeID);
                var Addendum_data = db.tblContractModifications.Where(x => x.Initiator == EmployeeID || x.NextApprover == "" + EmployeeID);


                DataPoints[0] = Contract_data.Where(x => x.Status == "Approved").Count();
                DataPoints[1] = Contract_data.Where(x => x.Status == "Expired").Count();
                DataPoints[2] = Contract_data.Where(x => x.Status == "Pending Approval").Count();
                DataPoints[3] = Contract_data.Where(x => x.Status == "Rejected").Count();

                DataPoints[0] = DataPoints[0] + Addendum_data.Where(x => x.Status == "Approved").Count();
                DataPoints[1] = DataPoints[1] + Addendum_data.Where(x => x.Status == "Expired").Count();
                DataPoints[2] = DataPoints[2] + Addendum_data.Where(x => x.Status == "Pending Approval").Count();
                DataPoints[3] = DataPoints[3] + Addendum_data.Where(x => x.Status == "Rejected").Count();


                Logger.Info("Accesed DB, Checking Contract Statistic Details : Status Count Found");
                return Json(DataPoints);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Home' Controller , 'ContractStatistics' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }


        [HttpPost]
        public JsonResult GetNextApproverName(int NextApproverID)
        {
            Logger.Info("Attempt Home GetNextApproverName");
            try
            {
                Logger.Info("Accessing DB for UserMaster Details: ID match");
                var Datas = db.tblUserMasters.Where(x => x.UserEmployeeID == NextApproverID).Select(x => new {x.UserEmployeeName });
                //result.se.LastOrDefault();
                var result = Datas.SingleOrDefault();
                Logger.Info("Accessed DB, Checking UserMaster Details : Status Found");
                return Json(result.UserEmployeeName);
            }
            catch (Exception Ex)
            {
                Logger.Error(Ex, "'Home' Controller , 'GetNextApproverName' Action HTTP POST Main Exception Message: " + Ex.Message + " | Exception Stacktrace" + Ex.StackTrace);
                //MessageBox.Show(ex.ToString());
                return Json("error");
            }
        }

    }
}