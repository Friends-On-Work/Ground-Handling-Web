using System.Threading.Tasks;

namespace Ground_Handlng.Abstractions.Identity
{
    public interface IETUserManager
    {
        Task<bool> AddUserToRole(string userId, string roleName);
        Task ClearUserRoles(string userId);
        Task RemoveFromRole(string userId, string roleName);
        Task DeleteRole(string roleId);
        Task<string> GetUserRolesAsync(string userId);
    }
}
