namespace AdventOfCode;

public class Day12 : BaseDay
{
    private char[][] _input;

    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.ToArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        return new(Solve(false).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(Solve(true).ToString());
    }

    private int Solve(bool updated)
    {
        bool[][] visited = new bool[_input.Length][];
        for (int i = 0; i < visited.Length; ++i)
        {
            visited[i] = new bool[_input[i].Length];
        }

        List<List<Point>> areas = new List<List<Point>>();

        Point? nextStart = GetNextNotVisited(visited);
        while (nextStart != null)
        {
            areas.Add(GetArea(nextStart, visited));
            nextStart = GetNextNotVisited(visited);
        }

        if (updated)
        {
            return areas.Sum(GetNewAreaPrice);
        }
        return areas.Sum(GetAreaPrice);
    }

    private List<Point> GetArea(Point start, bool[][] visited)
    {
        visited[start.I][start.J] = true;
        char value = _input[start.I][start.J];
        List<Point> area = new List<Point>() { start };
        List<Point> pointsToCheck = new List<Point>();
        pointsToCheck.AddRange(GetNeighbourPoints(start));
        while (pointsToCheck.Any())
        {
            Point pointToCheck = pointsToCheck[0];
            pointsToCheck.RemoveAt(0);

            if (visited[pointToCheck.I][pointToCheck.J])
            {
                continue;
            }
            if (_input[pointToCheck.I][pointToCheck.J] == value)
            {
                visited[pointToCheck.I][pointToCheck.J] = true;
                area.Add(pointToCheck);

                pointsToCheck.AddRange(GetNeighbourPoints(pointToCheck));
            }
        }

        return area;
    }

    private int GetAreaPrice(List<Point> area)
    {
        int areaSize = area.Count;

        int perimeter = 0;
        foreach (Point point in area)
        {
            List<Point> neighbours = GetNeighbourPoints(point).ToList();
            perimeter += 4 - neighbours.Count; // edges
            perimeter += neighbours.Count(n => !area.Contains(n));
        }

        return perimeter * areaSize;
    }

    private int GetNewAreaPrice(List<Point> area)
    {
        int areaSize = area.Count;

        HashSet<PointSide> sides = new HashSet<PointSide>();

        foreach (Point point in area)
        {
            // top
            if (point.I == 0 || !area.Contains(new Point(point.I - 1, point.J)))
            {
                TryAddSide(point, Side.Top, sides, area);
            }

            // bottom
            if (point.I == _input.Length - 1 || !area.Contains(new Point(point.I + 1, point.J)))
            {
                TryAddSide(point, Side.Bottom, sides, area);
            }

            // left
            if (point.J == 0 || !area.Contains(new Point(point.I, point.J - 1)))
            {
                TryAddSide(point, Side.Left, sides, area);
            }

            // right
            if (point.J == _input[0].Length || !area.Contains(new Point(point.I, point.J + 1)))
            {
                TryAddSide(point, Side.Right, sides, area);
            }
        }

        return sides.Count * areaSize;
    }

    private void TryAddSide(Point point, Side side, HashSet<PointSide> pointSides, List<Point> area)
    {
        if (side == Side.Top || side == Side.Bottom)
        {
            var existingSides = pointSides.Where(ps => ps.Point.I == point.I && ps.Side == side);
            foreach (var existing in existingSides)
            {
                if (CanReachHorizontally(point, existing, side))
                {
                    return;
                }
            }
            pointSides.Add(new PointSide(point, side));
        }

        if (side == Side.Left || side == Side.Right)
        {
            var existingSides = pointSides.Where(ps => ps.Point.J == point.J && ps.Side == side);
            foreach (var existing in existingSides)
            {
                if (CanReachVertically(point, existing, side))
                {
                    return;
                }
            }
            pointSides.Add(new PointSide(point, side));
        }
    }

    private bool CanReachHorizontally(Point point, PointSide existing, Side side)
    {
        char value = _input[point.I][point.J];
        if (point.J < existing.Point.J)
        {
            for (int j= point.J + 1; j < existing.Point.J; ++j)
            {
                if (_input[point.I][j] != value)
                {
                    return false;
                }
                if (side == Side.Top && point.I != 0 && _input[point.I - 1][j] == value)
                {
                    return false;
                }
                if (side == Side.Bottom && point.I != _input.Length - 1 && _input[point.I + 1][j] == value)
                {
                    return false;
                }
            }

            return true;
        }

        if (point.J > existing.Point.J)
        {
            for (int j = point.J - 1; j > existing.Point.J; --j)
            {
                if (_input[point.I][j] != value)
                {
                    return false;
                }
                if (side == Side.Top && point.I != 0 && _input[point.I - 1][j] == value)
                {
                    return false;
                }
                if (side == Side.Bottom && point.I != _input.Length - 1 && _input[point.I + 1][j] == value)
                {
                    return false;
                }
            }

            return true;
        }

        throw new Exception();
    }

    private bool CanReachVertically(Point point, PointSide existing, Side side)
    {
        char value = _input[point.I][point.J];
        if (point.I < existing.Point.I)
        {
            for (int i = point.I + 1; i < existing.Point.I; ++i)
            {
                if (_input[i][point.J] != value)
                {
                    return false;
                }
                if (side == Side.Left && point.J != 0 && _input[i][point.J - 1] == value)
                {
                    return false;
                }
                if (side == Side.Right && point.J != _input[i].Length - 1 && _input[i][point.J + 1] == value)
                {
                    return false;
                }
            }

            return true;
        }

        if (point.I > existing.Point.I)
        {
            for (int i = point.I - 1; i > existing.Point.I; --i)
            {
                if (_input[i][point.J] != value)
                {
                    return false;
                }
                if (side == Side.Left && point.J != 0 && _input[i][point.J - 1] == value)
                {
                    return false;
                }
                if (side == Side.Right && point.J != _input[i].Length - 1 && _input[i][point.J + 1] == value)
                {
                    return false;
                }
            }

            return true;
        }

        throw new Exception();
    }

    private Point? GetNextNotVisited(bool[][] visited)
    {
        for (int i = 0; i < visited.Length; ++i)
        {
            for (int j = 0; j < visited[i].Length; ++j) 
            {
                if (!visited[i][j])
                { 
                    return new Point(i, j);
                }
            }
        }
        return null;
    }

    private IEnumerable<Point> GetNeighbourPoints(Point current)
    {
        // right
        if (current.J < _input[0].Length - 1)
        {
            yield return new Point(current.I, current.J + 1);
        }

        // left
        if (current.J > 0)
        {
            yield return new Point(current.I, current.J - 1);
        }

        // top
        if (current.I > 0)
        {
            yield return new Point(current.I - 1, current.J);
        }

        // top
        if (current.I < _input.Length - 1)
        {
            yield return new Point(current.I + 1, current.J);
        }
    }

    private record Point(int I, int J);

    private enum Side
    { 
        Top,
        Bottom,
        Left,
        Right
    }

    private record PointSide(Point Point, Side Side);
}
