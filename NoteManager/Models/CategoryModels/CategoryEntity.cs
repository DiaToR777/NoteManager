namespace ToDoListV2.Models.CategoryModels;

public class CategoryEntity
{
    public string Name { get; set; }
    public List<Guid> NoteIds { get; set; }
    public CategoryEntity(string name, List<Guid> noteIds)
    {
        NoteIds = new List<Guid>();
        Name = name;
        NoteIds = noteIds;
    }
}
