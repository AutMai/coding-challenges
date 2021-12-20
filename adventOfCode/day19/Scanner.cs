using System.Numerics;

namespace day19;

public class Scanner {
    public Vector3 Position { get; set; }
    public int Number { get; set; }
    public List<Vector3> Beacons { get; set; }

    public Scanner(int number, Vector3 pos) {
        Number = number;
        Beacons = new List<Vector3>();
        Position = pos;
    }
}