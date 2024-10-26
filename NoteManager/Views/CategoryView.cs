using System.ComponentModel.Design;
using ToDoListV2.Models.CategoryModels;
using ToDoListV2.Sevices.Category;

namespace ToDoListV2.Views;

internal class CategoryView : ViewBase
{
    public CategoryManager _manager { get; }
    private NoteView _noteView { get; }
        private List<CategoryEntity> _categories;

    public CategoryView(CategoryManager manager, NoteView noteView)
    {
        _manager = manager;
        _noteView = noteView;
    }

    public void Main()
    {
        if (ChackCategoryesExist(out List<CategoryEntity>? categories) && categories != null)
            HandleCategoryInput(categories);
        else
        {
            Console.WriteLine("Категорій немає, створіть хоча б 1!\n");
            CreateCategory();
        }
    }

    public void CreateCategory()
    {
        Console.WriteLine("Створення категорії | Введіть назву категорії:");
        string categoryName = GetValidInput();

        if (!_manager.IsNewCategory(categoryName))
        {
            Console.WriteLine("Категорія з такою назвою вже існує");
            return;
        }
        _manager.Add(categoryName);
        Console.WriteLine();
    }

    public void HandleCategoryInput(List<CategoryEntity> categories)
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
                var category = _manager.GetCategoryEntityByID(categoryId);
                if (category != null)
                {
                    _manager.ChangeCurrentCategory(category);
                    ShowCategory();
                }
            }
            else
                Console.WriteLine("Не вірнний ввід");
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
            Console.WriteLine($"Категорія \"{_manager._currentCategory!.Name}\" не містить нотаток ");

            Console.Write("\nc) додати нотатку до категорії\nd) видалити категорію\nb) повернутись в головне меню\n");
            return GetValidInput(true);
        }
        else
        {
            
            string command = _noteView.DisplayNotesAndGetCommand(_manager.GetNotesInCategory());
            return command;
        }
    }

    private void ProcessCommand(string command)
    {
        // Спочатку перевіряємо, чи введена команда є числом (номер нотатки)
        if (IsCategoryHaveNotes() && int.TryParse(command, out int trueNoteNumber))
        {
            _noteView.ProcessNoteCommand(trueNoteNumber, _manager._currentCategory!);
            return;
        }
        else
            ExecuteCategoryCommand(command);
    }
    private void ExecuteCategoryCommand(string command)
    {
        switch (command)
        {
            case "c":
            case "с":
                _noteView.CreateNote();
                break;
            case "d":
            case "в":
                DeleteCategory();
                break;
            case "b":
            case "и":
                break;
            default:
                Console.WriteLine("Ви ввели не правильну команду!");
                break;
        }
    }
    private bool ChackCategoryesExist(out List<CategoryEntity>? categories)
    {
        categories = _manager.GetAllCategories();
        return categories != null ? true : false;
    }

    private void DeleteCategory() => _manager.Remove();

    public void ViewAllCategoryes(List<CategoryEntity> categories)
    {
        int index = 1;
        foreach (var category in categories!)
            Console.WriteLine($"{index++}. {category.Name}");
    }
    private bool IsCategoryHaveNotes() => _manager._currentCategory?.NoteIds.Count != 0;
}