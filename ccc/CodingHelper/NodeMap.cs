using System.Numerics;
using System.Text;

namespace CodingHelper;

// create generic class NodeMap that is derived from two-dimensional array of generic type T

public class NodeMap<T> {
    public Node<T>[,] Map { get; set; }
    public List<Node<T>> NodeList { get; set; } = new List<Node<T>>();

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

    public List<Node<T>> DFS(Node<T> start, Node<T> end, List<T> exclude = null) {
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
            foreach (var neighbor in current.Neighbors) {
                if (neighbor is null)
                    continue;
                if (exclude is not null && exclude.Contains(neighbor.Value))
                    continue;
                stack.Push(neighbor);
            }
        }

        return null;
    }

    public List<Node<T>> BFS(Node<T> start, Node<T> end, List<T> exclude = null) {
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
            foreach (var neighbor in current.Neighbors) {
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

    public List<Node<int>> FloodFill(Node<int> start, List<int> exclude) {
        var visited = new List<Node<int>>();
        var queue = new Queue<Node<int>>();
        queue.Enqueue(start);
        while (queue.Count > 0) {
            var current = queue.Dequeue();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            foreach (var neighbor in current.Neighbors) {
                if (neighbor is null)
                    continue;
                // also check if the value of the neighbor is within a certain deviation of the start node
                if (exclude is not null && exclude.Contains(neighbor.Value))
                    continue;
                queue.Enqueue(neighbor);
            }
        }

        return visited;
    }

    // implement same dfs and bfs but with full neighbors instead of just neighbors (diagonals included) 

    public List<Node<T>> DFSFull(Node<T> start, Node<T> end, List<T> exclude = null) {
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
            foreach (var neighbor in current.FullNeighbors) {
                if (neighbor is null)
                    continue;
                if (exclude is not null && exclude.Contains(neighbor.Value))
                    continue;
                stack.Push(neighbor);
            }
        }

        return null;
    }

    public List<Node<T>> BFSFull(Node<T> start, Node<T> end, List<T> exclude = null) {
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
            foreach (var neighbor in current.FullNeighbors) {
                if (neighbor is null)
                    continue;
                if (exclude is not null && exclude.Contains(neighbor.Value))
                    continue;
                queue.Enqueue(neighbor);
            }
        }

        return null;
    }

    // implement shortest path with exclusion of nodes that have a certain value but diagonal neighbors are allowed

    public Tuple<List<Node<T>>,double> ShortestPath(Node<T> start, Node<T> end, Func<Node<T>, IEnumerable<Node<T>>> neighbors, Func<Node<T>, int> cost, List<T> exclude = null) {
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
    
    public void PrintPath(List<Node<T>> path) {
        // print the whole map and mark the path from start to end

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

    public Node<T> GetNode(int x, int y) => Map[x, y];
    public Node<T> GetNode(Vector2 pos) => Map[(int)pos.X, (int)pos.Y];

    public override string ToString() {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                sb.Append(Map[x, y].Value);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}