using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractManagementSystem.Models
{
    public class FunctionRepositoryModel
    {
        public int SubCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public System.DateTime LastModified { get; set; }
    }
}