using Post.Host.Models.Requests.Bases;

namespace Post.Host.Models.Dtos
{
    public class UpdatePostCategoryRequest : BasePostCategoryRequest
    {
        public int Id { get; set; }
    }
}
