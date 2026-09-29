namespace todolistnew.Request;

public record CreateUserReqest
{
    public required int UserId { get; set; }
    public required string? UserName { get; set; }
}

public readonly record struct UserDtoItemList(string UserId, string UserName);