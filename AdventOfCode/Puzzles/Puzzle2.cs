namespace AdventOfCode.Puzzles;

public class Puzzle2 : PuzzleBaseWithProgress
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

        var result = list1
            .Select(number => list2.Count(x => x == number) * number)
            .Sum();

        return result.ToString();
    }
}
