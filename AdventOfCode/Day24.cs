using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day24 : BaseDay
{
    private (Dictionary<string, bool> values, List<Rule> rules) Init()
    {
        Dictionary<string, bool> values = new Dictionary<string, bool>();
        List<Rule> rules = new List<Rule>();

        string[] lines = File.ReadAllLines(InputFilePath);
        int i = 0;
        for (; i < lines.Length; i++) 
        {
            if (string.IsNullOrEmpty(lines[i]))
            {
                break;
            }
            string[] parts = lines[i].Split(": ");
            values[parts[0]] = parts[1] == "1" ? true : false;
        }

        string pattern = "(.{3}) (AND|OR|XOR) (.{3}) -> (.{3})";
        for (i += 1; i < lines.Length; i++)
        {
            Match match = Regex.Match(lines[i], pattern);
            string inputA = match.Groups[1].Value;
            string oper = match.Groups[2].Value;
            string inputB = match.Groups[3].Value;
            string output = match.Groups[4].Value;

            rules.Add(new Rule(inputA, inputB, (Operator)Enum.Parse(typeof(Operator), oper), output));
        }

        return (values, rules);
    }

    public override ValueTask<string> Solve_1()
    {
        (Dictionary<string, bool> values, List<Rule> rules) = Init();

        Dictionary<string, Rule> rulesByOutput = ToDicitonary(rules);

        foreach (string output in rulesByOutput.Keys.Where(k => k.StartsWith("z")))
        {
            CalculateValueForOutput(output, rulesByOutput, values);
        }

        string[] outputs = rulesByOutput.Keys.Where(k => k.StartsWith("z")).OrderBy(k => k).ToArray();

        BitArray bitArray = new BitArray(outputs.Select(o => values[o]).ToArray());

        long res = GetIntFromBitArray(bitArray);

        return new(res.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private Dictionary<string, Rule> ToDicitonary(List<Rule> rules)
    {
        Dictionary<string, Rule> result = new Dictionary<string, Rule>();

        foreach (Rule rule in rules)
        {
            result[rule.Output] = rule;
        }

        return result;
    }

    private void CalculateValueForOutput(string output, Dictionary<string, Rule> rulesByOutput, Dictionary<string, bool> values)
    {
        if (values.ContainsKey(output))
        {
            return;
        }

        Rule rule = rulesByOutput[output];
        if (!values.ContainsKey(rule.InputA))
        {
            CalculateValueForOutput(rule.InputA, rulesByOutput, values);
        }
        if (!values.ContainsKey(rule.InputB))
        {
            CalculateValueForOutput(rule.InputB, rulesByOutput, values);
        }

        bool a = values[rule.InputA];
        bool b = values[rule.InputB];

        switch (rule.Operator)
        {
            case Operator.AND:
                values[rule.Output] = a && b; 
                break;
            case Operator.OR:
                values[rule.Output] = a || b;
                break;
            case Operator.XOR:
                values[rule.Output] = a ^ b;
                break;
        }
    }

    private long GetIntFromBitArray(BitArray bitArray)
    {
        var array = new byte[8];
        bitArray.CopyTo(array, 0);
        return BitConverter.ToInt64(array, 0);
    }

    private record Rule (string InputA, string InputB, Operator Operator, string Output);

    private enum Operator
    { 
        AND,
        OR,
        XOR
    }
}
