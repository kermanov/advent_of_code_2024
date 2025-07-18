namespace AdventOfCode.Puzzles;

public class Puzzle24 : PuzzleBaseWithProgress
{
    int _progress = 0;

    protected override string Solution(string input)
    {
        var map = new Map(input, 140, 140);
        var regions = new Dictionary<string, List<Point>>();

        foreach (var point in map.GetAllPoints())
        {
            if (!regions.TryAdd(point.Value, [point.Key]))
            {
                regions[point.Value].Add(point.Key);
            }
        }

        for (int i = 0; i < regions.Count; i++)
        {
            var region = regions.Keys.Order().ElementAt(i);
            var points = regions[region];

            for (int j = 1; j < points.Count; j++)
            {
                var visited = new HashSet<Point>();
                if (!Connected(map, points[0], points[j], region, visited))
                {
                    var notConnectedPoint = points[j];
                    points.RemoveAt(j);
                    j--;

                    var newRegion = region + region.Last();
                    if (!regions.TryAdd(newRegion, [notConnectedPoint]))
                    {
                        regions[newRegion].Add(notConnectedPoint);
                    }

                    map.SetTile(notConnectedPoint, newRegion);
                }
            }
                
            SetProgress((int)Math.Round(++_progress / (double)regions.Count * 100));
        }

        var result = 0;

        foreach (var region in regions)
        {
            var sides = Enum.GetValues<Direction>()
                .Select(direction => CountSides(map, region, direction))
                .Sum();

            result += sides * region.Value.Count;
        }

        return result.ToString();
    }

    static int CountSides(Map map, KeyValuePair<string, List<Point>> region, Direction direction)
    {
        var sides = 0;

        var sidePointsGroups = region.Value
            .Where(point => map.GetTile(point + Point.GetByDirection(direction)) != region.Key)
            .OrderBy(point => direction switch
            {
                Direction.Up => point.X,
                Direction.Right => point.Y,
                Direction.Down => point.X, 
                Direction.Left => point.Y,
                _ => throw new Exception("Invalid direction"),
            })
            .GroupBy(point => direction switch
            {
                Direction.Up => point.Y,
                Direction.Right => point.X,
                Direction.Down => point.Y, 
                Direction.Left => point.X,
                _ => throw new Exception("Invalid direction"),
            })
            .ToArray();

        foreach (var group in sidePointsGroups)
        {
            for (int i = 0; i < group.Count(); i++)
            {
                if (i == 0 || !IsSameSide(group.ElementAt(i), group.ElementAt(i - 1), direction))
                {
                    sides++;
                }
            }
        }

        return sides;
    }

    static bool IsSameSide(Point point1, Point point2, Direction direction) => direction switch
    {
        Direction.Up => Math.Abs(point1.X - point2.X) == 1,
        Direction.Right => Math.Abs(point1.Y - point2.Y) == 1,
        Direction.Down => Math.Abs(point1.X - point2.X) == 1,
        Direction.Left => Math.Abs(point1.Y - point2.Y) == 1,
        _ => throw new Exception("Invalid direction"),
    };

    static bool Connected(Map map, Point point1, Point point2, string tile, HashSet<Point> visited)
    {
        if (point1 == point2)
        {
            return true;
        }

        visited.Add(point1);

        return GetNeighbors(point1)
            .Where(neighbor => !visited.Contains(neighbor) && map.GetTile(neighbor) == tile)
            .Any(neighbor => Connected(map, neighbor, point2, tile, visited));
    }

    static Point[] GetNeighbors(Point point)
    {
        return Enum.GetValues<Direction>()
            .Select(direction => point + Point.GetByDirection(direction))
            .ToArray();
    }

    class Map
    {
        readonly Dictionary<Point, string> _map;

        public Map(string input, int width, int height)
        {
            Width = width;
            Height = height;

            _map = [];

            var lines = input.Split("\n").Select(l => l.Trim()).ToArray();
        
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _map[new Point(x, y)] = lines[y][x].ToString();
                }
            }
        }

        public int Width { get; }
        public int Height { get; }

        public string GetTile(Point point)
        {
            if (point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height)
            {
                return "?";
            }

            return _map[point];
        }

        public void SetTile(Point point, string tile)
        {
            _map[point] = tile;
        }

        public Point[] GetPointsOfTile(string tile)
        {
            return _map
                .Where(pair => pair.Value == tile)
                .Select(pair => pair.Key)
                .ToArray();
        }

        public KeyValuePair<Point, string>[] GetAllPoints()
        {
            return _map.ToArray();
        }
    }

    struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point GetByDirection(Direction direction) => direction switch
        {
            Direction.Up => new Point(0, -1),
            Direction.Right => new Point(1, 0),
            Direction.Down => new Point(0, 1),
            Direction.Left => new Point(-1, 0),
            _ => throw new Exception("Invalid direction"),
        };

        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(Point left, Point right) => !(left == right);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }
}
