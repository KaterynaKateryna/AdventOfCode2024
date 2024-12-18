namespace AdventOfCode;

public class Day18 : BaseDay
{
    private readonly List<Position> _corrupted = new List<Position>();

    public Day18()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] parts = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
            int corruptedI = int.Parse(parts[1]);
            int corruptedJ = int.Parse(parts[0]);

            _corrupted.Add(new Position(corruptedI, corruptedJ));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int length = 71;
        (char[][] map, Position start, Position end) = Init(length, 1024);
        long[][] costs = InitCosts(length, start);

        GetCost(map, costs, start, 0, end, Direction.East, length);

        long cost = costs[end.I][end.J];

        return new(cost.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int length = 71;
        (char[][] map, Position start, Position end) = Init(length, _corrupted.Count);

        int i = _corrupted.Count - 1;
        for (; i >= 0; --i)
        {
            map[_corrupted[i].I][_corrupted[i].J] = '.';

            long[][] costs = InitCosts(length, start);
            GetCost(map, costs, start, 0, end, Direction.East, length);

            long cost = costs[end.I][end.J];
            if (cost != long.MaxValue)
            {
                break;
            }
        }

        return new($"{_corrupted[i].J},{_corrupted[i].I}");
    }

    private (char[][] map, Position start, Position end) Init(int length, int corrupted)
    {
        char[][] map = new char[length][];
        for (int i = 0; i < map.Length; ++i)
        {
            map[i] = new char[length];
            for (int j = 0; j < map[i].Length; ++j)
            {
                map[i][j] = '.';
            }
        }

        for (int i = 0; i < corrupted; ++i)
        {
            map[_corrupted[i].I][_corrupted[i].J] = '#';
        }

        return (map, new Position(0, 0), new Position(70, 70));
    }

    private long[][] InitCosts(int length, Position start)
    {
        long[][] costs = new long[length][];
        for (int i = 0; i < length; ++i)
        {
            costs[i] = new long[length];
            for (int j = 0; j < length; ++j)
            {
                if (i == start.I && j == start.J)
                {
                    costs[i][j] = 0;
                }
                else
                {
                    costs[i][j] = long.MaxValue;
                }
            }
        }
        return costs;
    }

    private void GetCost(char[][] map, long[][] costs, Position current, long currentCost, Position end, Direction direction, int maxSize)
    {
        if (current == end)
        {
            return;
        }

        Position? straight = GetNext(current, direction, maxSize);
        if (straight != null && map[straight.I][straight.J] != '#')
        {
            if (currentCost + 1 < costs[straight.I][straight.J])
            {
                costs[straight.I][straight.J] = currentCost + 1;

                GetCost(map, costs, straight, currentCost + 1, end, direction, maxSize);
            }
        }

        Direction clockwiseDirection = RotateClockwise(direction);
        Position? clockwise = GetNext(current, clockwiseDirection, maxSize);
        if (clockwise != null && map[clockwise.I][clockwise.J] != '#')
        {
            if (currentCost + 1 < costs[clockwise.I][clockwise.J])
            {
                costs[clockwise.I][clockwise.J] = currentCost + 1;

                GetCost(map, costs, clockwise, currentCost + 1, end, clockwiseDirection, maxSize);
            }
        }

        Direction counterDirection = RotateCounterClockwise(direction);
        Position? counter = GetNext(current, counterDirection, maxSize);
        if (counter != null && map[counter.I][counter.J] != '#')
        {
            if (currentCost + 1 < costs[counter.I][counter.J])
            {
                costs[counter.I][counter.J] = currentCost + 1;

                GetCost(map, costs, counter, currentCost + 1, end, counterDirection, maxSize);
            }
        }
    }

    private Direction RotateClockwise(Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
                return Direction.South;
            case Direction.South:
                return Direction.West;
            case Direction.West:
                return Direction.North;
            case Direction.North:
                return Direction.East;
            default:
                throw new NotImplementedException();
        }
    }

    private Direction RotateCounterClockwise(Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
                return Direction.North;
            case Direction.South:
                return Direction.East;
            case Direction.West:
                return Direction.South;
            case Direction.North:
                return Direction.West;
            default:
                throw new NotImplementedException();
        }
    }

    private Position? GetNext(Position from, Direction direction, int maxSize)
    {
        switch (direction)
        {
            case Direction.North:
                return from.I > 0 ? new Position(from.I - 1, from.J) : null;
            case Direction.South:
                return from.I < maxSize - 1 ? new Position(from.I + 1, from.J) : null;
            case Direction.East:
                return from.J < maxSize - 1 ? new Position(from.I, from.J + 1) : null;
            case Direction.West:
                return from.J > 0 ? new Position(from.I, from.J - 1) : null;
            default:
                throw new NotImplementedException();
        }
    }
    private record Position(int I, int J);

    private enum Direction
    {
        East,
        West,
        North,
        South
    }
}
