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
        return new(_equations.Where(x => IsSolvable(x, useConcatenation: false)).Sum(x => x.TestValue).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(_equations.Where(x => IsSolvable(x, useConcatenation: true)).Sum(x => x.TestValue).ToString());
    }

    private bool IsSolvable(Equation equation, bool useConcatenation)
    {
        return IsSolvable(equation, equation.Numbers[0], 1, useConcatenation);
    }

    private bool IsSolvable(Equation equation, long result, int operandIndex, bool useConcatenation)
    {
        if (operandIndex == equation.Numbers.Count)
        { 
            return result == equation.TestValue;
        }

        if (result > equation.TestValue)
        { 
            return false;
        }

        long resultAdd = result + equation.Numbers[operandIndex];
        bool a = IsSolvable(equation, resultAdd, operandIndex + 1, useConcatenation);

        long resultMultiply = result * equation.Numbers[operandIndex];
        bool b = IsSolvable(equation, resultMultiply, operandIndex + 1, useConcatenation);

        if (useConcatenation)
        {
            long resultConcatenate = long.Parse(result.ToString() + equation.Numbers[operandIndex].ToString());
            bool c = IsSolvable(equation, resultConcatenate, operandIndex + 1, useConcatenation);

            return a || b || c;
        }

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
