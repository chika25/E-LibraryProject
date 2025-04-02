using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using LibrarySystemProject.Models;


using System;
using System.Linq;
using System.Web.Security;

namespace LibrarySystemProject.Providers
{
    public class MyRoleProvider : RoleProvider
    {
        private LibraryContext db = new LibraryContext();

        public override string ApplicationName
        {
            get => throw new NotImplementedException();
            set { }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            // Implement logic to add users to roles
        }

        public override void CreateRole(string roleName)
        {
            // Implement logic to create a role
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            // Implement logic to delete a role
            return true;
        }

        public override string[] GetRolesForUser(string username)
        {
            Console.WriteLine($"Checking roles for {username}");

            // Fetch the user from the database
            var user = db.Users.FirstOrDefault(u => u.FirstName.ToLower() == username.ToLower());

            if (user != null && user.FirstName.ToLower().Contains("admin"))
            {
                return new string[] { "Admin" };
            }

            return new string[0];
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            // Implement logic to check if the user is in the specified role
            return true;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            // Implement logic to remove users from roles
        }

        public override string[] GetUsersInRole(string roleName)
        {
            // Implement logic to retrieve users in a specific role
            return new string[0]; 
        }

        public override string[] GetAllRoles()
        {
            // Implement logic to get all roles
            return new string[0]; 
        }

        public override bool RoleExists(string roleName)
        {
            // Implement logic to check if the role exists
            return false; 
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            // Implement logic to find users in a role
            return new string[0]; 
        }
    }
}
