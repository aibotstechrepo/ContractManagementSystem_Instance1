using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContractManagementSystem.Models
{
    public class ClusterRepositoryModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public System.DateTime LastModified { get; set; }
    }
}