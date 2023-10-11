using TestWorkAPI.API.Helpers;
using TestWorkAPI.API.Models;
using TestWorkAPI.API.Requests;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.API.Interfaces
{
    /// <summary>
    /// Service for work with roles.
    /// </summary>
    public interface IRoleManager
    {
        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <returns>All role models</returns>
        Task<PageList<Role>> GetAllRolesAsync(ListParameters listParameters);

        /// <summary>
        /// Get roles by user.
        /// </summary>
        /// <param name="userid">user id</param>
        /// <returns>List roles</returns>
        Task<List<RoleViewModel>> GetUserRolesAsync(int userid);

        /// <summary>
        /// Create role.
        /// </summary>
        /// <param name="userid">role model</param>
        /// <returns>result</returns>
        Task CreateRoleAsync(RoleRequest model);

        /// <summary>
        /// Get role by id.
        /// </summary>
        /// <param name="roleid">role id</param>
        /// <returns>Role model</returns>
        Task<RoleViewModel> GetRoleByIdAsync(int roleid);

        /// <summary>
        /// Delete role.
        /// </summary>
        /// <param name="roleid">role id</param>
        /// <returns>result</returns>
        Task DeleteRoleAsync(int roleid);

        /// <summary>
        /// Update role.
        /// </summary>
        /// <param name="roleid">role id</param>
        /// <returns>result</returns>
        Task UpdateRoleAsync(RoleViewModel model);
    }
}
