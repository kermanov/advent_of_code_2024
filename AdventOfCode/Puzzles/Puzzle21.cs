namespace AdventOfCode.Puzzles;

public class Puzzle21 : PuzzleBase
{
    override protected bool ReportProgress => true;

    protected override string Solution(string input)
    {
        List<long> stones = input.Trim().Split()
            .Select(long.Parse)
            .ToList();

        for (var i = 0; i < 25; i++)
        {
            Blink(stones);
            SetProgress((i + 1) / 25);
        }

        return stones.Count.ToString();
    }

    static void Blink(List<long> stones)
    {
        for (var i = 0; i < stones.Count; i++)
        {
            if (stones[i] == 0)
            {
                stones[i] = 1;
            }
            else if (stones[i].ToString().Length % 2 == 0)
            {
                var stringNumber = stones[i].ToString();
                var stone1 = long.Parse(stringNumber[..(stringNumber.Length / 2)]);
                var stone2 = long.Parse(stringNumber[(stringNumber.Length / 2)..]);
                stones[i] = stone1;
                stones.Insert(i + 1, stone2);
                i++;
            }
            else
            {
                stones[i] *= 2024;
            }
        }
    }
}
