using System.Collections;
using System.Collections.Generic;

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

        int price = areas.Sum(GetAreaPrice);

        return new(price.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
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
}
