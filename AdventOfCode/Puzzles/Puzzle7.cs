namespace AdventOfCode.Puzzles;

public class Puzzle7 : PuzzleBase
{
    readonly (int X, int Y)[] directions = 
    [
        (0, -1),
        (1, -1),
        (1, 0),
        (1, 1),
        (0, 1),
        (-1, 1),
        (-1, 0),
        (-1, -1),
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
                foreach (var direction in directions)
                {
                    var word = GetWord(x, y, direction.X, direction.Y, 4, rows);

                    if (word == "XMAS")
                    {
                        count++;
                    }
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
