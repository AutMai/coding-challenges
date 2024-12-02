using aocTools;

namespace aoc24.day1;

public class Day1 : AAocDay {
    public Day1() : base() {
        ReadInput();
    }

    List<int> left = new List<int>();
    List<int> right = new List<int>();

    private void ReadInput() {
        left.Clear();
        right.Clear();
        
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            var split = line.Split("   ");
            var a = int.Parse(split[0]);
            var b = int.Parse(split[1]);
            left.Add(a);
            right.Add(b);
        }
    }

    public override void PuzzleOne() {
        // sort both lists
        left.Sort();
        right.Sort();
        
        // now loop both list simultaneously, calculate the delta between two elements and sum all up
        var x = 0;
        for (var i = 0; i < left.Count; i++) {
            x += Math.Abs(left[i] - right[i]);
        } 
        Console.WriteLine(x);
    }


    public override void PuzzleTwo() {
        ResetInput();
        ReadInput();
        
        var res = 0;

        foreach (var e in left) {
            // check how often e is in right
            var count = right.Count(x => x == e);
            res += count * e;
        }
        
        Console.WriteLine(res);
    }
}