using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Host.Models.Requests
{
    public class RoleRequest
    {
        [Required]
        public string Role { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
