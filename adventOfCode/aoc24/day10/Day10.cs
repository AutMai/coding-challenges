using System.Text;
using aocTools;

namespace aoc24.day10;

public class Day10 : AAocDay {
    private NodeMap<int> _map;

    public Day10() : base() {
        _map = new NodeMap<int>(InputTokens);
    }

    public override void PuzzleOne() {
        var sum = _map.NodeList.Where(n => n.Value == 0).Sum(node => _map.GetTrails(node).Count);
        Console.WriteLine(sum);
    }

    public override void PuzzleTwo() {
        throw new NotImplementedException();
    }
}