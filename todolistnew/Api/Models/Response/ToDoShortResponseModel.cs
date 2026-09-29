using todolistnew.Data.Enum;

namespace todolistnew.Api.Models.Response;

public class ToDoShortResponseModel
{
    public required int Id { get; set; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public required TaskStatusNum TaskStatus { get; set; }
    public required TaskPriority TaskPriority { get; set; }
}