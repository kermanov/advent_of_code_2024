namespace AdventOfCode.Puzzles;

public class Puzzle22 : PuzzleBase
{
    override protected bool ReportProgress => true;

    protected override string Solution(string input)
    {
        LinkedList<Stone> stones = new(input.Trim().Split().Select(number => new Stone
        {
            Number = long.Parse(number),
        }));

        var blinks = 75;

        for (var i = 0; i < blinks; i++)
        {
            Blink(stones);
            SetProgress((int)Math.Round((i + 1) / (double)blinks * 100));
        }

        return stones.Count.ToString();
    }

    static void Blink(LinkedList<Stone> stones)
    {
        foreach (var stone in stones)
        {
            if (stone.Number == 0)
            {
                stone.Number = 1;
            }
            else if (stone.Number.ToString().Length % 2 == 0)
            {
                var stringNumber = stone.Number.ToString();
                var stone1 = long.Parse(stringNumber[..(stringNumber.Length / 2)]);
                var stone2 = long.Parse(stringNumber[(stringNumber.Length / 2)..]);
                stone.Number = stone1;
                stones.AddAfter(new LinkedListNode<Stone>(stone), new Stone { Number = stone2 });
                i++;
            }
            else
            {
                stones[i] *= 2024;
            }
        }
    }

    class Stone
    {
        public long Number { get; set; }
    }

    class MyLinkedList<T>
    {
        
    }
}
