using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public partial class Puzzle25 : PuzzleBase
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex GetRegex();

    const int MaxPresses = 100;

    readonly Dictionary<int, long> _results = new();

    protected override string Solution(string input)
    {
        var matches = GetRegex().Matches(input);
        var arcades = new List<Arcade>();

        for (int i = 0; i < matches.Count; i += 6)
        {
            arcades.Add(new Arcade
            {
                ButtonA = new Point(int.Parse(matches[i].Value), int.Parse(matches[i + 1].Value)),
                ButtonB = new Point(int.Parse(matches[i + 2].Value), int.Parse(matches[i + 3].Value)),
                Prize = new Point(int.Parse(matches[i + 4].Value), int.Parse(matches[i + 5].Value)),
            });
        }

        checked
        {
            var result = 0;

            for (int i = 0; i < arcades.Count; i++)
            {
                var arcade = arcades[i];
                int? minTokens = null;

                for (int a = 0; a <= MaxPresses; a++)
                for (int b = 0; b <= MaxPresses; b++)
                {
                    if (arcade.ButtonA * a + arcade.ButtonB * b == arcade.Prize)
                    {
                        var tokens = a * 3 + b;

                        if (tokens < (minTokens ?? int.MaxValue))
                        {
                            minTokens = tokens;
                        }
                    }
                }

                if (minTokens.HasValue)
                {
                    result += minTokens.Value;
                    _results[i] = minTokens.Value;
                }
            }

            return $"{result}, {JsonSerializer.Serialize(_results)}";
        }
    }

    class Arcade
    {
        public required Point ButtonA { get; init; }
        public required Point ButtonB { get; init; }
        public required Point Prize { get; init; }
    }

    record Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
        public static Point operator *(Point left, int value) => new(left.X * value, left.Y * value);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
