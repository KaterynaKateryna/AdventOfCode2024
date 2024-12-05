namespace AdventOfCode;

internal class Day05 : BaseDay
{
    private readonly Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>();
    private readonly Dictionary<int, List<int>> rulesReversed = new Dictionary<int, List<int>>();
    private readonly List<List<int>> updates = new List<List<int>>();

    public Day05()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        bool isRule = true;
        foreach (string line in lines)
        {
            if (line == string.Empty)
            {
                isRule = false;
                continue;
            }
            if (isRule)
            {
                string[] ruleParts = line.Split('|');
                int left = int.Parse(ruleParts[0]);
                int right = int.Parse(ruleParts[1]);

                if (!rules.TryAdd(left, new List<int> { right }))
                {
                    rules[left].Add(right);
                }

                if (!rulesReversed.TryAdd(right, new List<int> { left }))
                {
                    rulesReversed[right].Add(left);
                }
            }
            else 
            {
                updates.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;
        foreach (var update in updates)
        {
            if (IsCorrectOrder(update))
            {
                int middle = update.Skip(update.Count / 2).First();
                result += middle;
            }
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private bool IsCorrectOrder(List<int> update)
    {
        for(int i = 0; i < update.Count; ++i)
        {
            if (rules.TryGetValue(update[i], out List<int> haveToBeToTheRight))
            {
                var toTheLeft = update.Take(i).ToList();
                if (toTheLeft.Intersect(haveToBeToTheRight).Any())
                {
                    return false;
                }
            }

            if (rulesReversed.TryGetValue(update[i], out List<int> haveToBeToTheLeft))
            {
                var toTheRight = update.Skip(i+1).ToList();
                if (toTheRight.Intersect(haveToBeToTheLeft).Any())
                {
                    return false;
                }
            }
        }

        return true;
    }
}
