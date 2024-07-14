namespace Post.Host.Data.Entities
{
    public class PostItemEntity : BaseEntity
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public List<PostCommentEntity> Comments { get; set; }
        public PostCategoryEntity Category { get; set; }
    }
}
