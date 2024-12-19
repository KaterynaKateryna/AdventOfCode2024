namespace AdventOfCode;

public class Day19 : BaseDay
{
    string[] _towels;
    string[] _patterns;

    public Day19()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        _towels = lines[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
        _patterns = new string[lines.Length - 2];
        for (int i = 2; i < lines.Length; ++i)
        { 
            _patterns[i-2] = lines[i].Trim();
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int result = _patterns.Count(p => IsPatternPossible(p, 0));

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("test");
    }

    private bool IsPatternPossible(string pattern, int position)
    {
        if (position == pattern.Length)
        {
            return true;
        }

        foreach (string towel in _towels)
        {
            if (towel.Length <= pattern.Length - position)
            {
                bool matches = true;
                for (int i = 0; i < towel.Length; ++i)
                {
                    if (pattern[position + i] != towel[i])
                    {
                        matches = false;
                        break;
                    }
                }
                if (matches && IsPatternPossible(pattern, position + towel.Length))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
