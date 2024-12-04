using System.Text;
using CodingHelper;

var r = new InputReader(6, true, " ", true);


foreach (var l in r.GetInputs()) {
    //l.SetOutput();

    var nLawn = l.ReadInt();

    for (int i = 0; i < nLawn; i++) {
        var lawnHeight = int.Parse(l.Read().Split(" ")[1]);
        var lines = new List<string>();
        for (int j = 0; j < lawnHeight; j++) {
            lines.Add(l.Read());
        }

        var lawn = new NodeMap<char>(lines);

        var path = lawn.FindHamiltonianCycle(node => node.ExcludeNeighbors(['X']), ['X']);
        var inst = lawn.ConvertPathToInstructions(path);
        Console.WriteLine(inst);
    }
}