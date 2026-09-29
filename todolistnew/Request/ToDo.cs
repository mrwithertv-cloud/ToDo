using System.ComponentModel.DataAnnotations;
using todolistnew.Enum;
using todolistnew.Validation;

namespace todolistnew.Request;

[DateTimeAtrebyte]
public record ToDoRequest
{
    public string TaskId { get; set; } = null!;

    [Required(ErrorMessage = "Назавение должно быть заполено")]
    public required string TaskName { get; init; }

    [Required(ErrorMessage = "Описание должно быть заполенено")]
    public required string TaskDescription { get; init; }

    [EnumDataType(typeof(TaskStatusNum), ErrorMessage = "Статус должен стоять от 1 до 5 ")]
    public required TaskStatusNum TaskStatus { get; init; }

    public DateTimeOffset TaskDate { get; set; } = DateTimeOffset.Now;
    public required DateTimeOffset DateStart { get; set; }
    public required DateTimeOffset DateEnd { get; set; }

    [EnumDataType(typeof(TaskPriority), ErrorMessage = "Приоритет должен быть от 1 (High) до 3 (Low)")]
    public required TaskPriority TaskPriority { get; init; }
    [Required(ErrorMessage = "Укажите ID пользователя")]
    public required int UserId { get; init; }
}

public record ToDoResponse(
    int Id,
    string TaskName,
    string TaskDescription,
    TaskStatusNum TaskStatus,
    DateTimeOffset TaskDate,
    DateTimeOffset DateStart,
    DateTimeOffset DateEnd,
    TaskPriority TaskPriority
);

public readonly record struct TaskListItemDto(string TaskId, string TaskName, TaskPriority TaskPriority);