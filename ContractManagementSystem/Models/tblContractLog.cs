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
    
    public partial class tblContractLog
    {
        public int ID { get; set; }
        public int LogContractUID { get; set; }
        public string ModifiedBy { get; set; }
        public string LogActivity { get; set; }
        public string ChangedFrom { get; set; }
        public string ChangedTo { get; set; }
        public string DateandTime { get; set; }
    }
}
