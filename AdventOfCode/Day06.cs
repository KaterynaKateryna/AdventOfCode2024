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
        return new(GetOriginalRoute().Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int loops = 0;

        Point currentGuardPoint = GetGuard();
        Direction currentGuardDirection = Direction.Up;

        HashSet<Point> originalLocationsVisited = GetOriginalRoute();
        originalLocationsVisited.Remove(currentGuardPoint);

        List<Point> obstacles = GetObstacles();

        foreach (var originalLocation in originalLocationsVisited)
        {
            obstacles.Add(originalLocation);
            if (CanCreateALoop(currentGuardPoint, currentGuardDirection, obstacles))
            {
                loops++;
            }
            obstacles.Remove(originalLocation);
        }

        return new(loops.ToString());
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

    private List<Point> GetObstacles()
    {
        List<Point> obstacles = new List<Point>();
        for (int i = 0; i < _map.Length; ++i)
        {
            for (int j = 0; j < _map[0].Length; ++j)
            {
                if (_map[i][j] == '#')
                {
                    obstacles.Add(new Point(i, j));
                }
            }
        }
        return obstacles;
    }

    private HashSet<Point> GetOriginalRoute()
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

        return visitedLocations;
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

    private PointWithDirection? GetNextMove2(
        PointWithDirection point, 
        Dictionary<int, List<int>> obstaclesByI,
        Dictionary<int, List<int>> obstaclesByJ
    )
    {
        switch (point.Direction)
        {
            case Direction.Up:
                if(obstaclesByJ.TryGetValue(point.J, out List<int>? rows) && rows.Any(x => x < point.I))
                {
                    int row = rows.Last(x => x < point.I);
                    return new PointWithDirection(row + 1, point.J, Direction.Right);
                }
                return null;
            case Direction.Right:
                if (obstaclesByI.TryGetValue(point.I, out List<int>? columns) && columns.Any(x => x > point.J))
                {
                    int column = columns.First(x => x > point.J);
                    return new PointWithDirection(point.I, column - 1, Direction.Down);
                }
                return null;
            case Direction.Down:
                if (obstaclesByJ.TryGetValue(point.J, out List<int>? rows2) && rows2.Any(x => x > point.I))
                {
                    int row = rows2.First(x => x > point.I);
                    return new PointWithDirection(row - 1, point.J, Direction.Left);
                }
                return null;
            case Direction.Left:
                if (obstaclesByI.TryGetValue(point.I, out List<int>? columns2) && columns2.Any(x => x < point.J))
                {
                    int column = columns2.Last(x => x < point.J);
                    return new PointWithDirection(point.I, column + 1, Direction.Up);
                }
                return null;
        }

        throw new NotImplementedException();
    }

    private bool CanCreateALoop(Point startingPoint, Direction startingDirection, List<Point> obstacles)
    {
        HashSet<PointWithDirection> visited = new HashSet<PointWithDirection>();
        PointWithDirection? currentPointWithDirection = new PointWithDirection(startingPoint.I, startingPoint.J, startingDirection);
        visited.Add(currentPointWithDirection);

        Dictionary<int, List<int>> obstaclesByI = obstacles
            .GroupBy(x => x.I)
            .ToDictionary(x => x.Key, x => x.Select(y => y.J).OrderBy(y => y).ToList());

        Dictionary<int, List<int>> obstaclesByJ = obstacles
            .GroupBy(x => x.J)
            .ToDictionary(x => x.Key, x => x.Select(y => y.I).OrderBy(y => y).ToList());


        while (true)
        {
            currentPointWithDirection = GetNextMove2(currentPointWithDirection, obstaclesByI, obstaclesByJ);
            if (currentPointWithDirection == null)
            {
                return false;
            }
            if (!visited.Add(currentPointWithDirection))
            {
                return true;
            }
        }

        throw new Exception("Unreachable code");
    }
 
    private record Point(int I, int J);

    private record PointWithDirection(int I, int J, Direction Direction);

    private enum Direction
    { 
        Up,
        Right,
        Down,
        Left
    }
}
