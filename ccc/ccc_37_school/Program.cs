using System.Drawing;
using System.Runtime.CompilerServices;
using CodingHelper;

void SetOutputToFile(string fn) {
    FileStream filestream = new FileStream(
        GetOutputPath("outputs/" + fn),
        FileMode.Create);
    var streamwriter = new StreamWriter(filestream);
    streamwriter.AutoFlush = true;
    Console.SetOut(streamwriter);
}

string GetOutputPath(string fileName) {
    var path = fileName.Replace(".in", "")
        .Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
    path[^2] += "Output";
    Directory.CreateDirectory(InputReader.GetCompletePath(path[^2]));
    return "../../../" + path[^2] + "/" + path[^1] + ".out";
}

List<List<string>> fileContents = new();

List<string> filenames = new();

foreach (string filePath in Directory.GetFiles("../../../files/level4", "*.in", SearchOption.AllDirectories)) {
    fileContents.Add(File.ReadAllLines(filePath).ToList());
    filenames.Add(Path.GetFileName(filePath));
}

var index = 0;

// print out the list of file contents
foreach (var content in fileContents) {
    //SetOutputToFile(filenames[index++]);
    var honeyCombCount = Convert.ToInt32(content[0]);
    // honeycombs are separated by a blank line
    // split content into honeycombs (there are honeyCombCount honeycombs)
    var honeyCombContents = content.Skip(1).Split(b => b == "").ToList();

    foreach (var honeyComb in honeyCombContents) {
        var borderCount = Convert.ToInt32(honeyComb[0]);
        var honeyCombLines = honeyComb.Skip(1).ToList();
        List<List<NodeKonst>> nodes = new();
        for (var i = 0; i < honeyCombLines.Count; i++) {
            nodes.Add(honeyCombLines[i].ToArray().Select((k, index) => new NodeKonst() { Type = k, PosX = index, PosY = i })
                .ToList());
        }

        var mapNodes = (nodes.Select(k => k.ToArray())).ToArray();
        var allNodes = mapNodes.ToNeighborsFull();

        allNodes.Where(k => k.Type == 'W' || k.Type == 'O').ToList().ForEach(node => {
            SwitchConnection(EDirectionKonst.Left);
            SwitchConnection(EDirectionKonst.Right);

            void SwitchConnection(EDirectionKonst dir) {
                var con = node.GetDirection(dir);
                if (con is null)
                    return;
                if (con.NodeKonst.GetDirection(dir) is null)
                    return;

                con.NodeKonst = con.NodeKonst.GetDirection(dir).NodeKonst;
            }

            node.Neighbors.RemoveAll(k => k.NodeKonst.Type == '-');
        });

        // remove all nodes from allnodes that are '-'
        allNodes.RemoveAll(k => k.Type == '-');

        // we have a connected network of nodes, one nodeKonst is of type W (wasp)
        // check with a search algorithm if the wasp reach the outer border of the map
        // if yes, print out the path
        // if no, print out "Impossible"

        // get the wasp nodeKonst
        var waspNode = allNodes.Single(k => k.Type == 'W');

        // get all nodes that are on the outer border (less than 6 neighbors)
        var outerNodes = allNodes.Where(k => k.Neighbors.Count < 6).ToList();
        var path = new List<NodeKonst>();

        var borderPermutations = new List<NodeKonst[]>();


        while (path is not null) {
            var permutation = borderPermutations.First();
            borderPermutations.RemoveAt(0);
            foreach (var n in permutation) {
                allNodes.Single(n2 => n2.PosX == n.PosX && n2.PosY == n.PosY).Type = 'X';
            }
            path = BFS(waspNode, outerNodes);
            // reset the map
            foreach (var n in permutation) {
                allNodes.Single(n2 => n2.PosX == n.PosX && n2.PosY == n.PosY).Type = 'O';
            }
        }
        
        if (path is null)
            Console.WriteLine("TRAPPED");
        else {
            Console.WriteLine("FREE");
        }
    }

    Console.WriteLine();
    Console.WriteLine();
}

static List<NodeKonst> BFS(NodeKonst start, List<NodeKonst> targets) {
    Queue<NodeKonst> queue = new Queue<NodeKonst>();
    HashSet<NodeKonst> visited = new HashSet<NodeKonst>();

    queue.Enqueue(start);
    visited.Add(start);

    while (queue.Count > 0) {
        NodeKonst current = queue.Dequeue();

        if (targets.Contains(current)) {
            return BuildPath(current);
        }

        foreach (Connection neighbor in current.Neighbors) {
            if (!visited.Contains(neighbor.NodeKonst) && neighbor.NodeKonst.Type != 'X') {
                queue.Enqueue(neighbor.NodeKonst);
                visited.Add(neighbor.NodeKonst);
                neighbor.NodeKonst.Parent = current;
            }
        }
    }

    return null;
}

static List<NodeKonst> BuildPath(NodeKonst target) {
    List<NodeKonst> path = new List<NodeKonst>();
    NodeKonst current = target;

    while (current != null) {
        path.Add(current);
        current = current.Parent;
    }

    path.Reverse();
    return path;
}

static List<List<Tuple<int, int>>> GeneratePermutations(string[] grid, int walls) {
    List<Tuple<int, int>> openSpaces = new List<Tuple<int, int>>();

    for (int i = 0; i < grid.Length; i++) {
        for (int j = 0; j < grid[i].Length; j++) {
            if (grid[i][j] == 'O') {
                openSpaces.Add(new Tuple<int, int>(i, j));
            }
        }
    }

    List<List<Tuple<int, int>>> permutations = new List<List<Tuple<int, int>>>();
    GeneratePermutationsRecursive(permutations, new List<Tuple<int, int>>(), openSpaces, 0, walls);

    return permutations;
}

static void GeneratePermutationsRecursive(List<List<Tuple<int, int>>> permutations,
    List<Tuple<int, int>> currentPermutation, List<Tuple<int, int>> openSpaces, int index, int walls) {
    if (currentPermutation.Count == walls) {
        permutations.Add(new List<Tuple<int, int>>(currentPermutation));
        return;
    }

    for (int i = index; i < openSpaces.Count; i++) {
        currentPermutation.Add(openSpaces[i]);
        GeneratePermutationsRecursive(permutations, currentPermutation, openSpaces, i + 1, walls);
        currentPermutation.RemoveAt(currentPermutation.Count - 1);
    }
}
