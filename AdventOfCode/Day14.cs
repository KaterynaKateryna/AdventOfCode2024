
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day14 : BaseDay
{
    List<Robot> _robots = new List<Robot>();

    public Day14()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        _robots = lines.Select(Robot.Parse).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        int width = 101;
        int height = 103;
        for (int i = 0; i < 100; i++)
        {
            foreach (Robot robot in _robots)
            {
                robot.Move(width, height);
            }
        }

        int quadrant1 = GetRobotsCount(0, width / 2 - 1, 0, height / 2 - 1);
        int quadrant2 = GetRobotsCount(width / 2 + 1, width - 1, 0, height / 2 - 1);
        int quadrant3 = GetRobotsCount(0, width / 2 - 1, height / 2 + 1, height - 1);
        int quadrant4 = GetRobotsCount(width / 2 + 1, width - 1, height / 2 + 1, height - 1);

        int result = quadrant1 * quadrant2 * quadrant3 * quadrant4;

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private int GetRobotsCount(int xStart, int xEnd, int yStart, int yEnd)
    { 
        return _robots.Count(r => r.XPosition >= xStart && r.XPosition <= xEnd
            && r.YPosition >= yStart && r.YPosition <= yEnd);
    }

    public class Robot(int XPosition, int YPosition, int XVelocity, int YVelocity)
    {
        public static Robot Parse(string input)
        {
            Match match = Regex.Match(input, "p=([0-9]+),([0-9]+) v=([\\-0-9]+),([\\-0-9]+)");
            return new Robot(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value)
            );
        }

        public int XPosition { get; private set; } = XPosition;
        public int YPosition { get; private set; } = YPosition;
        public int XVelocity { get; private set; } = XVelocity;
        public int YVelocity { get; private set; } = YVelocity;

        public void Move(int fieldWidth, int fieldHeight)
        {
            int newXPosition = XPosition + XVelocity;
            if (newXPosition < 0)
            {
                newXPosition = fieldWidth + newXPosition;
            }
            else if (newXPosition >= fieldWidth)
            {
                newXPosition -= fieldWidth;
            }

            int newYPosition = YPosition + YVelocity;
            if (newYPosition < 0)
            {
                newYPosition = fieldHeight + newYPosition;
            }
            else if (newYPosition >= fieldHeight)
            {
                newYPosition -= fieldHeight;
            }

            XPosition = newXPosition;
            YPosition = newYPosition;
        }
    }
}
