using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    List<List<int>> reports = new List<List<int>>();

    public Day02()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        foreach (string line in lines)
        { 
            List<int> levels = line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            reports.Add(levels);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int result = reports.Where(IsSafe).Count();
        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
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
            if (absDiff == 0 || absDiff > 3)
            {
                return false;
            }
            if ((diff > 0 && !isAscending) || (diff < 0 && isAscending))
            {
                return false;
            }
        }

        return true;
    }
}
