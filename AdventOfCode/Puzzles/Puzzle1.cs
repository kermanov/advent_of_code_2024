namespace AdventOfCode.Puzzles;

public class Puzzle1 : PuzzleBase
{
    override protected string Solution(string input)
    {
        var list1 = new List<int>();
        var list2 = new List<int>();

        foreach (var line in input.Split("\n"))
        {
            var numbers = line
                .Trim()
                .Split()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .ToArray();

            list1.Add(numbers[0]);
            list2.Add(numbers[1]);
        }

        var sortedList1 = list1.Order();
        var sortedList2 = list2.Order();

        var sum = 0;

        for (var i = 0; i < sortedList1.Count(); i++)
        {
            sum += Math.Abs(sortedList1.ElementAt(i) - sortedList2.ElementAt(i));
        }

        return sum.ToString();
    }
}
