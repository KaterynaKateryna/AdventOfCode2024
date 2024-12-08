namespace AdventOfCode;

internal class Day08 : BaseDay
{
    char[][] _input;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<char, List<Point>> antennas = GetAntennas();

        HashSet<Point> antinodes = new HashSet<Point>();
        foreach (var kv in antennas)
        {
            antinodes.UnionWith(GetAntinodes(kv.Value, updated: false));
        }

        return new(antinodes.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<char, List<Point>> antennas = GetAntennas();

        HashSet<Point> antinodes = new HashSet<Point>();
        foreach (var kv in antennas)
        {
            antinodes.UnionWith(GetAntinodes(kv.Value, updated: true));
        }

        return new(antinodes.Count.ToString());
    }

    private Dictionary<char, List<Point>> GetAntennas()
    {
        Dictionary<char, List<Point>> antennas = new Dictionary<char, List<Point>>();
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[i].Length; j++)
            {
                char ch = _input[i][j];
                if (ch != '.')
                {
                    if (!antennas.TryAdd(ch, [new Point(i, j)]))
                    {
                        antennas[ch].Add(new Point(i, j));
                    }
                }
            }
        }
        return antennas;
    }

    private HashSet<Point> GetAntinodes(List<Point> antennas, bool updated)
    {
        HashSet<Point> antinodes = new HashSet<Point>();

        for (int i = 0; i < antennas.Count - 1; ++i)
        {
            for (int j = i + 1; j < antennas.Count; ++j)
            {
                List<Point> a = updated
                    ? GetAntinodesUpdated(antennas[i], antennas[j]) : GetAntinodes(antennas[i], antennas[j]);
                antinodes.UnionWith(a);
            }
        }

        return antinodes;
    }

    private List<Point> GetAntinodes(Point a, Point b)
    {
        List<Point> result = new List<Point>();

        var antinodeOne = GetAntinode(a, b, 2);
        var antinodeTwo = GetAntinode(b, a, 2);

        if (IsWithinBounds(antinodeOne))
        { 
            result.Add(antinodeOne);
        }
        if (IsWithinBounds(antinodeTwo))
        {
            result.Add(antinodeTwo);
        }

        return result;
    }

    private List<Point> GetAntinodesUpdated(Point a, Point b)
    {
        List<Point> resultOne = GetAntinodesUpdatedOneDirection(a, b);
        List<Point> resultTwo = GetAntinodesUpdatedOneDirection(b, a);

        return resultOne.Concat(resultTwo).ToList();
    }

    private List<Point> GetAntinodesUpdatedOneDirection(Point a, Point b)
    {
        List<Point> result = new List<Point>();

        int n = 1;
        var antinode = GetAntinode(a, b, n);
        while (IsWithinBounds(antinode))
        {
            result.Add(antinode);
            n++;
            antinode = GetAntinode(a, b, n);
        }

        return result;
    }

    private Point GetAntinode(Point a, Point b, int n)
    { 
        return new Point(n * b.I - (n - 1) * a.I, n * b.J - (n - 1) * a.J);
    }

    private bool IsWithinBounds(Point point)
    { 
        return point.I >= 0 && point.I < _input.Length
            && point.J >= 0 && point.J < _input[0].Length;
    }

    private record Point(int I, int J);
}
