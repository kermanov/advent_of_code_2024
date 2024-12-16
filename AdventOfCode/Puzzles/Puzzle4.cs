namespace AdventOfCode.Puzzles;

public class Puzzle4 : PuzzleBase
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

            if (IsSorted(numbers) && IsValidDelta(numbers) || TryMakeSafe(numbers))
            {
                total++;
            }
        }

        return total.ToString();
    }

    static bool IsSorted(IEnumerable<int> numbers)
    {
        return Enumerable.SequenceEqual(numbers, numbers.Order()) || 
            Enumerable.SequenceEqual(numbers, numbers.OrderDescending());
    }

    static bool IsValidDelta(IEnumerable<int> numbers)
    {
        for (var i = 0; i < numbers.Count() - 1; i++)
        {
            var delta = Math.Abs(numbers.ElementAt(i + 1) - numbers.ElementAt(i));

            if (delta < 1 || delta > 3)
            {
                return false;
            }
        }

        return true;
    }

    static bool TryMakeSafe(int[] numbers)
    {
        for (var i = 0; i < numbers.Length; i++)
        {
            var list = numbers.ToList();
            list.RemoveAt(i);

            if (IsSorted(list) && IsValidDelta(list))
            {
                return true;
            }
        }

        return false;
    }
}
