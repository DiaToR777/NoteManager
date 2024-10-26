using Microsoft.EntityFrameworkCore;

namespace ToDoListV2.Models.CategoryModels;

public class CategoryesDbContext : DbContext
{

    private readonly string _path;
    public CategoryesDbContext(string path) => _path = path;

    public DbSet<CategoryEntityDTO> Categories { get; set; }

    public List<CategoryEntity> GetAll()
    {
        var dto = Categories.ToList();
        return Convert(dto);
    }

    private static List<CategoryEntity> Convert(List<CategoryEntityDTO> categoryDTOs) => categoryDTOs.Select(dto => dto.ToEntity()).ToList();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_path}");
    }
}
