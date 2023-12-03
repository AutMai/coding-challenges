using aocTools;

namespace aoc23.day3;

public class Day3 : AAocDay {
    NodeMap<char> map;

    public Day3() : base() {
        map = new NodeMap<char>(InputTokens);
    }

    public override void PuzzleOne() {
        var partNumbers = GetPartNumbers();
        var res = partNumbers.Where(pn => pn.Neighbors.Count > 0).Sum(pn => pn.Number);
        Console.WriteLine(res);
    }

    private List<PartNumber> GetPartNumbers() {
        List<PartNumber> PartNumbers = new();

        for (var i = 0; i < map.NodeList.Count; i++) {
            // if node is a number check next nodes to see if the number is part of a larger number
            if (map.NodeList[i].Value is >= '0' and <= '9') {
                PartNumbers.Add(new PartNumber() { Number = map.NodeList[i].Value.ToInt() });
                PartNumbers[^1].Neighbors
                    .UnionWith(map.NodeList[i].FullNeighbors.Where(x => x.Value is < '0' or > '9' && x.Value != '.'));
                while (map.NodeList[++i].Value is >= '0' and <= '9') {
                    PartNumbers[^1].Number *= 10;
                    PartNumbers[^1].Number += map.NodeList[i].Value.ToInt();
                    PartNumbers[^1].Neighbors
                        .UnionWith(
                            map.NodeList[i].FullNeighbors.Where(x => x.Value is < '0' or > '9' && x.Value != '.'));

                    if (map.NodeList[i].PosX == map.Width - 1) break; // to prevent numbers over two lines
                }
            }
        }

        return PartNumbers;
    }

    public override void PuzzleTwo() {
        var partNumbers = GetPartNumbers();
        var gears = new Dictionary<Node<char>, int>();

        foreach (var pn in partNumbers) {
            var asteriskNeighbors = pn.Neighbors.Where(x => x.Value == '*').ToList();
            // check if any part number has the same asterisk neighbor
            foreach (var pn2 in partNumbers) {
                if (pn2 == pn) continue;
                foreach (var gearNode in pn2.Neighbors.Intersect(asteriskNeighbors)) {
                    if (gears.ContainsKey(gearNode)) continue;
                    gears.Add(gearNode, pn.Number * pn2.Number);
                }
            }
        }

        Console.WriteLine(gears.Values.Sum());
    }
}

class PartNumber {
    public int Number { get; set; }
    public HashSet<Node<char>> Neighbors { get; set; } = new();
}