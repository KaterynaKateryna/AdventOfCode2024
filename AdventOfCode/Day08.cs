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
            antinodes.UnionWith(GetAntinodes(kv.Value));
        }

        return new(antinodes.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
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

    private HashSet<Point> GetAntinodes(List<Point> antennas)
    {
        HashSet<Point> antinodes = new HashSet<Point>();

        for (int i = 0; i < antennas.Count - 1; ++i)
        {
            for (int j = i + 1; j < antennas.Count; ++j)
            {
                (Point? antinodeOne, Point? antinodeTwo) = GetAntinodes(antennas[i], antennas[j]);

                if (antinodeOne != null)
                {
                    antinodes.Add(antinodeOne);
                }
                if (antinodeTwo != null)
                {
                    antinodes.Add(antinodeTwo);
                }
            }
        }

        return antinodes;
    }

    private (Point? antinodeOne, Point? antinodeTwo) GetAntinodes(Point a, Point b)
    {
        var antinodeOne = new Point(2 * b.I - a.I, 2 * b.J - a.J);
        var antinodeTwo = new Point(2 * a.I - b.I, 2 * a.J - b.J);

        return (IsWithinBounds(antinodeOne) ? antinodeOne : null, 
            IsWithinBounds(antinodeTwo) ? antinodeTwo : null);
    }

    private bool IsWithinBounds(Point point)
    { 
        return point.I >= 0 && point.I < _input.Length
            && point.J >= 0 && point.J < _input[0].Length;
    }

    private record Point(int I, int J);
}
