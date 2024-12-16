namespace AdventOfCode.Puzzles;

public class Puzzle17 : PuzzleBase
{
    protected override string Solution(string input)
    {
        var diskMap = input.Trim();
        var blocks = ToIndividualBlocks(diskMap);
        RemoveGaps(blocks);

        var result = blocks
            .Where(number => number != -1)
            .Select((number, index) => number * index)
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

    static void RemoveGaps(List<long> blocks)
    {
        var totalGaps = blocks.Count(block => block == -1);

        for (int i = 0; i < totalGaps; i++)
        {
            var lastNumber = blocks[blocks.Count - 1 - i];

            if (lastNumber != -1)
            {
                var indexOfFirstGap = blocks.IndexOf(-1);

                blocks.Insert(indexOfFirstGap, lastNumber);
                blocks.RemoveAt(indexOfFirstGap + 1);
                blocks[blocks.Count - 1 - i] = -1;
            }
        }
    }
}
