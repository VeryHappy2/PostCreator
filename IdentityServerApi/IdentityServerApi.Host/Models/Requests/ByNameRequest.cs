using System.ComponentModel.DataAnnotations;

namespace IdentityServerApi.Models
{
    public class ByNameRequest<T>
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(10, ErrorMessage = "Name can have only 10 symbols")]
        public T Name { get; set; }
    }
}