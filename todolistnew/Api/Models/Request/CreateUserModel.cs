namespace todolistnew.Api.Models.Request;

public record CreateUserModel
{
    public required string UserName { get; init; }
}