using System.Text.Json;

namespace DataCollector;

using static Markup;

public class TasksView: View<Task>
{
    public TasksView(List<Task> options, string prompt) : base(options, prompt)
    {
        
    }

    protected override void ButtonsView()
    {
        Console.SetCursorPosition(0, Console.BufferHeight - 2);
        Formula("A");
        Success(" - Add new || ", false);
        Formula("R");
        Success(" - Remove this || ", false);
        Formula("E");
        Success("- Export ||", false);
        Formula("U");
        Success(" - Move up ||", false);
        Formula("D");
        Success(" - Move Down ||", false);
        Formula("Q");
        Success(" - Exit ", false);
    }

    private void WriteFile(string path)
    {
        var jsonString = JsonSerializer.Serialize(Options);
        File.WriteAllText(path, jsonString);
    }
    
    private void Export()
    {
        Console.Clear();
        Header($"Ready to export file, choose path: ");
        Console.CursorVisible = true;
        var name = Console.ReadLine() ?? Directory.GetCurrentDirectory() + "data.json";
        try
        {
            WriteFile(name);
            Success($"Export was succeed: {name}. Press enter");
        }
        catch (Exception e)
        {
            Warning($"There's been a error with you file. \n{e.Message} \nPress enter to return.");
        }
        Console.ReadLine();
        Console.CursorVisible = false;
    }

    private void AddNew()
    {
        Console.CursorVisible = true;
        Console.Clear();
        Header("Adding new task");
        var task = new Task();
        Console.Write("Name for the task: ");
        task.Name = Console.ReadLine() ?? task.Name;
        Console.Write("Was the material new to you? (0 - nothing was new, 5 - absolutely new topic): ");
        while (true)
        {
            if (!double.TryParse(Console.ReadLine(), out double val))
            {
                Warning("Try again with a double");
                continue;
            }

            try
            {
                task.Material = val;
                break;
            }
            catch (Exception e)
            {
                Warning($"{e.Message}, Try again");
            }
        }
        Console.Write("Try to approximately count the number of pages that contain the material: ");
        while (true)
        {
            if (!double.TryParse(Console.ReadLine(), out double val))
            {
                Warning("Try again with a double");
                continue;
            }

            try
            {
                task.Pages = val;
                break;
            }
            catch (Exception e)
            {
                Warning($"{e.Message}, Try again");
            }
        }
        Console.Write("How much time did you spend learning this material (in minutes) -> try to specify clear time (excluding breaks): ");
        while (true)
        {
            if (!double.TryParse(Console.ReadLine(), out double val))
            {
                Warning("Try again with a double");
                continue;
            }

            try
            {
                task.TimeSpent = val;
                break;
            }
            catch (Exception e)
            {
                Warning($"{e.Message}, Try again");
            }
        }
        Console.Write("Try to assess task's complexity with a weight-double value within the range (0, 2): ");
        while (true)
        {
            if (!double.TryParse(Console.ReadLine(), out double val))
            {
                Warning("Try again with a double");
                continue;
            }

            try
            {
                task.Weight = val;
                break;
            }
            catch (Exception e)
            {
                Warning($"{e.Message}, Try again");
            }
        }
        Options.Add(task);
        Console.CursorVisible = false;
    }

    private void DeleteThis()
    {
        Options.RemoveAt(SelectedOption);
        SelectedOption = Math.Max(0, SelectedOption - 1);
    }
    
    private void Swap(int i, int j)
    {
        (Options[i], Options[j]) = (Options[j], Options[i]);
    }

    private void MoveUp() => Swap(SelectedOption, SelectedOption - 1);

    private void MoveDown() => Swap(SelectedOption, SelectedOption + 1);
    
    public void Run()
    {
        Init();
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow when SelectedOption != 0:
                    Up();
                    break;
                case ConsoleKey.DownArrow when SelectedOption != Options.Count - 1:
                    Down();
                    break;
                case ConsoleKey.E:
                    Console.ResetColor();
                    try
                    {
                        Export();
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Warning($"There's been an error while exporting.\n{e.Message}\n Press enter to try again!");
                        Console.ReadLine();
                    }

                    Init();
                    break;
                case ConsoleKey.A:
                    Console.ResetColor();
                    try
                    {
                        AddNew();
                    }
                    catch(Exception e)
                    {
                        Console.Clear();
                        Warning($"There's been an error.\n{e.Message}\n Press enter to try again!");
                        Console.ReadLine();
                    }
                    Init();
                    break;
                case ConsoleKey.R:
                    Console.ResetColor();
                    try
                    {
                        DeleteThis();
                    }
                    catch(Exception e)
                    {
                        Console.Clear();
                        Warning($"There's been an error .\n{e.Message}\n Press enter to try again!");
                        Console.ReadLine();
                    }
                    Init();
                    break;
                case ConsoleKey.U when SelectedOption != 0:
                    MoveUp();
                    Up();
                    break;
                case ConsoleKey.D when SelectedOption != Options.Count - 1:
                    MoveDown();
                    Down();
                    break;
            }
        } while (key != ConsoleKey.Q);

        Console.ResetColor();
        Console.Clear();
    }
}