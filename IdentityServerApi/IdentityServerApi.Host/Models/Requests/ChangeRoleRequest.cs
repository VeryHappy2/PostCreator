using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Host.Models.Requests
{
    public class RoleRequest
    {
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        [Required(ErrorMessage = "User name is required")]
        [MaxLength(10, ErrorMessage = "Name can have only 10 symbols")]
        public string UserName { get; set; }
    }
}
