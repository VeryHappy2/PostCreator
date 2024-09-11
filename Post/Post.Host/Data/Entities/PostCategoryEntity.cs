using System.ComponentModel.DataAnnotations;

namespace Post.Host.Data.Entities
{
    public class PostCategoryEntity : BaseEntity
    {
        public string Category { get; set; }
    }
}
