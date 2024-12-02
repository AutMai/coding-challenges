using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;

namespace aoc23.day17;

public class Day17 : AAocDay {
    NodeMap<int> cityMap;

    public Day17() {
        cityMap = new NodeMap<int>(InputTokens);
    }


    public override void PuzzleOne() {
        var start = cityMap.GetNode(0, 0);
        // end node is bottom right corner
        var end = cityMap.GetNode(cityMap.Width - 1, cityMap.Height - 1);
        var path = cityMap.ShortestPath(start, end, n => n.Neighbors, n => n.Value);
        cityMap.PrintPath(start,end,path.Item1);
        Console.WriteLine($"Shortest path is {path.Item2}");
    }

    public override void PuzzleTwo() {
    }
}