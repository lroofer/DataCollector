using System.Text;

namespace  DataCollector;

using static Markup;

/// <summary>
/// The abstract class description of the view.
/// </summary>
/// <typeparam name="T">Generic type of representing objects</typeparam>
public abstract class View<T>
{
    // Currently selected option.
    protected int SelectedOption;

    // List of options.
    protected List<T> Options;

    // The position of the option on the terminal (is for redrawing without updating).
    private int[] _optionLocations;

    // Display text preview.
    private readonly string _prompt;

    protected View(List<T> options, string prompt)
    {
        SelectedOption = 0;
        Options = options;
        _prompt = prompt;
        _optionLocations = new int[Options.Count];
    }

    /// <summary>
    /// The information about the controls.
    /// </summary>
    protected virtual void ButtonsView()
    {
        Console.SetCursorPosition(0, Console.BufferHeight - 2);
        Formula("Enter");
        Success(" - Expand || ", false);
        Formula("X");
        Success(" - Back ", false);
    }

    /// <summary>
    /// Initial drawing of the interface.
    /// </summary>
    /// <param name="startPosition">The first item displayed.</param>
    protected void Init(int startPosition = 0)
    {
        SelectedOption = startPosition;
        Console.Clear();
        Header(_prompt);
        _optionLocations = new int[Options.Count];
        for (var i = 0; i < startPosition; ++i)
        {
            _optionLocations[i] = -1;
        }

        for (var i = startPosition; i < Options.Count; ++i)
        {
            if (i - startPosition >= Console.BufferHeight - 3)
            {
                _optionLocations[i] = -1;
                continue;
            }

            Console.ForegroundColor = i == SelectedOption ? BackgroundColor : ForegroundColor;
            Console.BackgroundColor = i == SelectedOption ? ForegroundColor : BackgroundColor;
            _optionLocations[i] = Console.GetCursorPosition().Top;
            Console.WriteLine($"<{(i == SelectedOption ? '*' : '-')}> {Options[i]}");
        }

        ButtonsView();
        Console.WriteLine();
    }

    /// <summary>
    /// Move one element up.
    /// </summary>
    protected void Up()
    {
        if (_optionLocations[SelectedOption - 1] == -1)
        {
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Success("Press ", false);
            Formula("M");
            Success(" button to see previous widgets", false);
            if (Console.ReadKey(true).Key != ConsoleKey.M)
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                Console.ResetColor();
                Console.Write("                                      ");
                return;
            }

            SelectedOption--;
            Init(Math.Max(0, SelectedOption - Console.BufferHeight + 2));
            return;
        }

        Console.SetCursorPosition(0, _optionLocations[--SelectedOption]);
        Console.ForegroundColor = BackgroundColor;
        Console.BackgroundColor = ForegroundColor;
        var v1 = new StringBuilder($"<*> {Options[SelectedOption]}");
        while (v1.Length < Console.BufferWidth) v1.Append(' ');
        Console.WriteLine(v1.ToString());
        Console.BackgroundColor = BackgroundColor;
        Console.ForegroundColor = ForegroundColor;
        var v2 = new StringBuilder($"<-> {Options[SelectedOption + 1]}");
        while (v2.Length < Console.BufferWidth) v2.Append(' ');
        Console.WriteLine(v2.ToString());
    }

    /// <summary>
    /// Move one element down.
    /// </summary>
    protected void Down()
    {
        if (_optionLocations[SelectedOption + 1] == -1)
        {
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Console.ResetColor();
            Success("Press ", false);
            Formula("M");
            Success(" button to see more widgets", false);
            if (Console.ReadKey(true).Key != ConsoleKey.M)
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                Console.ResetColor();
                Console.Write("                                  ");
                return;
            }

            Init(++SelectedOption);
            return;
        }

        Console.SetCursorPosition(0, _optionLocations[SelectedOption]);
        Console.ForegroundColor = ForegroundColor;
        Console.BackgroundColor = BackgroundColor;
        var v1 = new StringBuilder($"<-> {Options[SelectedOption++]}");
        while (v1.Length < Console.BufferWidth) v1.Append(' ');
        Console.WriteLine(v1.ToString());
        Console.BackgroundColor = ForegroundColor;
        Console.ForegroundColor = BackgroundColor;
        var v2 = new StringBuilder($"<*> {Options[SelectedOption]}");
        while (v2.Length < Console.BufferWidth) v2.Append(' ');
        Console.WriteLine(v2.ToString());
    }
}