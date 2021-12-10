using System.Text;

namespace day5;

public class Diagram {
    public int counter = 1;
    private Point[,] PointGrid { get; set; }

    public Diagram(int maxX, int maxY) {
        PointGrid = new Point[maxX + 1, maxY + 1];
        for (int y = 0; y <= maxY; y++) {
            for (int x = 0; x <= maxX; x++) {
                PointGrid[x, y] = new Point(x, y);
            }
        }
    }

    public void DrawDiagram(string filename = "") {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < PointGrid.GetLength(1); y++) {
            for (int x = 0; x < PointGrid.GetLength(0); x++) {
                if (PointGrid[x, y].Value == 0) {
                    sb.Append(".");
                    //Console.Write(".");
                }
                else {
                    sb.Append(PointGrid[x, y].Value);
                    //Console.Write(PointGrid[x, y].Value);
                }
            }

            sb.Append("\n");
            //Console.WriteLine();
        }

        
        filename = "output" + counter++ + ".txt";
        File.WriteAllText(@"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\adventOfCode\day5\" + filename, sb.ToString());
    }


    public void DrawLines(Dictionary<Point, Point> lines) {
        foreach (var line in lines) {
            if (line.Key.X == line.Value.X)
                DrawLineVer(line.Key, line.Value);
            else if (line.Key.Y == line.Value.Y)
                DrawLineHor(line.Key, line.Value);
            else
                DrawLineDiag(line.Key, line.Value);
            // DrawDiagram(); for stepwise output
        }
    }

    private void DrawLineHor(Point from, Point to) {
        int y = from.Y;
        int smallX = new int[] { from.X, to.X }.Min();
        int bigX = new int[] { from.X, to.X }.Max();
        for (int x = smallX; x <= bigX; x++) {
            PointGrid[x, y].Value++;
        }
    }

    private void DrawLineVer(Point from, Point to) {
        int x = from.X;
        int smallY = new int[] { from.Y, to.Y }.Min();
        int bigY = new int[] { from.Y, to.Y }.Max();
        for (int y = smallY; y <= bigY; y++) {
            PointGrid[x, y].Value++;
        }
    }

    private void DrawLineDiag(Point from, Point to) {
        int smallX = new int[] { from.X, to.X }.Min();
        int bigX = new int[] { from.X, to.X }.Max();
        int smallY = new int[] { from.Y, to.Y }.Min();
        int bigY = new int[] { from.Y, to.Y }.Max();

        if (!((from.X == smallX && from.Y == smallY) || (from.X == bigX && from.Y == bigY))) {
            DrawLineDiag2(from, to);
            return;
        }
        
        for (int y = smallY, x = smallX; y <= bigY && x <= bigX; x++, y++) {
            PointGrid[x, y].Value++;
        }
    }
    
    private void DrawLineDiag2(Point from, Point to) {
        int smallX = new int[] { from.X, to.X }.Min();
        int bigX = new int[] { from.X, to.X }.Max();
        int smallY = new int[] { from.Y, to.Y }.Min();
        int bigY = new int[] { from.Y, to.Y }.Max();
        
        for (int x = bigX, y = smallY; x >= smallX; x--, y++) {
            PointGrid[x, y].Value++;
        }
    }
   

    public int CountPointsOver2() {
        var x = PointGrid.Cast<Point>();
        return PointGrid.Cast<Point>().Count(p => p.Value >= 2);
    }
}