using System.Numerics;

namespace AocLib;

public class Grid<T>
    where T : IEquatable<T>
{
    private List<List<T>> grid = [[]];
    
    private Grid() {}
    
    public T this[int x, int y]
    {
        get => grid[y][x];
        set => grid[y][x] = value;
    }

    public T this[Point point]
    {
        get => this[point.X, point.Y];
        set => this[point.X, point.Y] = value;
    }
    
    public int Width => grid[0].Count;
    
    public int Height => grid.Count;

    public T At((int X, int Y) position) => 
        grid[position.Y][position.X];
    
    public void Set((int X, int Y) position, T value) =>
        grid[position.Y][position.X] = value;
    
    public (int X, int Y) Find(T value)
    {
        for (int y = 0; y < grid.Count; y++)
        for (int x = 0; x < grid[0].Count; x++)
        {
            if (grid[y][x].Equals(value))
                return (x, y);
        }
        
        throw new KeyNotFoundException($"Value {value} was not found.");
    }

    public static Grid<T> Create(string input, Func<string, T[]> parser) =>
        Create(input.SplitLines(), parser);

    public static Grid<T> Create(IEnumerable<string> input, Func<string, T[]> parser)
    {
        var list = input.Select(parser).Select(p => p.ToList()).ToList();
        
        if (list.Any(row => row.Count != list[0].Count))
            throw new Exception("Invalid input. All rows must be the same length.");
        
        return new() { grid = list };
    }
}

public static class GridExtensions
{
    public static Grid<T> ToGrid<T>(this string input)
        where T : IEquatable<T> => ToGrid<T>(input.SplitLines());
    
    public static Grid<T> ToGrid<T>(this IEnumerable<string> input)
        where T : IEquatable<T> => typeof(T) switch
        {
            var t when t == typeof(char) => Grid<char>.Create(input, str => str.ToCharArray()) as Grid<T>
                ?? throw new Exception("Unable to convert string to Grid<char>"),

            _ => throw new ArgumentException($"Invalid type {typeof(T)}", nameof(input)),
        };
}