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
            foreach (var username in usernames)
            {
                var user = db.Users.FirstOrDefault(u => u.FirstName.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user != null && roleNames.Contains("Admin"))
                {
                    // Find the "Admin" role
                    var adminRole = db.Roles.FirstOrDefault(r => r.Name == "Admin");
                    if (adminRole != null)
                    {
                        // Check if the user is already in the "Admin" role
                        var existingUserRole = db.UserRoles.FirstOrDefault(ur => ur.UserId == user.UserID && ur.RoleId == adminRole.RoleId);
                        if (existingUserRole == null)
                        {
                            // Assign "Admin" role to the user
                            db.UserRoles.Add(new UserRole { UserId = user.UserID, RoleId = adminRole.RoleId });
                            db.SaveChanges();
                        }
                    }
                }
            }
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
            var user = db.Users.FirstOrDefault(u => u.FirstName.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                // Fetch roles assigned in the UserRoles table
                var roles = db.UserRoles
                    .Where(ur => ur.UserId == user.UserID)
                    .Join(db.Roles, ur => ur.RoleId, r => r.RoleId, (ur, r) => r.Name)
                    .ToList();

                // Automatically assign "Admin" role if FirstName contains "admin"
                if (user.FirstName.ToLower().Contains("admin") && !roles.Contains("Admin"))
                {
                    roles.Add("Admin");
                }

                return roles.ToArray();
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
            foreach (var username in usernames)
            {
                var user = db.Users.FirstOrDefault(u => u.FirstName.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user != null && roleNames.Contains("Admin"))
                {
                    // Find the "Admin" role
                    var adminRole = db.Roles.FirstOrDefault(r => r.Name == "Admin");
                    if (adminRole != null)
                    {
                        // Find and remove the "Admin" role for the user
                        var userRole = db.UserRoles.FirstOrDefault(ur => ur.UserId == user.UserID && ur.RoleId == adminRole.RoleId);
                        if (userRole != null)
                        {
                            db.UserRoles.Remove(userRole);
                            db.SaveChanges();
                        }
                    }
                }
            }
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
