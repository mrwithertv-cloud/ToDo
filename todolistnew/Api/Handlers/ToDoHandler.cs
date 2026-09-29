using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using todolistnew.Api.Mappers;
using todolistnew.Api.Models.Request;
using todolistnew.Api.Models.Response;
using todolistnew.Data.Contexts;
using todolistnew.Data.Entities;
using todolistnew.Data.Enum;

namespace todolistnew.Api.Handlers;

public static class ToDoHandler
{
    public static void MapToDoEndpoints(this IEndpointRouteBuilder app)
    {
//Запрос на добавления задачи в список
        app.MapPost("/ToDo", async (CreateToDoModel request, AppDbContext db) =>
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);

            var isValid =
                Validator.TryValidateObject(request, validationContext, validationResults, true);

            if (!isValid)
            {
                var errors = validationResults.Select(r => r.ErrorMessage).ToList();
                return Results.BadRequest(errors);
            }

            var entity = new ToDo
            {
                TaskName = request.TaskName,
                TaskDescription = request.TaskDescription,
                TaskStatus = request.TaskStatus,
                DateStart = request.DateStart,
                DateEnd = request.DateEnd,
                TaskPriority = request.TaskPriority,
                UserId = request.UserId
            };

            db.ToDos.Add(entity);

            await db.SaveChangesAsync();

            return Results.Ok(entity.ToResponseModel());
        });


// Запрос на редактирование задачи по ID
        app.MapPut("/ToDoEdit/{id:int}", async (int id, CreateToDoModel request, AppDbContext db) =>
        {
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

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

            return Results.Ok(existingTask.ToResponseModel());
        });


//Запрос на получение ID
        app.MapGet("/ListTask", async (AppDbContext db) =>
        {
            var items = await db.ToDos
                .Select(item => new ToDoShortResponseModel
                {
                    Id = item.Id,
                    TaskName = item.TaskName,
                    TaskDescription = item.TaskDescription,
                    TaskStatus = item.TaskStatus,
                    TaskPriority = item.TaskPriority
                })
                .ToListAsync();

            if (items.Count == 0)

                return Results.NoContent();

            return Results.Ok(items);
        });


//Запрос на получение конкретного ID
        app.MapGet("/TaskId/{id:int}", async (int id, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task == null)
                return Results.NotFound();

            return Results.Ok(task.ToResponseModel());
        });


//Запрос на удаление задачи по ID
        app.MapDelete("/TaskDelete/{id:int}", async (int id, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task == null)
                return Results.NotFound();

            db.ToDos.Remove(task);

            await db.SaveChangesAsync();

            return Results.Ok();
        });
    }
}