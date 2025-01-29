using System;
using System.Collections.Generic;
using System.DirectoryServices;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using PMChecklist_PD_API.Models;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace PMChecklist_PD_API.Services
{
    public class LdapService
    {
        private readonly string _ldapServer;
        private readonly string _ldapBaseDn;
        private readonly PCMhecklistContext _context;

        public LdapService(IConfiguration configuration, PCMhecklistContext context)
        {
            _context = context;
            _ldapServer = configuration["LdapSettings:LdapServer"] ?? "LDAP://10.99.100.5";
            _ldapBaseDn = configuration["LdapSettings:LdapBaseDn"] ?? "dc=kerrykfm01,dc=local";
        }

        public async Task<List<LdapUser>> AuthenticateAsync(string username, string password)
        {
            var ldapUrl = $"{_ldapServer}/{_ldapBaseDn}";
            var userData = new List<LdapUser>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    using (var entry = new DirectoryEntry(ldapUrl, username, password))
                    {
                        object nativeObject = entry.NativeObject;

                        var search = new DirectorySearcher(entry);

                        search.Filter = $"(SAMAccountName={username})";
                        search.PropertiesToLoad.Add("sAMAccountName");
                        search.PropertiesToLoad.Add("cn");
                        search.PropertiesToLoad.Add("title");
                        search.PropertiesToLoad.Add("department");

                        var result = search.FindOne();

                        if (result != null)
                        {
                            var samAccountName = result.Properties["sAMAccountName"][0].ToString();
                            var fullName = result.Properties["cn"][0].ToString();
                            var position = result.Properties["title"][0].ToString();
                            var department = result.Properties["department"][0].ToString();

                            var users = await _context.Users
                                .Where(u => u.IsActive && u.UserName == fullName)
                                .Include(u => u.GroupUser)
                                .ToListAsync();

                            var sqlQuery = "SELECT p.PermissionName FROM GroupPermissions as gp INNER JOIN Permissions as p ON gp.PermissionID = p.PermissionID WHERE GuserID = {0} AND gp.IsActive = 1 AND gp.PermissionStatus = 1";

                            var permissionNames = await _context.Permissions
                                .FromSqlRaw(sqlQuery, users[0].GUserID)
                                .Select(p => p.PermissionName)
                                .ToListAsync();

                            string[] permissionNamesArray = permissionNames.ToArray()!;

                            if (users.Any())
                            {
                                userData.Add(new LdapUser
                                {
                                    UserName = samAccountName,
                                    FullName = fullName,
                                    Position = position,
                                    Department = department,
                                    UserID = users[0].UserID,
                                    GUserID = users[0].GUserID,
                                    GUserName = users[0].GroupUser?.GUserName,
                                    Permissons = permissionNamesArray
                                });
                            }
                            Console.WriteLine(userData);
                        }
                    }
                }
                catch (Exception)
                {
                    return userData;
                }


            }

            return userData;
        }

    }
}
