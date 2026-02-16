using System.Xml.Linq;
using ToDoListManager.Models.CategoryModels;
using ToDoListManager.Models.NoteModels;
using ToDoListManager.Sevices.FileManagement;

namespace ToDoListManager.Sevices.Note;

public class NoteManager
{
    private readonly NoteDbManager _noteDbManager;
    private NoteEntity? _currentNote;
    public List<NoteEntity> Notes { get; private set; }

    public NoteManager()
    {
        var fileManager = new DbFileManager();
        string path = fileManager.CheckDatabaseCreation();
        _noteDbManager = new NoteDbManager(path);

        Notes = _noteDbManager.GetAll() ?? new List<NoteEntity>();
    }

    public void ChangeCurrentNote(NoteEntity note) => _currentNote = note;

    public void Add(string title, string content, string categoryName, CategoryEntity currentCategory)
    {
        var note = new NoteEntity(title, content)
        {
            CategoryName = categoryName
        };

        _noteDbManager.Add(note);
        Notes.Add(note);

        if (currentCategory != null && currentCategory.Name == categoryName)
        {
            currentCategory.Notes.Add(note);
        }
    }

    public void Remove(CategoryEntity category)
    {
        if (_currentNote == null) return;

        _noteDbManager.Remove(_currentNote);
        Notes.Remove(_currentNote);
        category.Notes.Remove(_currentNote); 
        _currentNote = null;
    }

    public void Edit(string newContent)
    {
        if (_currentNote == null) return;

        _noteDbManager.Edit(_currentNote, newContent);
    }

    public bool IsNewNote(string title)
        => !Notes.Any(n => n.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

    public NoteEntity? GetNoteFromCategory(CategoryEntity category, int index)
    {
        var notesInCategory = category.Notes;

        if (index > 0 && index <= notesInCategory.Count)
        {
            return notesInCategory[index - 1];
        }

        Console.WriteLine("Помилка: Невірний номер нотатки.");
        return null;
    }
}