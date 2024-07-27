using Post.Host.Models.Requests.Bases;
using System.ComponentModel.DataAnnotations;

namespace Post.Host.Models.Dtos
{
    public class UpdatePostCategoryRequest : BasePostCategoryRequest
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
    }
}
