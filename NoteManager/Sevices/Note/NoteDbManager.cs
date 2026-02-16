using Microsoft.EntityFrameworkCore;
using ToDoListManager.Date;
using ToDoListManager.Models.NoteModels;

namespace ToDoListManager.Sevices.Note;

public class NoteDbManager
{
    private readonly string _path;
    public NoteDbManager(string dbPath) => _path = dbPath;

    public void Add(NoteEntity note)
    {
        using var dbContext = new AppDbContext(_path);
        try
        {
            // Просто додаємо об'єкт. EF Core сам знає, як його зберегти.
            dbContext.Notes.Add(note);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка додавання нотатки {note.Title}. \n {ex.Message}");
        }
    }

    public List<NoteEntity> GetAll()
    {
        using var dbContext = new AppDbContext(_path);
        // Завантажуємо разом із посиланням на категорію, якщо потрібно
        return dbContext.Notes.Include(n => n.Category).ToList();
    }

    public void Remove(NoteEntity note)
    {
        using var dbContext = new AppDbContext(_path);
        try
        {
            dbContext.Notes.Remove(note);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка видалення {note.Title}. \n {ex.Message}");
        }
    }

    public void RemoveByIds(List<Guid> noteIds)
    {
        using var dbContext = new AppDbContext(_path);
        try
        {
            // Елегантний спосіб видалити групу за ID без циклів
            var notesToRemove = dbContext.Notes.Where(n => noteIds.Contains(n.NoteId));
            dbContext.Notes.RemoveRange(notesToRemove);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка масового видалення нотаток. \n {ex.Message}");
        }
    }

    public void Edit(NoteEntity note, string newContent)
    {
        using var dbContext = new AppDbContext(_path);
        try
        {
            // Оновлюємо контент і зберігаємо
            note.Content = newContent;
            dbContext.Notes.Update(note);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка редагування {note.Title}. \n {ex.Message}");
        }
    }
}