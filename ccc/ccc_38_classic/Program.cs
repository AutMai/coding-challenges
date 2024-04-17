// See https://aka.ms/new-console-template for more information

using CodingHelper;

var r = new InputReader();

//r.ReadZipFile("files/level5.zip", " ", true);
r.ReadWholeFile("files/level5/level5_example.in", " ");

foreach (var l in r.GetInputs()) {
    //l.SetOutput();
    var mapSize = l.ReadInt();

    string[] maplines = new string[mapSize];

    for (int i = 0; i < mapSize; i++) {
        maplines[i] = l.Read();
    }

    var map = new NodeMap<char>(maplines);

    var coordCount = l.ReadInt();

    for (int i = 0; i < coordCount; i++) {
        var node = map[l.Read()];

        var route = map.FindShortestPathAroundIsland(node, new List<char>() { 'W' });

        if (route is null) {
            Console.WriteLine("No route found");
        }
        else {
            // print out the route
            Console.WriteLine(string.Join(" ", route));
            map.PrintPath(node, node,route);
        }
    }
}


bool RouteCrossesItself(List<string> route) {
    // route is a list of strings of coordinates like "1,2", "2,3", "3,4", ...
    // check if the route crosses itself
    // a route crosses itself if it visits a node that it has already visited or if a diagonal crosses another diagonal

    // 1. check if the route visits a node that it has already visited
    // check if duplicate strings exist in the route
    if (route.Distinct().Count() != route.Count)
        return true;

    // 2. check if a diagonal crosses another diagonal
    // example:
    // 1,1 -> 2,2 -> 1,2 -> 2,1

    var ListOfPaths = new List<Tuple<string, string>>();
    for (int i = 0; i < route.Count - 1; i++) {
        ListOfPaths.Add(new Tuple<string, string>(route[i], route[i + 1]));
    }

    foreach (var path in ListOfPaths) {
        if (!IsDiagonal(path))
            continue;
        var diagonal = GetDiagonal(path.Item1, path.Item2);
        if (ListOfPaths.Contains(diagonal))
            return true;
    }

    return false;
}

bool IsDiagonal(Tuple<string, string> path) {
    // check if x and y coordinates are different
    var x1 = Convert.ToInt32(path.Item1.Split(',')[0]);
    var y1 = Convert.ToInt32(path.Item1.Split(',')[1]);
    var x2 = Convert.ToInt32(path.Item2.Split(',')[0]);
    var y2 = Convert.ToInt32(path.Item2.Split(',')[1]);
    return x1 != x2 && y1 != y2;
}

Tuple<string, string> GetDiagonal(string coord1, string coord2) {
    var x1 = Convert.ToInt32(coord1.Split(',')[0]);
    var y1 = Convert.ToInt32(coord1.Split(',')[1]);
    var x2 = Convert.ToInt32(coord2.Split(',')[0]);
    var y2 = Convert.ToInt32(coord2.Split(',')[1]);
    return new Tuple<string, string>($"{x1},{y2}", $"{x2},{y1}");
}