using System.Text;
using System.Text.RegularExpressions;
using aocTools;

namespace aoc24.day3;

public class Day3 : AAocDay {
    public Day3() : base() {
        ReadInput();
    }

    private List<(int,int)> _multiplications = [];

    private void ReadInput(bool puzzleTwo = false) {
        _multiplications.Clear();
        var input = new StringBuilder();
        
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            input.Append(line);
        }
        
        if (puzzleTwo) {
            input.Insert(0, "do()");
            input.Append("don't()");
            var inputStr = input.ToString();
            var validBlocks = Regex.Matches(inputStr, "do\\(\\)(.*?)don't\\(\\)");
            
            foreach (Match validBlock in validBlocks) {
                var matches = Regex.Matches(validBlock.Groups[1].Value, "mul\\((\\d{1,3}),(\\d{1,3})\\)");
            
                foreach (Match match in matches) {
                    var x = int.Parse(match.Groups[1].Value);
                    var y = int.Parse(match.Groups[2].Value);
                    _multiplications.Add((x, y));
                }
            }
        }
        else {
            var matches = Regex.Matches(input.ToString(), "mul\\((\\d{1,3}),(\\d{1,3})\\)");
            
            foreach (Match match in matches) {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                _multiplications.Add((x, y));
            }
        }
        
    }

    public override void PuzzleOne() {
        var result = 0;
        
        foreach (var (x, y) in _multiplications) {
            result += x * y;
        }
        
        Console.WriteLine(result);
    }

    public override void PuzzleTwo() {
        ResetInput();
        ReadInput(true);
        
        var result = 0;
        
        foreach (var (x, y) in _multiplications) {
            result += x * y;
        }
        
        Console.WriteLine(result);
    }
}