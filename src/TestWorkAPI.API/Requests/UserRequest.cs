using System.ComponentModel.DataAnnotations;

namespace TestWorkAPI.API.Requests
{
    /// <summary>
    /// Request model for user.
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// User age.
        /// </summary>
        [Required]
        public int Age { get; set; }

        /// <summary>
        /// User email.
        /// </summary>      
        [Required]
        public string Email { get; set; }
    }
}
