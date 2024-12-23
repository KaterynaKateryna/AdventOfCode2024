namespace AdventOfCode;

public class Day23 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        HashSet<(string, string, string)> groups = new HashSet<(string, string, string)>();
        Dictionary<string, List<string>> dictionary = ToDictionary(lines);

        foreach (var kv in dictionary)
        {
            string computer1 = kv.Key;
            if (kv.Value.Count < 2)
            {
                continue;
            }
            for (int i = 0; i < kv.Value.Count - 1; ++i)
            {
                for (int j = i + 1; j < kv.Value.Count; ++j)
                {
                    if (dictionary[kv.Value[i]].Contains(kv.Value[j]))
                    {
                        var group = new List<string> { computer1, kv.Value[i], kv.Value[j] };
                        group = group.OrderBy(x => x).ToList();
                        groups.Add((group[0], group[1], group[2]));
                    }
                }
            }
        }

        int res = groups.Count(g => g.Item1.StartsWith("t") || g.Item2.StartsWith("t") || g.Item3.StartsWith("t"));

        return new (res.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new ("");
    }

    private Dictionary<string, List<string>> ToDictionary(string[] lines)
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

        foreach (string line in lines)
        {
            string[] computers = line.Split("-", StringSplitOptions.RemoveEmptyEntries);

            if (!result.TryAdd(computers[0], new List<string>() { computers[1] }))
            {
                result[computers[0]].Add(computers[1]);
            }

            if (!result.TryAdd(computers[1], new List<string>() { computers[0] }))
            {
                result[computers[1]].Add(computers[0]);
            }
        }

        return result;
    }
}
