using System.ComponentModel.DataAnnotations;
using todolistnew.Api.Validation;
using todolistnew.Data.Enum;

namespace todolistnew.Api.Models.Request;

[DateTimeAtrebyte]
public record CreateToDoModel
{
    [Required(ErrorMessage = "Назавение должно быть заполено")]
    public required string TaskName { get; init; }

    [Required(ErrorMessage = "Описание должно быть заполенено")]
    public required string TaskDescription { get; init; }

    [EnumDataType(typeof(TaskStatusNum), ErrorMessage = "Статус должен стоять от 1 до 5 ")]
    public required TaskStatusNum TaskStatus { get; init; }
    public required DateTimeOffset DateStart { get; init; }
    public required DateTimeOffset DateEnd { get; init; }

    [EnumDataType(typeof(TaskPriority), ErrorMessage = "Приоритет должен быть от 1 (High) до 3 (Low)")]
    public required TaskPriority TaskPriority { get; init; }

    [Required(ErrorMessage = "Укажите ID пользователя")]
    public required int UserId { get; init; }
    
}