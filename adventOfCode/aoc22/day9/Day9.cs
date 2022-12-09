using System.Numerics;
using aoc22.day8;
using aocTools;
using Raylib_cs;

namespace aoc22.day9;

public class Day9 : AAocDay {
    public List<Vector2> Steps = new List<Vector2>();
    private const int DisplayWidth = 2300;
    private const int DisplayHeight = 1300;
    private const int UnitSize = 4;
    private HashSet<Vector2> _visited = new();
    private bool displayNumbers = false;

    public Day9() : base(true) {
    }

    public override void PuzzleOne() {
        ReadSteps();
        Console.WriteLine(Steps);
        var head = new Vector2(DisplayWidth / 2, 1000);
        var tail = new Vector2(DisplayWidth / 2, 1000);
        _visited.Add(tail);

        Raylib.InitWindow(DisplayWidth, DisplayHeight, "Hello World");
        while (!Raylib.WindowShouldClose()) {
            // draw head and tail
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);
            Raylib.DrawRectangle((int) head.X, (int) head.Y, UnitSize, UnitSize, Color.RED);
            Raylib.DrawRectangle((int) tail.X, (int) tail.Y, UnitSize, UnitSize, Color.BLUE);
            Raylib.EndDrawing();

            // move head
            if (Steps.Count > 0) {
                var step = Steps[0];
                Steps.RemoveAt(0);
                head += step;
            }
            else {
                break;
            }

            if (Vector2.Distance(head, tail) >= 2 * UnitSize) // move tail 
            {
                var distance = Vector2.Subtract(head, tail);
                var tailStep = new Vector2(Math.Sign(distance.X) * UnitSize, Math.Sign(distance.Y) * UnitSize);
                tail += tailStep;

                if (!_visited.Contains(tail)) {
                    _visited.Add(tail);
                }
            }
        }

        Console.WriteLine(_visited.Count);
    }

    private void ReadSteps() {
        while (InputTokens.HasMoreTokens()) {
            var step = InputTokens.Read();
            var amount = InputTokens.ReadInt();

            for (int i = 0; i < amount; i++) {
                Steps.Add(step switch {
                    "D" => // down
                        new Vector2(0, UnitSize),
                    "U" => //up
                        new Vector2(0, -UnitSize),
                    "L" => //left
                        new Vector2(-UnitSize, 0),
                    "R" => //right
                        new Vector2(UnitSize, 0),
                    _ => default
                });
            }
        }
    }

    public override void PuzzleTwo() {
        ResetInput();
        ReadSteps();
        _visited = new HashSet<Vector2>();

        List<Vector2> rope = new List<Vector2>();
        for (int i = 0; i < 10; i++) {
            rope.Add(new Vector2(600, 1200));
        }

        _visited.Add(rope.Last());

        Raylib.InitWindow(DisplayWidth, DisplayHeight, "Hello World");
        while (!Raylib.WindowShouldClose()) {
            // draw head and tail
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);
            //draw visited
            foreach (var visited in _visited) {
                Raylib.DrawRectangle((int) visited.X, (int) visited.Y, UnitSize, UnitSize, Color.LIGHTGRAY);
            }
            
            for (int i = 0; i < rope.Count; i++) {
                Raylib.DrawRectangle((int) rope[i].X, (int) rope[i].Y, UnitSize,UnitSize, Color.RED);
                if (displayNumbers)
                    Raylib.DrawText(i.ToString(), (int) rope[i].X+UnitSize/4, (int) rope[i].Y+UnitSize/10, 18, Color.BLACK);
            }
            
            

            Raylib.EndDrawing();

            // move head
            if (Steps.Count > 0) {
                var step = Steps[0];
                Steps.RemoveAt(0);
                rope[0] += step;
            }
            else {
                // save canvas as image
                Raylib.TakeScreenshot("day9.png");
                break;
            }

            for (int i = 1; i < rope.Count; i++) {
                if (Vector2.Distance(rope[i - 1], rope[i]) >= 2 * UnitSize) // move tail 
                {
                    var distance = Vector2.Subtract(rope[i - 1], rope[i]);
                    var tailStep = new Vector2(Math.Sign(distance.X) * UnitSize, Math.Sign(distance.Y) * UnitSize);
                    rope[i] += tailStep;
                }
            }

            if (!_visited.Contains(rope.Last())) {
                _visited.Add(rope.Last());
            }

            //Thread.Sleep(10);
        }
        Console.WriteLine(_visited.Count);
    }
}

public class Rope {
    public Vector2 Head { get; set; }
    public Vector2 Tail { get; set; }

    public Rope(Vector2 head, Vector2 tail) {
        Head = head;
        Tail = tail;
    }

    public bool TailIsMoreThanOneAwayFromHead() {
        return Math.Abs(Head.X - Tail.X) > 1 && Math.Abs(Head.Y - Tail.Y) > 1;
    }

    public bool TailWouldBeMoreThanOneAwayFromHead(Vector2 newHead) {
        return Math.Abs(newHead.X - Tail.X) > 1 && Math.Abs(newHead.Y - Tail.Y) > 1;
    }

    public bool HeadTailAreDiagonal() {
        return Head.X != Tail.X && Head.Y != Tail.Y;
    }

    public bool HeadTailAreDiagonal(Vector2 head) {
        return head.X != Tail.X && head.Y != Tail.Y;
    }

    public bool HeadCoversTail() {
        return Head.Equals(Tail);
    }

    public void MoveHead(char direction, int distance) {
        for (int i = 0; i < distance; i++) {
            switch (direction) {
                case 'U':
                    Head -= Vector2.UnitY;
                    break;
                case 'D':
                    Head += Vector2.UnitY;
                    break;
                case 'L':
                    Head -= Vector2.UnitX;
                    break;
                case 'R':
                    MoveHeadRight();
                    break;
            }
        }
    }

    public void MoveHeadRight() {
        var oldHead = Head;
        Head += Vector2.UnitX;
        if (TailIsMoreThanOneAwayFromHead()) {
            MoveTailToHead(oldHead);
        }
    }

    public void MoveTailToHead(Vector2 oldHead) {
        // tail has to keep up with head
        // if tail is diagonal to head, move it to the same x or y as head
        if (HeadTailAreDiagonal(oldHead)) {
            if (Head.X > Tail.X) {
                Tail += Vector2.UnitX;
            }
            else if (Head.X < Tail.X) {
                Tail -= Vector2.UnitX;
            }
            else if (Head.Y > Tail.Y) {
                Tail += Vector2.UnitY;
            }
            else if (Head.Y < Tail.Y) {
                Tail -= Vector2.UnitY;
            }
        }
        else {
            // if tail is not diagonal to head, move it to the same x and y as head
            Tail = Head;
        }
    }
}