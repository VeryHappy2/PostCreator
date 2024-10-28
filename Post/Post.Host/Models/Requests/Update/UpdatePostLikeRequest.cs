using Catalog.Host.Models.Requests;
using Post.Host.Models.Requests.Bases;

namespace Post.Host.Models.Dtos
{
    public class UpdatePostLikeRequest : BasePostLikeRequest
    {
        public int Id { get; set; }
    }
}
