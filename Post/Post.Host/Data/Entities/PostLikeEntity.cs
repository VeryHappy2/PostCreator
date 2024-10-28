namespace Post.Host.Data.Entities
{
    public class PostLikeEntity : BaseEntity
    {
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
}