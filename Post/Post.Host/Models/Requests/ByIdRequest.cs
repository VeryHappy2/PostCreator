using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class ByIdRequest<T>
    {
        [Required(ErrorMessage = "Id is required")]
        public T Id { get; set; }
    }
}