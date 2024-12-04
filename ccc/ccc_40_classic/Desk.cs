using System.Numerics;

namespace ccc_40_classic;

public class Desk {
    // public int Id { get; set; }

    public int XWidth { get; set; }
    public int YHeight { get; set; }

    public bool Horizontal { get; set; }
    
    public Vector2 Position {get; set;}
    
    public const int XOffset = 1;
    public const int YOffset = 1;

    public Desk(int xWidth, int yHeight, Vector2 position, bool horizontal) {
       // Id = id;
        XWidth = xWidth;
        YHeight = yHeight;
        Position = position;
        DeskPositions = GetDeskPositions();
        DeskPositionsWithGap = GetDeskPositionsWithGap();
        Horizontal = horizontal;

        if (!Horizontal) {
            (XWidth, YHeight) = (YHeight, XWidth);
        }
    }

    public IEnumerable<Vector2> DeskPositions { get; set; }
    public IEnumerable<Vector2> DeskPositionsWithGap { get; set; }
    
    private IEnumerable<Vector2> GetDeskPositions() {
        var positions = new List<Vector2>();
        
        for (var x = 0; x < XWidth; x++) {
            for (var y = 0; y < YHeight; y++) {
                positions.Add(new Vector2(Position.X + x, Position.Y + y));
            }
        }
        
        return positions;
    }
    
    private IEnumerable<Vector2> GetDeskPositionsWithGap() {
        var positions = new List<Vector2>();
        
        for (var x = 0 - XOffset; x < XWidth + XOffset; x++) {
            for (var y = 0 - YOffset; y < YHeight + YOffset; y++) {
                positions.Add(new Vector2(Position.X + x, Position.Y + y));
            }
        }
        
        return positions;
    }
    
    public bool CollidesWith(Desk other) {
        return DeskPositions.Any(p => other.DeskPositions.Any(p2 => p2 == p));
    }
    
    // another version of collides with that checks for the gap
    public bool CollidesWithGap(Desk other) {
        // but keep in mind if gap is 1, then one desk should use normal border and the other should use the gap border because else we would have a gap of 2
        return DeskPositions.Any(p => other.DeskPositionsWithGap.Any(p2 => p2 == p));
    }
    
    // equals method
    public override bool Equals(object obj) {
        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }
        
        var desk = (Desk)obj;
        
        return XWidth == desk.XWidth && YHeight == desk.YHeight && Horizontal == desk.Horizontal && Position == desk.Position;
    }
}