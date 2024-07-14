using System.ComponentModel.DataAnnotations;
using Post.Host.Models.Requests.Bases;

namespace Post.Host.Data.Entities
{
    public class BasePostItemRequest
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(3000)]
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}
