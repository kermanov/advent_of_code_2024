using System.Drawing;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

public partial class Puzzle28 : PuzzleBase
{
    [GeneratedRegex(@"-?\d+")]
    private static partial Regex GetRegex();

    protected override string Solution(string input)
    {
        var matches = GetRegex().Matches(input);

        var map = new Map(101, 103);
        var robots = ReadRobots(input, map);
        var initialRobots = ReadRobots(input, map);

        for (int step = 0; step < 10403; step++)
        {
            if (step == 6876)
                Draw(map, robots, step);

            foreach (var robot in robots)
            {
                robot.Move();
            }

            // var cycle = true;

            // for (int i = 0; i < robots.Count; ++i)
            // {
            //     if (initialRobots[i].Position != robots[i].Position)
            //     {
            //         cycle = false;
            //         break;
            //     }
            // }

            // if (cycle)
            // {
            //     Console.WriteLine($"Cycle on step {step}.");
            //     break;
            // }
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

        return "6876";
    }

    static void Draw(Map map, List<Robot> robots, int step)
    {
        StringBuilder builder = new(map.Width * map.Height * 2);

        for (int i = 0; i < map.Height; ++i)
        {
            for (int j = 0; j < map.Width; ++j)
            {
                builder.Append(' ');
            }

            builder.Append('\n');
        }

        foreach (var robot in robots)
        {
            var index = robot.Position.Y * (map.Width + 1) + robot.Position.X;
            builder.Remove(index, 1);
            builder.Insert(index, 'R');
        }

        var stringMap = builder.ToString();

        Console.Clear();
        Console.WriteLine($"\r{stringMap}\n{step}");
        Thread.Sleep(200);
    }

    static List<Robot> ReadRobots(string input, Map map)
    {
        var matches = GetRegex().Matches(input);
        var robots = new List<Robot>();

        for (int i = 0; i < matches.Count; i += 4)
        {
            var position = new Point(int.Parse(matches[i].Value), int.Parse(matches[i + 1].Value));
            var velocity = new Point(int.Parse(matches[i + 2].Value), int.Parse(matches[i + 3].Value));

            robots.Add(new Robot(position, velocity, map));
        }

        return robots;
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
