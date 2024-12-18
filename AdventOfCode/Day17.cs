namespace AdventOfCode;

public class Day17 : BaseDay
{
    private (long A, long B, long C, int[] instructions) Init()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        long A = long.Parse(lines[0].Replace("Register A: ", ""));
        long B = long.Parse(lines[1].Replace("Register B: ", ""));
        long C = long.Parse(lines[2].Replace("Register C: ", ""));

        int[] instructions = lines[4].Replace("Program: ", "")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        return (A, B, C, instructions);
    }

    public override ValueTask<string> Solve_1()
    {
        (long A, long B, long C, int[] instructions) = Init();

        int index = 0;
        List<long> output = new List<long>();

        while (index < instructions.Length - 1)
        {
            switch (instructions[index])
            {
                case 0:
                    A = A / (long)Math.Pow(2, (GetComboOperand(instructions[index + 1], A, B, C)));
                    index += 2;
                    continue;
                case 1:
                    B = B ^ instructions[index + 1];
                    index += 2;
                    continue;
                case 2:
                    B = GetComboOperand(instructions[index + 1], A, B, C) % 8;
                    index += 2;
                    continue;
                case 3:
                    if (A == 0)
                    {
                        index += 2;
                        continue;
                    }
                    else
                    {
                        index = instructions[index + 1];
                        continue;
                    }
                case 4:
                    B = B ^ C;
                    index += 2;
                    continue;
                case 5:
                    output.Add(GetComboOperand(instructions[index + 1], A, B, C) % 8);
                    index += 2;
                    continue;
                case 6:
                    B = A / (long)Math.Pow(2, (GetComboOperand(instructions[index + 1], A, B, C)));
                    index += 2;
                    continue;
                case 7:
                    C = A / (long)Math.Pow(2, (GetComboOperand(instructions[index + 1], A, B, C)));
                    index += 2;
                    continue;
            }
        }

        return new(string.Join(",", output));
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private long GetComboOperand(int operand, long A, long B, long C)
    {
        switch (operand)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return operand;
            case 4:
                return A;
            case 5:
                return B;
            case 6:
                return C;
            default:
                throw new NotImplementedException();
        }
    }
}
