namespace AdventOfCode.Puzzles;

public class Puzzle14 : PuzzleBase
{
    protected override string Solution(string input)
    {
        var equations = input.Split('\n')
            .Select(line => Equation.Parse(line.Trim()))
            .ToArray();

        long result = 0;

        foreach (var equation in equations)
        {
            var permutator = new OperationPermutator(equation.Values.Length - 1);

            while (permutator.TryGetNextPermutation(out var operations))
            {
                if (equation.Evaluate(operations) == equation.Result)
                {
                    result += equation.Result;
                    break;
                }
            }
        }

        return result.ToString();
    }

    class Equation
    {
        public required long Result { get; init; }
        public required long[] Values { get; init; }

        public long Evaluate(Operation[] operations)
        {
            var result = Values[0];

            for (var i = 0; i < operations.Length; i++)
            {
                var operation = operations[i];

                result = operation switch
                {
                    Operation.Add => result + Values[i + 1],
                    Operation.Multiply => result * Values[i + 1],
                    Operation.Concatenate => long.Parse(result.ToString() + Values[i + 1].ToString()),
                    _ => throw new Exception("Invalid operation."),
                };
            }

            return result;
        }

        public static Equation Parse(string input)
        {
            var parts = input.Split();
            
            return new Equation
            {
                Result = long.Parse(parts[0][..^1]),
                Values = parts.Skip(1).Select(long.Parse).ToArray(),
            };
        }
    }

    class OperationPermutator
    {
        readonly Operation[] _permutation;
        readonly int _count;

        public OperationPermutator(int count)
        {
            _permutation = new Operation[count + 1];
            _count = count;
        }

        public bool TryGetNextPermutation(out Operation[] result)
        {
            if (_permutation.Last() != 0)
            {
                result = [];
                return false;
            }

            result = new Operation[_count];
            Array.Copy(_permutation, 0, result, 0, _count);

            NextPermutation();

            return true;
        }

        void NextPermutation()
        {
            var index = 0;

            while (++_permutation[index] == (Operation)3)
            {
                _permutation[index] = 0;
                index++;
            }
        }
    }

    enum Operation
    {
        Add,
        Multiply,
        Concatenate,
    }
}
