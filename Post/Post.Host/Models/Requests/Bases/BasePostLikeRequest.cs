using System.ComponentModel.DataAnnotations;

namespace Post.Host.Models.Requests.Bases
{
    public class BasePostLikeRequest
    {
        [Required(ErrorMessage = "Post id is required")]
        public int PostId { get; set; }
        public string UserId { get; set; }
    }
}
