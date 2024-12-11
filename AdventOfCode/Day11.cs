
namespace AdventOfCode;

public class Day11 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        List<Stone> stones = GetInput();

        for (int i = 0; i < 25; ++i)
        {
            Blink(stones);
        }

        return new(stones.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private void Blink(List<Stone> stones)
    {
        for (int i = 0; i < stones.Count; i++)
        {
            if (stones[i].Value == 0)
            {
                stones[i].Value++;
            }
            else if (stones[i].Value.ToString().Length % 2 == 0)
            {
                string stoneNumberString = stones[i].Value.ToString();
                long one = long.Parse(stoneNumberString.Substring(0, stoneNumberString.Length / 2));
                long two = long.Parse(stoneNumberString.Substring(stoneNumberString.Length / 2));

                stones[i].Value = one;
                stones.Insert(i + 1, new Stone(two));
                i++;
            }
            else
            {
                stones[i].Value *= 2024;
            }
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
