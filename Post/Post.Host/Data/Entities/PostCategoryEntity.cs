using System.ComponentModel.DataAnnotations;

namespace Post.Host.Data.Entities
{
    public class PostCategoryEntity : BaseEntity
    {
        [MaxLength(100, ErrorMessage = "Category can have only 100 symbols")]
        public string Category { get; set; }
    }
}
