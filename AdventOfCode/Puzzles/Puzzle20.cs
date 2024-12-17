namespace AdventOfCode.Puzzles;

public class Puzzle20 : PuzzleBase
{
    protected override string Solution(string input)
    {
        var map = new Map(input, 54, 54);
        var starts = map.GetPointsOfTile(0);

        var result = starts
            .Select((point, index) =>  
            {
                List<Point> tops = [];
                FindScore(map, point, tops);
                SetProgress((index + 1) / starts.Length);
                return tops.Count;
            })
            .Sum();

        return result.ToString();
    }

    static void FindScore(Map map, Point point, List<Point> tops)
    {
        if (map.GetTile(point) == 9)
        {
            tops.Add(point);
            return;
        }

        var nextPoints = GetNeighbors(point)
            .Where(neighbor => map.GetTile(neighbor) == map.GetTile(point) + 1)
            .ToArray();

        foreach (var nextPoint in nextPoints)
        {
            FindScore(map, nextPoint, tops);
        }
    }

    static Point[] GetNeighbors(Point point)
    {
        return Enum.GetValues<Direction>()
            .Select(direction => point + Point.GetByDirection(direction))
            .ToArray();
    }
    
    class Map
    {
        readonly Dictionary<Point, int> _map;

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
                    _map[new Point(x, y)] = lines[y][x] - 48;
                }
            }
        }

        public int Width { get; }
        public int Height { get; }

        public int GetTile(Point point)
        {
            if (point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height)
            {
                return -1;
            }

            return _map[point];
        }

        public Point[] GetPointsOfTile(int tile)
        {
            return _map
                .Where(pair => pair.Value == tile)
                .Select(pair => pair.Key)
                .ToArray();
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
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }
}
