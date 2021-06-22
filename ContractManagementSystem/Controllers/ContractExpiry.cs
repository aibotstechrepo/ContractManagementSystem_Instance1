using ContractManagementSystem.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ContractManagementSystem.Controllers
{
    public class ContractExpiry : IJob
    {
        ContractManagementSystemDBEntities db = new ContractManagementSystemDBEntities();
        readonly string ApplicationLink = WebConfigurationManager.AppSettings["ApplicationLink"];
        Task IJob.Execute(IJobExecutionContext context)
        {
            
            ContractExpiryMethod();
            return null;
        }

        void ContractExpiryMethod()
        {
            try
            {
                int Found = 1;
                var ContractTable = from tblContractMaster in db.tblContractMasters.Where(x => x.Status == "In Effect") select tblContractMaster;
                foreach (var eachContract in ContractTable)
                {
                    if (!string.IsNullOrWhiteSpace(eachContract.InEffectTo))
                    {
                        var dateAndTime = DateTime.Now;
                        var date = dateAndTime.Date;

                        DateTime ExpieryDate = DateTime.ParseExact(eachContract.InEffectTo, "dd/MM/yyyy", null);

                        Found = DateTime.Compare(ExpieryDate, date);

                        if (Found < 0)
                        {
                            eachContract.Status = "Expired";
                        }
                    }
                    db.Entry(eachContract).State = EntityState.Modified;

                }
                db.SaveChanges();

                var ModifiedContractTable = from tblContractModification in db.tblContractModifications.Where(x => x.Status == "In Effect") select tblContractModification;
                foreach (var eachContract in ModifiedContractTable)
                {
                    if (!string.IsNullOrWhiteSpace(eachContract.InEffectTo))
                    {
                        var dateAndTime = DateTime.Now;
                        var date = dateAndTime.Date;

                        DateTime ExpieryDate = DateTime.ParseExact(eachContract.InEffectTo, "dd/MM/yyyy", null);

                        Found = DateTime.Compare(ExpieryDate, date);

                        if (Found < 0)
                        {
                            eachContract.Status = "Expired";
                        }
                    }
                    db.Entry(eachContract).State = EntityState.Modified;

                }
                db.SaveChanges();

            }
            catch { }
        }
      
          }
}