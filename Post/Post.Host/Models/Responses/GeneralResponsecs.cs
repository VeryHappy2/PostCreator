namespace Post.Host.Models.Responses
{
    public record class GeneralResponse<T>(bool Flag, string Message, T Data);
    public record class GeneralResponse(bool Flag, string Message);
}
