
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day14 : BaseDay
{
    public override ValueTask<string> Solve_1()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        List<Robot> robots = lines.Select(Robot.Parse).ToList();

        int width = 101;
        int height = 103;
        for (int i = 0; i < 100; i++)
        {
            foreach (Robot robot in robots)
            {
                robot.Move(width, height);
            }
        }

        int quadrant1 = GetRobotsCount(0, width / 2 - 1, 0, height / 2 - 1, robots);
        int quadrant2 = GetRobotsCount(width / 2 + 1, width - 1, 0, height / 2 - 1, robots);
        int quadrant3 = GetRobotsCount(0, width / 2 - 1, height / 2 + 1, height - 1, robots);
        int quadrant4 = GetRobotsCount(width / 2 + 1, width - 1, height / 2 + 1, height - 1, robots);

        int result = quadrant1 * quadrant2 * quadrant3 * quadrant4;

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        List<Robot> robots = lines.Select(Robot.Parse).ToList();

        int width = 101;
        int height = 103;

        int move = 0;

        while (true)
        {
            if (MayBeTree(robots, width, height))
            {
                DisplayRobots(robots, width, height);
                Console.WriteLine("Seconds elapsed: " + move);
                Console.WriteLine("Is it tree? y/n");
                var key = Console.ReadKey();
                if (key.KeyChar == 'y')
                {
                    break;
                }
            }

            foreach (Robot robot in robots)
            {
                robot.Move(width, height);
            }

            move++;
        }

        return new(move.ToString());
    }

    private int GetRobotsCount(int xStart, int xEnd, int yStart, int yEnd, List<Robot> robots)
    { 
        return robots.Count(r => r.XPosition >= xStart && r.XPosition <= xEnd
            && r.YPosition >= yStart && r.YPosition <= yEnd);
    }

    private bool MayBeTree(List<Robot> robots, int width, int height)
    {
        foreach (Robot robot in robots)
        {
            bool a = robots.Any(r => r.XPosition == robot.XPosition + 1 && r.YPosition == robot.YPosition);
            bool b = robots.Any(r => r.XPosition == robot.XPosition && r.YPosition == robot.YPosition + 1);
            bool c = robots.Any(r => r.XPosition == robot.XPosition + 1 && r.YPosition == robot.YPosition + 1);
            bool d = robots.Any(r => r.XPosition == robot.XPosition && r.YPosition == robot.YPosition + 2);
            bool e = robots.Any(r => r.XPosition == robot.XPosition + 1 && r.YPosition == robot.YPosition + 2);

            if (a && b && c && d && e)
            { 
                return true;
            }
        }

        return false;
    }

    private void DisplayRobots(List<Robot> robots, int width, int height)
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        for (int i = 0; i < height; i++)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < width; j++)
            {
                char ch = robots.Any(r => r.XPosition == j && r.YPosition == i) ? '*' : '.';
                sb.Append(ch);
            }
            Console.WriteLine(sb.ToString());
        }
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
