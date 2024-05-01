using Post.Host.Models.Requests.Bases;

namespace Post.Host.Data.Entities
{
    public class BasePostItemRequest
    {
        public string Date { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public List<BasePostCommentRequest> Comments { get; set; }
        public PostCategoryEntity Category { get; set; }
    }
}
