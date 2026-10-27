using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
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
        app.MapPost("/todo", async (CreateToDoModel request, AppDbContext db) =>
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
        
        
//Запрос на получение ID
        app.MapGet("/list-task", async (AppDbContext db) =>
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
        app.MapGet("/task-id/{id:int}", async (int id, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task == null)
                return Results.NotFound();

            return Results.Ok(task.ToResponseModel());
        });


//Запрос на удаление задачи по ID
        app.MapDelete("/task-delete/{id:int}", async (int id, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task == null)
                return Results.NotFound();

            db.ToDos.Remove(task);

            await db.SaveChangesAsync();

            return Results.Ok();
        });
        
        // Запрос на редактирование задачи по ID
        app.MapPut("/todo-edit/{id:int}", async (int id, CreateToDoModel request, AppDbContext db) =>
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
        
        app.MapPatch("/change-status/{id:int}/{status}", async  (int id, string status, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task is null) return Results.NotFound();

            if (!Enum.TryParse<TaskStatusNum>(status, true, out var statusNum))
            {
                return Results.BadRequest();
            }
            task.TaskStatus = statusNum;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapPatch("/change-user/{id:int}/{userId:int}", async (int id, int userId, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task is null) return Results.NotFound();
            
            var userExists =  await db.Users.AnyAsync(u=> u.UserId == userId);
            if (!userExists)
            {
                return Results.BadRequest("User not found");
            }

            if (task.UserId == userId)
            {
                return Results.Ok("Задача уже принадлежит пользователю");
            }
            
            task.UserId = userId;
            await  db.SaveChangesAsync();
            return Results.Ok();
        });

        app.MapPatch("/change-priority/{id:int}/{priority}", async(int id, string priority, AppDbContext db) =>
        {
            var task = await db.ToDos.FindAsync(id);
            if (task is null) return Results.NotFound();

            if (!Enum.TryParse<TaskPriority>(priority, true, out var priorityNum ))
            {
                return Results.BadRequest();
            }

            task.TaskPriority = priorityNum;
            await db.SaveChangesAsync();
            return Results.Ok();
        });


        app.MapPost("/little-task/{id:int}", async (CreateToDoModel request,int id, AppDbContext db) =>
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
            
            var ParentTask = await db.ToDos.FindAsync(id);
            if (ParentTask is null) return Results.NotFound();
            
            
            var userExists = await db.Users.AnyAsync(u => u.UserId == ParentTask.UserId);
            if (!userExists)
            {
                return Results.BadRequest("User not found");
            }
            
            var entity = new ToDo
            {
                TaskName = request.TaskName,
                TaskDescription = request.TaskDescription,
                TaskStatus = request.TaskStatus,
                DateStart = request.DateStart,
                DateEnd = request.DateEnd,
                TaskPriority = request.TaskPriority,
                UserId = request.UserId,
                ParentId = id,
            };

            db.ToDos.Add(entity);

            await db.SaveChangesAsync();

            return Results.Ok();
        });
    }
}