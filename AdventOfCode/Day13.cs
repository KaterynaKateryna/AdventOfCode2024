using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private List<ClawMachine> Init(long offset)
    {
        List<ClawMachine> clawMachines = new List<ClawMachine>();

        string[] lines = File.ReadAllLines(InputFilePath);

        Button? a = null, b = null;
        Prize prize;
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            if (line.StartsWith("Button"))
            {
                Match match = Regex.Match(line, "Button (A|B): X\\+([0-9]+), Y\\+([0-9]+)");
                int x = int.Parse(match.Groups[2].Value);
                int y = int.Parse(match.Groups[3].Value);
                Button button = new Button(x, y);
                if (match.Groups[1].Value == "A")
                {
                    a = button;
                }
                else
                {
                    b = button;
                }
            }
            else if (line.StartsWith("Prize"))
            {
                Match match = Regex.Match(line, "Prize: X=([0-9]+), Y=([0-9]+)");
                long x = int.Parse(match.Groups[1].Value) + offset;
                long y = int.Parse(match.Groups[2].Value) + offset;
                prize = new Prize(x, y);

                clawMachines.Add(new ClawMachine(a!, b!, prize));
            }
        }

        return clawMachines;
    }

    public override ValueTask<string> Solve_1()
    {
        return new(Solve(0).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(Solve(10000000000000).ToString());
    }

    private long Solve(long offset)
    {
        List<ClawMachine> clawMachines = Init(offset);

        long price = 0;
        foreach (var clawMachine in clawMachines)
        {
            (long? aMoves, long? bMoves) = GetSolution(clawMachine);
            if (aMoves.HasValue && bMoves.HasValue)
            {
                price += GetPrice(aMoves.Value, bMoves.Value);
            }
        }

        return price;
    }

    private (long? aMoves, long? bMoves) GetSolution(ClawMachine c)
    {
         bool bSolutionExists = (c.ButtonA.Y * c.Prize.X - c.Prize.Y * c.ButtonA.X) 
            % (c.ButtonA.Y * c.ButtonB.X - c.ButtonA.X * c.ButtonB.Y) == 0;

        if (!bSolutionExists)
        {
            return (null, null);
        }

        long bMoves = (c.ButtonA.Y * c.Prize.X - c.Prize.Y * c.ButtonA.X)
            / (c.ButtonA.Y * c.ButtonB.X - c.ButtonA.X * c.ButtonB.Y);

        bool aSolutionExists = (c.Prize.Y - c.ButtonB.Y * bMoves) % c.ButtonA.Y == 0;

        if (!aSolutionExists)
        {
            return (null, null);
        }

        long aMoves = (c.Prize.Y - c.ButtonB.Y * bMoves) / c.ButtonA.Y;

        return (aMoves, bMoves);
    }

    private long GetPrice(long aMoves, long bMoves)
    { 
        return aMoves * 3 + bMoves;
    }

    private record Button(int X, int Y);

    private record Prize(long X, long Y);

    private record ClawMachine(Button ButtonA, Button ButtonB, Prize Prize);
}
