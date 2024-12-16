using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public class Puzzle5 : PuzzleBase
{
    protected override string Solution(string input)
    {
        Regex regex = new(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)");

        var result = regex.Matches(input)
            .Select(match => int.Parse(match.Result("$1")) * int.Parse(match.Result("$2")))
            .Sum();

        return result.ToString();
    }
}
