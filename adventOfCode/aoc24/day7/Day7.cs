using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using aocTools;
using aocTools.Neo4J;

namespace aoc24.day7;

public class Day7 : AAocDay {
    private readonly List<Operation> _operations = [];

    public Day7() : base() {
        ReadInput();
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            var parts = line.Split(": ");
            var result = long.Parse(parts[0]);
            var numbers = parts[1].Split(" ").Select(long.Parse).ToList();
            _operations.Add(new Operation(result, numbers));
        }
    }


    public override void PuzzleOne() {
        var res = _operations.Where(o => o.IsSatisfiable()).Sum(operation => operation.Result);
        Console.WriteLine(res);
    }
    public override void PuzzleTwo() {
        var res = _operations.Where(o => o.IsSatisfiable(true)).Sum(operation => operation.Result);
        Console.WriteLine(res);
    }
}

public class Operation {
    public long Result { get; set; }
    public List<long> Numbers { get; set; }

    public Operation(long result, List<long> numbers) {
        Result = result;
        Numbers = numbers;
    }

    public bool IsSatisfiable(bool puzzle2 = false) {
        var operators = new List<string> { "*", "+" };
        if (puzzle2) {
            operators.Add("|");
        }

        var permutations = GenerateOperatorPermutations(Numbers.Count - 1, operators);


        foreach (var permutation in permutations) {
            var result = Numbers[0];
            for (var i = 1; i < Numbers.Count; i++) {
                var op = permutation[i - 1];
                var num = Numbers[i];

                switch (op) {
                    case '+':
                        result += num;
                        break;
                    case '*':
                        result *= num;
                        break;
                    case '|':
                        // string concatenation
                        result = long.Parse(string.Concat(result, num));
                        break;
                }
            }

            if (result == Result) {
                return true;
            }
        }

        return false;
    }

    private static List<string> GenerateOperatorPermutations(int numberOfOperators, List<string> operators) {
        var permutations = new List<string>();

        for (var i = 0; i < Math.Pow(operators.Count, numberOfOperators); i++) {
            var permutation = new StringBuilder();
            var temp = i;
            for (var j = 0; j < numberOfOperators; j++) {
                permutation.Append(operators[temp % operators.Count]);
                temp /= operators.Count;
            }

            permutations.Add(permutation.ToString());
        }

        return permutations;
    }
}