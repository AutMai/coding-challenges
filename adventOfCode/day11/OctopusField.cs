namespace day11;

public static class OctopusField {
    public static Octopus[,] Field { get; set; }

    public static int FlashCount = 0;

    public static void CreateField(List<string> inputLines) {
        Field = new Octopus[inputLines[0].Length, inputLines.Count];

        for (int y = 0; y < Field.GetLength(1); y++) {
            for (int x = 0; x < Field.GetLength(0); x++) {
                Field[x, y] = new Octopus(Convert.ToInt32(inputLines[y][x].ToString()));
            }
        }
    }

    public static void PrintField() {
        Console.WriteLine("-------------------------");
        for (int y = 0; y < Field.GetLength(1); y++) {
            for (int x = 0; x < Field.GetLength(0); x++) {
                Console.ForegroundColor = Field[x, y].EnergyLevel == 0 ? ConsoleColor.Yellow : ConsoleColor.White;

                Console.Write(Field[x, y].EnergyLevel);
            }

            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("-------------------------");
    }

    public static void IncreaseEnergyLevel() {
        for (int y = 0; y < Field.GetLength(1); y++) {
            for (int x = 0; x < Field.GetLength(0); x++) {
                Field[x, y].EnergyLevel++;
            }
        }
    }

    public static bool TryFlash() {
        int tempFlashCount = FlashCount;
        TryFlashR();
        return AllFlashed(tempFlashCount);
    }

    public static bool AllFlashed(int oldFlashCount) {
        if (FlashCount - oldFlashCount >= Field.Length) {
            return true;
        }
        return false;
    }

    public static bool TryFlashR() {
        bool noFlashes = true;
        for (int y = 0; y < Field.GetLength(1); y++) {
            for (int x = 0; x < Field.GetLength(0); x++) {
                if (Field[x, y].EnergyLevel > 9) {
                    FlashCount++;
                    noFlashes = false;
                    Field[x, y].EnergyLevel = 0;
                    var neighbors = GetNeighbors(x, y);
                    neighbors.ForEach(Flash);
                }
            }
        }

        if (noFlashes == true) {
            return true;
        }
        else {
            TryFlashR();
        }

        return noFlashes;
    }

    private static void Flash(Octopus o) {
        if (o.EnergyLevel > 0)
            o.EnergyLevel++;
    }

    private static List<Octopus> GetNeighbors(int x, int y) {
        int yMax = Field.GetLength(1);
        int xMax = Field.GetLength(0);

        List<Octopus> neighbors = new List<Octopus>();

        if (x - 1 >= 0 && y - 1 >= 0)
            neighbors.Add(Field[x - 1, y - 1]); // top left

        if (y - 1 >= 0) neighbors.Add(Field[x, y - 1]); // top

        if (x + 1 < xMax && y - 1 >= 0)
            neighbors.Add(Field[x + 1, y - 1]); // top right

        if (x + 1 < xMax)
            neighbors.Add(Field[x + 1, y]); // right

        if (x + 1 < xMax && y + 1 < yMax)
            neighbors.Add(Field[x + 1, y + 1]); // bottom right

        if (y + 1 < yMax) neighbors.Add(Field[x, y + 1]); // bottom

        if (x - 1 >= 0 && y + 1 < yMax)
            neighbors.Add(Field[x - 1, y + 1]); // bottom left

        if (x - 1 >= 0) neighbors.Add(Field[x - 1, y]); // left

        return neighbors;
    }
}