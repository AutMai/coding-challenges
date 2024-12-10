using aocTools;

namespace day9;

public class Day9 : AAocDay {
    NodeMap<int> _map;

    public Day9() : base() {
        _map = new NodeMap<int>(InputTokens);
    }

    public override void PuzzleOne() {
        var riskLevel = GetLowPoints().Sum(lp => lp.Value + 1);
        Console.WriteLine(riskLevel);
    }

    public override void PuzzleTwo() {
        var lowPoints = GetLowPoints();
        var basins = new Dictionary<Node<int>, int>();
        foreach (var lowPoint in lowPoints) {
            basins.Add(lowPoint, GetBasinSize(lowPoint));
        }
        
        var largestBasin = basins.OrderByDescending(b => b.Value).Take(3);

        foreach (var lb in largestBasin) {
            PrintMap(GetBasin(lb.Key));
            Console.WriteLine();
            Console.WriteLine();
        }
        
        // multiple the three largest basins together
        
        var result = largestBasin.Aggregate(1, (current, lb) => current * lb.Value);
        Console.WriteLine(result);
    }

    private void PrintMap(List<Node<int>> basin) {
        var minX = basin.Min(n => n.PosX);
        var maxX = basin.Max(n => n.PosX);
        var minY = basin.Min(n => n.PosY);
        var maxY = basin.Max(n => n.PosY);

        for (int y = minY; y <= maxY; y++) {
            for (int x = minX; x <= maxX; x++) {
                var node = basin.FirstOrDefault(n => n.PosX == x && n.PosY == y);
                if (node != null) {
                    Console.Write(node.Value);
                }
                else {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    private IEnumerable<Node<int>> GetLowPoints() =>
        _map.NodeList.Where(node => node.FullNeighbors.All(n => n.Value > node.Value));

    private int GetBasinSize(Node<int> node) {
        var basin = _map.FloodFill(node, new List<int>() { 9 });
        return basin.Count;
    }
    
    private List<Node<int>> GetBasin(Node<int> node) {
        var basin = _map.FloodFill(node, new List<int>() { 9 });
        return basin.ToList();
    }
}