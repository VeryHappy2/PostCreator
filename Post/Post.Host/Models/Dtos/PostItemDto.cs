namespace Post.Host.Models.Dtos
{
    public class PostItemDto : BaseDto
    {
        public string Title { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public PostCategoryDto Category { get; set; }
        public List<PostCommentDto> Comments { get; set; }
    }
}
