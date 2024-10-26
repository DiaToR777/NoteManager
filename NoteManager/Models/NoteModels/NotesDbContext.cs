using Microsoft.EntityFrameworkCore;

namespace ToDoListV2.Models.NoteModels;

public class NotesDbContext : DbContext
{
    private string _path;
    public NotesDbContext(string path) => _path = path;

    public DbSet<NoteEntityDTO> Notes { get; set; }

    public void Add(NoteEntityDTO note) => Notes.Add(note);

    public void Remove(NoteEntityDTO note) => Notes.Remove(note);
    public void RemoveNotes(List<Guid> noteIds)
    {
        foreach (var id in noteIds)
        {
            var idStr = id.ToString();
            var targetDTO = Notes.FirstOrDefault(x => x.NoteId == idStr);
            if (targetDTO != null)
                Notes.Remove(targetDTO);
        }
    }

    public void Edit(NoteEntityDTO note, string newContent)
    {
        note.Content = newContent;
        SaveChanges();
    }
    //public void LoadTable() => Notes.Load();

    public List<NoteEntity> GetAll()
    {
        var dto = Notes.ToList();
        return Convert(dto);
    }
    private static List<NoteEntity> Convert(List<NoteEntityDTO> addressesDTO) => addressesDTO.Select(dto => dto.ToEntity()).ToList();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_path}");
    }
}
