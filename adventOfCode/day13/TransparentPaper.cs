namespace day13;

using System.Linq;

public static class TransparentPaper {
    public static bool[,] Paper { get; set; }

    public static void DrawPaper(string[] dotCoords) {
        int maxX = Int32.MinValue;
        int maxY = Int32.MinValue;
        ;
        foreach (var dotCoord in dotCoords) {
            var xy = dotCoord.Split(',');
            if (Convert.ToInt32(xy[0]) > maxX)
                maxX = Convert.ToInt32(xy[0]);
            if (Convert.ToInt32(xy[1]) > maxY)
                maxY = Convert.ToInt32(xy[1]);
        }

        Paper = new bool[maxX + 1, maxY + 1];

        // Default false
        for (int y = 0; y < Paper.GetLength(1); y++) {
            for (int x = 0; x < Paper.GetLength(0); x++) {
                Paper[x, y] = false;
            }
        }

        foreach (var dotCoord in dotCoords) {
            var xy = Array.ConvertAll(dotCoord.Split(','), int.Parse);
            Paper[xy[0], xy[1]] = true;
        }
    }

    public static void PrintPaper() {
        for (int y = 0; y < Paper.GetLength(1); y++) {
            for (int x = 0; x < Paper.GetLength(0); x++) {
                Console.Write(Paper[x, y] ? "██" : "  ");
            }

            Console.WriteLine();
        }
    }

    public static void FoldPaperH(int yLine) {
        var newMaxY = Math.Max(yLine, Paper.GetLength(1) - yLine - 1);
        var newPaper = new bool[Paper.GetLength(0), newMaxY];

        for (int y = 0; y < Paper.GetLength(1); y++) {
            for (int x = 0; x < Paper.GetLength(0); x++) {
                if (y < yLine) newPaper[x, y] = Paper[x, y]; // above line copy
                else if (y != yLine) {
                    if (Paper[x, y]) newPaper[x, yLine - (y - yLine)] = Paper[x, y];
                }
            }
        }

        Paper = newPaper;
    }

    public static void FoldPaperV(int xLine) {
        var newMaxX = Math.Max(xLine, Paper.GetLength(0) - xLine - 1);
        var newPaper = new bool[newMaxX, Paper.GetLength(1)];

        for (int y = 0; y < Paper.GetLength(1); y++) {
            for (int x = 0; x < Paper.GetLength(0); x++) {
                if (x < xLine) newPaper[x, y] = Paper[x, y]; // above line copy
                else if (x != xLine) {
                    if (Paper[x, y]) newPaper[xLine - (x - xLine), y] = Paper[x, y];
                }
            }
        }

        Paper = newPaper;
    }

    public static int CountDots() {
        int sum = 0;
        
        for (int y = 0; y < Paper.GetLength(1); y++) {
            for (int x = 0; x < Paper.GetLength(0); x++) {
                if(Paper[x, y]) sum++;
            }
        }

        return sum;
    }
}