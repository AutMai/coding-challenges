using System.Numerics;
using aocTools;

// https://www.reddit.com/r/adventofcode/comments/rjpf7f/comment/hp7xftl/?utm_source=share&utm_medium=web2x&context=3
var input = Helper.ReadLines("input.txt").ToArray();
Console.WriteLine(Run(input));


(long, long) Run(string[] input) {
    var scans = string.Join(" ", input).Split("  ")
        .Select(s => s.Split("--- ").Last().Split(" ")
            .Select(l => l.Split(",").Select(float.Parse).ToArray())
            .Select(f => new Vector3(f[0], f[1], f[2])).ToArray())
        .ToList();

    Vector3 Rotate(Vector3 v, int direction) {
        var rotated = direction switch {
            0 => (v.X, v.Y, v.Z), 1 => (v.Y, v.Z, v.X), 2 => (-v.Y, v.X, v.Z), 3 => (-v.X, -v.Y, v.Z),
            4 => (v.Y, -v.X, v.Z), 5 => (v.Z, v.Y, -v.X), 6 => (v.Z, v.X, v.Y), 7 => (v.Z, -v.Y, v.X),
            8 => (v.Z, -v.X, -v.Y), 9 => (-v.X, v.Y, -v.Z), 10 => (v.Y, v.X, -v.Z), 11 => (v.X, -v.Y, -v.Z),
            12 => (-v.Y, -v.X, -v.Z), 13 => (-v.Z, v.Y, v.X), 14 => (-v.Z, v.X, -v.Y), 15 => (-v.Z, -v.Y, -v.X),
            16 => (-v.Z, -v.X, v.Y), 17 => (v.X, -v.Z, v.Y), 18 => (-v.Y, -v.Z, v.X), 19 => (-v.X, -v.Z, -v.Y),
            20 => (v.Y, -v.Z, -v.X), 21 => (v.X, v.Z, -v.Y), 22 => (-v.Y, v.Z, -v.X), 23 => (-v.X, v.Z, v.Y),
        };
        return new Vector3(rotated.Item1, rotated.Item2, rotated.Item3);
    }

    var scanners = new List<Vector3> { new Vector3(0, 0, 0) };
    var system = scans[0].ToArray();
    scans.RemoveAt(0);

    do {
        var linked = from match in scans
            from orient in Enumerable.Range(0, 24)
            let rotated = match.Select(s => Rotate(s, orient)).ToArray()
            from vector in system
            from offset in rotated.Select(s => vector - s)
            let translated = rotated.Select(s => s + offset).ToArray()
            where system.Intersect(translated).Count() >= 12
            select (match, translated, offset);

        var link = linked.First();
        system = system.Union(link.translated).ToArray();
        scans.Remove(link.match);
        scanners.Add(link.offset);
    } while (scans.Count > 0);


    var distances = from s in scanners
        from s2 in scanners
        select Math.Abs(s.X - s2.X) + Math.Abs(s.Y - s2.Y) + Math.Abs(s.Z - s2.Z);

    return (system.Count(), (long)distances.Max());
}