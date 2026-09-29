using Microsoft.EntityFrameworkCore;
using todolistnew.Data;
using todolistnew.Request;
using todolistnew.Entities;

namespace todolistnew.Endpoints;

public static class User
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/UserCreate", async (CreateUserReqest reqest, AppDbContext db) =>
        {
            var user = new UserEntity
            {
                UserName = reqest.UserName,
                UserId = reqest.UserId
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();

            return Results.Ok((user));
        });


        app.MapGet("/UserList", async (AppDbContext db) =>
        {
            var items = await db.Users
                .Select(item => new UserDtoItemList(item.UserId.ToString(), item.UserName.ToString()))
                .ToListAsync();

            if (items.Count == 0)

                return Results.NoContent();

            return Results.Ok(items);
        });


        app.MapGet("/UserTasks/{userId}", async (int userId, AppDbContext db) =>
        {
            var tasks = await db.ToDos
                .Where(t => t.UserId == userId)
                .Select(item => new TaskListItemDto(item.Id.ToString(), item.TaskName, item.TaskPriority))
                .ToListAsync();

            if (tasks.Count == 0)
                return Results.NoContent();

            return Results.Ok(tasks);
        });


        app.MapDelete("/UserDelete/{id}", async (int id, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user == null)
                return Results.NotFound();

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Results.Ok(user);
        });
    }
}    