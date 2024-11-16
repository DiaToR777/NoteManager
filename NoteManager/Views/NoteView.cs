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
               string noteContent = GetValidInput(message: "Редагування нотатки \nВведіть новий контент нотатки:");

               Manager.Edit(noteContent);
          }

          public void CreateNote()
          {
               string noteTitle = GetValidInput(message: "Створення нотатки \nВведіть назву нотатки:"); //отримуємо корректну назву нотатки

               if (!Manager.IsNewNote(noteTitle))
               {
                    Console.WriteLine("Нотатка з такою назвою вже існує");
                    return;
               }
               string noteContent = GetValidInput(message: "Введіть вміст нотатки: \n"); //заповняємо вміст

               Manager.Add(noteTitle, noteContent);
          }

          public void DisplayNotes(List<NoteEntity> notes, int pageSize = 5)
          {
               int currentPage = 1;
               int totalPages = (int)Math.Ceiling((double)notes.Count / pageSize);
               bool isRunning = true;

               while (isRunning)
               {
                    Console.Clear();

                    WriteNotesToDisplay(notes, currentPage, pageSize);

                    Console.WriteLine($"\nСторінка {currentPage}/{totalPages}");
                    Console.WriteLine("\n← попередня сторінка | → наступна сторінка | Натисніть 'b' для завершення перегляду.\n");

                    var input = Console.ReadKey().Key;
                    switch (input)
                    {
                         case ConsoleKey.LeftArrow:
                              currentPage = currentPage > 1 ? currentPage - 1 : totalPages;
                              break;
                         case ConsoleKey.RightArrow:
                              currentPage = currentPage < totalPages ? currentPage + 1 : 1;
                              break;
                         case ConsoleKey.B:
                              isRunning = false;
                              break;
                    }
               }
          }

          private void WriteNotesToDisplay(List<NoteEntity> notes, int currentPage, int pageSize = 5)
          {
               var notesToDisplay = NotesForCurrentPage(notes, currentPage, pageSize);
               for (int i = 0; i < notesToDisplay.Count; i++)
               {
                    var note = notesToDisplay[i];
                    Console.WriteLine($"Нотатка номер {((currentPage - 1) * pageSize) + (i + 1)}\n{note.ToString(false)}\n");
               }
          }

          public void DisplayOnePageNotes(List<NoteEntity> notes)
          {
               for (int i = 0; i < notes.Count; i++)
               {
                    var note = notes[i];
                    Console.WriteLine($"Нотатка номер {i + 1}\n{note.ToString(false)}\n");
               }
          }

          private static List<NoteEntity> NotesForCurrentPage(List<NoteEntity> notes, int currentPage, int pageSize)
          {
               var notesForPage = notes.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
               return notesForPage;
          }
     }
}

