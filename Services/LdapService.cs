using System;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;

public class LdapService
{
    private readonly string _ldapServer = "10.99.100.5";
    private readonly int _ldapPort = 389;
    private readonly string _baseDn = "dc=kerrykfm01, dc=local";

    public bool AuthenticateUser(string username, string password)
    {
        try
        {
            using (var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, _ldapPort)))
            {
                ldapConnection.Credential = new NetworkCredential(username, password);
                ldapConnection.AuthType = AuthType.Basic;

                ldapConnection.Bind();
                return true;
            }
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"LDAP Authentication failed: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return false;
        }
    }


    public string GetUserInfo(string username)
    {
        try
        {
            using (var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer, _ldapPort)))
            {
                ldapConnection.Bind();

                var searchFilter = $"(&(objectClass=user)(sAMAccountName={username}))";
                var request = new SearchRequest(_baseDn, searchFilter, SearchScope.Subtree, "cn", "title", "department");

                var response = (SearchResponse)ldapConnection.SendRequest(request);

                if (response.Entries.Count > 0)
                {
                    var entry = response.Entries[0];

                    string fullName = entry.Attributes["cn"]?.ToString() ?? "N/A";
                    string title = entry.Attributes["title"]?.ToString() ?? "N/A";
                    string department = entry.Attributes["department"]?.ToString() ?? "N/A";

                    return $"Full Name: {fullName}, Title: {title}, Department: {department}";
                }

                return "User not found";
            }
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"LDAP search failed: {ex.Message}");
            return "Error during search";
        }
    }
}
