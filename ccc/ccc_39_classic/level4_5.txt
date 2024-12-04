using System.Text;
using CodingHelper;

var r = new InputReader(5, false, " ", true);


foreach (var l in r.GetInputs()) {
    Console.WriteLine();
    Console.WriteLine(l.Index);
    Console.WriteLine();
    l.SetOutput();

    var nLawn = l.ReadInt();

    for (int i = 0; i < nLawn; i++) {
        var lawnHeight = int.Parse(l.Read().Split(" ")[1]);
        var lines = new List<string>();
        for (int j = 0; j < lawnHeight; j++) {
            lines.Add(l.Read());
        }

        var lawn = new NodeMap<char>(lines);
        var treeNodes = lawn.GetNode('X');
        
        // remove tree node but correct the neighbors 
        foreach (var node in treeNodes.FullNeighbors) {
            node.RemoveNeighbor(treeNodes);
        }
        
        var path = lawn.FindPath();
        // foreach (var p in paths) {
        //     lawn.PrintPath(p);
        //     Console.WriteLine();
        // }
        var inst = lawn.ConvertPathToInstructions(path);
        Console.WriteLine(inst);
    }
}