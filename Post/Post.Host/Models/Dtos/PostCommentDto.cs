namespace Post.Host.Models.Dtos
{
    public class PostCommentDto : BaseDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
