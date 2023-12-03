using System.Text;
using aocTools;

namespace day14;

public class Day14 : AAocDay {
    public Day14() : base(true) {
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
        ReadInput();
    }

    Dictionary<string, string> _pairInsertionRules = new();
    string _polymerTemplate = "";

    private void ReadInput() {
        _polymerTemplate = InputTokens.Read();
        while (InputTokens.HasMoreTokens()) {
            var from = InputTokens.Read();
            InputTokens.Remove(1);
            var to = from[0] + InputTokens.Read() + from[1];
            _pairInsertionRules.Add(from, to);
        }
    }

    public override void PuzzleOne() {
        var polymer = _polymerTemplate;
        for (int i = 0; i < 10; i++) {
            StartPolymerization(ref polymer);
            Console.WriteLine($"After {i + 1} iterations:\t{polymer.Length}");
        }

        var leastCommonElementQuantity = polymer.GroupBy(x => x).OrderBy(x => x.Count()).First().Count();
        var mostCommonElementQuantity = polymer.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Count();
        Console.WriteLine(mostCommonElementQuantity - leastCommonElementQuantity);
    }

    private void StartPolymerization(ref string polymer) {
        var newPolymer = new StringBuilder();
        for (int i = 0; i < polymer.Length - 1; i++) {
            var p = polymer.Substring(i, 2);
            newPolymer.Append(_pairInsertionRules.TryGetValue(p, out var res) ? res[..^1] : polymer[..^1]);
        }

        newPolymer.Append(polymer[^1]);
        polymer = newPolymer.ToString();
    }

    Dictionary<string, long> _polymerCount = new();
    Dictionary<char, long> _monomerCount = new();

    public override void PuzzleTwo() {
        SetupForPuzzleTwo();
        StartPolymerization(40);
        var leastCommonElementQuantity = _monomerCount.OrderBy(x => x.Value).First().Value;
        var mostCommonElementQuantity = _monomerCount.OrderByDescending(x => x.Value).First().Value;
        Console.WriteLine("After 40 iterations:");
        Console.WriteLine(mostCommonElementQuantity - leastCommonElementQuantity);
    }

    private void SetupForPuzzleTwo() {
        for (int i = 0; i < _polymerTemplate.Length - 1; i++) {
            var p = _polymerTemplate.Substring(i, 2);
            if (!_polymerCount.ContainsKey(p)) {
                _polymerCount.Add(p, 1);
            }
            else {
                _polymerCount[p]++;
            }
        }

        for (int i = 0; i < _polymerTemplate.Length; i++) {
            var p = _polymerTemplate[i];
            if (!_monomerCount.ContainsKey(p)) {
                _monomerCount.Add(p, 1);
            }
            else {
                _monomerCount[p]++;
            }
        }
    }

    private void StartPolymerization(int iterations) {
        for (var i = 0; i < iterations; i++) {
            var newPolymerCount = new Dictionary<string, long>();
            foreach (var polymer in _polymerCount) {
                var newPolymer = _pairInsertionRules[polymer.Key];
                if (!_monomerCount.ContainsKey(newPolymer[1])) {
                    _monomerCount.Add(newPolymer[1], polymer.Value);
                }
                else {
                    _monomerCount[newPolymer[1]] += polymer.Value;
                }

                var newPolymers = new List<string>() { newPolymer.Substring(0, 2), newPolymer.Substring(1, 2) };
                foreach (var newP in newPolymers) {
                    if (!newPolymerCount.ContainsKey(newP))
                        newPolymerCount.Add(newP, polymer.Value);
                    else
                        newPolymerCount[newP] += polymer.Value;
                }
            }
            
            _polymerCount = newPolymerCount;
        }
    }
}