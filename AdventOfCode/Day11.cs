namespace AdventOfCode;

public class Day11 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        List<long> stones = GetInput();

        for (int i = 0; i < 25; ++i)
        {
            stones = Blink(stones);
        }

        return new(stones.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<long> stones = GetInput();

        Dictionary<long,Dictionary<int, long>> cache = new Dictionary<long, Dictionary<int, long>>();

        long result = BlinkRecursive(stones, 75, cache);

        return new(result.ToString());
    }

    private List<long> Blink(List<long> stones)
    {
        List<long> result = new List<long>();
        for (int i = 0; i < stones.Count; i++)
        {
            (long a, long? b) = Blink(stones[i]);
            result.Add(a);
            if (b.HasValue)
            {
                result.Add(b.Value);
            }
        }
        return result;
    }

    private (long a, long? b) Blink(long stone)
    {
        if (stone == 0)
        {
            return (1L, null);
        }
        else if (stone.ToString().Length % 2 == 0)
        {
            string stoneNumberString = stone.ToString();
            long one = long.Parse(stoneNumberString.Substring(0, stoneNumberString.Length / 2));
            long two = long.Parse(stoneNumberString.Substring(stoneNumberString.Length / 2));

            return (one, two);
        }
        else
        {
            return (stone * 2024L, null);
        }
    }

    private List<long> GetInput()
    {
        return File.ReadAllText(InputFilePath)
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
    }

    #region recursive

    private long BlinkRecursive(List<long> stones, int times, Dictionary<long, Dictionary<int, long>> cache)
    {
        long result = 0;
        for (int i = 0; i < stones.Count; i++)
        {
            result += BlinkRecursive(stones[i], times, cache);
        }
        return result;
    }

    private long BlinkRecursive(long stone, int times, Dictionary<long, Dictionary<int, long>> cache)
    {
        if (cache.TryGetValue(stone, out var hits))
        {
            if (hits.TryGetValue(times, out long cachedResult))
            { 
                return cachedResult;
            }
        }

        (long a, long? b) = Blink(stone);

        if (times == 1)
        {
            return b.HasValue ? 2 : 1;
        }

        long result = BlinkRecursive(a, times - 1, cache);
        if (b.HasValue)
        {
            result += BlinkRecursive(b.Value, times - 1, cache);
        }

        cache.TryAdd(stone, new Dictionary<int, long>());
        cache[stone][times] = result;

        return result;
    }

    #endregion
}