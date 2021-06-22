using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractManagementSystem.Models
{
    public class ApprovalsRepositoryModel
    {
        public int ApprovalID { get; set; }
        public string WorkflowType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public System.DateTime LastModified { get; set; }
    }
}