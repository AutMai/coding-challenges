namespace day9;

public class Basin {
    private List<Point> Points { get; set; }

    public Basin(List<Point> points) {
        Points = points;
    }

    public int GetBasinSize() => Points.Count;
}