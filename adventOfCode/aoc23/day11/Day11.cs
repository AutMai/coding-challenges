using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;

namespace aoc23.day11;

public class Day11 : AAocDay {
    public Day11() {
        ExtendInput();
        ReadInput();
    }

    private void ExtendInput() {
        // detect all lines and columns that contain only dots and double them

        var newInput = new TokenList(new List<string>());
        var lines = InputTokens.ToList();
        for (var i = 0; i < lines.Count; i++) {
            var line = lines[i];
            if (line.All(c => c == '.')) {
                newInput.Add(line);
                newInput.Add(line);
            }
            else {
                newInput.Add(line);
            }
        }

        var columns = new TokenList(new List<string>());
        for (var i = 0; i < newInput[0].Length; i++) {
            var column = new StringBuilder();
            for (var j = 0; j < newInput.Count; j++) {
                column.Append(newInput[j][i].ToString());
            }

            columns.Add(column.ToString());
        }

        // for each column that contains only dots -> add to newInput (get index and add dots to this index)
    var alreadyInsertedColumnCount = 0;
        for (var i = 0; i < columns.Count; i++) {
            var column = columns[i];
            if (column.All(c => c == '.')) {
                for (var j = 0; j < newInput.Count; j++) {
                    var line = newInput[j];
                    var newLine = new StringBuilder(line);
                    newLine.Insert(i + alreadyInsertedColumnCount, ".");
                    newInput[j] = newLine.ToString();
                }
                alreadyInsertedColumnCount++;
            }
        }


        InputTokens = newInput;
    }

    private NodeMap<string> _nodeMap;

    private void ReadInput() {
        _nodeMap = new NodeMap<string>(InputTokens);
        // replace all # with ascended numbers
        var counter = 1;
        foreach (var node in _nodeMap.NodeList.Where(node => node.Value == "#")) {
            node.Value = counter.ToString();
            counter++;
        }
    }

    public override void PuzzleOne() {
        List<Tuple<Node<string>,Node<string>>> nodePairs = new();
        var nodes = _nodeMap.NodeList.Where(node => node.Value != ".").ToList();
        // get all permutations of nodes
        for (var i = 0; i < nodes.Count; i++) {
            for (var j = i + 1; j < nodes.Count; j++) {
                nodePairs.Add(new Tuple<Node<string>, Node<string>>(nodes[i], nodes[j]));
            }
        }
        
        //var sumShortestPaths = nodePairs.Sum(pair => _nodeMap.ShortestPathNoDiagonals(pair.Item1, pair.Item2).Count);
        var sumShortestPaths = 0;
        foreach (var pair in nodePairs) {
            var vector1 = new Vector2(pair.Item1.PosX, pair.Item1.PosY);
            var vector2 = new Vector2(pair.Item2.PosX, pair.Item2.PosY);
            var shortestPath = (int) (Math.Abs(vector1.X - vector2.X) + Math.Abs(vector1.Y - vector2.Y));
            sumShortestPaths += shortestPath;
        }
        Console.WriteLine($"Sum of shortest paths: {sumShortestPaths}");
    }

    public override void PuzzleTwo() {
    }
}