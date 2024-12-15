namespace AdventOfCode;

public class Day15 : BaseDay
{
    private (char[][] map, List<char> moves, Position robot) Init(bool expand)
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        Position robot = default!;
        int width = lines[0].Length;
        int height = lines.Count(l => l.StartsWith("#"));

        if (expand)
        {
            width *= 2;
        }

        char[][] map = new char[height][];
        for (int i = 0; i < height; ++i)
        {
            char[] characters = lines[i].ToCharArray();
            if (expand)
            {
                characters = string.Join("", characters.Select(c => Expand(c))).ToCharArray();
            }

            map[i] = characters;

            bool hasRobot = map[i].Index().Any(x => x.Item == '@');
            if (hasRobot)
            {
                robot = new Position(i, map[i].Index().First(x => x.Item == '@').Index);
            }
        }

        List<char> moves = new List<char>();
        foreach (var line in lines.Skip(height + 1))
        {
            moves.AddRange(line.ToList());
        }

        return (map, moves, robot);
    }

    private string Expand(char c)
    {
        switch (c) 
        {
            case '#':
                return "##";
            case 'O':
                return "[]";
            case '.':
                return "..";
            case '@':
                return "@.";
            default:
                throw new NotImplementedException();
        }
    }

    public override ValueTask<string> Solve_1()
    {
        (char[][] map, List<char> moves, Position robot) = Init(expand: false);
        foreach (char move in moves)
        {
            robot = Move(move, robot, map);
        }

        int gps = GetGPS(map, 'O');

        return new(gps.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        (char[][] map, List<char> moves, Position robot) = Init(expand: true);

        Display(map);
        Task.Delay(200).Wait();
        foreach (char move in moves)
        {
            robot = MoveExpanded(move, robot, map);
            Display(map);
            Task.Delay(200).Wait();
        }

        int gps = GetGPS(map, '[');
        return new(gps.ToString());
    }

    private Position Move(char move, Position robot, char[][] map)
    {
        Position next = GetNext(move, robot);

        Position firstNext = next;
        bool movesBoxes = false;

        while (true)
        {
            char nextChar = map[next.I][next.J];
            if (nextChar == '#')
            {
                return robot; // wall = do nothing
            }
            if (nextChar == '.')
            {
                if (movesBoxes)
                {
                    map[next.I][next.J] = 'O';
                }
                map[firstNext.I][firstNext.J] = '@';
                map[robot.I][robot.J] = '.';
                robot = firstNext;
                return robot;
            }
            if (nextChar == 'O')
            {
                next = GetNext(move, next);
                movesBoxes = true;
            }
        }
    }

    private Position MoveExpanded(char move, Position robot, char[][] map)
    {
        List<Node> next = new List<Node> { new Node (GetNext(move, robot), robot) };
        List<Node> nextRow = next;

        while (true)
        {
            List<char> nextRowChars = nextRow.Select(n => map[n.Position.I][n.Position.J]).ToList();

            if (nextRowChars.Any(c => c == '#'))
            {
                return robot; // wall = do nothing
            }
            if (nextRowChars.All(c => c == '.'))
            {
                for (int i = next.Count - 1; i >= 0; i--)
                {
                    Node toReplace = next[i];
                    if (toReplace.Previous == null)
                    {
                        map[toReplace.Position.I][toReplace.Position.J] = '.';
                    }
                    else if (toReplace.Previous == robot)
                    {
                        map[toReplace.Position.I][toReplace.Position.J] = '@';
                        map[toReplace.Previous.I][toReplace.Previous.J] = '.';
                        robot = toReplace.Position;
                    }
                    else
                    {
                        map[toReplace.Position.I][toReplace.Position.J] = map[toReplace.Previous.I][toReplace.Previous.J];
                    }
                }
                return robot;
            }

            if (move == 'v' || move == '^')
            {
                char first = map[nextRow.First().Position.I][nextRow.First().Position.J];
                char last = map[nextRow.Last().Position.I][nextRow.Last().Position.J];
                if (first == ']')
                {
                    nextRow.Insert(0, new Node(new Position(nextRow.First().Position.I, nextRow.First().Position.J - 1), null));
                }
                if (last == '[')
                {
                    nextRow.Add(new Node(new Position(nextRow.First().Position.I, nextRow.First().Position.J + 1), null));
                }
            }

            nextRow = nextRow.Select(n => new Node(GetNext(move, n.Position), n.Position)).ToList();
            next.AddRange(nextRow);
        }
    }

    private Position GetNext(char move, Position from)
    {
        switch (move)
        {
            case '^':
                return new Position(from.I - 1, from.J);
            case 'v':
                return new Position(from.I + 1, from.J);
            case '>':
                return new Position(from.I, from.J + 1);
            case '<':
                return new Position(from.I, from.J - 1);
            default:
                throw new NotImplementedException();
        }
    }

    private Position GetPrevious(char move, Position from)
    {
        switch (move)
        {
            case '^':
                return new Position(from.I + 1, from.J);
            case 'v':
                return new Position(from.I - 1, from.J);
            case '>':
                return new Position(from.I, from.J - 1);
            case '<':
                return new Position(from.I, from.J + 1);
            default:
                throw new NotImplementedException();
        }
    }

    private int GetGPS(char[][] map, char box)
    {
        int result = 0;
        for (int i = 0; i < map.Length; ++i)
        {
            for (int j = 0; j < map[i].Length; ++j)
            {
                if (map[i][j] == box)
                {
                    result += 100 * i + j;
                }
            }
        }

        return result;
    }

    private void Display(char[][] map)
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        for (int i = 0; i < map.Length; i++)
        {
            Console.WriteLine(string.Join("", map[i]));
        }
    }

    private record Position(int I, int J);

    private record Node(Position Position, Position? Previous);
}
