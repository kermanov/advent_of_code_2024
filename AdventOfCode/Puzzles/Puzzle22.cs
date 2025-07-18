namespace AdventOfCode.Puzzles;

public class Puzzle22 : PuzzleBaseWithProgress
{
    readonly int _blinks = 75;

    protected override string Solution(string input)
    {
        var stones = input.Trim().Split()
            .Select(long.Parse)
            .ToArray();

        var stonesDict = stones
            .Distinct()
            .ToDictionary(number => number, number => (long)stones.Count(stone => stone == number));

        for (var i = 0; i < _blinks; i++)
        {
            Blink(ref stonesDict);
        }

        return stonesDict.Values.Sum().ToString();
    }

    static void Blink(ref Dictionary<long, long> stones)
    {
        var newStones = new Dictionary<long, long>();
        var stoneValues = stones.Keys.ToArray();

        foreach (var stone in stoneValues)
        {
            if (stone == 0)
            {
                SetStoneValue(stones, newStones, 0, 1);
            }
            else if (stone.ToString().Length % 2 == 0)
            {
                var stringNumber = stone.ToString();
                var stone1 = long.Parse(stringNumber[..(stringNumber.Length / 2)]);
                var stone2 = long.Parse(stringNumber[(stringNumber.Length / 2)..]);

                AddStoneCount(newStones, stone1, stones[stone]);
                AddStoneCount(newStones, stone2, stones[stone]);
            }
            else
            {
                SetStoneValue(stones, newStones, stone, stone * 2024);
            }
        }

        stones = newStones;
    }

    static void SetStoneValue(Dictionary<long, long> stones, Dictionary<long, long> newStones, long currentStoneValue, long newStoneValue)
    {
        var count = stones[currentStoneValue];

        if (!newStones.TryAdd(newStoneValue, count))
        {
            newStones[newStoneValue] += count;
        }
    }

    static void AddStoneCount(Dictionary<long, long> newStones, long stoneValue, long countToAdd)
    {
        if (!newStones.TryAdd(stoneValue, countToAdd))
        {
            newStones[stoneValue] += countToAdd;
        }
    }
}
