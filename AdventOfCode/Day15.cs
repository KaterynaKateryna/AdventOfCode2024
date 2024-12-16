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

        for (int i = 0; i < moves.Count; ++i)
        {
            robot = MoveExpanded(moves[i], robot, map);

            if (!AssertValid(map))
            {
                Display(map);
                Console.WriteLine(i);
                Console.ReadLine();
            }
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
        Position f = GetNext(move, robot);
        List<Node> next = new List<Node> { new Node (f, robot) };

        if (move == 'v' || move == '^')
        {
            char first = map[f.I][f.J];
            if (first == ']')
            {
                next.Insert(0, new Node(new Position(f.I, f.J - 1), null));
            }
            if (first == '[')
            {
                next.Add(new Node(new Position(f.I, f.J + 1), null));
            }
        }

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
                List<Node> add = new List<Node>();
                foreach (Node n in nextRow)
                {
                    char ch = map[n.Position.I][n.Position.J];
                    if (ch == '[' && !nextRow.Any(n => n.Position == new Position(n.Position.I, n.Position.J + 1)))
                    {
                        add.Add(new Node(new Position(n.Position.I, n.Position.J + 1), null));
                    }
                    if (ch == ']' && !nextRow.Any(n => n.Position == new Position(n.Position.I, n.Position.J - 1)))
                    {
                        add.Add(new Node(new Position(n.Position.I, n.Position.J - 1), null));
                    }
                }

                nextRow.AddRange(add);
                next.AddRange(add);

            }

            nextRow = nextRow.Where(n => map[n.Position.I][n.Position.J] != '.')
                .Select(n => new Node(GetNext(move, n.Position), n.Position)).ToList();
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

    private bool AssertValid(char[][] map)
    {
        for (int i = 0; i < map.Length; ++i)
        {
            for (int j = 0; j < map[i].Length; ++j)
            {
                if (map[i][j] == '[' && map[i][j + 1] != ']')
                {
                    return false;
                }
                if (map[i][j] == ']' && map[i][j - 1] != '[')
                {
                    return false;
                }
            }
        }
        return true;
    }

    private record Position(int I, int J);

    private record Node(Position Position, Position? Previous);
}
