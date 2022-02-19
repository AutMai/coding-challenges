namespace day22;

public static class Reactor {
    private const int MaxDim = 50;
    private static readonly bool[,,] Cubes = new bool[MaxDim * 2 + 1, MaxDim * 2 + 1, MaxDim * 2 + 1];
    private static List<Point> OnPoints = new();
    private static readonly List<Point> OffPoints = new();

    public static void Part2(IEnumerable<string> lines) {
        foreach (var line in lines) {
            ProcessRebootStep2(line);
        }
    }

    private static void ProcessRebootStep2(string s) {
        var operation = (s.Split(' ')[0] == "on");

        var coords2 = s.Split(' ')[1]
            .Split(new[] { 'x', 'y', 'z', '=', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
        var coords = Array.ConvertAll(coords2, x => int.Parse(x));

        var list1 = new List<Point>();
        var list2 = new List<Point>();

        for (int x = coords[0]; x <= coords[1]; x++) {
            for (int y = coords[2]; y <= coords[3]; y++) {
                for (int z = coords[4]; z <= coords[5]; z++) {
                    if (operation == false)
                        OffPoints.Add(new Point(x, y, z));
                    else
                        OnPoints.Add(new Point(x, y, z));
                }
            }
        }

        OnPoints = OnPoints.Except(OffPoints).ToList();
    }

    public static void Part1(IEnumerable<string> lines) {
        foreach (var line in lines) {
            ProcessRebootStep(line);
        }
    }

    private static void ProcessRebootStep(string s) {
        var operation = (s.Split(' ')[0] == "on");

        var coords2 = s.Split(' ')[1]
            .Split(new[] { 'x', 'y', 'z', '=', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
        var coords = Array.ConvertAll(coords2, x => int.Parse(x));

        for (int x = coords[0]; x <= coords[1]; x++) {
            for (int y = coords[2]; y <= coords[3]; y++) {
                for (int z = coords[4]; z <= coords[5]; z++) {
                    Cubes[x + MaxDim, y + MaxDim, z + MaxDim] = operation;
                }
            }
        }
    }

    public static long CountActiveCubes() {
        long count = 0;
        for (int x = 0; x < Cubes.GetLength(0); x++) {
            for (int y = 0; y < Cubes.GetLength(1); y++) {
                for (int z = 0; z < Cubes.GetLength(2); z++) {
                    if (Cubes[x, y, z]) count++;
                }
            }
        }

        return count;
    }
}