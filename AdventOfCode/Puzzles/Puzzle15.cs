namespace AdventOfCode.Puzzles;

public class Puzzle15 : PuzzleBaseWithProgress
{
    protected override string Solution(string input)
    {
        var inputWidth = 50;
        var inputHeight = 50;

        var inputMap = new Map(input, inputWidth, inputHeight);
        var resultMap = new Map(inputWidth, inputHeight);

        foreach (var currentAntenna in inputMap.GetAllTiles())
        {
            var currentAntennaType = inputMap.GetTile(currentAntenna);
            foreach (var otherAntenna in inputMap.GetTilesByType(currentAntennaType))
            {
                if (currentAntenna == otherAntenna)
                {
                    continue;
                }

                var antiNodes = GetAntiNodes(currentAntenna, otherAntenna);
                foreach (var antiNode in antiNodes)
                {
                    resultMap.SetTile(antiNode, '#');
                }
            }
        }

        // resultMap.Draw();
        // Console.WriteLine();

        return resultMap.GetTilesByType('#').Count.ToString();
    }

    static Point[] GetAntiNodes(Point antenna1, Point antenna2)
    {
        var delta = antenna2 - antenna1;

        return 
        [
            antenna2 + delta,
            antenna1 - delta,
        ];
    }

    class Map
    {
        readonly Dictionary<char, HashSet<Point>> _tiles;

        public Map(string input, int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = [];

            var lines = input.Split("\n").Select(l => l.Trim()).ToArray();
        
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (lines[y][x] == '.')
                    {
                        continue;
                    }

                    SetTile(new Point(x, y), lines[y][x]);
                }
            }
        }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            _tiles = [];
        }

        public int Width { get; }
        public int Height { get; }

        public char GetTile(Point point)
        {
            if (point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height)
            {
                return '?';
            }

            return _tiles.First(pair => pair.Value.Contains(point)).Key;
        }

        public void SetTile(Point point, char tile)
        {
            if (point.X < 0 || point.X >= Width || point.Y < 0 || point.Y >= Height)
            {
                return;
            }

            if (!_tiles.TryAdd(tile, [point]))
            {
                _tiles[tile].Add(point);
            }
        }

        public Point[] GetAllTiles()
        {
            return _tiles.Values.Aggregate(new Point[] {}, (result, next) => result.Concat(next).ToArray());
        }

        public HashSet<Point> GetTilesByType(char tile)
        {
            return _tiles[tile];
        }

        public void Draw()
        {
            var tiles = GetAllTiles().ToHashSet();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var point = new Point(x, y);

                    if (tiles.Contains(point))
                    {
                        Console.Write(GetTile(point));
                    }
                    else 
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }
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

        public static Point operator +(Point left, Point right) => new(left.X + right.X, left.Y + right.Y);
        public static Point operator -(Point left, Point right) => new(left.X - right.X, left.Y - right.Y);
        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(Point left, Point right) => !(left == right);
    }
}
