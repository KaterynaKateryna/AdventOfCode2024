using System.Numerics;

namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<int?> representation = GetRepresentation();

        Defragment(representation);

        long checksum = GetChecksum(representation);
        return new(checksum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new("");
    }

    private List<int?> GetRepresentation()
    { 
        List<int?> blocks = new List<int?>();

        bool isFile = true;
        int fileIndex = 0;
        foreach (char c in _input) 
        {
            int size = int.Parse(c.ToString());

            for (int i = 0; i < size; ++i)
            {
                if (isFile)
                {
                    blocks.Add(fileIndex);
                }
                else
                { 
                    blocks.Add(null);
                }
            }

            if (isFile)
            {
                fileIndex++;
            }

            isFile = !isFile;
        }

        return blocks;
    }

    private void Defragment(List<int?> blocks)
    {
        int firstEmptySpace = blocks.IndexOf(null);
        for (int i = blocks.Count - 1; i > firstEmptySpace; --i)
        {
            if (blocks[i] != null)
            {
                blocks[firstEmptySpace] = blocks[i];
                blocks[i] = null;

                firstEmptySpace = blocks.IndexOf(null);
            }
        }
    }

    private long GetChecksum(List<int?> representation)
    {
        long result = 0;
        for (int i = 0; i < representation.Count; ++i)
        {
            if (representation[i] != null)
            {
                result += representation[i]!.Value * i;
            }
        }
        return result;
    }
}
