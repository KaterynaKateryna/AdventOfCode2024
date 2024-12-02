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
        int result = reports.Count(IsSafe);
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var reports = Init();
        int result = reports.Count(IsSafeWithDampener);
        return new(result.ToString());
    }

    private bool IsSafeWithDampener(List<int> report)
    {
        for (int i = 0; i < report.Count; ++i)
        {
            var attempt = report.Take(i).Concat(report.Skip(i+1)).ToList();
            if (IsSafe(attempt))
            { 
                return true;
            }
        }
        return false;
    }

    private bool IsSafe(List<int> report)
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

        for (int i = 1; i < report.Count; ++i)
        {
            int diff = report[i] - report[i - 1];
            int absDiff = Math.Abs(diff);
            if (absDiff == 0 || absDiff > 3 || (diff > 0 && !isAscending) || (diff < 0 && isAscending))
            {
                return false;
            }
        }

        return true;
    }
}
