using System.Drawing;

namespace AdventOfCode.Puzzles;

public class Puzzle11 : PuzzleBaseWithProgress
{
    protected override string Solution(string input)
    {
        var map = new Map(input, 130, 130);
        var guard = new Guard(map);

        while (!guard.IsOutOfBounds())
        {
            guard.Move();
        }

        return guard.PathLength.ToString();
    }

    class Guard 
    {
        readonly Map _map;
        Point _position;
        Direction _direction;
        readonly HashSet<Point> _path;

        public Guard(Map map)
        {
            _map = map;
            _position = map.Start;
            _direction = Direction.Up;
            _path = new HashSet<Point>();
        }

        public int PathLength => _path.Count;

        public void Move()
        {
            _path.Add(_position);

            Point nextPosition = NextPosition();

            while (_map.GetTile(nextPosition) == TileType.Obstacle)
            {
                _direction = (Direction)((int)(_direction + 1) % 4);
                nextPosition = NextPosition();
            }

            _position = nextPosition;
        }

        public bool IsOutOfBounds() => _map.GetTile(_position) == TileType.OutOfBounds;

        Point NextPosition() => _direction switch
        {
            Direction.Up => new Point(_position.X, _position.Y - 1),
            Direction.Right => new Point(_position.X + 1, _position.Y),
            Direction.Down => new Point(_position.X, _position.Y + 1),
            Direction.Left => new Point(_position.X - 1, _position.Y),
            _ => throw new Exception("Invalid direction"),
        };

        enum Direction
        {
            Up,
            Right,
            Down,
            Left,
        }
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
    }

    enum TileType
    {
        Empty,
        Obstacle,
        OutOfBounds,
        Start,
    }
}
