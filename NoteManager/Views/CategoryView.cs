using ToDoListManager.Models.CategoryModels;

namespace ToDoListManager.Views;

internal class CategoryView : ViewBase
{
     private CategoryManager _manager { get; }
     private NoteView _noteView { get; }

     public CategoryView(CategoryManager manager, NoteView noteView)
     {
          _manager = manager;
          _noteView = noteView;
     }

     public void Main()
     {
          Console.Clear();
          if (CheckCategoriesExists(out List<CategoryEntity>? categories) && categories != null)
               HandleCategoryInput(categories);
          else
          {
               Console.WriteLine("Категорій немає, створіть хоча б 1!\n");
               CreateCategory();
          }
     }

     private void CreateCategory()
     {
          string categoryName = GetValidInput(message: "Створення категорії | Введіть назву категорії:\n");

          if (!_manager.IsNewCategory(categoryName))
          {
               Console.WriteLine("Категорія з такою назвою вже існує");
               Console.ReadKey();
          }
          else
               _manager.Add(categoryName);
     }

     private void HandleCategoryInput(List<CategoryEntity> categories)
     {
          Console.WriteLine("\na) додати нову категорію чи обрати за Id\n");
          ViewAllCategoryes(categories);
          string command = GetValidInput(true);

          if (command == "a" || command == "а")
               CreateCategory();
          else
          {
               var categoryId = IsValidNumber(command);
               if (categoryId != 0)
               {
                    var category = _manager.GetCategoryByIndex(categoryId);
                    if (category != null)
                    {
                         _manager.ChangeCurrentCategory(category);
                         ShowCategory();
                    }
               }
               else
               {
                    Console.WriteLine("Не вірнний ввід");
                    Console.ReadKey();
               }
          }
     }

     private void ShowCategory()
     {
          var command = GetCommand();
          ProcessCommand(command);
     }

     private string GetCommand()
     {
          if (!IsCategoryHaveNotes())
          {
               Console.WriteLine($"Категорія \"{_manager.CurrentCategory!.Name}\" не містить нотаток ");
               return GetValidInput(true, "\nc) додати нотатку до категорії\nd) видалити категорію\nb) повернутись в головне меню\n");
          }
          else
          {
               var notes = _manager.GetNotesInCategory();
               if (notes.Count > 5)
                    _noteView.DisplayNotes(_manager.GetNotesInCategory());
               else
                    _noteView.DisplayOnePageNotes(notes);

               return GetValidInput(true, "\nc) додати нотатку до категорії\nd) видалити категорію\nb) повернутись в головне меню\nАбо виберіть нотатку\n");
          }
     }

     private void ProcessCommand(string command)
     {
          if (IsCategoryHaveNotes() && int.TryParse(command, out int trueNoteNumber))
          {
               _noteView.ProcessNoteCommand(trueNoteNumber, _manager.CurrentCategory!);
               return;
          }
          else
               ExecuteCategoryCommand(command);
     }
    private void ExecuteCategoryCommand(string command)
    {
        switch (command.ToLower())
        {
            case "c":
            case "с":
                _noteView.CreateNote(_manager.CurrentCategory!.Name, _manager.CurrentCategory);
                break;
            case "d":
            case "в":
                DeleteCategory();
                break;
            case "b":
            case "и":
                break;
            default:
                Console.WriteLine("Невідома команда!");
                Console.ReadKey();
                break;
        }
    }
    private bool CheckCategoriesExists(out List<CategoryEntity>? categories)
    {
          categories = _manager.Categories;

        return categories != null && categories.Any();
    }

     private void DeleteCategory() => _manager.Remove();

     private static void ViewAllCategoryes(List<CategoryEntity> categories)
     {
          int index = 1;
          foreach (var category in categories!)
               Console.WriteLine($"{index++}. {category.Name}");
     }
    private bool IsCategoryHaveNotes()
            => _manager.CurrentCategory?.Notes != null && _manager.CurrentCategory.Notes.Any();
}