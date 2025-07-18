namespace AdventOfCode.Puzzles;

public abstract class PuzzleBase : IPuzzle
{
    public virtual Task Solve(string input)
    {
        var result = Solution(input);

        Console.WriteLine(result);

        return Task.CompletedTask;
    }
    
    protected abstract string Solution(string input);
}
