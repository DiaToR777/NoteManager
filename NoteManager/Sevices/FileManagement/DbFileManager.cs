using Microsoft.Data.Sqlite;

namespace ToDoListV2.FileManagement;

public class DbFileManager
{
    public void CreateDatabase(string _pathToDb)
    {
        using (var connection = new SqliteConnection($"Data Source={_pathToDb}"))
        {
            connection.Open();

            using (SqliteCommand command = new())
            {
                command.Connection = connection;

                command.CommandText = @"
                CREATE TABLE Notes (
                    NoteId TEXT ,
                    Title TEXT NOT NULL,
                    Content TEXT NOT NULL,
                    DateCreate TEXT NOT NULL
                );";

                command.ExecuteNonQuery();

                command.CommandText = @"
                CREATE TABLE Categories (
                    Name TEXT NOT NULL,
                    NoteIds TEXT
                );";

                command.ExecuteNonQuery();
                command.Dispose();

                // Закриття підключення
                connection.Close();
            }
        }
    }
    public string CheckDatabaseCreation()
    {
        string userDesctop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string desktopPath = Path.Combine(userDesctop, "Note.sqlite");

        if (!File.Exists(desktopPath))
        {
            DbFileManager manager = new();
            manager.CreateDatabase(desktopPath);
        }return desktopPath;
    }

}
