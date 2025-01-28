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

            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    using (var entry = new DirectoryEntry(ldapUrl, username, password))
                    {
                        using (var search = new DirectorySearcher(entry))
                        {
                            search.Filter = $"(SAMAccountName={username})";
                            search.PropertiesToLoad.Add("samaccountname");
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
                                        GUserName = users[0].GroupUser?.GUserName
                                    });
                                }
                                else
                                {
                                    Console.WriteLine($"User {fullName} not found in the local database.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"User {username} not found in LDAP.");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("DirectorySearcher is not supported on this platform.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during LDAP authentication: {ex.Message}");
            }

            return userData;
        }
    }
}
