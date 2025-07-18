using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public partial class Puzzle27 : PuzzleBase
{
    [GeneratedRegex(@"-?\d+")]
    private static partial Regex GetRegex();

    protected override string Solution(string input)
    {
        var matches = GetRegex().Matches(input);

        var map = new Map(101, 103);
        var robots = new List<Robot>();

        for (int i = 0; i < matches.Count; i += 4)
        {
            var position = new Point(int.Parse(matches[i].Value), int.Parse(matches[i + 1].Value));
            var velocity = new Point(int.Parse(matches[i + 2].Value), int.Parse(matches[i + 3].Value));

            robots.Add(new Robot(position, velocity, map));
        }

        for (int step = 0; step < 100; step++)
        {
            foreach (var robot in robots)
            {
                robot.Move();
            }
        }

        Dictionary<int, int> quadrantCounts = new()
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
        };

        foreach (var robot in robots)
        {
            var quadrant = robot.GetQuadrant();

            if (quadrant == 0)
            {
                continue;
            }

            quadrantCounts[quadrant]++;
        }

        var result = quadrantCounts.Values.Aggregate(1, (value, count) => value *= count);

        return result.ToString();
    }

    class Robot
    {
        public Point Position { get; private set; }
        public Point Velocity { get; }
        public Map Map { get; }

        public Robot(Point position, Point velocity, Map map)
        {
            Position = position;
            Velocity = velocity;
            Map = map;
        }

        public void Move()
        {
            Position = new Point(
                (Position.X + Velocity.X + Map.Width) % Map.Width,
                (Position.Y + Velocity.Y + Map.Height) % Map.Height);
        }

        public int GetQuadrant()
        {
            if (Position.X < Map.Width / 2 && Position.Y < Map.Height / 2)
            {
                return 1;
            }
            else if (Position.X > Map.Width / 2 && Position.Y < Map.Height / 2)
            {
                return 2;
            }
            else if (Position.X < Map.Width / 2 && Position.Y > Map.Height / 2)
            {
                return 3;
            }
            else if (Position.X > Map.Width / 2 && Position.Y > Map.Height / 2)
            {
                return 4;
            }

            return 0;
        }
    }

    class Map
    {
        public int Width { get; }
        public int Height { get; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
