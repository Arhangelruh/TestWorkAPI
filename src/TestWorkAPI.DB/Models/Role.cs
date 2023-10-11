namespace TestWorkAPI.DB.Models
{
    /// <summary>
    /// Roles.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Identity key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Role name.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Navigation to UserRole.
        /// </summary>
        public ICollection<UserRole> UsersRoles { get; set; }

    }
}
