using System.ComponentModel.DataAnnotations;

namespace Post.Host.Models.Requests.Bases
{
    public class BasePostCommentRequest
    {
        [MaxLength(1000, ErrorMessage = "Content can has only 50 symbols")]
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Post id is required")]
        public int PostId { get; set; }
    }
}
