using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Host.Models.Requests
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
