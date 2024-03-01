using System.Text.Json;

namespace DataCollector;

using static Markup;

class Program
{
    static void Main()
    {
        do
        {
            Console.Clear();
            Header(
                "[To ensure great performance and correct displaying of data please EXPAND your terminal window]");

            Success("By pressing enter you admit that window size won't be changed by yourself");
            Console.ReadLine();
            List<Task> taskList;
            try
            {
                try
                {
                    var stream = File.OpenRead("data.json");
                    taskList = JsonSerializer.Deserialize<List<Task>>(stream) ?? new List<Task>();
                }
                catch (Exception e)
                {
                    Warning("data.json was not found, specify the absolute path to it");
                    var stream = File.OpenRead(Console.ReadLine()!);
                    taskList = JsonSerializer.Deserialize<List<Task>>(stream) ?? new List<Task>();
                }
            }
            catch (Exception e)
            {
                taskList = new List<Task>();
            }
            Console.CursorVisible = false;
            try
            {
                var tasksView = new TasksView(taskList, "[Tasks] - use U and D to sort values in complexity increasing order");
                tasksView.Run();
            }
            catch (Exception e)
            {
                Warning($"Something went wrong: {e.Message}");
            }

            Console.CursorVisible = true;
            Header("Press Q to quit the app or any other key to continue...");
        } while (Console.ReadKey(true).Key != ConsoleKey.Q);
    }
    
}