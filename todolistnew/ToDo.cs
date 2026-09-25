using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("data source = app.db"));


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


var app = builder.Build();

app.UseStaticFiles();
app.UseCors();



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
    var items = await db.Users.Select(item =>new UserDtoItemList (item.UserId.ToString(), item.UserName.ToString()))
        .ToListAsync();

    if (items.Count == 0)

        return Results.NoContent();

    return Results.Ok(items);
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
        TaskPriority = request.TaskPriority
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
    
    return Results.Ok(existingTask.ToResponse());
});


//Запрос на получение ID
app.MapGet("/ListTask", async (AppDbContext db) =>
{
    var items = await db.ToDos.Select(item => new TaskListItemDto(item.Id.ToString(), item.TaskName, item.TaskPriority))
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

    return Results.Ok(task.ToResponse());
});


//Запрос на удаление задачи по ID
app.MapDelete("/TaskDelete/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.ToDos.FindAsync(id);
    if (task == null)
        return Results.NotFound();
    
    db.ToDos.Remove(task);
    
    await db.SaveChangesAsync();
    
    return Results.Ok(task.ToResponse());
});

app.MapGet("/", () => Results.File("index.html", "text/html"));

app.Run();


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
}


public record ToDoEntity
{
    public int Id { get; set; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public required TaskStatusNum TaskStatus { get; set; }
    public DateTimeOffset TaskDate { get; set; }
    public DateTimeOffset DateStart { get; set; }
    public DateTimeOffset DateEnd { get; set; }
    public required TaskPriority TaskPriority { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
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

//Структура для отдачи списка задач
public readonly record struct TaskListItemDto(string TaskId, string TaskName, TaskPriority TaskPriority);

public record CreateUserReqest
{
    [Key]
    public int UserId { get; set; }
    public string? UserName { get; set; }
}

public record UserEntity
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    
    public List<ToDoEntity> Todos { get; set; }
}

public readonly record struct UserDtoItemList(string UserId, string UserName);

public static class MappingExtensions
{
    public static ToDoResponse ToResponse(this ToDoEntity entity) => new(
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


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoEntity> ToDos => Set<ToDoEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
}




//Проверка дат
[AttributeUsage(AttributeTargets.Class)]
public class DateTimeAtrebyte : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is ToDoRequest request)
        {
            if (request.DateStart > request.DateEnd)
            {
                return new ValidationResult("Дата начала не может быть позже даты окончания");
            }
        }

        return ValidationResult.Success;
    }
}

//ограничение диапазона для статуса
public enum TaskStatusNum
{
    Completed = 5,
    Canceled = 4,
    Failed = 3,
    InProgres = 2,
    NotStarted = 1
}


//ограничение диапазона для приоритета задачи
public enum TaskPriority
{
    High = 1,
    Medium = 2,
    Low = 3
}

