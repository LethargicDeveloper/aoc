using AocLib;

namespace _2024.Day08;

public class Part02 : PuzzleSolver<long>
{
    protected override long InternalSolve()
    {
        var grid = input.SplitLines();

        var antennas = new Dictionary<char, List<(int, int)>>();
        
        for (int x = 0; x < grid[0].Length; x++)
        for (int y = 0; y < grid.Length; y++)
        {
            if (grid[y][x] == '.')
                continue;
            
            if (!antennas.ContainsKey(grid[y][x]))
                antennas.Add(grid[y][x], []);
            
            antennas[grid[y][x]].Add((x, y));
        }

        var antinodes = new HashSet<Point>();

        foreach (var key in antennas.Keys)
        {
            var pairs = (
                from a1 in antennas[key]
                from a2 in antennas[key]
                where a1 != a2 && a1.CompareTo(a2) < 0
                select (Antenna1: (Point)a1, Antenna2: (Point)a2));

            foreach (var pair in pairs)
            {
                var (ant1, ant2) = pair;
                antinodes.Add(ant1);
                antinodes.Add(ant2);
                
                var dir = ant1 - ant2;

                for (int i = 1; i < 200; i++)
                {
                    var node1 = (ant1 + (dir * (i, i)));
                    var node2 = (ant2 + ((Point)(dir.X * -1, dir.Y * -1) * (i, i)));
                    antinodes.Add(node1);
                    antinodes.Add(node2);
                }

            }
        }
        
        var signals = antinodes
            .Where(n => n.InBounds(0, 0, grid[0].Length - 1, grid.Length - 1));
        
        return signals.Count();
    }
}