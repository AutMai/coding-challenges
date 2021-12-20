using System.Numerics;

namespace day19;

public class Scanner {
    public int Number { get; set; }
    public List<Vector3> Beacons { get; set; }

    public Scanner(int number) {
        Number = number;
        Beacons = new List<Vector3>();
    }
}