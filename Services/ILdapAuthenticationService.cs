using System.Data;
using PMChecklist_PD_API.Models;

namespace PMChecklist_PD_API.Services
{
    public interface ILdapAuthenticationService
    {
        Task<List<LdapUser>> AuthenticateAsync(string username, string password);
    }
}
