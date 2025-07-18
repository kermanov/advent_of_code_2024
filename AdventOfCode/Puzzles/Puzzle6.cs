using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public class Puzzle6 : PuzzleBaseWithProgress
{
    protected override string Solution(string input)
    {
        Regex regex = new(@"mul\(([0-9]{1,3}),([0-9]{1,3})\)");

        var result = regex.Matches(input)
            .Where(match => 
            {
                var subString = input[..match.Index];
                return subString.LastIndexOf("do()") >= subString.LastIndexOf("don't()");
            }) 
            .Select(match => int.Parse(match.Result("$1")) * int.Parse(match.Result("$2")))
            .Sum();

        return result.ToString();
    }
}