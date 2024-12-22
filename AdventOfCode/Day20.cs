namespace AdventOfCode;

public class Day20 : BaseDay
{
    private (char[][] map, int[][] distances, Position start, Position end) Init()
    {
        char[][] map = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();

        Position start = default!;
        Position end = default!;

        int[][] distances = new int[map.Length][];

        for (int i = 0; i < map.Length; ++i)
        {
            distances[i] = new int [map[i].Length];
            for (int j = 0; j < map[i].Length; ++j)
            {
                distances[i][j] = int.MaxValue;
                if (map[i][j] == 'S')
                {
                    start = new Position(i, j);
                    distances[i][j] = 0;
                }
                if (map[i][j] == 'E')
                {
                    end = new Position(i, j);
                }
            }
        }

        return (map, distances, start, end);
    }

    public override ValueTask<string> Solve_1()
    {
        (char[][] map, int[][] distances, Position start, Position end) = Init();

        CalculateDistances(map, distances, start, 0, end, Direction.South);

        int cheats = CalculateCheats(map, distances, start, end, Direction.South);

        return new(cheats.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private void CalculateDistances(
        char[][] map, 
        int[][] distances, 
        Position current, 
        int currentCost, 
        Position end, 
        Direction direction
    )
    {
        if (current == end)
        {
            return;
        }

        (Position next, direction) = GetNext(map, current, direction, map.Length, map[0].Length);
        distances[next.I][next.J] = currentCost + 1;
        CalculateDistances(map, distances, next, currentCost + 1, end, direction);
    }

    private int CalculateCheats(
        char[][] map,
        int[][] distances,
        Position start,
        Position end,
        Direction direction
    )
    {
        int cheats = 0;

        Position current = start;
        while (current != end)
        {
            (Position next, Direction newDirection) = GetNext(map, current, direction, map.Length, map[0].Length);

            for (int i = 0; i < 4; ++i)
            {
                Direction d = (Direction)i;

                if (d == newDirection)
                {
                    continue;
                }

                Position? nxtCheat = GetNextInDirection(current, d, map.Length, map[0].Length);
                if (nxtCheat == null || map[nxtCheat.I][nxtCheat.J] != '#')
                {
                    continue;
                }
                Position? nxtCheat2 = GetNextInDirection(nxtCheat, d, map.Length, map[0].Length);
                if (nxtCheat2 == null || map[nxtCheat2.I][nxtCheat2.J] == '#')
                {
                    continue;
                }
                int originalDistance = distances[nxtCheat2.I][nxtCheat2.J];
                int cheatDistance = distances[current.I][current.J] + 2;
                if (originalDistance - cheatDistance >= 100)
                {
                    cheats++;
                }
            }

            current = next;
            direction = newDirection;
        }

        return cheats;
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

    private (Position position, Direction direction) GetNext(char[][] map, Position from, Direction direction, int maxI, int maxJ)
    {
        Position? straight = GetNextInDirection(from, direction, maxI, maxJ);
        if (straight != null && map[straight.I][straight.J] != '#')
        {
            return (straight, direction);
        }
        Direction clockwiseDirection = RotateClockwise(direction);
        Position? clockwise = GetNextInDirection(from, clockwiseDirection, maxI, maxJ);
        if (clockwise != null && map[clockwise.I][clockwise.J] != '#')
        {
            return (clockwise, clockwiseDirection);
        }
        Direction counterDirection = RotateCounterClockwise(direction);
        Position? counter = GetNextInDirection(from, counterDirection, maxI, maxJ);
        if (counter != null && map[counter.I][counter.J] != '#')
        {
            return (counter, counterDirection);
        }

        throw new NotImplementedException();
    }

    private Position? GetNextInDirection(Position from, Direction direction, int maxI, int maxJ)
    {
        switch (direction)
        {
            case Direction.North:
                return from.I > 0 ? new Position(from.I - 1, from.J) : null;
            case Direction.South:
                return from.I < maxI - 1 ? new Position(from.I + 1, from.J) : null;
            case Direction.East:
                return from.J < maxJ - 1 ? new Position(from.I, from.J + 1) : null;
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
