using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

namespace aoc23.day23;

public class Day23 : AAocDay {
    private NodeMap<char> Map;

    public Day23() {
        Map = new NodeMap<char>(InputTokens);
    }

    public override void PuzzleOne() {
        var startNode = Map.NodeList.Single(n => n is { PosY: 0, Value: '.' });
        var end = Map.NodeList.Single(n => n.PosY == Map.Height - 1 && n.Value == '.');
        var path = LongestPath(startNode,end,new List<char>() {'#'});
        Map.PrintPath(startNode, end, path);
        Console.WriteLine($"P1: Longest path length: {path.Count-1}");
    }
    
    public List<Node<char>> LongestPath(Node<char> start, Node<char> end, List<char>? exclude = null) {
        // find longest path from start to end
        // the only condition is that there are nodes with >, <, ^, v in the path
        // these nodes can only be traversed in the direction of the arrow
        // each node can only be visited once
        // # is a wall and cannot be traversed
        // . is a floor and can be traversed
        
        var visited = new HashSet<Node<char>>();
        var currentPath = new List<Node<char>>();
        var longestPath = new List<Node<char>>();

        // Helper function to check if a node is valid to be visited
        bool IsValid(Node<char> node) {
            // check if it moves into the direction of the arrow
            if (node.Value == '>') {
                return node.PosX > currentPath.Last().PosX;
            } else if (node.Value == '<') {
                return node.PosX < currentPath.Last().PosX;
            } else if (node.Value == '^') {
                return node.PosY < currentPath.Last().PosY;
            } else if (node.Value == 'v') {
                return node.PosY > currentPath.Last().PosY;
            }
            return !visited.Contains(node) && (exclude == null || !exclude.Contains(node.Value));
        }

        // Recursive DFS function to find the longest path
        void Dfs(Node<char> current) {
            visited.Add(current);
            currentPath.Add(current);

            if (current == end) {
                // Update the longest path if the current path is longer
                if (currentPath.Count > longestPath.Count) {
                    longestPath.Clear();
                    longestPath.AddRange(currentPath);
                }
            } else {
                // Explore neighbors
                var neighbors = current.Neighbors;

                foreach (var neighbor in neighbors) {
                    if (IsValid(neighbor)) {
                        Dfs(neighbor);
                    }
                }
            }

            // Backtrack
            visited.Remove(current);
            currentPath.RemoveAt(currentPath.Count - 1);
        }

        // Start DFS from the given start node
        Dfs(start);

        return longestPath;
    }

    public override void PuzzleTwo() {
        // turn arrow nodes into normal nodes
        Map.NodeList.ForEach(n => {
            if (n.Value is '>' or '<' or '^' or 'v') {
                n.Value = '.';
            }
        });
        var startNode = Map.NodeList.Single(n => n is { PosY: 0, Value: '.' });
        var end = Map.NodeList.Single(n => n.PosY == Map.Height - 1 && n.Value == '.');
        var path = LongestPath(startNode,end,new List<char>() {'#'});
        Map.PrintPath(startNode, end, path);
        Console.WriteLine($"P2: Longest path length: {path.Count-1}");
    }
}