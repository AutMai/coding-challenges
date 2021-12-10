namespace day5; 

public class Point {
    public int X { get; set; }
    public int Y { get; set; }
    public int Value { get; set; }

    public Point(int x, int y, int value = 0) {
        X = x;
        Y = y;
        Value = value;
    }
}