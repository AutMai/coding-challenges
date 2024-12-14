using System.Numerics;
using System.Text;
using aocTools;

namespace aoc24.day12;

public class Day12 : AAocDay {
    private NodeMap<char> _map;

    public Day12() : base() {
        _map = new NodeMap<char>(InputTokens);
    }

    public override void PuzzleOne() {
        var regions = GetRegions();
        var result = regions.Sum(region => GetArea(region) * GetPerimeter(region));
        Console.WriteLine(result);
    }

    private HashSet<HashSet<Node<char>>> GetRegions() {
        var regions = new HashSet<HashSet<Node<char>>>();
        var visitedNodes = new HashSet<Node<char>>();
        foreach (var region in _map.NodeList.Where(n => n.Value != '.' && !visitedNodes.Contains(n))
                     .Select(n => _map.FloodFill(n, include: [n.Value]))) {
            regions.Add(region);
            visitedNodes.UnionWith(region);
        }

        return regions;
    }

    private int GetArea(HashSet<Node<char>> region) {
        return region.Count;
    }

    private int GetPerimeter(HashSet<Node<char>> region) {
        // merge all adjacent coordinates from all region nodes and remove the region nodes
        var perimeter = new List<Vector2>();
        foreach (var node in region) {
            perimeter.AddRange(GetAdjacentCoordinates(node));
        }

        // remove all region nodes
        perimeter.RemoveAll(p => region.Contains(_map.GetNode(p)));
        return perimeter.Count;
    }

    private int GetPerimeterInStraightLines(HashSet<Node<char>> region) {
        // merge all adjacent coordinates from all region nodes and remove the region nodes
        var perimeter = new List<Vector2>();
        foreach (var node in region) {
            perimeter.AddRange(GetAdjacentCoordinates(node));
        }

        // remove all region nodes
        perimeter.RemoveAll(p => region.Contains(_map.GetNode(p)));

        // now we have all perimeter coordinates. now we have to form lines from these coordinates and count them
        var lines = new List<List<Vector2>>();
        var uses = new Dictionary<Vector2, int>();
        foreach (var p in perimeter.Where(p => !uses.TryAdd(p, 1))) {
            uses[p]++;
        }
        while (perimeter.Count > 0) {
            var line = new List<Vector2>();
            var current = perimeter[0];
            line.Add(current);

            // if uses is 1, remove it from perimeter, else remove from uses
            if (uses[current] == 1) {
                perimeter.Remove(current);
            }
            else {
                uses[current]--;
            }

            while (true) {
                var next = perimeter.Except(line)
                    .FirstOrDefault(p => Math.Abs(p.X - current.X) + Math.Abs(p.Y - current.Y) == 1);
                if (next == default) {
                    break;
                }

                line.Add(next);

                // if uses is 1, remove it from perimeter, else remove from uses
                if (uses[next] == 1) {
                    perimeter.Remove(next);
                }
                else {
                    uses[next]--;
                }

                current = next;
            }

            lines.Add(line);
        }

        return lines.Count;
    }

    private HashSet<Vector2> GetAdjacentCoordinates(Node<char> node) {
        // return all 4 adjacent coordinates
        var coordinates = new HashSet<Vector2>();
        coordinates.Add(node.GetVector() + new Vector2(0, 1));
        coordinates.Add(node.GetVector() + new Vector2(0, -1));
        coordinates.Add(node.GetVector() + new Vector2(1, 0));
        coordinates.Add(node.GetVector() + new Vector2(-1, 0));
        return coordinates;
    }

    public override void PuzzleTwo() {
        var regions = GetRegions();

        // print each region with its letter, area and perimeter and the number of straight lines
        foreach (var region in regions) {
            var area = GetArea(region);
            var perimeter = GetPerimeter(region);
            var perimeterStraight = GetPerimeterInStraightLines(region);
            Console.WriteLine($"{region.First().Value}: {area} {perimeter} {perimeterStraight}");
        }
    }

    // var regions = GetRegions();
    // var result = regions.Sum(region => GetArea(region) * GetPerimeterInStraightLines(region));
    // Console.WriteLine(result);
}