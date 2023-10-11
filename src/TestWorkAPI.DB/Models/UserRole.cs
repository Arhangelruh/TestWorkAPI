using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace TestWorkAPI.DB.Models
{
    /// <summary>
    /// If we make role table like list of static roles, we need to make many-to-many relationship.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Identity key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation to Users.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Role identifier.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Navigation to Roles.
        /// </summary>
        public Role Role { get; set; }
    }
}
