namespace Post.Host.Models.Dtos
{
    public class PostItemDto : BaseDto
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public List<PostCommentDto> Comments { get; set; }
    }
}
