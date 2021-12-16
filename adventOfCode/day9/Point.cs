namespace day9; 

public class Point {
    public int X { get; set; }
    public int Y { get; set; }
    public int Height { get; set; }

    public Point(int x, int y, int height) {
        X = x;
        Y = y;
        Height = height;
    }

    public override string ToString() {
        return $"Point({X},{Y}) = {Height}";
    }
}