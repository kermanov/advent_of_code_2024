using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public partial class Puzzle26 : PuzzleBaseWithProgress
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex GetRegex();

    int _progress = 0;
    readonly Dictionary<int, long> _results = new();
    readonly List<int> _failed = [];

    protected override string Solution(string input)
    {
        var matches = GetRegex().Matches(input);
        var arcades = new List<Arcade>();

        checked
        {
            for (int i = 0; i < matches.Count; i += 6)
            {
                arcades.Add(new Arcade
                {
                    ButtonA = new Point(long.Parse(matches[i].Value), long.Parse(matches[i + 1].Value)),
                    ButtonB = new Point(long.Parse(matches[i + 2].Value), long.Parse(matches[i + 3].Value)),
                    Prize = new Point(long.Parse(matches[i + 4].Value) + 10000000000000, long.Parse(matches[i + 5].Value) + 10000000000000),
                });
            }

            long result = 0;

            for (int i = 0; i < arcades.Count; i++)
            {
                var arcade = arcades[i];

                var bPresses = (double)(arcade.ButtonA.X * arcade.Prize.Y - arcade.ButtonA.Y * arcade.Prize.X) /
                    (arcade.ButtonB.Y * arcade.ButtonA.X - arcade.ButtonA.Y * arcade.ButtonB.X);

                var aPresses = (double)(arcade.Prize.X - bPresses * arcade.ButtonB.X) / arcade.ButtonA.X;

                if (!IsInteger(aPresses) || !IsInteger(bPresses))
                {
                    continue;
                }

                var sum = Math.Round(3 * aPresses + bPresses);

                result += (long)sum;
            }

            return $"{result}, {JsonSerializer.Serialize(_results)}";
        }
        
    }

    static bool IsInteger(double number)
    {
        return number - (long)number < 0.01;
    }

    class Arcade
    {
        public required Point ButtonA { get; init; }
        public required Point ButtonB { get; init; }
        public required Point Prize { get; init; }
    }

    record Point
    {
        public long X { get; }
        public long Y { get; }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
        public static Point operator -(Point left, Point right) => left + -1 * right;
        public static Point operator *(Point left, long value) => new(left.X * value, left.Y * value);
        public static Point operator *(long value, Point right) => right * value;
        public static Point operator /(Point left, long value) => new(left.X / value, left.Y / value);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
