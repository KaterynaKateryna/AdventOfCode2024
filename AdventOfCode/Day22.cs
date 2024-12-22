
namespace AdventOfCode;

public class Day22 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        long[] numbers = File.ReadAllLines(InputFilePath).Select(long.Parse).ToArray();

        long result = numbers.Sum(n => GetIteration(n, 2000));

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private long GetIteration(long number, int iterations)
    {
        for (int i = 0; i < iterations; ++i)
        {
            // 1
            number = (number << 6) ^ number;
            number = number % 16777216;

            // 2
            number = (number / 32) ^ number;
            number = number % 16777216;

            // 3
            number = (number << 11) ^ number;
            number = number % 16777216;
        }

        return number;
    }
}
