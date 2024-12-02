using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;

namespace aoc23.day21;

public class Day21 : AAocDay {
    private NodeMap<char> Map;

    public Day21() {
        Map = new NodeMap<char>(InputTokens);
    }

    public override void PuzzleOne() {
        var start = Map.NodeList.Single(n => n.Value == 'S');
        int steps = 64;
        var targetNodes = new HashSet<Node<char>>();
        targetNodes.Add(start);
        for (int i = 1; i <= steps; i++) {
            var newTargetNodes = new HashSet<Node<char>>();
            foreach (var targetNode in targetNodes) {
                var newNodes = targetNode.Neighbors.Where(n => n.Value != '#');
                newTargetNodes.UnionWith(newNodes);
            }

            //Console.WriteLine("Step " + i + ": " + newTargetNodes.Count);
            targetNodes = newTargetNodes;
        }

        Console.WriteLine("Target nodes: " + targetNodes.Count);
    }

    public override void PuzzleTwo() {
        // extend input from 1x1 to 5x5
        // begin with adding to the right
        var lines = InputTokens.ToList();
        var m = 3;
        for (int i = 0; i < lines.Count; i++) {
            var x = new StringBuilder().Insert(0, lines[i], m);
            lines[i] = x.ToString();
        }

        // multiply list by 5
        var newLines = new List<string>();
        for (int i = 0; i < m; i++) {
            newLines.AddRange(lines);
        }

        // replace all S with . and add S to the middle
        var middleIdx = (newLines.Count - 1) / 2;
        for (int i = 0; i < newLines.Count(); i++) { 
            newLines[i] = newLines[i].Replace('S', '.');
        }
        
        newLines[middleIdx] = newLines[middleIdx].Substring(0, middleIdx) + "S" + newLines[middleIdx].Substring(middleIdx + 1);
        
        // write the lines to the input file
        var sb = new StringBuilder();
        foreach (var line in newLines) {
            sb.AppendLine(line);
        }

        Helper.WriteFile(Path.Join(GetType().Name.ToLower(), "input2.txt"), sb.ToString());

        Map = new NodeMap<char>(newLines);
        
        var start = Map.NodeList.Single(n => n.Value == 'S');
        int steps = 65 + 131 + 131;
        var targetNodes = new HashSet<Node<char>>();
        targetNodes.Add(start);
        for (int i = 1; i <= steps; i++) {
            var newTargetNodes = new HashSet<Node<char>>();
            foreach (var targetNode in targetNodes) {
                var newNodes = targetNode.Neighbors.Where(n => n.Value != '#');
                newTargetNodes.UnionWith(newNodes);
            }

            //Console.WriteLine("Step " + i + ": " + newTargetNodes.Count);
            targetNodes = newTargetNodes;
        }

        Console.WriteLine("Target nodes: " + targetNodes.Count);
    }
}