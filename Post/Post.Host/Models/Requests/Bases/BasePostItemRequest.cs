using System.ComponentModel.DataAnnotations;
using Post.Host.Models.Requests.Bases;

namespace Post.Host.Data.Entities
{
    public class BasePostItemRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(150, ErrorMessage = "Title can has only 150 symbols")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [MaxLength(3000, ErrorMessage = "Content can has only 3000 symbols")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
    }
}
