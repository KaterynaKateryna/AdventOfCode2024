using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly List<ClawMachine> _clawMachines = new List<ClawMachine>();

    public Day13() 
    {
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
                int x = int.Parse(match.Groups[1].Value);
                int y = int.Parse(match.Groups[2].Value);
                prize = new Prize(x, y);

                _clawMachines.Add(new ClawMachine(a!, b!, prize));
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int price = 0;
        foreach (var clawMachine in _clawMachines)
        {
            (int? aMoves, int? bMoves) = GetSolution(clawMachine);
            if (aMoves.HasValue && bMoves.HasValue)
            {
                price += GetPrice(aMoves.Value, bMoves.Value);
            }
        }

        return new(price.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private (int? aMoves, int? bMoves) GetSolution(ClawMachine c)
    {
         bool bSolutionExists = (c.ButtonA.Y * c.Prize.X - c.Prize.Y * c.ButtonA.X) 
            % (c.ButtonA.Y * c.ButtonB.X - c.ButtonA.X * c.ButtonB.Y) == 0;

        if (!bSolutionExists)
        {
            return (null, null);
        }

        int bMoves = (c.ButtonA.Y * c.Prize.X - c.Prize.Y * c.ButtonA.X)
            / (c.ButtonA.Y * c.ButtonB.X - c.ButtonA.X * c.ButtonB.Y);

        bool aSolutionExists = (c.Prize.Y - c.ButtonB.Y * bMoves) % c.ButtonA.Y == 0;

        if (!aSolutionExists)
        {
            return (null, null);
        }

        int aMoves = (c.Prize.Y - c.ButtonB.Y * bMoves) / c.ButtonA.Y;

        return (aMoves, bMoves);
    }

    private int GetPrice(int aMoves, int bMoves)
    { 
        return aMoves * 3 + bMoves;
    }

    private record Button(int X, int Y);

    private record Prize(int X, int Y);

    private record ClawMachine(Button ButtonA, Button ButtonB, Prize Prize);
}
