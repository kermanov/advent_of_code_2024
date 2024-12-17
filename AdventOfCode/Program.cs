using System.Text;
using AdventOfCode.Puzzles;

var input = File.ReadAllText("D:\\Repos\\advent_of_code_2024\\AdventOfCode\\input.txt", Encoding.UTF8);

PuzzleBase puzzle = new Puzzle22();
await puzzle.Solve(input);