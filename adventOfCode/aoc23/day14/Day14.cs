using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;
using aocTools.Neo4J;

namespace aoc23.day14;

public class Day14 : AAocDay {
    public static Rocklist Rocks = new();
    public HashSet<int> KnownStates = new();
    public static HashSet<Vector2> Occupied = new HashSet<Vector2>();
    public static NodeMap<char> Grid { get; set; }

    public Day14() {
        ReadInput();
    }
    
    private void ReadInput() {
        Rocks = new Rocklist();
        Grid = new NodeMap<char>(InputTokens);
        Grid.NodeList.Where(n => n.Value == 'O').ToList()
            .ForEach(n => Rocks.Add(new Rock(new Vector2(n.PosX, n.PosY), Direction.North)));
        Grid.NodeList.Where(n => n.Value == 'O').ToList().ForEach(n => n.Value = '.');
        Occupied = new HashSet<Vector2>(Grid.NodeList.Where(n => n.Value == '#').Select(n => new Vector2(n.PosX, n.PosY)));
        // add rocks to occupied
        Occupied.UnionWith(Rocks.Select(r => r.Position));
    }

    public override void PuzzleOne() {
        Rocks = [..Rocks.OrderBy(r => r.Position.Y)];

        MoveRocks();


        var sum = Rocks.Sum(r => Grid.Height - r.Position.Y);
        Console.WriteLine($"Sum of y positions: {sum}");
    }

    private void MoveRocks() {
        while (Rocks.Any(r => !r.Stuck)) {
            Rocks.Where(r => !r.Stuck).ToList().ForEach(r => r.Move());
        }
    }

    private void PrintGrid() {
        // print grid but if a rock is on a node, print 'O'
        var sb = new StringBuilder();
        for (var y = 0; y < Grid.Height; y++) {
            for (var x = 0; x < Grid.Width; x++) {
                var node = Grid.GetNode(x, y);
                var rock = Rocks.FirstOrDefault(r => r.Position == new Vector2(x, y));
                if (rock != null) {
                    sb.Append('O');
                }
                else {
                    sb.Append(node.Value);
                }
            }

            sb.Append('\n');
        }

        Console.WriteLine(sb.ToString());
    }

    public override void PuzzleTwo() {
        var cycleBegin = 0;
        var cycleEnd = 0;
        bool cycleFound = false;
        ReadInput();
        Console.WriteLine();
        int cycles = 1000000000;
        List<Direction> directions = new() { Direction.North, Direction.West, Direction.South, Direction.East };
        for (int i = 1; i <= cycles; i++) {
            // in each cycle, move all rocks in all directions
            foreach (var direction in directions) {
                // unstuck rocks
                Rocks.Where(r => r.Stuck).ToList().ForEach(r => r.Stuck = false);
                // order rocks
                if (direction == Direction.North) {
                    Rocks = new Rocklist(Rocks.OrderBy(r => r.Position.Y));
                }
                else if (direction == Direction.South) {
                    Rocks = new Rocklist(Rocks.OrderByDescending(r => r.Position.Y));
                }
                else if (direction == Direction.West) {
                    Rocks = new Rocklist(Rocks.OrderBy(r => r.Position.X));
                }
                else if (direction == Direction.East) {
                    Rocks = new Rocklist(Rocks.OrderByDescending(r => r.Position.X));
                }
                // set direction of rocks
                Rocks.Where(r => !r.Stuck).ToList().ForEach(r => r.Direction = direction);
                // move rocks
                MoveRocks();
            }
            if (cycleFound) {
                continue;
            }
            // add state to known states
            var state = Rocks.GetHashCode();
            // if state is already known, we found a cycle
            if (!KnownStates.Add(state)) {
                cycleFound = true;
                //PrintGrid();
                Console.WriteLine($"Cycle {i} is the same as cycle {KnownStates.ToList().IndexOf(state) + 1}");
                cycleBegin = KnownStates.ToList().IndexOf(state) + 1;
                cycleEnd = i;
                var cycleLength = cycleEnd - cycleBegin;
                // set cycles to the remaining cycles
                var remainingCycles = cycles - cycleEnd;
                // set cycles to the remaining cycles modulo cycleLength
                cycles = i + remainingCycles % cycleLength;
            }
        }
        var sum = Rocks.Sum(r => Grid.Height - r.Position.Y);
        Console.WriteLine($"Sum of y positions: {sum}");
    }
}

public class Rock {
    public Vector2 Position { get; set; }
    public Direction Direction { get; set; }

    public bool Stuck { get; set; }

    public Rock(Vector2 position, Direction direction) {
        Position = position;
        Direction = direction;
    }

    public void Move() {
        // move rock in direction if it is '.'
        var newPos = this.Position + this.Direction.ToVector2();

        // if rock is out of bounds dont add it
        if (newPos.Y < 0 || newPos.Y >= Day14.Grid.Height || newPos.X < 0 || newPos.X >= Day14.Grid.Width) {
            this.Stuck = true;
            return;
        }

        // if on newPos is already a rock, dont add it
        
        if (Day14.Occupied.Contains(newPos)) {
            this.Stuck = true;
            return;
        }
        
        Day14.Occupied.Remove(this.Position);
        this.Position = newPos;
        Day14.Occupied.Add(newPos);
    }
}

public class Rocklist : HashSet<Rock> {
    public Rocklist() {
    }
    public Rocklist(IEnumerable<Rock> rocks) : base(rocks) {
    }
    public override int GetHashCode() {
        // create a unique hashcode for the rocks that includes their position
        var sb = new StringBuilder();
        foreach (var rock in this) {
            sb.Append(rock.Position);
        }
        
        return sb.ToString().GetHashCode();
    }
}