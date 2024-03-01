namespace DataCollector;

using static Markup;

class Program
{
    static void Main()
    {
        do
        {
            Console.CursorVisible = false;
            Console.Clear();
            Header(
                "[To ensure great performance and correct displaying of data please EXPAND your terminal window]");

            Success("By pressing enter you admit that window size won't be changed by yourself");
            Console.ReadLine();
            try
            {
                var tasksView = new TasksView(new List<Task>(), "[Tasks]");
                tasksView.Run();
            }
            catch (Exception e)
            {
                Warning($"Something went wrong: {e.Message}");
            }
            Header("Press Q to quit the app or any other key to continue...");
        } while (Console.ReadKey(true).Key != ConsoleKey.Q);
    }
    
}