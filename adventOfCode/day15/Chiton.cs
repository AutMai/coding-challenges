﻿namespace day15; 

public static class Chiton {
    public static object PartOne(string input) => Solve(GetRiskLevelMap(input));
    public static object PartTwo(string input) => Solve(ScaleUp(GetRiskLevelMap(input)));

    private static  int Solve(Dictionary<Point, int> riskMap) {
        // Disjktra algorithm

        var topLeft = new Point(0, 0);
        var bottomRight = new Point(riskMap.Keys.MaxBy(p => p.x).x, riskMap.Keys.MaxBy(p => p.y).y);

        // Visit points in order of cumulted risk
        // ⭐ .Net 6 finally has a PriorityQueue collection :)
        var q = new PriorityQueue<Point, int>();
        var totalRiskMap = new Dictionary<Point, int> {
            [topLeft] = 0
        };

        q.Enqueue(topLeft, 0);

        // It would be enough to go until we find the bottom right corner, but computing all 
        // risk levels is not much more work 
        while (q.Count > 0) {
            var p = q.Dequeue();

            foreach (var n in Neighbours(p)) {
                if (riskMap.ContainsKey(n) && !totalRiskMap.ContainsKey(n)) {
                    var totalRisk = totalRiskMap[p] + riskMap[n];
                    totalRiskMap[n] = totalRisk;
                    if (n == bottomRight) {
                        break;
                    }
                    var h = 0; //bottomRight.y - n.y + bottomRight.x - n.x;
                    q.Enqueue(n, totalRisk + h);
                }
            }
        }

        // return bottom right corner's total risk:
        return totalRiskMap[bottomRight];
    }

    // Create an 5x scaled up map, as described in part 2
    static Dictionary<Point, int> ScaleUp(Dictionary<Point, int> map) {
        var (ccol, crow) = (map.Keys.MaxBy(p => p.x).x + 1, map.Keys.MaxBy(p => p.y).y + 1);

        var res = new Dictionary<Point, int>(
            from y in Enumerable.Range(0, crow * 5)
            from x in Enumerable.Range(0, ccol * 5)

            // x, y and risk level in the original map:
            let tileY = y % crow
            let tileX = x % ccol
            let tileRiskLevel = map[new Point(tileX, tileY)]

            // risk level is increased by tile distance from origin:
            let tileDistance = (y / crow) + (x / ccol)

            // risk level wraps around from 9 to 1:
            let riskLevel = (tileRiskLevel + tileDistance - 1) % 9 + 1
            select new KeyValuePair<Point, int>(new Point(x, y), riskLevel)
        );

        return res;
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area
    static Dictionary<Point, int> GetRiskLevelMap(string input) {
        var map = input.Split("\n");
        return new Dictionary<Point, int>(
            from y in Enumerable.Range(0, map[0].Length)
            from x in Enumerable.Range(0, map.Length)
            select new KeyValuePair<Point, int>(new Point(x, y), map[y][x] - '0')
        );
    }

    static IEnumerable<Point> Neighbours(Point point) =>
        new[] {
           point with {y = point.y + 1},
           point with {y = point.y - 1},
           point with {x = point.x + 1},
           point with {x = point.x - 1},
        };
}