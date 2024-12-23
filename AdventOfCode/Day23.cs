using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day23 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        Dictionary<string, List<string>> dictionary = ToDictionary(lines);
        HashSet<string> groups = GetSetsOfThree(lines, dictionary);

        int res = groups.Count(g => g.Split(",").Any(x => x.StartsWith("t")));

        return new (res.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        Dictionary<string, List<string>> dictionary = ToDictionary(lines);
        HashSet<string> groups = GetSetsOfThree(lines, dictionary);

        while (groups.Count != 1)
        {
            groups = GetGroupsPlusOne(groups, dictionary);
            Console.WriteLine(groups.Count);
        }

        return new (groups.Single().ToString());
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

    private HashSet<string> GetSetsOfThree(string[] lines, Dictionary<string, List<string>> dictionary)
    {
        HashSet<string> groups = new HashSet<string>();

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
                        groups.Add($"{group[0]},{group[1]},{group[2]}");
                    }
                }
            }
        }
        return groups;
    }

    private HashSet<string> GetGroupsPlusOne(HashSet<string> groups, Dictionary<string, List<string>> dictionary)
    {
        HashSet<string> groupsPlusOne = new HashSet<string>();
        foreach (var group in groups)
        {
            List<string> computers = group.Split(",").ToList();
            foreach (var computer in dictionary.Keys)
            {
                if (computers.Contains(computer))
                {
                    continue;
                }
                int intersect = dictionary[computer].Intersect(computers).Count();
                if (intersect == computers.Count)
                {
                    var newGroup = computers.Concat(new List<string> { computer });
                    newGroup = newGroup.OrderBy(x => x).ToList();
                    groupsPlusOne.Add(string.Join(",", newGroup));
                }
            }
        }

        return groupsPlusOne;
    }
}
