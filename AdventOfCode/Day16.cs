
namespace AdventOfCode;

internal class Day16 : BaseDay
{
    private (char[][] map, Position start, Position end) Init()
    {
        char[][] map = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();

        Position start = default!;
        Position end = default!;

        for (int i = 0; i < map.Length; ++i)
        {
            for (int j = 0; j < map[i].Length; ++j)
            {
                if (map[i][j] == 'S')
                {
                    start = new Position(i, j);
                }
                if (map[i][j] == 'E')
                {
                    end = new Position(i, j);
                }
            }
        }

        return (map, start, end);
    }

    public override ValueTask<string> Solve_1()
    {
        (char[][] map, Position start, Position end) = Init();

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

        GetCost(map, costs, start, 0, end, Direction.East);

        long cost = costs[end.I][end.J];

        return new(cost.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        (char[][] map, Position start, Position end) = Init();

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

        GetCost(map, costs, start, 0, end, Direction.East);
        DisplayCosts(costs);
        long finalCost = costs[end.I][end.J];

        List<Position> paths = GetPath(map, costs, start, 0, end, Direction.East, new List<Position>() { start }, finalCost);
        long path = paths.Distinct().Count();

        return new(path.ToString());
    }

    private void GetCost(char[][] map, long[][] costs, Position current, long currentCost, Position end, Direction direction)
    {
        if (current == end)
        {
            return;
        }

        Position straight = GetNext(current, direction);
        if (map[straight.I][straight.J] != '#')
        {
            if (currentCost + 1 < costs[straight.I][straight.J])
            {
                costs[straight.I][straight.J] = currentCost + 1;

                GetCost(map, costs, straight, currentCost + 1, end, direction);
            }
        }

        Direction clockwiseDirection = RotateClockwise(direction);
        Position clockwise = GetNext(current, clockwiseDirection);
        if (map[clockwise.I][clockwise.J] != '#')
        {
            if (currentCost + 1001 < costs[clockwise.I][clockwise.J])
            {
                costs[clockwise.I][clockwise.J] = currentCost + 1001;

                GetCost(map, costs, clockwise, currentCost + 1001, end, clockwiseDirection);
            }
        }

        Direction counterDirection = RotateCounterClockwise(direction);
        Position counter = GetNext(current, counterDirection);
        if (map[counter.I][counter.J] != '#')
        {
            if (currentCost + 1001 < costs[counter.I][counter.J])
            {
                costs[counter.I][counter.J] = currentCost + 1001;

                GetCost(map, costs, counter, currentCost + 1001, end, counterDirection);
            }
        }
    }

    private List<Position> GetPath(
        char[][] map, 
        long[][] costs,
        Position current,
        long currentCost,
        Position end, 
        Direction direction, 
        List<Position> path,
        long finalCost
    )
    {
        if (current == end)
        {
            return path;
        }

        List<Position> positions = new List<Position>();

        Position straight = GetNext(current, direction);
        if (map[straight.I][straight.J] != '#')
        {
            if (currentCost + 1 <= finalCost)
            {
                path.Add(straight);
                positions.AddRange(GetPath(map, costs, straight, currentCost + 1, end, direction, path, finalCost));
                path.RemoveAt(path.Count - 1);
            }
        }

        Direction clockwiseDirection = RotateClockwise(direction);
        Position clockwise = GetNext(current, clockwiseDirection);
        if (map[clockwise.I][clockwise.J] != '#')
        {
            if (currentCost + 1001 <= finalCost)
            {
                path.Add(clockwise);
                positions.AddRange(GetPath(map, costs, clockwise, currentCost + 1001, end, clockwiseDirection, path, finalCost));
                path.RemoveAt(path.Count - 1);
            }
        }

        Direction counterDirection = RotateCounterClockwise(direction);
        Position counter = GetNext(current, counterDirection);
        if (map[counter.I][counter.J] != '#')
        {
            if (currentCost + 1001 <= finalCost)
            {
                path.Add(counter);
                positions.AddRange(GetPath(map, costs, counter, currentCost + 1001, end, counterDirection, path, finalCost));
                path.RemoveAt(path.Count - 1);
            }
        }

        return positions;
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

    private Position GetNext(Position from, Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return new Position(from.I - 1, from.J);
            case Direction.South:
                return new Position(from.I + 1, from.J);
            case Direction.East:
                return new Position(from.I, from.J + 1);
            case Direction.West:
                return new Position(from.I, from.J - 1);
            default:
                throw new NotImplementedException();
        }
    }

    private void DisplayCosts(long[][] costs)
    {
        for (int i = 0; i < costs.Length; ++i)
        {
            for (int j = 0; j < costs[i].Length; ++j)
            {
                if (costs[i][j] == long.MaxValue)
                {
                    Console.Write("    max");
                }
                else
                {
                    Console.Write(String.Format("{0, 7}", costs[i][j]));
                }
            }
            Console.WriteLine();
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
