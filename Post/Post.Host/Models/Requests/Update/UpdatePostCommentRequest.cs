using Catalog.Host.Models.Requests;

namespace Post.Host.Models.Dtos
{
    public class UpdatePostCommentRequest : ByIdRequest
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
