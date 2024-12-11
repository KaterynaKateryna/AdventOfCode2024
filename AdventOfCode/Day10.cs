namespace AdventOfCode;

public class Day10 : BaseDay
{
    int[][] _map;

    public Day10()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        _map = lines.Select(l => l.Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        int totalScore = 0;
        for (int i = 0; i < _map.Length; ++i)
        {
            for (int j = 0; j < _map[0].Length; ++j)
            {
                if (_map[i][j] == 0)
                {
                    totalScore += GetTrailheadScore(i, j, getRating: false);
                }
            }
        }

        return new(totalScore.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int totalScore = 0;
        for (int i = 0; i < _map.Length; ++i)
        {
            for (int j = 0; j < _map[0].Length; ++j)
            {
                if (_map[i][j] == 0)
                {
                    totalScore += GetTrailheadScore(i, j, getRating: true);
                }
            }
        }

        return new(totalScore.ToString());
    }

    private int GetTrailheadScore(int i, int j, bool getRating)
    {
        Point start = new Point(i, j);
        return GetTrailheadScore(start, new HashSet<Point>() { start }, new HashSet<Point>(), getRating);
    }

    private int GetTrailheadScore(Point current, HashSet<Point> visited, HashSet<Point> ninePositionsreached, bool getRating)
    {
        int currentValue = _map[current.I][current.J];
        if (currentValue == 9)
        {
            if (ninePositionsreached.Add(current))
            {
                return 1;
            }
            return getRating ? 1 : 0;
        }

        int score = 0;

        foreach (Point neighbour in GetNeighbourPoints(current))
        {
            if (neighbour != null && _map[neighbour.I][neighbour.J] == currentValue + 1)
            {
                visited.Add(neighbour);
                score += GetTrailheadScore(neighbour, visited, ninePositionsreached, getRating);
                visited.Remove(neighbour);
            }
        }

        return score;
    }

    private IEnumerable<Point> GetNeighbourPoints(Point current)
    {
        // right
        if (current.J < _map[0].Length - 1)
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
        if (current.I < _map.Length - 1)
        {
            yield return new Point(current.I + 1, current.J);
        }
    }

    private record Point(int I, int J);
}
