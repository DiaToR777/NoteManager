using System.ComponentModel.DataAnnotations;
using ToDoListManager.Models.CategoryModels;

namespace ToDoListManager.Models.NoteModels;

public class NoteEntity
{
    public NoteEntity(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public NoteEntity() { }

    [Key]
    public Guid NoteId { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DateCreate { get; private set; } = DateTime.Now;


    public string CategoryName { get; set; }
    public CategoryEntity Category { get; set; }

    public string ToString(bool includeContent)
    {
        if (includeContent)
            return $"Назва: {Title}\nКонтент: {Content}\nДата створення: {DateCreate}";

        else
            return $"Назва: {Title}\nДата створення: {DateCreate}";
    }

}
