using System.ComponentModel.DataAnnotations;
using Post.Host.Models.Requests.Bases;

namespace Post.Host.Models.Dtos
{
    public class UpdatePostCommentRequest : BasePostCommentRequest
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
    }
}
