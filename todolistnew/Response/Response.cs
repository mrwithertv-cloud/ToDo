using todolistnew.Request;
using todolistnew.Entities;

namespace todolistnew.Response;

public static class Response
{
    public static ToDoResponse ToResponse(ToDoEntity entity) => new(
        entity.Id,
        entity.TaskName,
        entity.TaskDescription,
        entity.TaskStatus,
        entity.TaskDate,
        entity.DateStart,
        entity.DateEnd,
        entity.TaskPriority
    );
    
}