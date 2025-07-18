using System.ComponentModel;

namespace AdventOfCode.Puzzles;

public class Puzzle10 : PuzzleBaseWithProgress
{
    protected override string Solution(string input)
    {
        var lines = input
            .Split('\n')
            .Select(line => line.Trim())
            .ToArray();

        List<(int Prevoius, int Next)> rules = new();
        List<List<int>> updates = new();

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
                    .ToList());
            }
        }

        var sum = 0;

        foreach (var update in updates)
        {
            if (!IsValidUpdate(update, rules))
            {
                while (!IsValidUpdate(update, rules))
                {
                    Correct(update, rules);
                }

                sum += update[update.Count / 2];
            }
        }

        return sum.ToString();
    }

    static void Correct(List<int> update, List<(int Prevoius, int Next)> rules)
    {
        foreach (var rule in rules)
        {
            var indexOfPrevoius = update.IndexOf(rule.Prevoius);
            var indexOfNext = update.IndexOf(rule.Next);

            if (update.Contains(rule.Prevoius) && update.Contains(rule.Next) && 
                indexOfPrevoius > indexOfNext)
            {
                update.RemoveAt(indexOfPrevoius);
                update.Insert(indexOfNext, rule.Prevoius);
            }
        }
    }

    static bool IsValidUpdate(List<int> update, List<(int Prevoius, int Next)> rules)
    {
        foreach (var rule in rules)
        {
            if (update.Contains(rule.Prevoius) && update.Contains(rule.Next) && 
                update.IndexOf(rule.Prevoius) > update.IndexOf(rule.Next))
            {
                return false;
            }
        }

        return true;
    }
}
