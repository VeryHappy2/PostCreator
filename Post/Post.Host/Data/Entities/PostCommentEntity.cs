namespace Post.Host.Data.Entities
{
    public class PostCommentEntity : BaseEntity
    {
        public string UserName { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
