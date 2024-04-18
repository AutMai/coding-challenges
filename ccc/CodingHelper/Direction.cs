using System.Numerics;

namespace CodingHelper;

public enum EDirection {
    North,
    East,
    South,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}

public struct Direction {
    public EDirection D { get; set; }
    
    public Direction(EDirection d) {
        D = d;
    }
    
    public static Direction North => new(EDirection.North);
    public static Direction East => new(EDirection.East);
    
    public static Direction South => new(EDirection.South);
    public static Direction West => new(EDirection.West);
    
    public static Direction NorthEast => new(EDirection.NorthEast);
    public static Direction NorthWest => new(EDirection.NorthWest);
    
    public static Direction SouthEast => new(EDirection.SouthEast);
    public static Direction SouthWest => new(EDirection.SouthWest);
    
    public Vector2 ToVector2() {
        return D switch {
            EDirection.North => new Vector2(0, -1),
            EDirection.East => new Vector2(1, 0),
            EDirection.South => new Vector2(0, 1),
            EDirection.West => new Vector2(-1, 0),
            EDirection.NorthEast => new Vector2(1, -1),
            EDirection.NorthWest => new Vector2(-1, -1),
            EDirection.SouthEast => new Vector2(1, 1),
            EDirection.SouthWest => new Vector2(-1, 1),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    // equals overload to check for value equality
    public static bool operator ==(Direction d1, Direction d2) {
        return d1.D == d2.D;
    }
    
    public static bool operator ==(Direction d1, EDirection d2) {
        return d1.D == d2;
    }

    public static bool operator !=(Direction d1, EDirection d2) {
        return !(d1 == d2);
    }

    public static bool operator !=(Direction d1, Direction d2) {
        return !(d1 == d2);
    }
}