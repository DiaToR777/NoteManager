using ToDoListManager.Date;

namespace ToDoListManager.Sevices.FileManagement
{
    public class DbFileManager
    {
        public string CheckDatabaseCreation()
        {
            string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Note.sqlite");

            using (var dbContext = new AppDbContext(desktopPath))
            {
                dbContext.Database.EnsureCreated();
            }

            return desktopPath;
        }
    }
}
