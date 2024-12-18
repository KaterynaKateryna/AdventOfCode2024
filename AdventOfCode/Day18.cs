namespace AdventOfCode;

public class Day18 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        (char[][] map, Position start, Position end) = Init(71);

        long[][] costs = new long[map.Length][];
        for (int i = 0; i < map.Length; ++i)
        {
            costs[i] = new long[map[i].Length];
            for (int j = 0; j < map[i].Length; ++j)
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

        GetCost(map, costs, start, 0, end, Direction.East, map.Length);

        long cost = costs[end.I][end.J];

        return new(cost.ToString());
    }

    private (char[][] map, Position start, Position end) Init(int length)
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

        string[] lines = File.ReadAllLines(InputFilePath);
        for (int i = 0; i < 1024; ++i)
        {
            string[] parts = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
            int corruptedI = int.Parse(parts[1]);
            int corruptedJ = int.Parse(parts[0]);

            map[corruptedI][corruptedJ] = '#';
        }

        return (map, new Position(0, 0), new Position(70, 70));
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
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
