namespace day9;

public class Part1 {
    /*
     
var inputLines = Helper.ReadFile("input.txt")
    .Replace("\r", "")
    .Split('\n', StringSplitOptions.RemoveEmptyEntries);

var caveMap = new int[inputLines[0].Length, inputLines.Length];

for (int y = 0; y < caveMap.GetLength(1); y++) {
    for (int x = 0; x < caveMap.GetLength(0); x++) {
        caveMap[x, y] = Convert.ToInt32(inputLines[y][x].ToString());
    }
}

var lowPoints = FindLowPoints();
lowPoints = lowPoints.Select(x => x + 1).ToList();

Console.WriteLine(lowPoints.Sum());


List<int> FindLowPoints() {
    List<int> lowPoints = new List<int>();
    int yMax = caveMap.GetLength(1);
    int xMax = caveMap.GetLength(0);

    for (int y = 0; y < yMax; y++) {
        for (int x = 0; x < xMax; x++) {
            List<int> neighbors = new List<int>();

            if (x + 1 < xMax) neighbors.Add(caveMap[x + 1, y]);

            if (x - 1 >= 0) neighbors.Add(caveMap[x - 1, y]);

            if (y + 1 < yMax) neighbors.Add(caveMap[x, y + 1]);

            if (y - 1 >= 0) neighbors.Add(caveMap[x, y - 1]);

            bool isLowPoint = true;

            foreach (var neighbor in neighbors) {
                if (caveMap[x, y] >= neighbor)
                    isLowPoint = false;
            }

            if (isLowPoint) lowPoints.Add(caveMap[x, y]);
        }
    }

    return lowPoints;
}
 
     */
}