namespace AdventOfCode;

public class Day02 : BaseDay
{
    public virtual string[] GetLines()
    { 
        return File.ReadAllLines(InputFilePath);
    }

    private List<List<int>> Init()
    {
        string[] lines = GetLines();

        List<List<int>> reports = new List<List<int>>();
        foreach (string line in lines)
        {
            List<int> levels = line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            reports.Add(levels);
        }

        return reports;
    }

    public override ValueTask<string> Solve_1()
    {
        var reports = Init();
        int result = reports.Where(x => IsSafe(x, useDampener: false)).Count();
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var reports = Init();
        int result = reports.Where(x => 
            IsSafe(x.Take(x.Count).ToList(), useDampener: true) || IsSafe(x.Skip(1).ToList(), useDampener: false)
        ).Count();
        return new(result.ToString());
    }

    private bool IsSafe(List<int> report, bool useDampener)
    {
        if (report.Count < 2)
        { 
            return false;
        }
        if (report[1] - report[0] == 0)
        {
            return false;
        }

        bool isAscending = report[1] - report[0] > 0;
        bool dampenerUsed = false;

        for (int i = 1; i < report.Count; ++i)
        {
            int diff = report[i] - report[i - 1];
            int absDiff = Math.Abs(diff);
            if (absDiff == 0 || absDiff > 3 || (diff > 0 && !isAscending) || (diff < 0 && isAscending))
            {
                if (useDampener && !dampenerUsed)
                {
                    report.RemoveAt(i);
                    i--;
                    dampenerUsed = true;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }
}
