namespace AdventOfCode.Puzzles;

public class Puzzle8 : PuzzleBaseWithProgress
{
    readonly (int X, int Y)[] directions = 
    [
        (1, 0),
        (1, 1),
        (0, 1),
    ];

    protected override string Solution(string input)
    {
        string[] rows = input
            .Split('\n')
            .Select(x => x.Trim())
            .ToArray();

        var count = 0;

        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].Length; x++)
            {
                var words = directions
                    .Select(direction => GetWord(x, y, direction.X, direction.Y, 3, rows))
                    .ToArray();

                string[] diagonals = 
                [
                    words[1],
                    new string([words[2][2], words[1][1], words[0][2]]),
                ];

                if (diagonals.All(diag => diag == "MAS" || diag == "SAM"))
                {
                    count++;
                }
            }
        }

        return count.ToString();
    }

    static string GetWord(int x, int y, int dx, int dy, int length, string[] rows)
    {
        var chars = new char[length];

        for (int i = 0; i < length; i++)
        {
            var currentX = x + dx * i;
            var currentY = y + dy * i;

            if (currentX < 0 || currentX >= rows[0].Length || currentY < 0 || currentY >= rows.Length)
            {
                chars[i] = '#';
            }
            else
            {
                chars[i] = rows[currentY][currentX];
            }
        }

        return new string(chars);
    }
}
