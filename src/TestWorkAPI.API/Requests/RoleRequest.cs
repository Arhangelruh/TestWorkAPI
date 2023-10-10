using System.ComponentModel.DataAnnotations;

namespace TestWorkAPI.API.Requests
{ 
    /// <summary>
    /// Request model for role.
    /// </summary>
    public class RoleRequest
    {
        /// <summary>
        /// Role name.
        /// </summary>
        [Required]
        public string RoleName { get; set; }
    }
}
