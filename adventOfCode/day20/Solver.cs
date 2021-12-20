using System.Text;

namespace day20;

public static class Solver {
    public static bool[,] Image { get; set; }
    public static List<bool> Algorithm { get; set; }

    private const int ImageSizeMultiplier = 3;

    public static void ReadAlgorithm(string algo) {
        Algorithm = new List<bool>();
        foreach (var l in algo) {
            Algorithm.Add(l.ToBool());
        }
    }

    public static void ReadImageInput(string imageInput) {
        var lines = imageInput.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Image = new bool[lines[0].Length * ImageSizeMultiplier, lines.Length * ImageSizeMultiplier];

        var initX = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Image.GetLength(0) / 2)));
        var initY = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Image.GetLength(1) / 2)));

        for (int y = 0; y < lines[0].Length; y++) {
            for (int x = 0; x < lines.Length; x++) {
                Image[x + initX, y + initY] = lines[y][x].ToBool();
            }
        }
    }

    public static void DrawImage(string filename) {
        var output = new StringBuilder();
        for (int y = 0; y < Image.GetLength(1); y++) {
            for (int x = 0; x < Image.GetLength(0); x++) {
                output.Append(Image[x, y] ? "#" : ".");
                //Console.Write(Image[x, y] ? "#" : ".");
            }

            output.Append("\n");
            //Console.WriteLine();
        }

        File.WriteAllText(
            @"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\coding-challenges\adventOfCode\day20\" + filename,
            output.ToString());
    }

    public static int CountLitPixels() {
        int litPixels = 0;
        for (int y = 0; y < Image.GetLength(1); y++) {
            for (int x = 0; x < Image.GetLength(0); x++) {
                if (Image[x, y]) litPixels++;
            }
        }

        return litPixels;
    }

    public static void ConvertImage(int iteration) {
        var newImage = new bool[Image.GetLength(0), Image.GetLength(1)];
        for (int y = 0; y < Image.GetLength(1); y++) {
            for (int x = 0; x < Image.GetLength(0); x++) {
                newImage[x, y] = GetNewPixelValue(x, y);
            }
        }

        Image = newImage;
        DrawImage("output" + iteration + ".txt");
    }

    private static bool GetNewPixelValue(int x, int y) {
        int yMax = Image.GetLength(1);
        int xMax = Image.GetLength(0);

        string neighbors = "";

        if (x - 1 >= 0 && y - 1 >= 0)
            neighbors += (Image[x - 1, y - 1] ? "1" : "0"); // top left
        else neighbors += Image[x, y] ? "1" : "0";
        if (y - 1 >= 0)
            neighbors += (Image[x, y - 1] ? "1" : "0"); // top
        else neighbors += Image[x, y] ? "1" : "0";

        if (x + 1 < xMax && y - 1 >= 0)
            neighbors += (Image[x + 1, y - 1] ? "1" : "0"); // top right
        else neighbors += Image[x, y] ? "1" : "0";
        if (x - 1 >= 0)
            neighbors += (Image[x - 1, y] ? "1" : "0"); // left
        else neighbors += Image[x, y] ? "1" : "0";

        neighbors += Image[x, y] ? "1" : "0"; // current element

        if (x + 1 < xMax)
            neighbors += (Image[x + 1, y] ? "1" : "0"); // right
        else neighbors += Image[x, y] ? "1" : "0";
        if (x - 1 >= 0 && y + 1 < yMax)
            neighbors += (Image[x - 1, y + 1] ? "1" : "0"); // bottom left
        else neighbors += Image[x, y] ? "1" : "0";
        if (y + 1 < yMax)
            neighbors += (Image[x, y + 1] ? "1" : "0"); // bottom
        else neighbors += Image[x, y] ? "1" : "0";
        if (x + 1 < xMax && y + 1 < yMax)
            neighbors += (Image[x + 1, y + 1] ? "1" : "0"); // bottom right
        else neighbors += Image[x, y] ? "1" : "0";


        return Algorithm[Convert.ToInt32(neighbors, 2)];
    }


    private static bool ToBool(this char @this) => (@this == '#');
}