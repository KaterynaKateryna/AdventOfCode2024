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
        List<int?> representation = Defragment2();

        long checksum = GetChecksum(representation);
        return new(checksum.ToString());
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

    private List<int?> Defragment2()
    {
        List<int> input = _input.Select(x => int.Parse(x.ToString())).ToList();

        List<Block> blocks = new List<Block>();

        int index = 0;
        int fileIndex = 0;
        for (int i = 0; i < input.Count; ++i)
        {
            blocks.Add(new Block(index, input[i], i % 2 == 0 ? fileIndex : null, Moved: false));
            index += input[i];
            if (i % 2 == 0)
            {
                fileIndex++;
            }
        }

        for (int i = blocks.Count - 1; i >= 0; i--)
        {
            Block toMove = blocks[i];
            if (toMove.FileIndex.HasValue && !toMove.Moved)
            {
                (int moveToIndex, Block? moveTo) = blocks.Index().FirstOrDefault(x => 
                    x.Item.Index < toMove.Index
                    && !x.Item.FileIndex.HasValue 
                    && x.Item.Length >= toMove.Length
                );
                if (moveTo != null)
                {
                    if (moveTo.Length > toMove.Length)
                    { 
                        blocks.Insert(
                            moveToIndex + 1,
                            new Block(
                                Index: moveTo.Index + toMove.Length, 
                                Length: moveTo.Length -  toMove.Length,
                                FileIndex: null,
                                Moved: false
                            )
                        );
                    }

                    moveTo.FileIndex = toMove.FileIndex;
                    moveTo.Length = toMove.Length;
                    moveTo.Moved = true;

                    toMove.FileIndex = null;
                }    
            }
        }

        List<int?> result = new List<int?>();
        foreach (var block in blocks)
        {
            for (int i = 0; i < block.Length; ++i)
            {
                result.Add(block.FileIndex);
            }
        }
        return result;
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

    private class Block(int Index, int Length, int? FileIndex, bool Moved)
    {
        public int Index { get; set; } = Index;
        public int Length { get; set; } = Length;
        public int? FileIndex { get; set; } = FileIndex;
        public bool Moved { get; set; } = Moved;
    }
}
