using System.Text;
using Spectre.Console;

namespace AocLib;

public class GridVisualizer<T>(Grid<T> grid)
    where T : IEquatable<T>
{
    private Func<int, int, bool> overlayPredicate;
    private T overlayValue;

    private Func<T, bool> valueStylePredicate;
    private string valueStyleMarkup;
    
    public GridVisualizer<T> WithOverlay(Func<int, int, bool> predicate, T value)
    {
        this.overlayPredicate = predicate;
        this.overlayValue = value;

        return this;
    }

    public GridVisualizer<T> WithValueStyle(Func<T, bool> valueStylePredicate, string markup)
    {
        this.valueStylePredicate = valueStylePredicate;
        this.valueStyleMarkup = markup;

        return this;
    }

    public void Display()
    {
        var sb = new StringBuilder();
        
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var value = (overlayPredicate?.Invoke(x, y) ?? false)
                    ? overlayValue
                    : grid[x, y];
                
                var styledValue = valueStylePredicate(value)
                    ? $"{valueStyleMarkup}{value}{"[/]".Expand(valueStyleMarkup.Count(c => c == '['))}"
                    : value.ToString();
                
                sb.Append(styledValue);
            }

            sb.AppendLine();
        }
        
        AnsiConsole.Markup(sb.ToString());
    }
}
