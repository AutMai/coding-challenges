using aocTools;
using day5;

string input = Helper.ReadFile("input.txt");

Dictionary<Point, Point> lines = new Dictionary<Point, Point>();

var coords = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

int maxX = int.MinValue;
int maxY = int.MinValue;

foreach (var coord in coords) {
    var coordPairs = coord.Split(" -> ");
    
    int x1 = Convert.ToInt32(coordPairs[0].Split(',')[0]);
    int y1 = Convert.ToInt32(coordPairs[0].Split(',')[1]);
    
    int x2 = Convert.ToInt32(coordPairs[1].Split(',')[0]);
    int y2 = Convert.ToInt32(coordPairs[1].Split(',')[1]);

    if (x1 > maxX) maxX = x1;
    if (x2 > maxX) maxX = x2;
    if (y1 > maxY) maxY = y1;
    if (y2 > maxY) maxY = y2;
    
    
    lines.Add(new Point(x1, y1), new Point(x2, y2));
}

Console.WriteLine("maxX:" + maxX);
Console.WriteLine("maxY:" + maxY);
Diagram d = new Diagram(maxX, maxY);
d.DrawLines(lines);
d.DrawDiagram();
Console.WriteLine(d.CountPointsOver2());

Console.WriteLine("test");

    