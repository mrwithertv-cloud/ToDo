using Microsoft.EntityFrameworkCore;
using todolistnew.Api.Mappers;
using todolistnew.Api.Models.Request;
using todolistnew.Api.Models.Response;
using todolistnew.Data.Contexts;
using todolistnew.Data.Entities;


namespace todolistnew.Api.Handlers;

public static class UserHandler
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/user-create", async (CreateUserModel model, AppDbContext db) =>
        {
            var user = new User
            {
                UserName = model.UserName
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();

            return Results.Ok(user);
        });


        app.MapGet("/user-list", async (AppDbContext db) =>
        {
            var items = await db.Users
                .Select(item => new UserResponseModel
                {
                    UserId = item.UserId,
                    UserName = item.UserName
                })
                .ToListAsync();

            if (items.Count == 0)
                return Results.NoContent();

            return Results.Ok(items);
        });


        app.MapGet("/user-tasks/{userId:int}", async (int userId, AppDbContext db) =>
        {
            var tasks = (await db.Users
                .Include(u => u.Todos)
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => u.Todos.Select(td => td.ToResponseModel()))
                .FirstOrDefaultAsync())?
                .ToList();

            if (tasks?.Count == 0)
                return Results.NoContent();

            return Results.Ok(tasks);
        });


        app.MapDelete("/user-delete/{id:int}", async (int id, AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user == null)
                return Results.NotFound();

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return Results.Ok();
        });

        app.MapPatch("/change-user-name/{userId:int}/{name}", async (int userId, string name ,AppDbContext db) =>
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) return Results.NotFound();

            bool isEqual = string.Equals(name, user.UserName, StringComparison.CurrentCultureIgnoreCase);
            if (isEqual) 
            {
                return Results.BadRequest("одинаковые");
            }

            user.UserName = name;
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}