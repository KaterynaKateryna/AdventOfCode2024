namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly List<Equation> _equations;

    public Day07()
    {
        _equations = File.ReadAllLines(InputFilePath).Select(Equation.Parse).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        return new(_equations.Where(IsSolvable).Sum(x => x.TestValue).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private bool IsSolvable(Equation equation)
    {
        return IsSolvable(equation, equation.Numbers[0], 1);
    }

    private bool IsSolvable(Equation equation, long result, int operandIndex)
    {
        if (operandIndex == equation.Numbers.Count)
        { 
            return result == equation.TestValue;
        }

        long resultAdd = result + equation.Numbers[operandIndex];
        bool a = IsSolvable(equation, resultAdd, operandIndex + 1);

        long resultMultiply = result * equation.Numbers[operandIndex];
        bool b = IsSolvable(equation, resultMultiply, operandIndex + 1);

        return a || b;
    }

    private record Equation(long TestValue, List<int> Numbers)
    {
        public static Equation Parse(string input)
        {
            string[] parts = input.Split(':', StringSplitOptions.RemoveEmptyEntries);
            long testValue = long.Parse(parts[0]);
            string[] numbersStr = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            List<int> numbers = numbersStr.Select(int.Parse).ToList();
            return new Equation(testValue, numbers);
        }
    }

    private enum Operator
    { 
        Add,
        Multiply
    }
}
