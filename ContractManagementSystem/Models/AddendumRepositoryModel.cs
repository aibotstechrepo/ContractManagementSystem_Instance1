using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractManagementSystem.Models
{
    public class AddendumRepositoryModel
    {
        public int ContractID { get; set; }
        public string ContractName { get; set; }
        public string ContractType { get; set; }
        public string ContractCategory { get; set; }
        public string ContractSubCategory { get; set; }
        public Nullable<int> Initiator { get; set; }
        public string Status { get; set; }
        public Nullable<int> OriginalContractID { get; set; }
        public string ContractModificationType { get; set; }
        public Nullable<System.DateTime> DateofInitiated { get; set; }
        public string PendingFrom { get; set; }
        public string NextApprover { get; set; }
        public string RejectedBy { get; set; }
        public string InEffectTo { get; set; }
        public string PhysicalCopyLocation { get; set; }
    }
}