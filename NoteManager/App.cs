using ToDoListManager.Sevices.Note;
using ToDoListManager.Views;

namespace ToDoListManager
{
    internal class App
    {
        private readonly CategoryManager _categoryManager;
        private readonly NoteManager _noteManager;



          public App()
        {
            _categoryManager = new CategoryManager();
            _noteManager = new NoteManager();
        }

        public void Start()
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            var noteView = new NoteView(_noteManager);
            var categoryView = new CategoryView(_categoryManager, noteView);

            while (true) categoryView.Main();
        }

    }
}
