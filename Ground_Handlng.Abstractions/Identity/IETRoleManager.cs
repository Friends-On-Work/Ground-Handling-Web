using System.Threading.Tasks;

namespace Ground_Handlng.Abstractions.Identity
{
    public interface IETRoleManager
    {
        Task<bool> RoleExists(string roleName);
        Task<bool> CreateRole(string roleName, string description = "");
    }
}
