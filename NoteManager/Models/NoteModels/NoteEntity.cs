namespace ToDoListManager.Models.NoteModels;

public class NoteEntity
{
    public NoteEntity(Guid noteId,string title, string content, DateTime dateCreate)
    {
        NoteId = noteId;
        Title = title;
        Content = content;
        DateCreate = dateCreate;
    }

    public Guid NoteId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DateCreate { get; set; }

    public string ToString(bool includeContent)
    {
        if (includeContent)
            return $"Назва: {Title}\nКонтент: {Content}\nДата створення: {DateCreate}";
        
        else
            return $"Назва: {Title}\nДата створення: {DateCreate}";
    }
}
