using ToDoListV2.FileManagement;
using ToDoListV2.Models.CategoryModels;
using ToDoListV2.Models.NoteModels;

namespace ToDoListV2.Sevices.Category;

public class CategoryManager
{
    DbFileManager fileManager = new();
    public List<CategoryEntity> Categories { get; set; }
    public CategoryEntity? _currentCategory;
    private readonly CategoryDbManager _CategoryDbManager;
    string _path;


    public Action<CategoryEntity>? RemoveNotes;

    public CategoryManager()
    {
        _path = fileManager.CheckDatabaseCreation();
        _CategoryDbManager = new CategoryDbManager(_path);
        Categories = _CategoryDbManager.GetAll();

    }

    public void ChangeCurrentCategory(CategoryEntity category)
    {
        if (category != null)
            _currentCategory = category;
    }

    public void Add(string categoryName)
    {
        CategoryEntity category = new(categoryName, new List<Guid>());

        _CategoryDbManager.Add(category);
        Categories.Add(category);
    }

    public void AddNoteId(Guid noteId)
    {
        _CategoryDbManager.AddNoteId(noteId, _currentCategory!);
        _currentCategory!.NoteIds.Add(noteId);
    }

    public void Update()
    {
        var newNotesList = _CategoryDbManager.GetAll();
        if (!Categories.SequenceEqual(newNotesList))
        {
            Categories = newNotesList;
        }
    }

    public void Remove()
    {
        _CategoryDbManager.Remove(_currentCategory!);
        Categories.Remove(_currentCategory!);
        RemoveNotes?.Invoke(_currentCategory!);
    }
    public void RemoveNoteId(Guid noteId)
    {
        _CategoryDbManager.RemoveNoteId(_currentCategory!, noteId.ToString());
        _currentCategory!.NoteIds.Remove(noteId);
        Update();

    }

    public CategoryEntity? GetCategoryEntityByID(int categoryID)
    {
        if (categoryID >= 1 && categoryID <= Categories.Count)
        {
            return Categories.ElementAt(categoryID - 1);
        }
        else
        {
            Console.WriteLine("Помилка: Невірний номер категорії.");
            return null;
        }
    }

    public bool IsNewCategory(string categoryName)
    {
        if (Categories is null)
            return false;
        return !Categories.Any(category => category.Name == categoryName);
    }

    public bool IsCategoriesExist()
    {
        if (Categories.Count != 0)
        {
            return true;
        }
        return false;
    }
    public List<CategoryEntity>? GetAllCategories()
    {
        List<CategoryEntity> categories = new List<CategoryEntity>();

        if (Categories.Count != 0)
        {
            foreach (var category in Categories)
            {
                categories.Add(category);
            }
            return categories;
        }
        return null;
    }

    public List<NoteEntity> GetNotesInCategory()
    {
        using (var notesContext = new NotesDbContext(_path))
        {
            List<NoteEntity>  notes = new();

            var guids = _currentCategory!.NoteIds;

            var notesDTO = notesContext.Notes.AsEnumerable()
                             .Where(note => guids.Contains(Guid.Parse(note.NoteId)))
                             .ToList();

            foreach (var noteDTO in notesDTO)
            {
                var note = noteDTO.ToEntity();
                notes.Add(note);
            }
            return notes;
        }
    }
}
