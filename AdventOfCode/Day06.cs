namespace AdventOfCode;

internal class Day06 : BaseDay
{
    private char[][] _map;

    public Day06()
    {
        _map = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        HashSet<Point> visitedLocations = new HashSet<Point>();
        Point currentGuardPoint = GetGuard();
        Direction currentGuardDirection = Direction.Up;
        visitedLocations.Add(currentGuardPoint);

        while (true)
        {
            try
            {
                (currentGuardPoint, currentGuardDirection) = GetNextMove(currentGuardPoint, currentGuardDirection);
                visitedLocations.Add(currentGuardPoint);
            }
            catch (IndexOutOfRangeException)
            {
                break;
            }
        }


        return new(visitedLocations.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private Point GetGuard()
    {
        for (int i = 0; i < _map.Length; ++i)
        {
            for (int j = 0; j < _map[0].Length; ++j)
            {
                if (_map[i][j] == '^')
                { 
                    return new Point(i, j);
                }
            }
        }
        throw new Exception("Guard not found.");
    }

    private (Point point, Direction direction) GetNextMove(Point point, Direction direction)
    {
        switch (direction)
        { 
            case Direction.Up:
                if (_map[point.I - 1][point.J] == '#')
                {
                    return (new Point(point.I, point.J + 1), Direction.Right);
                }
                else
                {
                    return (new Point(point.I - 1, point.J), Direction.Up);
                }
            case Direction.Right:
                if (_map[point.I][point.J + 1] == '#')
                {
                    return (new Point(point.I + 1, point.J), Direction.Down);
                }
                else
                {
                    return (new Point(point.I, point.J + 1), Direction.Right);
                }
            case Direction.Down:
                if (_map[point.I + 1][point.J] == '#')
                {
                    return (new Point(point.I, point.J - 1), Direction.Left);
                }
                else
                {
                    return (new Point(point.I + 1, point.J), Direction.Down);
                }
            case Direction.Left:
                if (_map[point.I][point.J - 1] == '#')
                {
                    return (new Point(point.I - 1, point.J), Direction.Up);
                }
                else
                {
                    return (new Point(point.I, point.J - 1), Direction.Left);
                }
        }

        throw new NotImplementedException();
    }

    private record Point(int I, int J);

    private enum Direction
    { 
        Up,
        Right,
        Down,
        Left
    }
}
