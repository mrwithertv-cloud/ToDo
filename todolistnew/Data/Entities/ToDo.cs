using todolistnew.Data.Enum;

namespace todolistnew.Data.Entities;

public record ToDo
{
    public int Id { get; init; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public required TaskStatusNum TaskStatus { get; set; }
    public DateTimeOffset TaskDate { get; set; } =  DateTimeOffset.Now;
    public required DateTimeOffset DateStart { get; set; }
    public required DateTimeOffset DateEnd { get; set; }
    public required TaskPriority TaskPriority { get; set; }
    public required int UserId { get; set; }
    public User User { get; set; }
}





