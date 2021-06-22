using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractManagementSystem.Models
{
    public class UsersRepositoryModel
    {

        public int UserID { get; set; }
        public int UserEmployeeID { get; set; }
        public string UserEmployeeName { get; set; }
        public string UserEmployeeEmail { get; set; }
        public string UserCategory { get; set; }
        public string UserSubCategory { get; set; }
        public bool UserRoleInitiator { get; set; }
        public bool UserRoleApprover { get; set; }
        public bool UserRoleLegal { get; set; }
        public bool UserRoleFinance { get; set; }
        public bool UserRoleAdmin { get; set; }
        public bool UserRoleReviewer { get; set; }
        public string UserStatus { get; set; }
    }
}
