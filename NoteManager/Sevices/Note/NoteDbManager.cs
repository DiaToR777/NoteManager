using ToDoListManager.Models.NoteModels;

namespace ToDoListManager.Sevices.Note;


public class NoteDbManager
{
    private readonly string _path;
    public NoteDbManager(string dbPath)
    {
        _path = dbPath;
    }

    public void Add(NoteEntity note)
    {
        using var dbContext = new NotesDbContext(_path);
        try
        {
            var noteDTO = NoteEntityDTO.ToDTO(note);
            dbContext.Add(noteDTO);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка додавання нотатки {note.Title} до бази даних {_path}. \n\r {ex.Message}");
        }
    }

    public List<NoteEntity> GetAll()
    {
        using var dbContext = new NotesDbContext(_path);
        try
        {
            var notes = dbContext.GetAll();
            return notes;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка підключення до бази даних {_path}. \n\r {ex.Message}");
        }
    }

    public void Remove(NoteEntity note)
    {
        using var dbContext = new NotesDbContext(_path);
        try
        {
            var noteDTO = NoteEntityDTO.ToDTO(note);
            dbContext.Remove(noteDTO);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка видалення {note.Title} з бази {_path}. \n\r {ex.Message}");
        }
    }

    public void RemoveByIds(List<Guid> noteIds)
    {
        using var dbContext = new NotesDbContext(_path);
        try
        {
            dbContext.RemoveNotes(noteIds);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка видалення нотаток з бази {_path}. \n\r {ex.Message}");
        }
    }

    public void Edit(NoteEntity note, string newContent)
    {
        using var dbContext = new NotesDbContext(_path);
        try
        {
            var noteDTO = NoteEntityDTO.ToDTO(note);
            var updatedNote = EditNoteContent(noteDTO, newContent);
            dbContext.Update(updatedNote);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка редагування {note.Title} в базі {_path}. \n\r {ex.Message}");
        }
    }

    private NoteEntityDTO EditNoteContent( NoteEntityDTO note, string newContent)
    {
        note.Content = newContent;
        return note;
    }
}