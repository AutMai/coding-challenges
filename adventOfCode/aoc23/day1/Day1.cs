using aocTools;

namespace aoc23.day1;

public class Day1 : AAocDay {
    public Day1() : base() {
        ReadInput();
    }

    List<string> input = new List<string>();

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            input.Add(InputTokens.Read());
        }
    }

    public override void PuzzleOne() {
        var sum = input.Select(line => line.Where(char.IsDigit))
            .Sum(digits => int.Parse(digits.First() + "" + digits.Last()));
        Console.WriteLine(sum);
    }

    private Dictionary<string, int> digits = new Dictionary<string, int>() {
        { "zero", 0 },
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 }
    };

    private string ReplaceDigits(string line) {
       var result = "";

        while (line.Length > 0) {
            if (char.IsDigit(line[0])) {
                result += line[0];
                line = line[1..];
                continue;
            }

            foreach (var digit in digits.Where(digit => line.StartsWith(digit.Key))) {
                result += digit.Value;
                line = line[1..];
                break;
            }

            line = line[1..];
        }

        return result;
    }

    public override void PuzzleTwo() {
        var x = input.Select(line => ReplaceDigits(line).Where(char.IsDigit)).ToList()
            .Sum(digits => int.Parse(digits.First() + "" + digits.Last()));
        Console.WriteLine(x);
    }
}