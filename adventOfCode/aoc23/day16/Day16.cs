using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;

namespace aoc23.day16;

public class Day16 : AAocDay {
    public BlockingCollection<Beam> Beams = new();
    public static Grid Grid { get; set; }
    public HashSet<Node<char>> VisitedNodes { get; set; } = new();
    public HashSet<Tuple<Vector2, Direction>> CachedBeams { get; set; } = new();

    public Day16() {
        Grid = new Grid(InputTokens);
    }


    public override void PuzzleOne() {
        Beams.Add(new Beam(Vector2.Zero, Direction.East));
        while (Beams.Any(b => !b.Deleted)) {
            // move all beams
            Beams.Where(b => !b.Deleted).ToList().ForEach(MoveBeam);
            
            /*Console.Clear();
            Grid.PrintPath(VisitedNodes.ToList());
            Thread.Sleep(100);*/
        }

        Console.WriteLine(VisitedNodes.Count);
    }

    private void MoveBeam(Beam b) {
        // check if we already had such a beam
        if (!CachedBeams.Add(new Tuple<Vector2, Direction>(b.Position, b.Direction))) {
            b.Delete();
            return;
        }
        
        
        // if beam has out of bounds, remove it
        if (b.Position.X < 0 || b.Position.Y < 0 || b.Position.X >= Grid.Width || b.Position.Y >= Grid.Height) {
            b.Delete();
            return;
        }

        // check on which tile of the grid the beam is
        var tile = Day16.Grid.GetNode(b.Position);
        // mark the tile as visited
        VisitedNodes.Add(tile);
        switch (tile.Value) {
            case '.':
                // move one step forward
                b.Position += b.Direction.ToVector2();
                break;
            case '|':
                // if pointy end is in the direction of the beam, move one step forward
                if (b.Direction == Direction.North || b.Direction == Direction.South) {
                    b.Position += b.Direction.ToVector2();
                }

                // else it acts as a splitter and creates two new beams on the pointy ends and move them one step forward
                else {
                    var newBeam1 = new Beam(b.Position + Direction.North.ToVector2(), Direction.North);
                    var newBeam2 = new Beam(b.Position + Direction.South.ToVector2(), Direction.South);
                    Beams.Add(newBeam1);
                    Beams.Add(newBeam2);
                    // remove the current beam
                    b.Delete();
                }

                break;
            case '-':
                // if pointy end is in the direction of the beam, move one step forward
                if (b.Direction == Direction.East || b.Direction == Direction.West) {
                    b.Position += b.Direction.ToVector2();
                }

                // else it acts as a splitter and creates two new beams on the pointy ends and move them one step forward
                else {
                    var newBeam1 = new Beam(b.Position + Direction.East.ToVector2(), Direction.East);
                    var newBeam2 = new Beam(b.Position + Direction.West.ToVector2(), Direction.West);
                    Beams.Add(newBeam1);
                    Beams.Add(newBeam2);
                    // remove the current beam
                    b.Delete();
                }

                break;
            case '/':
                // this is a mirror and the beam will be reflected
                if (b.Direction == EDirection.East) {
                    b.Direction = Direction.North;
                }
                else if (b.Direction == EDirection.North) {
                    b.Direction = Direction.East;
                }
                else if (b.Direction == EDirection.South) {
                    b.Direction = Direction.West;
                }
                else if (b.Direction == EDirection.West) {
                    b.Direction = Direction.South;
                }

                // move one step forward
                b.Position += b.Direction.ToVector2();
                break;
            case '\\':
                // this is a mirror and the beam will be reflected
                if (b.Direction == EDirection.East) {
                    b.Direction = Direction.South;
                }
                else if (b.Direction == EDirection.North) {
                    b.Direction = Direction.West;
                }
                else if (b.Direction == EDirection.South) {
                    b.Direction = Direction.East;
                }
                else if (b.Direction == EDirection.West) {
                    b.Direction = Direction.North;
                }

                // move one step forward
                b.Position += b.Direction.ToVector2();
                break;
        }

        // if the beam is on a node it has already visited, it is stuck
        if (b.History.Any(t => t.Item1 == b.Position && t.Item2 == b.Direction)) {
            b.Delete();
        }
        else {
            b.History.Add(new Tuple<Vector2, Direction>(b.Position, b.Direction));
        }
    }

    public override void PuzzleTwo() {
        // try which starting beam produces the longest path
        var longestPath = 0;
        var possibleBeams = new List<Beam>();
        // add all border beams
        for (int x = 0; x < Grid.Width; x++) {
            possibleBeams.Add(new Beam(new Vector2(x, 0), Direction.South));
            possibleBeams.Add(new Beam(new Vector2(x, Grid.Height - 1), Direction.North));
        }
        
        for (int y = 0; y < Grid.Height; y++) {
            possibleBeams.Add(new Beam(new Vector2(0, y), Direction.East));
            possibleBeams.Add(new Beam(new Vector2(Grid.Width - 1, y), Direction.West));
        }
        
        // try all possible starting beams
        foreach (var possibleBeam in possibleBeams) {
            Beams = new BlockingCollection<Beam>();
            CachedBeams = new HashSet<Tuple<Vector2, Direction>>();
            Beams.Add(possibleBeam);
            VisitedNodes = new HashSet<Node<char>>();
            while (Beams.Any(b => !b.Deleted)) {
                // move all beams
                Beams.Where(b => !b.Deleted).ToList().ForEach(MoveBeam);
            }

            // check if this path is longer than the longest path
            if (VisitedNodes.Count > longestPath) {
                longestPath = VisitedNodes.Count;
            }
        }
        
        Console.WriteLine(longestPath);
    }
}

public class Grid : NodeMap<char> {
    public Grid(int width, int height) : base(width, height) {
    }

    public Grid(string[] lines) : base(lines) {
    }

    public Grid(List<string> lines) : base(lines) {
    }
}

public class Beam {
    public Vector2 Position { get; set; }
    public Direction Direction { get; set; }

    public Beam(Vector2 position, Direction direction) {
        Position = position;
        Direction = direction;
        History.Add(new Tuple<Vector2, Direction>(position, direction));
    }

    public bool Deleted { get; set; }

    public void Delete() => Deleted = true;

    public List<Tuple<Vector2, Direction>> History { get; set; } = new();
    
    // value equality
    protected bool Equals(Beam other) {
        return Position.Equals(other.Position) && Direction.Equals(other.Direction);
    }

    public Beam Clone() {
        return new Beam(Position, Direction);
    }
}