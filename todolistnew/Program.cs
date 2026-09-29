using Microsoft.EntityFrameworkCore;
using todolistnew.Api.Handlers;
using todolistnew.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Настройка сервисов
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("data source = app.db"));

var app = builder.Build();

// Автоматическое создание БД
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Middleware
app.UseStaticFiles();
app.UseCors();

// Регистрация эндпоинтов из наших классов
app.MapUserEndpoints();
app.MapToDoEndpoints();

app.Run();