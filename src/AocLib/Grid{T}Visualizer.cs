using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace AocLib;

public class GridVisualizer<T>(Grid<T> grid)
    where T : IEquatable<T>
{
    private readonly List<(Predicate<Point>, T)> overlayPredicates = [];
    private readonly List<(Func<Point, T, bool>, string)> stylePredicates = [];
    private string? data;

    public GridVisualizer<T> WithData<TData>(TData data)
    {
        this.data = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        return this;
    }
    
    public GridVisualizer<T> WithOverlay(Predicate<Point> predicate, T value)
    {
        overlayPredicates.Add((predicate, value));
        return this;
    }
    
    public GridVisualizer<T> WithStyle(Func<Point, T, bool> predicate, string markup)
    {
        stylePredicates.Add((predicate, markup));
        return this;
    }

    public void Display(bool wait = false)
    {
        if (wait) Console.Clear();
        
        var sb = new StringBuilder();
        
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var value = grid[x, y];
                foreach (var (predicate, overlay) in overlayPredicates)
                {
                    value = predicate((x, y)) ? overlay : value;
                }

                string styledValue = value.ToString()!;
                foreach (var (predicate, style) in stylePredicates)
                {
                    styledValue = predicate((x, y), value)
                        ? $"{style}{value}{"[/]".Expand(style.Count(c => c == '['))}"
                        : styledValue;
                }
         
                sb.Append(styledValue);
            }

            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(data))
        {
            var formattedData = Regex.Replace(data, "\"(.*)\"", @"[grey39]""[/]$1[grey39]""[/]");
            
            sb.AppendLine();
            sb.Append(formattedData);
        }
        
        AnsiConsole.Markup(sb.ToString());
        
        if (wait) Console.ReadKey();
    }
}
