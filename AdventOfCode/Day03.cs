
using System.Text.RegularExpressions;

namespace AdventOfCode;

internal class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        long result = 0;

        MatchCollection matches = Regex.Matches(_input, @"mul\([0-9]+,[0-9]+\)");
        foreach (Match match in matches)
        {
            string value = match.Captures[0].Value;
            string[] parts = value.Split(new[] { "mul(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);
            result += long.Parse(parts[0]) * long.Parse(parts[1]);
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }
}
