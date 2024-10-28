namespace Post.Host.Models.Dtos;

public class PostLikeDto : BaseDto
{
    public string UserId { get; set; }
    public int PostId { get; set; }
}