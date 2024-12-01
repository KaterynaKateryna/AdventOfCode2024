namespace AdventOfCode;

public class Day01 : BaseDay
{
    private List<int> first = new List<int>();
    private List<int> second = new List<int>();

    public Day01()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            first.Add(int.Parse(parts[0]));
            second.Add(int.Parse(parts[1]));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        long result = 0;

        first = first.OrderBy(x => x).ToList();
        second = second.OrderBy(x => x).ToList();

        for (int i = 0; i < first.Count; i++)
        {
            result += Math.Abs(first[i] - second[i]);
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long result = 0;

        Dictionary<int, int> secondDictionary = new Dictionary<int, int>();
        foreach (int i in second)
        {
            if (secondDictionary.ContainsKey(i))
            {
                secondDictionary[i]++;
            }
            else
            {
                secondDictionary[i] = 1;
            }
        }

        foreach (int i in first)
        {
            if (secondDictionary.ContainsKey(i))
            {
                result += i * secondDictionary[i];
            }
        }
        return new(result.ToString());
    }
}
