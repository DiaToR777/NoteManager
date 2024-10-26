using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListV2.Sevices.Category;
using ToDoListV2.Sevices.Note;
using ToDoListV2.Views;

namespace ToDoListV2
{
     internal class App : IDisposable
     {
          private CategoryManager _categoryManager;
          private NoteManager _noteManager;

          public App() {
               _categoryManager = new CategoryManager();
               _noteManager = new NoteManager(_categoryManager.AddNoteId, _categoryManager.RemoveNoteId);
               _categoryManager.RemoveNotes += _noteManager.RemoveNotes;
          }

          public void Start() 
          {
               var noteView = new NoteView(_noteManager);
               var categoryView = new CategoryView(_categoryManager, noteView);
               categoryView.Main();
          }

          public void Dispose()
          {
               _categoryManager.RemoveNotes -= _noteManager.RemoveNotes;
          }
     }
}
