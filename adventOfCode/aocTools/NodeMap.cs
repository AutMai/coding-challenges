using System.Numerics;
using System.Text;

namespace aocTools;

// create generic class NodeMap that is derived from two-dimensional array of generic type T

public class NodeMap<T> {
    public Node<T>[,] Map { get; set; }
    public List<Node<T>> NodeList { get; set; } = new List<Node<T>>();

    public NodeMap(List<Node<T>> nodeList) {
        NodeList = nodeList;
        // there can be negative coordinates but i want to move them as much to get them all positive
        var minX = nodeList.Min(n => n.PosX);
        var minY = nodeList.Min(n => n.PosY);
        foreach (var n in nodeList) {
            n.PosX -= minX;
            n.PosY -= minY;
        }

        Map = new Node<T>[nodeList.Max(n => n.PosX) + 1, nodeList.Max(n => n.PosY) + 1];
        foreach (var n in nodeList) {
            Map[n.PosX, n.PosY] = n;
        }

        MapNeighbors();
    }

    public bool HasTwoNodesOnSameField() {
        return NodeList.GroupBy(n => new { n.PosX, n.PosY }).Any(g => g.Count() > 1);
    }

    public bool IsValid(NodeMap<char> lawn) {
        var treeNode = lawn.GetNode('X');

        // first check if there are not two nodes on same pos
        if (HasTwoNodesOnSameField()) {
            return false;
        }

        // check if there is not a node where the tree is
        var node = GetNode(treeNode.PosX, treeNode.PosY);
        if (node is not null) {
            return false;
        }

        // check if width and height is not bigger than lawn
        if (lawn.Width < treeNode.PosX || lawn.Height < treeNode.PosY) {
            return false;
        }

        // check if it contains nodes on all lawn nodes that are not the tree
        var nodesToVisit = lawn.GetNodes('.');
        foreach (var nv in nodesToVisit) {
            var n = GetNode(nv.PosX, nv.PosY);
            if (n is null) {
                return false;
            }
        }

        return true;
    }

    public string ConvertPathToInstructions(HashSet<Node<T>> path) {
        if (path.Count == 0) {
            return "";
        }

        var sb = new StringBuilder();
        var index = 0;


        var currentNode = path.ElementAt(index);
        index++;

        while (index < path.Count) {
            // if nextNode is currentNode.Top then add "U" to sb
            // if nextNode is currentNode.Bottom then add "D" to sb
            // if nextNode is currentNode.Left then add "L" to sb
            // if nextNode is currentNode.Right then add "R" to sb

            var nextNode = path.ElementAt(index);
            index++;

            if (nextNode == currentNode.Top) {
                sb.Append("W");
            }
            else if (nextNode == currentNode.Bottom) {
                sb.Append("S");
            }
            else if (nextNode == currentNode.Left) {
                sb.Append("A");
            }
            else if (nextNode == currentNode.Right) {
                sb.Append("D");
            }

            currentNode = nextNode;
        }

        return sb.ToString();
    }


    public NodeMap(int width, int height) {
        Map = new Node<T>[width, height];
    }

    public NodeMap(string[] lines) {
        Map = new Node<T>[lines.Max(l => l.Length), lines.Length];
        for (int y = 0; y < lines.Length; y++) {
            for (int x = 0; x < lines[y].Length; x++) {
                var n = new Node<T>
                    { Value = (T)Convert.ChangeType(lines[y][x].ToString(), typeof(T)), PosX = x, PosY = y };
                Map[x, y] = n;
                NodeList.Add(n);
            }
        }

        MapNeighbors();
    }

    public NodeMap(List<string> lines) {
        Map = new Node<T>[lines.Max(l => l.Length), lines.Count];
        for (int y = 0; y < lines.Count; y++) {
            for (int x = 0; x < lines[y].Length; x++) {
                var n = new Node<T>
                    { Value = (T)Convert.ChangeType(lines[y][x].ToString(), typeof(T)), PosX = x, PosY = y };
                Map[x, y] = n;
                NodeList.Add(n);
            }
        }

        MapNeighbors();
    }

    private void MapNeighbors() {
        // loop through all nodes and add neighbors that are inside the map
        foreach (var n in NodeList) {
            n.Top = IsInside(n.PosX, n.PosY - 1) ? Map[n.PosX, n.PosY - 1] : null;
            n.Bottom = IsInside(n.PosX, n.PosY + 1) ? Map[n.PosX, n.PosY + 1] : null;
            n.Left = IsInside(n.PosX - 1, n.PosY) ? Map[n.PosX - 1, n.PosY] : null;
            n.Right = IsInside(n.PosX + 1, n.PosY) ? Map[n.PosX + 1, n.PosY] : null;

            n.Neighbors = new List<Node<T>> { n.Top, n.Bottom, n.Left, n.Right };
            n.Neighbors.RemoveAll(x => x is null);

            n.TopLeft = IsInside(n.PosX - 1, n.PosY - 1) ? Map[n.PosX - 1, n.PosY - 1] : null;
            n.TopRight = IsInside(n.PosX + 1, n.PosY - 1) ? Map[n.PosX + 1, n.PosY - 1] : null;
            n.BottomLeft = IsInside(n.PosX - 1, n.PosY + 1) ? Map[n.PosX - 1, n.PosY + 1] : null;
            n.BottomRight = IsInside(n.PosX + 1, n.PosY + 1) ? Map[n.PosX + 1, n.PosY + 1] : null;

            n.FullNeighbors = new List<Node<T>>
                { n.Top, n.Bottom, n.Left, n.Right, n.TopLeft, n.TopRight, n.BottomLeft, n.BottomRight };
            n.FullNeighbors.RemoveAll(x => x is null);
        }
    }

    public T this[int x, int y] {
        get => Map[x, y].Value;
        set => Map[x, y].Value = value;
    }

    public Node<T> this[string xy] {
        // xy is a string like "1,2"
        get {
            var x = Convert.ToInt32(xy.Split(',')[0]);
            var y = Convert.ToInt32(xy.Split(',')[1]);
            return Map[x, y];
        }
    }

    public int Width => Map.GetLength(0);
    public int Height => Map.GetLength(1);

    public bool IsInside(int x, int y) {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    // implement dfs and bfs with exclusion of nodes that have a certain value

    public List<Node<T>> Dfs(Node<T> start, Node<T> end, Func<Node<T>, IEnumerable<Node<T>>> neighbors,
        List<T> exclude = null) {
        var visited = new List<Node<T>>();
        var stack = new Stack<Node<T>>();
        stack.Push(start);
        while (stack.Count > 0) {
            var current = stack.Pop();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            if (current == end)
                return visited;
            foreach (var neighbor in neighbors(current)) {
                if (neighbor is null)
                    continue;
                if (exclude is not null && exclude.Contains(neighbor.Value))
                    continue;
                stack.Push(neighbor);
            }
        }

        return null;
    }

    public List<Node<T>> Bfs(Node<T> start, Node<T> end, Func<Node<T>, IEnumerable<Node<T>>> neighbors,
        List<T> exclude = null) {
        var visited = new List<Node<T>>();
        var queue = new Queue<Node<T>>();
        queue.Enqueue(start);
        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            if (current == end)
                return visited;
            foreach (var neighbor in neighbors(current)) {
                if (neighbor is null)
                    continue;
                if (exclude is not null && exclude.Contains(neighbor.Value))
                    continue;
                queue.Enqueue(neighbor);
            }
        }

        return null;
    }

    // implement flood fill algorithm which is a special case of bfs and returns a list of nodes that have a certain value and are connected to a start node

    public HashSet<Node<T>> FloodFill(Node<T> start, List<T>? exclude = null, HashSet<Node<T>>? excludeNodes = null, List<T>? include = null) {
        var visited = new HashSet<Node<T>>();
        var queue = new Queue<Node<T>>();
        queue.Enqueue(start);
        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (!visited.Add(current))
                continue;
            foreach (var neighbor in current.Neighbors) {
                if (neighbor is null)
                    continue;
                // also check if the value of the neighbor is within a certain deviation of the start node
                if (exclude is not null && exclude.Contains(neighbor.Value))
                    continue;
                if (excludeNodes is not null && excludeNodes.Contains(neighbor))
                    continue;
                if (include is not null && !include.Contains(neighbor.Value))
                    continue;
                queue.Enqueue(neighbor);
            }
        }

        return visited;
    }

    // implement shortest path with exclusion of nodes that have a certain value but diagonal neighbors are allowed

    public Tuple<List<Node<T>>, double> ShortestPath(Node<T> start, Node<T> end,
        Func<Node<T>, IEnumerable<Node<T>>> neighbors, Func<Node<T>, int> cost, List<T> exclude = null) {
        var openSet = new HashSet<Node<T>> { start };
        var cameFrom = new Dictionary<Node<T>, Node<T>>();
        var gScore = new Dictionary<Node<T>, double> { { start, 0 } };
        var fScore = new Dictionary<Node<T>, double> { { start, Heuristic(start, end) } };

        while (openSet.Count > 0) {
            var current = openSet.OrderBy(node => fScore[node]).First();

            if (current == end) {
                return new Tuple<List<Node<T>>, double>(ReconstructPath(cameFrom, end), gScore[end]);
            }

            openSet.Remove(current);

            foreach (var neighbor in neighbors(current)) {
                if (neighbor is null || (exclude != null && exclude.Contains(neighbor.Value))) {
                    continue;
                }

                var tentativeGScore = gScore[current] + cost(neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, end);

                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // No path found
    }

    private double Heuristic(Node<T> a, Node<T> b) {
        // You can implement a heuristic function (e.g., Euclidean distance) here.
        // For example:
        // return Math.Sqrt(Math.Pow(a.PosX - b.PosX, 2) + Math.Pow(a.PosY - b.PosY, 2));
        return 0;
    }

    private List<Node<T>> ReconstructPath(Dictionary<Node<T>, Node<T>> cameFrom, Node<T> current) {
        var path = new List<Node<T>> { current };
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }

    // implement a algorithm that finds a path that visits every node exactly once on a graph and also chooses the start node and end node itself

    public List<Node<T>> FindPath() {
        var pathShouldContain = NodeList.Where(n => n.Value is not 'X').ToList();
        var edgeNodes = NodeList.Where(n => n.Neighbors.Count != 4).ToList();

        foreach (var n in edgeNodes) {
            // check if the node is not a tree node continue
            if (n.Value.Equals('X')) {
                continue;
            }

            for (int i = 0; i < 8; i++) {
                var path = FindPath(n, i);
                if (path.Count == pathShouldContain.Count)
                    return path;
            }
        }

        return null;
    }

    private List<Node<T>> FindPath(Node<T> startNode, int order) {
        // the idea is to start at a node and then choose a neighbor that has not been visited yet (priority: top, right, bottom, left)
        // but check if the step would cause the map to be split into two parts (if so, then choose another neighbor)
        // if there are no neighbors left then go back to the previous node and choose another neighbor

        var visited = new List<Node<T>>();
        var stack = new Stack<Node<T>>();
        var current = startNode;
        stack.Push(current);
        visited.Add(current);
        while (stack.Count > 0) {
            var neighbors = current.GetNeighborOrder(order);
            var next = neighbors.FirstOrDefault(n => !visited.Contains(n));
            if (next is null) {
                return visited;
                current = stack.Pop();
            }
            else {
                stack.Push(next);
                visited.Add(next);
                current = next;
            }
        }

        return visited;
    }

    // lets implement a method that finds a hamiltonian path cycle on a graph

    public HashSet<Node<T>> FindHamiltonianCycle(Func<Node<T>, IEnumerable<Node<T>>> neighborDefinition,
        List<T> exclude = null) {
        var excludedNodeList = NodeList.Where(n => exclude is null || !exclude.Contains(n.Value)).ToList();
        var startNode = excludedNodeList.First();

        // nodelist count without excluded
        var nodeListCount = excludedNodeList.Count;

        var cycleFound = false;

        var visitedNeighbors = new Dictionary<Node<T>, HashSet<Node<T>>>();

        var path = new HashSet<Node<T>>() { startNode };

        var mem = new HashSet<StateMem>();

        while (!cycleFound) {
            //Console.WriteLine(ConvertPathToInstructions(path.ToList()));
            var current = path.Last();

            var neighbors = neighborDefinition(current);
            var notVisitedNeighbors = neighbors.Where(n =>
                !path.Contains(n) &&
                (!visitedNeighbors.ContainsKey(current) || !visitedNeighbors[current].Contains(n)));


            // next node is from notVisitedNeighbors the node with the least neighbors that are not in path
            var next = notVisitedNeighbors.MinBy(n => neighborDefinition(n).Count(nn => !path.Contains(nn)));

            if (next is null) {
                // no options left -> go back to previous node

                path.Remove(current);
                visitedNeighbors.Remove(current);

                if (path.Count == 0)
                    throw new Exception("PATH NOT FOUND!");
            }
            else {
                path.Add(next);

                if (path.Count == nodeListCount) {
                    if (neighborDefinition(next).Contains(startNode)) {
                        cycleFound = true;
                    }
                    else {
                        // visited each node exactly once but the last node does not have the start node as a neighbor
                        path.Remove(next);
                        path.Remove(current);
                        visitedNeighbors.Remove(next);
                        visitedNeighbors.Remove(current);
                    }
                }
                else {
                    if (!visitedNeighbors.ContainsKey(current)) {
                        visitedNeighbors[current] = new HashSet<Node<T>>();
                    }

                    visitedNeighbors[current].Add(next);

                    // SOME CHECKS FOR BACKTRACKING

                    // 1. we need to check if there is a state that we already visited
                    var memState = new StateMem { Path = new HashSet<Node<T>>(path), Current = next };
                    // if the state is already in memory then we do not need to check further and can go back to the previous node
                    var foundMem = mem.FirstOrDefault(m =>
                        m.Path.Count == path.Count && m.Path.All(p => path.Contains(p)) && m.Current == next);

                    if (foundMem is not null) {
                        path.Remove(next);
                        visitedNeighbors.Remove(next);
                        continue;
                    }
                    else {
                        mem.Add(memState);
                    }

                    // 2. we need to check if we blocked the last possible path to the start node
                    var closingNodes = neighborDefinition(startNode).Where(snn => !path.Contains(snn)).ToList();
                    if (closingNodes.Count == 1) {
                        var closingNode = closingNodes.First();
                        //var freeNeighborsOfNextNode = neighborDefinition(next).Count(nn => !path.Contains(nn));
                        var ff = FloodFill(closingNode, exclude, path);
                        if (ff.Count + path.Count != nodeListCount) {
                            path.Remove(next);
                            visitedNeighbors.Remove(next);
                            //Console.WriteLine(ConvertPathToInstructions(path));
                            continue;
                        }
                    }

                    if (closingNodes.Count == 0) {
                        path.Remove(next);
                        visitedNeighbors.Remove(next);
                        continue;
                    }

                    // 3. we need to check if there is a node that has only one neighbor left that is not in the path and that is not adjacent to the current node
                    var singleNeighborNodes = excludedNodeList.Where(n =>
                        !path.Contains(n) && neighborDefinition(n).Count(nn => !path.Contains(nn)) == 1).ToList();
                    // remove closingNodes from singleNeighborNodes
                    singleNeighborNodes.RemoveAll(snn => closingNodes.Contains(snn));
                    // remove adjacent nodes from current node
                    var currentNeighbors = neighborDefinition(next).Where(n => !path.Contains(n)).ToList();
                    singleNeighborNodes.RemoveAll(snn => currentNeighbors.Contains(snn));
                    // remove current node from singleNeighborNodes
                    singleNeighborNodes.Remove(next);

                    // if there are nodes left then we need to go back to the previous node
                    if (singleNeighborNodes.Count > 0) {
                        //Console.WriteLine(ConvertPathToInstructions(path));
                        path.Remove(next);
                        visitedNeighbors.Remove(next);
                        continue;
                    }

                    // 4. we need to check if there is a narrow path that is only one node wide (the nodes behind the narrow path cannot be visited because you cannot go back then)
                    Console.WriteLine(ConvertPathToInstructions(path));
                    var narrowPaths = excludedNodeList.Where(n =>
                        !path.Contains(n) && neighborDefinition(n).Count(nn => !path.Contains(nn)) == 2).ToList();
                    // remove closingNodes from narrowPaths
                    narrowPaths.RemoveAll(np => closingNodes.Contains(np));
                    // remove adjacent nodes from current node
                    narrowPaths.RemoveAll(np => currentNeighbors.Contains(np));
                    
                    foreach (var np in narrowPaths) {
                        var neighbor1 = neighborDefinition(np).First(n => !path.Contains(n));
                        var neighbor2 = neighborDefinition(np).Last(n => !path.Contains(n));
                        // floodfill from neighbor1 and from neighbor2
                        // one of the floodfills should contain a closing node, the other one should contain a neighbor of the current node
                        var excludeNodes = new HashSet<Node<T>>(path) { np };
                        var ff1 = FloodFill(neighbor1, exclude, excludeNodes);
                        var ff2 = FloodFill(neighbor2, exclude, excludeNodes);
                        if (ff1.Any(f => closingNodes.Contains(f)) && ff2.Any(f => currentNeighbors.Contains(f)) || ff2.Any(f => closingNodes.Contains(f)) && ff1.Any(f => currentNeighbors.Contains(f))) {
                            
                        }
                        else {
                            Console.WriteLine(ConvertPathToInstructions(path));
                            path.Remove(next);
                            visitedNeighbors.Remove(next);
                            continue;
                        }
                    }
                }
            }
        }

        return path;
    }
    
    public void PrintPath(Node<T> start, Node<T> end, List<Node<T>> path) {
        // print the whole map and mark the path from start to end

        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                var n = Map[x, y];
                if (n == start)
                    Console.BackgroundColor = ConsoleColor.Green;
                else if (n == end)
                    Console.BackgroundColor = ConsoleColor.Red;
                else if (path.Contains(n))
                    Console.BackgroundColor = ConsoleColor.Blue;

                Console.Write(n.Value);

                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine();
        }
    }

    public void PrintPath(List<Node<T>> path, bool directional = false) {
        // print the whole map and mark the path from start to end

        if (directional) {
            for (int i = 0; i < path.Count - 1; i++) {
                var current = path[i];
                var next = path[i + 1];
                if (current.PosX == next.PosX) {
                    if (current.PosY > next.PosY) {
                        current.Value = (T)Convert.ChangeType('U', typeof(T));
                    }
                    else {
                        current.Value = (T)Convert.ChangeType('D', typeof(T));
                    }
                }
                else {
                    if (current.PosX > next.PosX) {
                        current.Value = (T)Convert.ChangeType('L', typeof(T));
                    }
                    else {
                        current.Value = (T)Convert.ChangeType('R', typeof(T));
                    }
                }
            }
        }

        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                var n = Map[x, y];
                if (path.Contains(n))
                    Console.BackgroundColor = ConsoleColor.Blue;

                Console.Write(n.Value);

                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine();
        }
    }

    public Node<T>? GetNode(int x, int y) {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return null;
        
        return Map[x, y];
    }
    public Node<T>? GetNode(Vector2 pos) => GetNode((int)pos.X, (int)pos.Y);

    public Node<T> GetNode(T value) => NodeList.FirstOrDefault(n => n.Value.Equals(value));

    public List<Node<T>> GetNodes(T value) => NodeList.Where(n => n.Value.Equals(value)).ToList();

    public override string ToString() {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Map[x, y] is null)
                    sb.Append(" ");
                else
                    sb.Append(Map[x, y].Value);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private class StateMem : IEquatable<StateMem> {
        public HashSet<Node<T>> Path { get; set; }
        public Node<T> Current { get; set; }

        // equals
        // equals should check if the nodes in the path are the same (order does not matter) and if the current node is the same
        public bool Equals(StateMem other) {
            if (other is null) {
                return false;
            }

            return Path.Count == other.Path.Count && Path.All(p => other.Path.Contains(p)) && Current == other.Current;
        }

        // gethashcode
        // gethashcode should return the hashcode of the path and the current node
        public override int GetHashCode() {
            return Path.GetHashCode() ^ Current.GetHashCode();
        }
    }

    public void PrintMap() {
        for (var y = 0; y < Height; y++) {
            for (var x = 0; x < Width; x++) {
                Console.Write(Map[x, y].Value);
            }

            Console.WriteLine();
        }
    }
}