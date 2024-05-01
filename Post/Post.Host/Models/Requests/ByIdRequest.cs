using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class ByIdRequest<T>
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {1} field must be greater than or equal to 1.")]
        public T Id { get; set; }
    }
}
