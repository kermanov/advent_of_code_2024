using System.Collections.Concurrent;

namespace AdventOfCode.Puzzles;

public class Puzzle12 : PuzzleBase
{
    protected override string Solution(string input)
    {
        var map = new Map(input, 130, 130);
        var guard = new Guard(map);

        while (!guard.IsOutOfBounds())
        {
            guard.Move();
        }

        ConcurrentBag<Point> placePositions = new();

        Parallel.ForEach(
            guard.Path.Keys, 
            new ParallelOptions 
            {
                MaxDegreeOfParallelism = 4,
            }, 
            position =>
            {
                var tile = map.GetTile(position);

                if (tile == TileType.Start)
                {
                    return;
                }

                var testMap = map.Copy();
                testMap.SetTile(position, TileType.Obstacle);
                var testGuard = new Guard(testMap);

                if (IsInCycle(testGuard))
                {
                    placePositions.Add(position);
                }
            });

        return placePositions.Distinct().Count().ToString();
    }

    static bool IsInCycle(Guard guard)
    {
        for (int i = 0; i < 10000; i++)
        {
            guard.Move();

            if (guard.IsOutOfBounds())
            {
                return false;
            }
        }

        return true;
    }

    class Guard 
    {
        readonly Map _map;
        Point _position;
        Direction _direction;
        readonly Dictionary<Point, List<Direction>> _path;

        public Guard(Map map)
        {
            _map = map;
            _position = map.Start;
            _direction = Direction.Up;
            _path = [];
        }

        public Dictionary<Point, List<Direction>> Path => _path;

        public void Move()
        {
            if (!_path.TryAdd(_position, [_direction]))
            {
                _path[_position].Add(_direction);
            }

            Point nextPosition = NextPosition();

            while (_map.GetTile(nextPosition) == TileType.Obstacle)
            {
                _direction = (Direction)((int)(_direction + 1) % 4);
                nextPosition = NextPosition();
            }

            _position = nextPosition;
        }

        public bool IsOutOfBounds() => _map.GetTile(_position) == TileType.OutOfBounds;

        Point NextPosition() => _position + Point.GetByDirection(_direction);
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    class Map
    {
        readonly TileType[,] _map;

        public Map(string input, int width, int height)
        {
            Width = width;
            Height = height;

            _map = new TileType[height, width];

            var lines = input.Split("\n").Select(l => l.Trim()).ToArray();
        
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    _map[y, x] = lines[y][x] switch 
                    {
                        '.' => TileType.Empty,
                        '#' => TileType.Obstacle,
                        '^' => TileType.Start,
                        _ => throw new Exception("Invalid tile type"),
                    };

                    if (_map[y, x] == TileType.Start)
                    {
                        Start = new Point(x, y);
                    }
                }
            }
        }

        public int Width { get; }
        public int Height { get; }
        public Point Start { get; }

        public TileType GetTile(Point point)
        {
            if (point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height)
            {
                return TileType.OutOfBounds;
            }

            return _map[point.Y, point.X];
        }

        public void SetTile(Point point, TileType tileType)
        {
            if (point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height)
            {
                return;
            }

            _map[point.Y, point.X] = tileType;
        }

        private Map(Map map)
        {
            _map = new TileType[map.Height, map.Width];
            Array.Copy(map._map, 0, _map, 0, map._map.Length);

            Width = map.Width;
            Height = map.Height;
            Start = map.Start;
        }

        public Map Copy() => new(this);
    }

    enum TileType
    {
        Empty,
        Obstacle,
        OutOfBounds,
        Start,
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
}
