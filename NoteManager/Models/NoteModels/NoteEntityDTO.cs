using System.ComponentModel.DataAnnotations;

namespace ToDoListManager.Models.NoteModels;

public class NoteEntityDTO
{
    [Key]
    public string NoteId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string DateCreate { get; private set; }
    public NoteEntityDTO(string noteId, string title, string content, string dateCreate)
    {
        NoteId = noteId;
        Title = title;
        Content = content;
        DateCreate = dateCreate;
    }

    public static NoteEntityDTO ToDTO(NoteEntity entity)
    {
        return new(entity.NoteId.ToString(), entity.Title, entity.Content, entity.DateCreate.ToString());
    }

    public NoteEntity ToEntity()
    {
        return new(Guid.Parse(NoteId), Title, Content, StringToDateTime(DateCreate));
    }

    public static DateTime StringToDateTime(string dateTimeInStr)
    {
        var dateTime = DateTime.Parse(dateTimeInStr);
        return dateTime;
    }
}
