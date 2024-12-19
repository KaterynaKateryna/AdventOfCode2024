namespace AdventOfCode;

public class Day19 : BaseDay
{
    string[] _towels;
    string[] _patterns;

    Dictionary<char, List<string>> _towelsByFirstLetter = new Dictionary<char, List<string>>();

    public Day19()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        _towels = lines[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
        _patterns = new string[lines.Length - 2];
        for (int i = 2; i < lines.Length; ++i)
        { 
            _patterns[i-2] = lines[i].Trim();
        }

        foreach (string towel in _towels) 
        {
            if (!_towelsByFirstLetter.TryAdd(towel[0], new List<string> { towel }))
            {
                _towelsByFirstLetter[towel[0]].Add(towel);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int result = _patterns.Count(p => IsPatternPossible(p, 0));

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int result = _patterns.Sum(p => CountPatterns(p, 0));

        //int result = CountPatterns(_patterns[0], 0);

        return new(result.ToString());
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

    private int CountPatterns(string pattern, int position)
    {
        if (position == pattern.Length)
        {
            return 1;
        }

        int res = 0;
        if (_towelsByFirstLetter.TryGetValue(pattern[position], out var towelsToCheck))
        {
            foreach (string towel in towelsToCheck)
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
                    if (matches)
                    {
                        res += CountPatterns(pattern, position + towel.Length);
                    }
                }
            }
        }

        return res;
    }
}
