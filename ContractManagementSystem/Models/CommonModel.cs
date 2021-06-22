using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using ContractManagementSystem.Models;

namespace ContractManagementSystem.Models
{
    public class CommonModel
    {
        [AllowHtml]
        public DbSet<tblUserMaster> Users { get; set; }
        [AllowHtml]
        public DbSet<tblClauseMaster> Clause { get; set; }
        [AllowHtml]
        public DbSet<tblCategory> Category { get; set; }
        [AllowHtml]
        public DbSet<tblSubCategory> SubCategory { get; set; }
        [AllowHtml]
        public DbSet<tblVendorMaster> Vendor { get; set; }
        [AllowHtml]
        public DbSet<tblAlertSystem> Alert { get; set; }
        [AllowHtml]
        public DbSet<tblApprovalMaster> Approval { get; set; }
        [AllowHtml]
        public DbSet<tblClauseAudit> ClauseAudit { get; set; }

        [AllowHtml]
       
        public DbSet<tblTemplateMaster> Template { get; set; }
        [AllowHtml]
        public DbSet<tblContractMaster> Contract { get; set; }
        [AllowHtml]
        public DbSet<tblContractContent> ContractContent { get; set; }

        [AllowHtml]
        public DbSet<tblContractModificationContent> ContractModificationContent { get; set; }
        [AllowHtml]
        public DbSet<tblTemplateContent> TemplateContent { get; set; }
        [AllowHtml]
        public DbSet<tblContractModification> ContractModification { get; set; }
        [AllowHtml]
        public DbSet<tblTemplateAudit> TemplateAudit { get; set; }
        [AllowHtml]
        public DbSet<tblContractAudit> ContractAudit { get; set; }
        [AllowHtml]
        public DbSet<tblVariable> Variables { get; set; }
        [AllowHtml]
        public DbSet<tblTemplateLog> TemplateLogs { get; set; }
        [AllowHtml]
        public DbSet<tblContractLog> ContractLogs { get; set; }
        [AllowHtml]
        public DbSet<tblVariableData> VariableDatas { get; set; }
 
    }
}

