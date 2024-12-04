namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly char[,] _input;
    private readonly int _height;
    private readonly int _width;

    public Day04()
    {
        string[] lines = File.ReadAllLines(InputFilePath);

        _input = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                _input[i, j] = lines[i][j];
            }
        }

        _height = _input.GetLength(0);
        _width = _input.GetLength(1);
    }

    public override ValueTask<string> Solve_1()
    {
        int result = 0;

        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                result += CountXmases(i, j);
            }
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private int CountXmases(int i, int j)
    {
        int res = 0;

        if (_input[i, j] != 'X')
        {
            return res;
        }

        // left to right
        if (j <= _width - 4 && IsMas(_input[i, j + 1], _input[i, j + 2], _input[i, j + 3]))
        {
            res += 1;
        }

        // right to left
        if (j >= 3 && IsMas(_input[i, j - 1], _input[i, j - 2], _input[i, j - 3]))
        {
            res += 1;
        }

        // top to bottom
        if (i <= _height - 4 && IsMas(_input[i + 1, j], _input[i + 2, j], _input[i + 3, j]))
        {
            res += 1;
        }

        // bottom to top
        if (i >= 3 && IsMas(_input[i - 1, j], _input[i - 2, j], _input[i - 3, j]))
        {
            res += 1;
        }

        // diagonal TL to BR
        if (i <= _height - 4  && j <= _width - 4 && IsMas(_input[i + 1, j + 1], _input[i + 2, j + 2], _input[i + 3, j + 3]))
        {
            res += 1;
        }

        // diagonal TR to BL
        if (i <= _height - 4 && j >= 3 && IsMas(_input[i + 1, j - 1], _input[i + 2, j - 2], _input[i + 3, j - 3]))
        {
            res += 1;
        }

        // diagonal BL to TR
        if (i >= 3 && j <= _width - 4 && IsMas(_input[i - 1, j + 1], _input[i - 2, j + 2], _input[i - 3, j + 3]))
        {
            res += 1;
        }

        // diagonal BR to TL
        if (i >= 3 && j >= 3 && IsMas(_input[i - 1, j - 1], _input[i - 2, j - 2], _input[i - 3, j - 3]))
        {
            res += 1;
        }

        return res;
    }

    private bool IsMas(char m, char a, char s)
    {
        return m == 'M' && a == 'A' && s == 'S';
    }
}
