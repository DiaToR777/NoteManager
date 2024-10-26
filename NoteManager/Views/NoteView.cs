using ToDoListV2.Models.CategoryModels;
using ToDoListV2.Models.NoteModels;
using ToDoListV2.Sevices.Note;

namespace ToDoListV2.Views
{

    internal class NoteView : ViewBase
    {
        public NoteManager Manager { get; }
        public NoteView(NoteManager manager)
        {
            Manager = manager;
        }

        public void ProcessNoteCommand(int noteId, CategoryEntity category)
        {
            var note = Manager.GetNoteEntityByID(category, noteId);
            if (note != null)
            {
                Manager.ChangeCurrentNote(note);
                Console.WriteLine($"\n{note.ToString(true)}\n");

                Console.Write("d) Видалити нотатку е) редагувати нотатку b) повернутись в головне меню\n");

                string commandWithNote = GetValidInput(true);
                switch (commandWithNote)
                {
                    case "d":
                    case "в":
                        Manager.Remove();
                        break;
                    case "e":
                    case "у":
                        EditNoteMain();
                        break;
                    case "b":
                    case "и":
                        break;
                    default:
                        Console.WriteLine("Ви ввели не правильну команду! Повторіть спробу (Press enter)");
                        break;
                }
            }

        }

        private void EditNoteMain()
        {
            Console.WriteLine("Редагування нотатки");
            Console.Write("Введіть новий контент нотатки:");

            string noteContent = GetValidInput();

            Manager.Edit(noteContent);
        }

        public void CreateNote()
        {
            Console.WriteLine("Створення нотатки");
            Console.Write("Введіть назву нотатки:");

            string noteTitle = GetValidInput(); //отримуємо корректну назву нотатки
            if (!Manager.IsNewNote(noteTitle))
            {
                Console.WriteLine("Нотатка з такою назвою вже існує");
                return;
            }
            Console.Write("Введіть вміст нотатки:");
            string noteContent = GetValidInput(); //заповняємо вміст

            Manager.Add(noteTitle, noteContent);

            Console.WriteLine();
        }

        public string DisplayNotesAndGetCommand(List<NoteEntity> notes, int pageSize = 5)
        {
            // Якщо нотаток менше або рівно кількості на сторінку, просто виводимо їх всі
            if (notes.Count <= pageSize) 
                return DisplayOnePageNotesAndGetCommand(notes); // Повертаємо рядок

            // Логіка пагінації для більше ніж pageSize нотаток
            int currentPage = 1;
            int totalPages = (int)Math.Ceiling((double)notes.Count / pageSize);

            while (true)
            {
                Console.Clear(); // Очищуємо екран перед показом нової сторінки

                // Отримуємо нотатки для поточної сторінки
                var notesToDisplay = NotesForCurrentPage(notes, currentPage, pageSize);

                //Виводимо
                for (int i = 0; i < notesToDisplay.Count; i++)
                {
                    var note = notesToDisplay[i];
                    Console.WriteLine($"Нотатка номер {((currentPage - 1) * pageSize) + (i + 1)}\n{note.ToString(false)}\n");
                }

                Console.WriteLine($"\nСторінка {currentPage}/{totalPages}");
                Console.WriteLine("\nc) додати нотатку до категорії\nd) видалити категорію\nb) повернутись в головне меню\nАбо виберіть нотатку\n");
                Console.WriteLine("← попередня сторінка | → наступна сторінка | Натисніть Enter та введіть індекс нотатки або команду:\n");

                var input = Console.ReadKey().Key;

                // Логіка для пагінації
                if (input == ConsoleKey.LeftArrow && currentPage > 1)
                {
                    currentPage--; // Переходимо на попередню сторінку
                }
                else if (input == ConsoleKey.RightArrow && currentPage < totalPages)
                {
                    currentPage++; // Переходимо на наступну сторінку
                }
                else if (input == ConsoleKey.B)
                {
                    return "b"; // Користувач ввів команду виходу
                }
                else
                {
                    // Якщо користувач ввів не стрілку, пропонуємо ввести команду або індекс нотатки
                    Console.WriteLine("\nВвід:");
                    return GetValidInput(true); // Повертаємо введене значення
                }
            }
        }

        private string DisplayOnePageNotesAndGetCommand(List<NoteEntity> notes)
        {

            for (int i = 0; i < notes.Count; i++)
            {
                var note = notes[i];
                Console.WriteLine($"Нотатка номер {i + 1}\n{note.ToString(false)}\n");
            }

            Console.WriteLine("c) додати нотатку до категорії\nd) видалити категорію\nb) повернутись в головне меню\nАбо виберіть нотатку\n");
            Console.WriteLine("Введіть команду або виберіть нотатку за індексом:\n");
            return GetValidInput(true); // Повертаємо рядок
        }

        private static List<NoteEntity> NotesForCurrentPage(List<NoteEntity> notes, int currentPage, int pageSize)
        {
            var notesForPage = notes.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return notesForPage;
        }
    }
}

