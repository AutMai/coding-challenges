using aocTools;

namespace aoc24.day2;

public class Day2 : AAocDay {
    public Day2() : base() {
        ReadInput();
    }

    private List<List<int>> _reports = [];

    private void ReadInput() {
        _reports.Clear();
        
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            var split = line.Split(" ");
            // convert to int
            var levels = split.Select(int.Parse).ToList();
            _reports.Add(levels);
        }
    }

    public override void PuzzleOne() {
        var safeCount = _reports.Count(Safe);
        Console.WriteLine(safeCount);
    }

    private static bool Safe(List<int> report) {
        // if the list contains two times the same element => not safe
        if (report.GroupBy(x => x).Any(g => g.Count() > 1)) {
            return false;
        }
        
        // if the list is sorted (either asc or desc) => safe
        if (report.OrderBy(x => x).SequenceEqual(report) || report.OrderByDescending(x => x).SequenceEqual(report)) {
            // max delta between each element and its neighbor is 2
            var safe = true;
            for (var i = 0; i < report.Count - 1; i++) {
                if (Math.Abs(report[i] - report[i + 1]) > 3) {
                    safe = false;
                }
            }

            return safe;
        }
        
        return false;
    }

    private static bool AdvancedSafe(List<int> report) {
        // remove 1 element and check if the list is safe
        // try all possible combinations
        for (var i = 0; i < report.Count; i++) {
            var copy = new List<int>(report);
            copy.RemoveAt(i);
            if (Safe(copy)) {
                return true;
            }
        }
        
        return false;
    }


    public override void PuzzleTwo() {
        var safeCount = _reports.Count(AdvancedSafe);
        Console.WriteLine(safeCount);
    }
}