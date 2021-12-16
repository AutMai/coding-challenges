using System.Threading.Channels;
using aocTools;
using day9;

var inputLines = Helper.ReadFile("input_small.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);


var caveMap = new Point[inputLines[0].Length, inputLines.Length];

for (int y = 0; y < caveMap.GetLength(1); y++) {
    for (int x = 0; x < caveMap.GetLength(0); x++) {
        caveMap[x, y] = new Point(x, y, Convert.ToInt32(inputLines[y][x].ToString()));
    }
}

var lowPoints = FindLowPoints();

List<Point> visitedPoints = new List<Point>();

GenerateBasins(lowPoints);


void GenerateBasins(List<Point> lowPoints) {
    foreach (var lowPoint in lowPoints) {
        Console.Clear();
        int basinSize = lowPoint.Height;
        var neighbors = GetNeighbors(lowPoint);
        visitedPoints.Add(lowPoint);
        foreach (var neighbor in neighbors) {
            basinSize = GenerateBasinR(neighbor, basinSize);
        }
    }
}


int GenerateBasinR(Point p, int basinSize) {
    if (p.Height == 9) return basinSize;

    Console.WriteLine(p);


    basinSize += 1;
    visitedPoints.Add(p);


    foreach (var neighbor in GetNeighbors(p)) {
        if (!visitedPoints.Exists(vp=>vp.X == neighbor.X && vp.Y == neighbor.Y)) {
            basinSize = GenerateBasinR(neighbor, basinSize);
        }
    }

    return basinSize;
}

List<Point> FindLowPoints() {
    List<Point> lowPoints = new List<Point>();
    int yMax = caveMap.GetLength(1);
    int xMax = caveMap.GetLength(0);

    for (int y = 0; y < yMax; y++) {
        for (int x = 0; x < xMax; x++) {
            List<Point> neighbors = new List<Point>();

            if (x + 1 < xMax) neighbors.Add(caveMap[x + 1, y]);

            if (x - 1 >= 0) neighbors.Add(caveMap[x - 1, y]);

            if (y + 1 < yMax) neighbors.Add(caveMap[x, y + 1]);

            if (y - 1 >= 0) neighbors.Add(caveMap[x, y - 1]);

            bool isLowPoint = true;

            foreach (var neighbor in neighbors) {
                if (caveMap[x, y].Height >= neighbor.Height)
                    isLowPoint = false;
            }

            if (isLowPoint) lowPoints.Add(caveMap[x, y]);
        }
    }

    return lowPoints;
}

List<Point> GetNeighbors(Point p) {
    int yMax = caveMap.GetLength(1);
    int xMax = caveMap.GetLength(0);
    List<Point> neighbors = new List<Point>();

    int x = p.X;
    int y = p.Y;

    if (x + 1 < xMax) neighbors.Add(caveMap[x + 1, y]);

    if (x - 1 >= 0) neighbors.Add(caveMap[x - 1, y]);

    if (y + 1 < yMax) neighbors.Add(caveMap[x, y + 1]);

    if (y - 1 >= 0) neighbors.Add(caveMap[x, y - 1]);


    return neighbors;
}