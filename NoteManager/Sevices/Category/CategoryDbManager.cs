using Microsoft.EntityFrameworkCore;
using ToDoListManager.Date;
using ToDoListManager.Models.CategoryModels;

public class CategoryDbManager
{
    private readonly string _path;
    public CategoryDbManager(string dbPath) => _path = dbPath;

    public List<CategoryEntity> GetAll()
    {
        using var dbContext = new AppDbContext(_path);
        return dbContext.Categories
                        .Include(c => c.Notes)
                        .ToList();
    }

    public void Add(CategoryEntity category)
    {
        using var dbContext = new AppDbContext(_path);
        dbContext.Categories.Add(category);
        dbContext.SaveChanges();
    }
    public void RemoveNoteFromCategory(string categoryName, Guid noteId)
    {
        using var dbContext = new AppDbContext(_path);

        var note = dbContext.Notes.FirstOrDefault(n => n.NoteId == noteId && n.CategoryName == categoryName);

        if (note != null)
        {
            note.CategoryName = null;
            dbContext.SaveChanges();
        }
    }

    public void Remove(CategoryEntity category)
    {
        using var dbContext = new AppDbContext(_path);
        dbContext.Categories.Remove(category);
        dbContext.SaveChanges();
    }
}