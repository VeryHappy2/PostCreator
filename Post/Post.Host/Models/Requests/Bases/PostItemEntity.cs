using Post.Host.Models.Requests.Bases;

namespace Post.Host.Data.Entities
{
    public class BasePostItemRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}
