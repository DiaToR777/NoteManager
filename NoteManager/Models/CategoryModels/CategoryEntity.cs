using System.ComponentModel.DataAnnotations;
using ToDoListManager.Models.NoteModels;

namespace ToDoListManager.Models.CategoryModels;

public class CategoryEntity
{
    public CategoryEntity(string name)
    {
        Name = name;
    }
    public CategoryEntity() { }

    [Key]
    public string Name { get; set; }
    public List<NoteEntity> Notes { get; set; } = new();
    public string CategoryName { get; }
} 

