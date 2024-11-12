namespace ToDoListV2.Views;

internal class ViewBase
{
     protected int IsValidNumber(string command)
     {
          var res = int.TryParse(command, out int trueNumber);
          if (res)
               return trueNumber;
          else
               return 0;
     }
     protected string GetValidInput(bool toLover = false, string? message = null)
     {
        if (message != null)
            Console.WriteLine(message);
          while (true)
          {
               var input = Console.ReadLine()!;
               var isValidInput = string.IsNullOrWhiteSpace(input) ? false : true;

            if (isValidInput)          
               return InputProcess(input, toLover);  
            
            else Console.WriteLine("Помилка: введено порожній рядок. Рядок має містити хоча б 1 символ! Спробуйте ще раз.") ;
          }
     }
     private string InputProcess(string input, bool toLover)
     {
          if (toLover) return input.ToLower();
          else return input;
     }
     private void ClearScreen()
     {
          Console.Clear();
          Console.WriteLine("Натисніть будь-яку клавішу");
     }
}