using ToDoListManager.Models.CategoryModels;
using ToDoListManager.Models.NoteModels;
using ToDoListManager.Sevices.FileManagement;

public class CategoryManager
{
    private readonly CategoryDbManager _categoryDbManager;
    private readonly string _path;

    public List<CategoryEntity> Categories { get; private set; }
    public CategoryEntity? CurrentCategory { get; private set; }

    public CategoryManager()
    {
        var fileManager = new DbFileManager();
        _path = fileManager.CheckDatabaseCreation();
        _categoryDbManager = new CategoryDbManager(_path);

        Categories = _categoryDbManager.GetAll();
    }

    public void ChangeCurrentCategory(CategoryEntity category)
    {
        CurrentCategory = category;
    }

    public void Add(string categoryName)
    {
        var category = new CategoryEntity(categoryName);
        _categoryDbManager.Add(category);
        Categories.Add(category);
    }

    public void Remove()
    {
        if (CurrentCategory == null) return;

        _categoryDbManager.Remove(CurrentCategory);
        Categories.Remove(CurrentCategory);
        CurrentCategory = null;
    }

    public List<NoteEntity> GetNotesInCategory()
    {
        return CurrentCategory?.Notes ?? new List<NoteEntity>();
    }

    public bool IsNewCategory(string categoryName)
        => !Categories.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

    public bool IsCategoriesExist() => Categories.Any();

    public CategoryEntity? GetCategoryByIndex(int index)
    {
        return (index > 0 && index <= Categories.Count) ? Categories[index - 1] : null;
    }
}