
namespace AdventOfCode;

public class Day15 : BaseDay
{
    private readonly char[][] _map;
    private readonly List<char> _moves = new List<char>();
    private Position _robot;

    public Day15()
    { 
        string[] lines = File.ReadAllLines(InputFilePath);

        int width = lines[0].Length;
        int height = lines.Count(l => l.StartsWith("#"));

        _map = new char[height][];
        for (int i = 0; i < height; ++i)
        {
            _map[i] = lines[i].ToCharArray();

            int j = lines[i].IndexOf('@');
            if (j >= 0)
            {
                _robot = new Position(i, j);
            }
        }

        foreach (var line in lines.Skip(height + 1))
        {
            _moves.AddRange(line.ToList());
        }
    }

    public override ValueTask<string> Solve_1()
    {
        foreach (char move in _moves)
        {
            Move(move);
        }

        int gps = GetGPS();

        return new(gps.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private void Move(char move)
    {
        Position next = GetNext(move, _robot);

        Position firstNext = next;
        bool movesBoxes = false;

        while (true)
        {
            char nextChar = _map[next.I][next.J];
            if (nextChar == '#')
            {
                return; // wall = do nothing
            }
            if (nextChar == '.')
            {
                if (movesBoxes)
                {
                    _map[next.I][next.J] = 'O';
                }
                _map[firstNext.I][firstNext.J] = '@';
                _map[_robot.I][_robot.J] = '.';
                _robot = firstNext;
                return;
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

    private int GetGPS()
    {
        int result = 0;
        for (int i = 0; i < _map.Length; ++i)
        {
            for (int j = 0; j < _map[i].Length; ++j)
            {
                if (_map[i][j] == 'O')
                {
                    result += 100 * i + j;
                }
            }
        }

        return result;
    }

    private record Position(int I, int J);
}
