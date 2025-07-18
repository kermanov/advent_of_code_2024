namespace AdventOfCode.Puzzles;

public class Puzzle18 : PuzzleBaseWithProgress
{
    protected override string Solution(string input)
    {
        var diskMap = input.Trim();
        var blocks = ToIndividualBlocks(diskMap);
        MoveFiles(blocks);

        // Console.WriteLine(string.Join("", blocks.Select(block => block == -1 ? '.' : (char)(block + 48))));

        var result = blocks
            .Select((number, index) => number == -1 ? 0 : number * index)
            .Sum();

        return result.ToString();
    }

    static List<long> ToIndividualBlocks(string diskMap)
    {
        var blocks = new List<long>();
        int currentFileId = 0;

        for (int i = 0; i < diskMap.Length; i++)
        {
            var isFile =  i % 2 == 0;
            var currentString = isFile ? currentFileId : -1;

            for (int j = 0; j < diskMap[i] - 48; j++)
            {
                blocks.Add(currentString);
            }

            if (isFile)
            {
                currentFileId++;
            }
        }

        return blocks;
    }

    void MoveFiles(List<long> blocks)
    {
        var lastNumberIndex = LastNumberIndex(blocks);
        var checkedFileIds = new HashSet<long>();

        while (lastNumberIndex > 0)
        {
            SetProgress((int)Math.Round((double)checkedFileIds.Count / blocks.Max() * 100));

            var fileSize = GetFileSize(blocks, lastNumberIndex);

            if (checkedFileIds.Contains(blocks[lastNumberIndex]))
            {
                lastNumberIndex -= fileSize;
                goto End;
            }

            checkedFileIds.Add(blocks[lastNumberIndex]);

            var indexOfFirstGap = GetIndexOfFirstGap(blocks, fileSize);
            
            if (indexOfFirstGap != -1 && indexOfFirstGap < lastNumberIndex)
            {
                blocks.RemoveRange(indexOfFirstGap, fileSize);
                blocks.InsertRange(indexOfFirstGap, Enumerable.Repeat(blocks[lastNumberIndex - fileSize], fileSize));

                var fileStartIndex = lastNumberIndex - fileSize + 1;
                blocks.RemoveRange(fileStartIndex, fileSize);
                blocks.InsertRange(fileStartIndex, Enumerable.Repeat((long)-1, fileSize));

                lastNumberIndex = LastNumberIndex(blocks.Take(fileStartIndex).ToList());
            }
            else
            {
                lastNumberIndex -= fileSize;
            }

            End:
            if (lastNumberIndex >= 0 && blocks[lastNumberIndex] == -1)
            {
                lastNumberIndex = LastNumberIndex(blocks.Take(lastNumberIndex + 1).ToList());
            }
        }
    }

    static int GetFileSize(List<long> blocks, int lastNumberIndex)
    {
        var fileSize = 1;

        while (lastNumberIndex - fileSize >= 0 && blocks[lastNumberIndex - fileSize] == blocks[lastNumberIndex])
        {
            fileSize++;
        }

        return fileSize;
    }

    static int GetIndexOfFirstGap(List<long> blocks, int gapSize)
    {
        for (int i = 0; i < blocks.Count - gapSize - 1; i++)
        {
            if (blocks.Skip(i).Take(gapSize).All(block => block == -1))
            {
                return i;
            }
        }

        return -1;
    }

    static int LastNumberIndex(List<long> blocks)
    {
        for (int i = blocks.Count - 1; i >= 0; i--)
        {
            if (blocks[i] != -1)
            {
                return i;
            }
        }

        throw new Exception("No number found");
    }
}
