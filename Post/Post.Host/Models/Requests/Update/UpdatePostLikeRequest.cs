using System.ComponentModel.DataAnnotations;
using Post.Host.Models.Requests.Bases;

namespace Post.Host.Models.Dtos
{
    public class UpdatePostLikeRequest : BasePostLikeRequest
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
    }
}
