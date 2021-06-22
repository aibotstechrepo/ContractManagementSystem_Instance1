using ContractManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ContractManagementSystem.Controllers
{
    public class WebRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var context = new ContractManagementSystemDBEntities())
            {
                int id = Convert.ToInt32(username.Split('|')[1]);
                string[] roles = new string[0];
                var result = from tblUserMaster in context.tblUserMasters.Where(x => x.UserEmployeeID == id) select tblUserMaster;
                foreach (var User in result)
                {
                    if(User.UserRoleAdmin)
                    {
                        Array.Resize(ref roles, roles.Length + 1);
                        roles[roles.Length - 1] = "admin";
                    }
                    if (User.UserRoleReviewer)
                    {
                        Array.Resize(ref roles, roles.Length + 1);
                        roles[roles.Length - 1] = "reviewer";
                    }
                    if (User.UserRoleApprover || User.UserRoleFinance)
                    {
                        Array.Resize(ref roles, roles.Length + 1);
                        roles[roles.Length - 1] = "approver";
                    }
                    if (User.UserRoleLegal)
                    {
                        Array.Resize(ref roles, roles.Length + 2);
                        roles[roles.Length - 2] = "legal";
                        roles[roles.Length - 1] = "approver";
                    }
                    if (User.UserRoleInitiator)
                    {
                        Array.Resize(ref roles, roles.Length + 1);
                        roles[roles.Length - 1] = "initiator";
                    }
                }
                return roles;
            }
                
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}