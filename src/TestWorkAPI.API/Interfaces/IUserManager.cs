using TestWorkAPI.API.Helpers;
using TestWorkAPI.API.Models;
using TestWorkAPI.API.Requests;
using TestWorkAPI.DB.Models;

namespace TestWorkAPI.API.Interfaces
{
    /// <summary>
    /// Service for work with users
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="listParameters">Request parsmeters</param>
        /// <returns>user's models and metadata</returns>
        Task<PageList<User>> GetAllUsersAsync(ListParameters listParameters);

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="userid">user id</param>
        /// <returns>user's model</returns>
        Task<UserViewModel> GetUserByIdAsync(int userid);

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="model">User model.</param>
        Task CreateUserAsync(UserRequest model);

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="userid">user model</param>
        /// <returns>result</returns>
        Task UpdateUserAsync(UserViewModel model);

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="userid">user id</param>
        /// <returns>result</returns>
        Task DeleteUserAsync(int userid);

        /// <summary>
        /// Check email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>result to find email in base</returns>
        Task<bool> CheckEmailAsync(string email);

        /// <summary>
        /// Add role to user.
        /// </summary>
        /// <param name="userid">User id</param>
        /// <param name="roleid">Role id</param>
        /// <returns></returns>
        Task AddUserRole(int userid, int roleid);

        /// <summary>
        /// Delete role from user.
        /// </summary>
        /// <param name="userid">User id</param>
        /// <param name="roleid">Role id</param>
        Task DeleteUserRole(int userid, int roleid);
    }
}
