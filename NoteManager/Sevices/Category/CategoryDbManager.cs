using ToDoListManager.Models.CategoryModels;

namespace ToDoListManager.Sevices.Category;

public class CategoryDbManager
{
    private readonly string _path;
    public CategoryDbManager(string dbPath)
    {
        _path = dbPath;
    }

    public void Add(CategoryEntity category)
    {
        using var dbContext = new CategoryesDbContext(_path);
        try
        {
            var categoryDTO = CategoryEntityDTO.ToDTO(category);
            dbContext.Add(categoryDTO);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка додавання категорії {category.Name} до бази даних {_path}. \n\r {ex.Message}");
        }
    }
    public void AddNoteId(Guid noteId, CategoryEntity category)
    {
        using var dbContext = new CategoryesDbContext(_path);
        try
        {
            var categoryDTO = CategoryEntityDTO.ToDTO(category);
            var changedCategoryDTO = AddNoteIdToCategory(categoryDTO, noteId.ToString());
            dbContext.Update(changedCategoryDTO);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка додавання індитифікатора {category.NoteIds} до категорії {category.Name} \n\r {ex.Message}");
        }
    }

    public void Remove(CategoryEntity category)
    {
        using var dbContext = new CategoryesDbContext(_path);
        try
        {
            var categoryDTO = CategoryEntityDTO.ToDTO(category);
            dbContext.Remove(categoryDTO);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка видалення категорії{category.Name} з бази {_path}. \n\r {ex.Message}");
        }
    }

    public void RemoveNoteId(CategoryEntity category, string noteId)
    {
        using var dbContext = new CategoryesDbContext(_path);
        try
        {
            var categoryDTO = CategoryEntityDTO.ToDTO(category);
            var updatedDTO = RemoveNoteIDFromCategory(categoryDTO, noteId);
            dbContext.Update(updatedDTO);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка видалення з категорії{category.Name} індитифікатора {noteId} з бази даних{_path}. \n\r {ex.Message}");
        }
    }
    public List<CategoryEntity> GetAll()
    {
        using var dbContext = new CategoryesDbContext(_path);
        try
        {
            var categories = dbContext.GetAll();
            return categories;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка підключення до бази даних {_path}. \n\r {ex.Message}");
        }
    }
    private CategoryEntityDTO AddNoteIdToCategory(CategoryEntityDTO category, string noteId)
    {
        var noteIds = category.NoteIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        if (!noteIds.Contains(noteId))
        {
            noteIds.Add(noteId);
            category.NoteIds = string.Join(",", noteIds);
        }
        return category;
    }

    private CategoryEntityDTO RemoveNoteIDFromCategory(CategoryEntityDTO category, string noteId)
    {
        Guid noteIdGuid = Guid.Parse(noteId);
        string finalNoteIds = RemoveGuidFromString(category.NoteIds, noteIdGuid);
        category.NoteIds = finalNoteIds;

        return category;
    }
    private static string RemoveGuidFromString(string guidsString, Guid guidToRemove)
    {
        // Перетворюємо рядок у список Guid
        var guids = ConvertStringToGuids(guidsString);

        // Видаляємо потрібний Guid
        guids.Remove(guidToRemove);

        // Перетворюємо список назад у рядок
        return ConvertGuidsToString(guids);
    }
    private static List<Guid> ConvertStringToGuids(string guidsString)
    {
        if (string.IsNullOrEmpty(guidsString))
        {
            return new List<Guid>();
        }

        return guidsString.Split(',')
                          .Select(g => Guid.Parse(g))
                          .ToList();
    }
    private static string ConvertGuidsToString(List<Guid> guids)
    {
        return string.Join(",", guids);
    }
}