namespace AdventOfCode;

public class Day21 : BaseDay
{
    string[] _codes;

    public Day21()
    {
        _codes = ["029A", "980A", "179A", "456A", "379A"]; //File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int res = 0;
        foreach (var code in _codes)
        {
            int length = GetShortestPath(code);
            int num = int.Parse(code.Replace("A", ""));
            Console.WriteLine($"{length} * {num}");
            res += length * num;
        }

        return new(res.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private int GetShortestPath(string code)
    {
        List<string> one = GetAllPaths(code, _numpad, new Dictionary<(Position, Position), List<string>>());


        var cache = new Dictionary<(Position, Position), List<string>>();

        List<string> two = new List<string>();
        foreach (string path in one)
        {
            two.AddRange(GetAllPaths(path, _directionalPad, cache));
        }

        int shortest = int.MaxValue;
        foreach (string path in two)
        {
            var t = GetPathLength(path, _directionalPad, cache);
            if (t < shortest)
            {
                shortest = t;
            }
        }

        return shortest;
    }

    private List<string> GetAllPaths(
        string code, 
        Dictionary<char, Position> pad, 
        Dictionary<(Position, Position), List<string>> cache
    )
    {
        Position from = pad['A'];
        List<string> paths = new List<string>() { "" };

        foreach (char button in code)
        {
            Position to = pad[button];

            List<string> segments = new List<string>();
            if (cache.ContainsKey((from, to)))
            {
                segments = cache[(from, to)];
            }
            else 
            {
                segments = GetAllPaths(from, to, "", cache);
                cache[(from, to)] = segments;
            }
            
            paths = paths.SelectMany(p => segments, (a, b) => a + b).Distinct()
                .Select(p => p + "A").ToList();

            from = to;
        }

        return paths;
    }

    private int GetPathLength(
        string code,
        Dictionary<char, Position> pad,
        Dictionary<(Position, Position), List<string>> cache
    )
    {
        Position from = pad['A'];
        int length = 0;

        foreach (char button in code)
        {
            Position to = pad[button];

            if (cache.ContainsKey((from, to)))
            {
                length += cache[(from, to)][0].Length;
                length++;
            }
            else
            {
                var segments = GetAllPaths(from, to, "", cache);
                cache[(from, to)] = segments;
                length += cache[(from, to)][0].Length;
                length++;
            }

            from = to;
        }

        return length;
    }

    private List<string> GetAllPaths(
        Position from, 
        Position to, 
        string currentPath, 
        Dictionary<(Position, Position), List<string>> cache
    )
    {
        List<string> paths = new List<string>();
        if (from == to)
        {
            paths.Add(currentPath);
            return paths;
        }

        if (from.I - to.I > 0)
        {
            paths.AddRange(GetAllPaths(new Position(from.I - 1, from.J), to, currentPath + "^", cache));
        }
        if (from.I - to.I < 0)
        {
            paths.AddRange(GetAllPaths(new Position(from.I + 1, from.J), to, currentPath + "V", cache));
        }
        if (from.J - to.J > 0)
        {
            paths.AddRange(GetAllPaths(new Position(from.I, from.J - 1), to, currentPath + "<", cache));
        }
        if (from.J - to.J < 0)
        {
            paths.AddRange(GetAllPaths(new Position(from.I, from.J + 1), to, currentPath + ">", cache));
        }

        return paths;
    }

    private readonly Dictionary<char, Position> _numpad = new Dictionary<char, Position>()
    {
        { '7', new Position(0, 0) },
        { '8', new Position(0, 1) },
        { '9', new Position(0, 2) },

        { '4', new Position(1, 0) },
        { '5', new Position(1, 1) },
        { '6', new Position(1, 2) },

        { '1', new Position(2, 0) },
        { '2', new Position(2, 1) },
        { '3', new Position(2, 2) },

        { '0', new Position(3, 1) },
        { 'A', new Position(3, 2) }
    };

    private readonly Dictionary<char, Position> _directionalPad = new Dictionary<char, Position>()
    {
        { '^', new Position(0, 1) },
        { 'A', new Position(0, 2) },

        { '<', new Position(1, 0) },
        { 'V', new Position(1, 1) },
        { '>', new Position(1, 2) }
    };

    private record Position(int I, int J);
}
