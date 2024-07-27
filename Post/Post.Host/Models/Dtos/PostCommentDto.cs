namespace Post.Host.Models.Dtos
{
    public class PostCommentDto : BaseDto
    {
        public string UserName { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
