using todolistnew.Api.Models.Request;
using todolistnew.Api.Models.Response;
using todolistnew.Data.Entities;

namespace todolistnew.Api.Mappers;

public static class ResponseMapper
{
    public static ToDoResponseModel ToResponseModel(this ToDo entity) => new ToDoResponseModel
    {
        Id = entity.Id,
        TaskName = entity.TaskName,
        TaskDescription = entity.TaskDescription,
        TaskStatus = entity.TaskStatus,
        TaskDate = entity.TaskDate,
        DateStart = entity.DateStart,
        DateEnd = entity.DateEnd,
        TaskPriority = entity.TaskPriority
    };
}