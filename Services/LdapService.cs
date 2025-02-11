using System.DirectoryServices;
using PMChecklist_PD_API.Models;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

public class LdapService
{
    private readonly string _ldapServer;
    private readonly string _ldapBaseDn;
    private readonly Connection _connection;

    public LdapService(IConfiguration configuration, Connection connection)
    {
        _connection = connection;
        _ldapServer = configuration["LdapSettings:LdapServer"]!;
        _ldapBaseDn = configuration["LdapSettings:LdapBaseDn"]!;
    }

    public List<LdapUser> AuthenticateAsync(string username, string password)
    {
        var ldapUrl = $"{_ldapServer}/{_ldapBaseDn}";
        var userData = new List<LdapUser>();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                var entry = new DirectoryEntry(ldapUrl, username, password);
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

                    var users = _connection.QueryData<Users>("SELECT g.UserID, gu.GUserID, gu.GUserName FROM Users as g LEFT JOIN GroupUsers as gu ON g.GUserID = gu.GUserID WHERE g.UserName = @cn", new { cn = fullName }).ToList();

                    var user = users.FirstOrDefault();
                    if (users.Any())
                    {
                        var permissionNames = _connection.QueryData<string>(
                            "SELECT p.PermissionName FROM GroupPermissions as gp " +
                            "INNER JOIN Permissions as p ON gp.PermissionID = p.PermissionID " +
                            "WHERE gp.GUserID = @GUserID AND gp.IsActive = 1 AND gp.PermissionStatus = 1",
                            new { GUserID = user!.GUserID! })
                            .ToArray();

                        userData.Add(new LdapUser
                        {
                            SAccout = samAccountName,
                            UserName = fullName,
                            Position = position,
                            DepartMent = department,
                            UserID = user.UserID,
                            GUserID = user.GUserID,
                            GUserName = user.GUserName,
                            Permissions = permissionNames
                        });
                    }
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                Console.WriteLine($"LDAP Connection Error: {ex.Message}");
                return userData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected LDAP error: {ex.Message}");
                return userData;
            }
        }

        return userData;
    }
}
