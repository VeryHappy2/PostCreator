namespace Post.Host.Models.Requests.Bases
{
    public class BasePostCommentRequest
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
