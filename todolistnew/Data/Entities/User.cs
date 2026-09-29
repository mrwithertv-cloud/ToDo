using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace todolistnew.Data.Entities;

public class User
{
    [Key] public int UserId { get; init; }

    public required string UserName { get; set; }

    public virtual Collection<ToDo> Todos { get; set; } = [];
}