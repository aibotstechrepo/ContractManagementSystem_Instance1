using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractManagementSystem.Models
{
    public class TemplateRepositoryModel
    {
        public int TemplateID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public Nullable<int> Initiator { get; set; }
        public Nullable<System.DateTime> DateofInitiated { get; set; }
        public string PendingFrom { get; set; }
        public string NextApprover { get; set; }
        public string RejectedBy { get; set; }
        public string Status { get; set; }
        public string PhysicalCopyLocation { get; set; }
    }
}