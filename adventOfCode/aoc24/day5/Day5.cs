using System.Text;
using System.Text.RegularExpressions;
using aocTools;
using aocTools.Neo4J;

namespace aoc24.day5;

public class Day5 : AAocDay {
    private HashSet<Tuple<int, int>> _pageOrderingRules = [];
    private List<List<int>> _updates = [];

    public Day5() : base() {
        ReadInput();
    }

    public override void PuzzleOne() {
        var res = _updates.Where(UpdateValid).Select(u => u[u.Count / 2]).Sum();
        Console.WriteLine(res);
    }

    private bool UpdateValid(List<int> update) {
        for (var i = 0; i < update.Count; i++) {
            var page = update[i];
            var successorPages = update.Skip(i + 1).ToList();
            if (successorPages.Any(sp => !_pageOrderingRules.Contains(new Tuple<int, int>(page, sp)))) {
                return false;
            }
        }
        
        return true;
    }

    private void ReadInput() {
        while (InputTokens.JustRead() != "") {
            var instr = InputTokens.Read().Split("|");
            _pageOrderingRules.Add(new Tuple<int, int>(int.Parse(instr[0]), int.Parse(instr[1])));
        }
        InputTokens.Remove(1);

        while (InputTokens.HasMoreTokens()) {
            var u = InputTokens.Read().Split(",");
            _updates.Add(u.Select(int.Parse).ToList());
        }
    }

    public override void PuzzleTwo() {
        var invalidUpdates = _updates.Where(u => !UpdateValid(u)).ToList();
        var res = invalidUpdates.Sum(FixUpdate);
        Console.WriteLine(res);
    }

    private int FixUpdate(List<int> update) {
        var validUpdate = new List<int>();
        for (var i = 0; i < update.Count + validUpdate.Count; i++) {
            // now we find each element separately. first element is the one that has all other elements as successors, then second element is the one that has all other elements except the first one as successors, etc.
            var nextPage = update.First(p => update.Except(new List<int> {p}).All(sp => _pageOrderingRules.Contains(new Tuple<int, int>(p, sp))));
            //Console.WriteLine($"Next page: {nextPage}");
            validUpdate.Add(nextPage);
            update.Remove(nextPage);
        }
        
        return validUpdate[validUpdate.Count / 2];
    }
}