using System.ComponentModel.DataAnnotations;

namespace Post.Host.Data.Entities
{
    public class UpdatePostItemRequest : BasePostItemRequest
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
    }
}
