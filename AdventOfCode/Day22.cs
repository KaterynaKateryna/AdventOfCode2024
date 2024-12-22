
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        long[] numbers = File.ReadAllLines(InputFilePath).Select(long.Parse).ToArray();

        Dictionary<(int, int, int, int), int> allBananas = new Dictionary<(int, int, int, int), int>();
        foreach (long n in numbers)
        {
            Dictionary<(int, int, int, int), int> bananas = new Dictionary<(int, int, int, int), int>();

            (int, int)[] changes = GetPricesWithChanges(n, 2000);

            for (int i = 0; i < changes.Length - 3; ++i)
            {
                bananas.TryAdd((changes[i].Item2, changes[i + 1].Item2, changes[i + 2].Item2, changes[i + 3].Item2), changes[i + 3].Item1);
            }

            foreach(var kv in bananas)
            {
                if (!allBananas.TryAdd(kv.Key, kv.Value))
                {
                    allBananas[kv.Key] += kv.Value;
                }
            }
        }

        int max = allBananas.Max(b => b.Value);

        return new(max.ToString());
    }

    private long GetIteration(long number, int iterations)
    {
        for (int i = 0; i < iterations; ++i)
        {
            number = GetNext(number);
        }

        return number;
    }

    private (int, int)[] GetPricesWithChanges(long number, int iterations)
    {
        (int, int)[] priceChanges = new (int, int)[iterations];
        int prevDigit = (int)(number % 10);

        for (int i = 0; i < iterations; ++i)
        {
            number = GetNext(number);
            int currDigit = (int)(number % 10);
            priceChanges[i] = (currDigit, currDigit - prevDigit);
            prevDigit = currDigit;
        }

        return priceChanges;    
    }

    private long GetNext(long number)
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

        return number;
    }
}
