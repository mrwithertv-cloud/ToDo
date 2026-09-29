using System.ComponentModel.DataAnnotations;

namespace todolistnew.Entities;

public record UserEntity
{
    [Key]
    public int UserId { get; set; }
    public string? UserName { get; set; }
    
    public List<ToDoEntity> Todos { get; set; }
}