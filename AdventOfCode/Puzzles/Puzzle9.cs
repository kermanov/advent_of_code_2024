using System;

namespace AdventOfCode.Puzzles;

public class Puzzle9 : PuzzleBase
{
    protected override string Solution(string input)
    {
        var lines = input
            .Split('\n')
            .Select(line => line.Trim())
            .ToArray();

        List<(int Prevoius, int Next)> rules = new();
        List<int[]> updates = new();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.Contains('|'))
            {
                var numbers = line.Split('|');
                rules.Add((int.Parse(numbers[0]), int.Parse(numbers[1])));
            }
            else 
            {
                updates.Add(line
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray());
            }
        }

        var sum = 0;

        foreach (var update in updates)
        {
            if (IsValidUpdate(update, rules))
            {
                sum += update[update.Length / 2];
            }
        }

        return sum.ToString();
    }

    static bool IsValidUpdate(int[] update, List<(int Prevoius, int Next)> rules)
    {
        foreach (var rule in rules)
        {
            if (update.Contains(rule.Prevoius) && update.Contains(rule.Next) && 
                Array.IndexOf(update, rule.Prevoius) > Array.IndexOf(update, rule.Next))
            {
                return false;
            }
        }

        return true;
    }
}
