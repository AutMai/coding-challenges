using System.Numerics;
using System.Text.RegularExpressions;
using aocTools;
using day19;

var input = Helper.ReadFile("input_small.txt");

var scannerReports = Regex.Split(input, @"--- scanner \d+ ---");
scannerReports = scannerReports.Where(s => s != String.Empty).ToArray();

var scanners = new List<Scanner>();

for (int i = 0; i < scannerReports.Length; i++) {
    var s = new Scanner(i);
    scanners.Add(s);
    var coords = scannerReports[i].Split('\n', StringSplitOptions.RemoveEmptyEntries);
    foreach (var coord in coords) {
        var coordParts = Array.ConvertAll(coord.Split(',', StringSplitOptions.RemoveEmptyEntries), float.Parse);
        var v = new Vector3(coordParts[0], coordParts[1], coordParts[2]);
        s.Beacons.Add(v);
    }
}

var scanner0 = new Vector3(0, 0, 0);

var distances = new Dictionary<(Vector3 b1, Vector3 b2), float>();
var distances2 = new Dictionary<(Vector3 b1, Vector3 b2), float>();


foreach (var b1 in scanners[0].Beacons)
foreach (var b2 in scanners[0].Beacons) {
    if (Vector3.Distance(b1, b2) != 0 && !distances.ContainsValue(Vector3.Distance(b1, b2)))
        distances[(b1, b2)] = (Vector3.Distance(b1, b2));
}


foreach (var b1 in scanners[1].Beacons)
foreach (var b2 in scanners[1].Beacons) {
    if (Vector3.Distance(b1, b2) != 0 && !distances2.ContainsValue(Vector3.Distance(b1, b2)))
        distances2[(b1, b2)] = (Vector3.Distance(b1, b2));
}

foreach (var d in distances)
foreach (var d2 in distances2) {
    if (d.Value == d2.Value) Console.WriteLine(d.Key + " = " + d2.Key + " = " + d.Value + "\n\n");
}