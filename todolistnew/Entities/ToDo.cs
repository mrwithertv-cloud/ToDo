using todolistnew.Enum;

namespace todolistnew.Entities;

public record ToDoEntity
{
    public int Id { get; set; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public required TaskStatusNum TaskStatus { get; set; }
    public DateTimeOffset TaskDate { get; set; }
    public DateTimeOffset DateStart { get; set; }
    public DateTimeOffset DateEnd { get; set; }
    public required TaskPriority TaskPriority { get; set; }
    public required int UserId { get; set; }
    public UserEntity User { get; set; }
}