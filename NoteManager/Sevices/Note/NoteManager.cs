using ToDoListV2.Models.CategoryModels;
using ToDoListV2.FileManagement;
using ToDoListV2.Models.NoteModels;

namespace ToDoListV2.Sevices.Note;

public class NoteManager
{
    public List<NoteEntity> Notes { get; set; }
    private Action<Guid> _add_ID_ToCategory { get; }
    private Action<Guid> _remove_ID_FromCategory { get; }
    private NoteEntity? _currentNote;

    private NoteDbManager _NoteDbManager;

    DbFileManager fileManager = new();

    string _path;

    public NoteManager(Action<Guid> add_ID_ToCategory, Action<Guid> remove_ID_FromCategory)
    {
        _path = fileManager.CheckDatabaseCreation();
        _NoteDbManager = new(_path);
        Notes = _NoteDbManager.GetAll();
        _add_ID_ToCategory = add_ID_ToCategory;
        _remove_ID_FromCategory = remove_ID_FromCategory;
    }

    public void ChangeCurrentNote(NoteEntity note)
    {
        if (note is not null)
            _currentNote = note;
    }

    public void Add(string noteName, string noteContent)
    {
        Guid noteId = Guid.NewGuid();

        NoteEntity note = new(noteId, noteName, noteContent, DateTime.Now);

        _NoteDbManager.Add(note);
        Notes.Add(note);
        _add_ID_ToCategory.Invoke(noteId);
    }

    public void RemoveNotes(CategoryEntity categoty)
    {
        var noteIds = categoty.NoteIds;
        if (noteIds.Count == 0)
            return;

        _NoteDbManager.RemoveByIds(noteIds);

        Update();
    }

    public bool IsNewNote(string title)
    {
        if (Notes is null)
            return false;
        return !Notes.Any(note => note.Title == title);
    }

    public void Remove()
    {
        _NoteDbManager.Remove(_currentNote!);
        Notes.Remove(_currentNote!);

        _remove_ID_FromCategory.Invoke(_currentNote!.NoteId);
    }

    public void Update()
    {
        var newNotesList = _NoteDbManager.GetAll();
        if (!Notes.SequenceEqual(newNotesList))
        {
            Notes = newNotesList;
        }
    }

    public NoteEntity? GetNoteEntityByID(CategoryEntity category, int noteID)
    {
        // Фільтруємо нотатки, що належать до вказаної категорії
        var notesInCategory = Notes.Where(note => category.NoteIds.Contains(note.NoteId)).ToList();

        // Перевіряємо, чи номер нотатки в межах списку нотаток цієї категорії
        if (noteID >= 1 && noteID <= notesInCategory.Count)
        {
            return notesInCategory.ElementAt(noteID - 1); // Повертаємо нотатку за індексом
        }
        else
        {
            Console.WriteLine("Помилка: Невірний номер нотатки.");
            return null;
        }
    }

    public void Edit(string newContent)
    {
        _NoteDbManager.Edit(_currentNote!, newContent);
        _currentNote!.Content = newContent;
    }
}