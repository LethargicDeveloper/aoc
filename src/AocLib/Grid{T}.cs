using System.Collections;

namespace AocLib;

public class Grid<T> : IEnumerable<(Point Index, T Value)>
    where T : IEquatable<T>
{
    private List<List<T>> grid = [];
    
    private Grid() {}

    public Grid(int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            grid.Add([..new T[width]]);
        }
    }
    
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
    
    public Point Find(T value)
    {
        for (int y = 0; y < grid.Count; y++)
        for (int x = 0; x < grid[0].Count; x++)
        {
            if (grid[y][x].Equals(value))
                return (x, y);
        }
        
        throw new KeyNotFoundException($"Value {value} was not found.");
    }
    
    public bool InBounds(Point point) =>
        point.InBounds(0, 0, Width - 1, Height - 1);

    public static Grid<T> Create(string input, Func<string, T[]> parser) =>
        Create(input.SplitLines(), parser);

    public static Grid<T> Create(IEnumerable<string> input, Func<string, T[]> parser)
    {
        var list = input.Select(parser).Select(p => p.ToList()).ToList();
        
        if (list.Any(row => row.Count != list[0].Count))
            throw new Exception("Invalid input. All rows must be the same length.");
        
        return new() { grid = list };
    }

    public IEnumerator<(Point Index, T Value)> GetEnumerator()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return ((x, y), this[x, y]);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class GridExtensions
{
    public static Grid<char> ToGrid(this string input)
        => ToGrid(input.SplitLines());

    public static Grid<char> ToGrid(this IEnumerable<string> input)
        => Grid<char>.Create(input, str => str.ToCharArray());
}