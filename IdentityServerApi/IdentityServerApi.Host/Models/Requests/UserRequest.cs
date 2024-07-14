using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Host.Models.Requests
{
    public class UserRequest
    {
        [Required]
        [MaxLength(10, ErrorMessage = "The name can't be more than 10 symbols")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "The addres is invalid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password, ErrorMessage = "The password is invalid")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password, ErrorMessage = "The password is invalid")]
        [Compare(nameof(Password), ErrorMessage = "The confirm password doesn't equals to the password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
