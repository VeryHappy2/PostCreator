namespace IdentityServerApi.Host.Models.Responses
{
    public record class GeneralResponse(bool Flag, string Message);
    public record class GeneralResponse<T>(bool Flag, string Message, T Data);
}
