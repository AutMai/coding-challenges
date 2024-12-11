using aocTools;

namespace aoc24.day10;

public static class NodeMapExtensions {
    public static List<List<int>> GetTrails(this NodeMap<int> map, Node<int> start) {
        List<List<int>> trails = new();
        // we try to find all trails (starting from 0 and ending at 9 - but only that increases by 1 each step)
            var finished = false;
            var exclude = new List<Node<int>>();
            while (!finished) {
                var trail = AdvancedDfs(start, exclude);
                //map.PrintPath(trail);
                if (trail is not null) {
                    trails.Add(trail.Select(n => n.Value).ToList());
                    exclude.Add(trail.Single(n=>n.Value == 9));
                }
                else {
                    finished = true;
                }
            }
        

        return trails;
    }

    private static List<Node<int>>? AdvancedDfs(Node<int> start, List<Node<int>> exclude) {
        var visited = new List<Node<int>>();
        var stack = new Stack<Node<int>>();
        stack.Push(start);
        while (stack.Count > 0) {
            var current = stack.Pop();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            if (current.Value == 9)
                return visited;
            // max diff to neighbor is +1
            foreach (var neighbor in current.Neighbors.Where(n => n.Value - current.Value == 1)) {
                if (neighbor is null)
                    continue;
                if (exclude is not null && exclude.Contains(neighbor))
                    continue;
                stack.Push(neighbor);
            }
        }

        return null;
    }
}