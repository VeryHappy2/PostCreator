#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Post.Host.Data.Entities
{
    public class PostItemEntity : BaseEntity
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public List<PostLikeEntity> Likes { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; } = 0;
        public int CategoryId { get; set; }
        public List<PostCommentEntity>? Comments { get; set; }
        public PostCategoryEntity Category { get; set; }
    }
}
