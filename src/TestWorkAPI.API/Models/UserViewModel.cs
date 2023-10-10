namespace TestWorkAPI.API.Models
{
    /// <summary>
    /// User model.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Identity key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User age.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// User email.
        /// </summary>      
        public string Email { get; set; }

        /// <summary>
        /// User roles.
        /// </summary>
        public List<RoleViewModel>? Roles { get; set; }
    }
}
