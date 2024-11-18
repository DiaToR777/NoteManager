using System.ComponentModel.DataAnnotations;

namespace ToDoListManager.Models.CategoryModels;

public class CategoryEntityDTO
{
    [Key]
    public string Name { get; set; }
    public string NoteIds { get; set; }
 
    public CategoryEntityDTO(string name, string noteIds)
    {
        Name = name;
        NoteIds = noteIds;
    }

    public CategoryEntity ToEntity()
    {
        return new(Name, ConvertStringToGuids(NoteIds));
    }

    public static CategoryEntityDTO ToDTO(CategoryEntity entity)
    {
        return new(entity.Name, ConvertGuidsToString(entity.NoteIds));
    }

    public static List<Guid> ConvertStringToGuids(string guidsString)
    {
        if (string.IsNullOrEmpty(guidsString))
            return new List<Guid>();

        return guidsString.Split(',') .Select(g => Guid.Parse(g)) .ToList();
    }

    public static string ConvertGuidsToString(List<Guid> guids)
    {
        if (guids == null || guids.Count == 0)
        {
            return string.Empty;
        }

        return string.Join(",", guids);
    }
} 

