using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using todolistnew.Data;
using todolistnew.Request;
using todolistnew.Entities;


namespace todolistnew.Endpoints;

public static class ToDo
{
    public static void MapToDoEndpoints(this IEndpointRouteBuilder app)
    {
//Запрос на добавления задачи в список
        app.MapPost("/ToDo", async (ToDoRequest request, AppDbContext db) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);

            bool isValid =
                Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);

            if (!isValid)
            {
                var errors = validationResults.Select(r => r.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var entity = new ToDoEntity
            {
                TaskName = request.TaskName,
                TaskDescription = request.TaskDescription,
                TaskStatus = request.TaskStatus,
                TaskDate = DateTimeOffset.Now,
                DateStart = request.DateStart,
                DateEnd = request.DateEnd,
                TaskPriority = request.TaskPriority,
                UserId = request.UserId
            };

            db.ToDos.Add(entity);

            await db.SaveChangesAsync();

            return Results.Ok((entity));
        });


// Запрос на редактирование задачи по ID
        app.MapPut("/ToDoEdit/{id}", async (int id, ToDoRequest request, AppDbContext db) =>
        {
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = validationResults.Select(r => r.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var existingTask = await db.ToDos.FindAsync(id);

            if (existingTask == null)
                return Results.NotFound();

            existingTask.TaskName = request.TaskName;
            existingTask.TaskDescription = request.TaskDescription;
            existingTask.TaskStatus = request.TaskStatus;
            existingTask.DateStart = request.DateStart;
            existingTask.DateEnd = request.DateEnd;
            existingTask.TaskPriority = request.TaskPriority;

            await db.SaveChangesAsync();

            return Results.Ok(existingTask);
        });


//Запрос на получение ID
        app.MapGet("/ListTask", async (AppDbContext db) =>
        {
            var items = await db.ToDos
                .Select(item => new TaskListItemDto(item.Id.ToString(), item.TaskName, item.TaskPriority))
                .ToListAsync();

            if (items.Count == 0)

                return Results.NoContent();

            return Results.Ok(items);
        });


//Запрос на получение конкретного ID
        app.MapGet("/TaskId/{id}", async (int id, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task == null)
                return Results.NotFound();

            return Results.Ok(task);
        });


//Запрос на удаление задачи по ID
        app.MapDelete("/TaskDelete/{id}", async (int id, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task == null)
                return Results.NotFound();

            db.ToDos.Remove(task);

            await db.SaveChangesAsync();

            return Results.Ok(task);
        });
    }
}