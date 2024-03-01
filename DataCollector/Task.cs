using System.Text.Json.Serialization;

namespace DataCollector;

public class Task: IComparable<Task>
{
    private double _material;
    private double _pages;
    private double _timeSpent;
    private double _weight;
    
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("new_material")]
    public double Material
    {
        get => _material;
        set => _material = value is >= 0 and <= 5 ? value : throw new ArgumentException("Value must be within the range [0, 5]");
    }

    [JsonPropertyName("pages")]
    public double Pages
    {
        get => _pages;
        set => _pages = value is > 0 and <= 30
            ? value
            : throw new ArgumentException("Value must be within the range (0, 5]");
    }

    [JsonPropertyName("time")]
    public double TimeSpent
    {
        get => _timeSpent;
        set => _timeSpent = value is > 0 and <= 2000
            ? value
            : throw new ArgumentException("Value must be within the range (0, 2000]");
    }

    [JsonPropertyName("weight")]
    public double Weight
    {
        get => _weight;
        set => _weight = value is > 0 and < 2
            ? value
            : throw new ArgumentException("Value must be within the range (0, 2)");
    }

    public Task(string name, double material, double pages, double time, double weight)
    {
        Name = name;
        Material = material;
        Pages = pages;
        TimeSpent = time;
        Weight = weight;
    }

    public Task() : this("New task", 3, 1, 10, 1.5)
    {
    }

    public int CompareTo(Task? other)
    {
        return Weight.CompareTo(other?.Weight ?? 0);
    }

    public override string ToString() => $"{Name}, {Math.Round(TimeSpent, 2)} min, {Math.Round(Pages, 2)} pages";
}