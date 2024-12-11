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

        for (int i = 0; i < 35; ++i) // TODO make work for 75
        {
            stones = Blink(stones);
        }

        return new(stones.Count.ToString());
    }

    private List<long> Blink(List<long> stones)
    {
        List<long> result = new List<long>();
        for (int i = 0; i < stones.Count; i++)
        {
            result.AddRange(Blink(stones[i]));
        }
        return result;
    }

    private List<long> Blink(long stone)
    {
        if (stone == 0)
        {
            return new List<long> { 1L };
        }
        else if (stone.ToString().Length % 2 == 0)
        {
            string stoneNumberString = stone.ToString();
            long one = long.Parse(stoneNumberString.Substring(0, stoneNumberString.Length / 2));
            long two = long.Parse(stoneNumberString.Substring(stoneNumberString.Length / 2));

            return new List<long> { one, two };
        }
        else
        {
            return new List<long> { stone * 2024L };
        }
    }

    private List<long> GetInput()
    { 
        return File.ReadAllText(InputFilePath)
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
    }
}
