//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContractManagementSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblContractModificationContent
    {
        public int UID { get; set; }
        public Nullable<int> ContractID { get; set; }
        public Nullable<int> EMPID { get; set; }
        public string ContractContent { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> Version { get; set; }
    }
}
