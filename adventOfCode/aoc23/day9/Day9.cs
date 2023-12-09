using System.Collections.Concurrent;
using aocTools;
using aocTools.Interval;

namespace aoc23.day9;

public class Day9 : AAocDay {
    public Day9() {
        ReadInput();
    }

    HashSet<List<int>> Values = new();
    
    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read().Split(" ");
            Values.Add(line.Select(int.Parse).ToList());
        }
    }

    public override void PuzzleOne() {
        var sum = Values.Sum(ProcessValuesForward);
        Console.WriteLine(sum);
    }
    
    private int ProcessValuesForward(List<int> values) {
        
        // create a new list with the differences between each value
        var diffs = new List<int>();
        for (var i = 0; i < values.Count - 1; i++) {
            diffs.Add(values[i + 1] - values[i]);
        }
        // if all differences are 0 return the last value
        if (diffs.All(d => d == 0)) {
            return values.Last();
        }
        
        var remainingValue = ProcessValuesForward(diffs);
        
        return values.Last() + remainingValue;
    }
    
    private int ProcessValuesBackward(List<int> values) {
        // create a new list with the differences between each value
        var diffs = new List<int>();
        for (var i = 0; i < values.Count - 1; i++) {
            diffs.Add(values[i + 1] - values[i]);
        }
        // if all differences are 0 return the last value
        if (diffs.All(d => d == 0)) {
            return values.Last();
        }
        
        var remainingValue = ProcessValuesBackward(diffs);
        
        return values.First() - remainingValue;
    }

    public override void PuzzleTwo() {
        var sum = Values.Sum(ProcessValuesBackward);
        Console.WriteLine(sum);
    }
}