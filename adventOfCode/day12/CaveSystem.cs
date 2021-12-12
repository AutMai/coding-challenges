namespace day12;

public static class CaveSystem {
    public static Dictionary<string, Cave> Caves = new Dictionary<string, Cave>();
    public static List<string> Paths = new List<string>();


    public static void CreateCaves(List<string> inputLines) {
        foreach (var inputLine in inputLines) {
            var caves = inputLine.Split('-');
            if (!Caves.ContainsKey(caves[0])) Caves.Add(caves[0], new Cave(caves[0]));
            if (!Caves.ContainsKey(caves[1])) Caves.Add(caves[1], new Cave(caves[1]));

            Caves[caves[0]].AddConnectedCave(caves[1], Caves[caves[1]]);
            Caves[caves[1]].AddConnectedCave(caves[0], Caves[caves[0]]);
        }
    }

    public static void CreatePaths(string currentCave = "start", string path = "", string smallVisited = "") {
        string pathSoFar = path += currentCave + ",";

        if (!Caves[currentCave].IsLarge) smallVisited += currentCave + ","; // visited small cave

        if (currentCave == "end") {
            Paths.Add(pathSoFar);
            return;
        }

        List<string> smallCavesVisited =
            smallVisited.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

        foreach (var connectedCave in Caves[currentCave].ConnectedCaves) {
            if (!smallCavesVisited.Contains(connectedCave.Key))
                CreatePaths(connectedCave.Key, pathSoFar, smallVisited);
        }
    }

    public static void CreatePaths2(string currentCave = "start", string path = "", string smallVisited = "",
        bool smallTwice = false) {
        string pathSoFar = path += currentCave + ",";
        bool smallTwiceNext = smallTwice;

        if (!Caves[currentCave].IsLarge)
            smallVisited += currentCave + ",";

        if (currentCave == "end") {
            Paths.Add(pathSoFar);
            return;
        }

        List<string> smallCavesVisited =
            smallVisited.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

        List<string> smallTwiceCheck = new List<string>();

        foreach (string s in smallCavesVisited) {
            if (smallTwiceCheck.Contains(s)) smallTwiceNext = true;
            else smallTwiceCheck.Add(s);
        }

        foreach (var connectedCave in Caves[currentCave].ConnectedCaves) {
            if (connectedCave.Key != "start") {
                if (smallTwiceNext) {
                    if (!smallCavesVisited.Contains(connectedCave.Key))
                        CreatePaths2(connectedCave.Key, pathSoFar, smallVisited, smallTwiceNext);
                }
                else
                    CreatePaths2(connectedCave.Key, pathSoFar, smallVisited, smallTwiceNext);
            }
        }
    }
}