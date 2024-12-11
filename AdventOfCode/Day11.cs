
namespace AdventOfCode;

public class Day11 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        List<Stone> stones = GetInput();

        for (int i = 0; i < 25; ++i)
        {
            stones = Blink(stones);
        }

        return new(stones.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<Stone> stones = GetInput();

        for (int i = 0; i < 35; ++i) // TODO make work for 75
        {
            stones = Blink(stones);
        }

        return new(stones.Count.ToString());
    }

    private List<Stone> Blink(List<Stone> stones)
    {
        List<Stone> result = new List<Stone>();
        for (int i = 0; i < stones.Count; i++)
        {
            result.AddRange(Blink(stones[i]));
        }
        return result;
    }

    private List<Stone> Blink(Stone stone)
    {
        if (stone.Value == 0)
        {
            return new List<Stone> { new Stone(1) };
        }
        else if (stone.Value.ToString().Length % 2 == 0)
        {
            string stoneNumberString = stone.Value.ToString();
            long one = long.Parse(stoneNumberString.Substring(0, stoneNumberString.Length / 2));
            long two = long.Parse(stoneNumberString.Substring(stoneNumberString.Length / 2));

            return new List<Stone> { new Stone(one), new Stone(two) };
        }
        else
        {
            return new List<Stone> { new Stone(stone.Value * 2024) };
        }
    }

    private List<Stone> GetInput()
    { 
        return File.ReadAllText(InputFilePath)
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new Stone(long.Parse(x)))
            .ToList();
    }

    private class Stone(long Value)
    {
        public long Value { get; set; } = Value;
    }
}
