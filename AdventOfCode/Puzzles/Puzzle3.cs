namespace AdventOfCode.Puzzles;

public class Puzzle3 : PuzzleBase
{
    protected override string Solution(string input)
    {
        var total = 0;

        foreach (var line in input.Split('\n'))
        {
            var numbers = line
                .Trim()
                .Split()
                .Select(int.Parse)
                .ToArray();

            if (IsSorted(numbers) && IsValidDelta(numbers))
            {
                total++;
            }
        }

        return total.ToString();
    }

    static bool IsSorted(int[] numbers)
    {
        return Enumerable.SequenceEqual(numbers, numbers.Order()) || 
            Enumerable.SequenceEqual(numbers, numbers.OrderDescending());
    }

    static bool IsValidDelta(int[] numbers)
    {
        for (var i = 0; i < numbers.Length - 1; i++)
        {
            var delta = Math.Abs(numbers[i + 1] - numbers[i]);

            if (delta < 1 || delta > 3)
            {
                return false;
            }
        }

        return true;
    }
}
