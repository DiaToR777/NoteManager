using ToDoListManager.Sevices.Category;
using ToDoListManager.Sevices.Note;
using ToDoListManager.Views;

namespace ToDoListManager
{
    internal class App : IDisposable
     {
          private CategoryManager _categoryManager;
          private NoteManager _noteManager;

          public App()
          {
               _categoryManager = new CategoryManager();
               _noteManager = new NoteManager(_categoryManager.AddNoteId, _categoryManager.RemoveNoteId);
               _categoryManager.RemoveNotes += _noteManager.RemoveNotes;
          }

          public void Start()
          {
               var noteView = new NoteView(_noteManager);
               var categoryView = new CategoryView(_categoryManager, noteView);

               while (true) categoryView.Main();
          }

          public void Dispose()
          {
               _categoryManager.RemoveNotes -= _noteManager.RemoveNotes;
          }
     }
}
