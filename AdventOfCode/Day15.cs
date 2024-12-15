namespace AdventOfCode;

public class Day15 : BaseDay
{
    private (char[][] map, List<char> moves, Position robot) Init()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        Position robot = default!;
        int width = lines[0].Length;
        int height = lines.Count(l => l.StartsWith("#"));

        char[][] map = new char[height][];
        for (int i = 0; i < height; ++i)
        {
            map[i] = lines[i].ToCharArray();

            int j = lines[i].IndexOf('@');
            if (j >= 0)
            {
                robot = new Position(i, j);
            }
        }

        List<char> moves = new List<char>();
        foreach (var line in lines.Skip(height + 1))
        {
            moves.AddRange(line.ToList());
        }

        return (map, moves, robot);
    }

    public override ValueTask<string> Solve_1()
    {
        (char[][] map, List<char> moves, Position robot) = Init();
        foreach (char move in moves)
        {
            robot = Move(move, robot, map);
        }

        int gps = GetGPS(map);

        return new(gps.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
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

    private int GetGPS(char[][] map)
    {
        int result = 0;
        for (int i = 0; i < map.Length; ++i)
        {
            for (int j = 0; j < map[i].Length; ++j)
            {
                if (map[i][j] == 'O')
                {
                    result += 100 * i + j;
                }
            }
        }

        return result;
    }

    private record Position(int I, int J);
}
