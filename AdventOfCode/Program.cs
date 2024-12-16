using System.Text;
using AdventOfCode.Puzzles;

var input = File.ReadAllText("D:\\projects\\advent_of_code\\input.txt", Encoding.UTF8);

PuzzleBase puzzle = new Puzzle22();
await puzzle.Solve(input);