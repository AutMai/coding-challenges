using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

namespace aoc24.day14;

public class Day14 : AAocDay {
    public BlockingCollection<Robot> Robots { get; set; } = [];

    public Day14() : base(true) {
        ReadInput();
    }

    private void ReadInput() {
        Robots = new BlockingCollection<Robot>();
        while (InputTokens.HasMoreTokens()) {
            var p = InputTokens.Read();
            var v = InputTokens.Read();
            var robot = new Robot(p, v);
            Robots.Add(robot);
        }
    }

    public override void PuzzleOne() {
        for (var i = 0; i < 100; i++) {
            foreach (var robot in Robots) {
                robot.Move();
            }
        }

        PrintSpace();
        Console.WriteLine(GetSafetyFactor());
    }

    private const int DisplayHeight = 900;

    private const int DisplayWidth = 1700;

    private void DrawLoop() {
        InitWindow(DisplayWidth, DisplayHeight, "Day14");
        var cam = new Camera2D();
        cam.Target = new Vector2(-80, -10);
        cam.Offset = new Vector2(-80, -10);
        cam.Rotation = 0.0f;
        cam.Zoom = 7.0f;

        while (!WindowShouldClose()) {
            // zoom based on wheel
            float wheel = GetMouseWheelMove();
            if (wheel != 0) {
                // get the world point that is under the mouse
                Vector2 mouseWorldPos = GetScreenToWorld2D(GetMousePosition(), cam);

                // set the offset to where the mouse is
                cam.Offset = GetMousePosition();

                // set the target to match, so that the camera maps the world space point under the cursor to the screen space point under the cursor at any zoom
                cam.Target = mouseWorldPos;

                // zoom
                cam.Zoom += wheel * 0.125f;
                if (cam.Zoom < 0.125f)
                    cam.Zoom = 0.125f;
            }

            // move camera based on arrow keys
            if (IsKeyDown(KeyboardKey.Right))
                cam.Target.X += 0.1f;

            if (IsKeyDown(KeyboardKey.Left))
                cam.Target.X -= 0.1f;

            if (IsKeyDown(KeyboardKey.Up))
                cam.Target.Y -= 0.1f;

            if (IsKeyDown(KeyboardKey.Down))
                cam.Target.Y += 0.1f;

            if (IsKeyDown(KeyboardKey.Space)) {
                MoveRobots();
            }

            if (IsKeyDown(KeyboardKey.LeftControl)) {
                MoveRobots(0);
            }

            if (IsKeyDown(KeyboardKey.LeftShift)) {
                // block keyboard input
                // start MoveRobots in a new thread
                MoveRobots(0, true);
            }

            // draw head and tail
            BeginDrawing();
            ClearBackground(Color.White);
            BeginMode2D(cam);

            // draw the space
            DrawRectangle(0, 0, Robot.SpaceWidth, Robot.SpaceHeight, Color.LightGray);

            // draw the elapsed seconds as text
            DrawText($"Elapsed seconds: {Interlocked.CompareExchange(ref _elapsedSeconds, 0, 0)}", -10, -10, 10,
                Color.Black);

            foreach (var robot in Robots) {
                // if the robot is adjacent to 10 other robots, draw it in red
                DrawRectangle((int)robot.Position.X, (int)robot.Position.Y, 1, 1, Color.Black);
            }

            foreach (var aRobots in _adjacentRobots) {
                DrawRectangle((int)aRobots.Position.X, (int)aRobots.Position.Y, 1, 1, Color.Red);
            }

            //Thread.Sleep(1);

            EndMode2D();
            EndDrawing();
        }
    }

    // add a field ElapsedSeconds to keep track of the time but use a lock because this field is accessed by multiple threads
    private int _elapsedSeconds = 0;

    private void MoveRobots(int sleep = 100, bool until10Adjacent = false) {
        while (true) {
            Parallel.ForEach(Robots, robot => robot.Move());
            Interlocked.Increment(ref _elapsedSeconds);
            Thread.Sleep(sleep);
            if (!until10Adjacent)
                break;

            if (Robots.Any(r => GetAdjacentCount(r) >= 15)) {
                break;
            }
        }
    }

    private int GetAdjacentCount(Robot robot) {
        // flood fill to find adjacent robots
        var visited = new HashSet<Vector2>();
        var queue = new Queue<Vector2>();
        queue.Enqueue(robot.Position);
        var count = 0;

        while (queue.Count > 0) {
            var pos = queue.Dequeue();
            if (visited.Contains(pos)) {
                continue;
            }

            visited.Add(pos);
            count++;
            // add adjacent positions to the queue
            var adjacent = new[] {
                new Vector2(pos.X + 1, pos.Y),
                new Vector2(pos.X - 1, pos.Y),
                new Vector2(pos.X, pos.Y + 1),
                new Vector2(pos.X, pos.Y - 1)
            };
            foreach (var adj in adjacent) {
                if (Robots.Any(r => r.Position == adj)) {
                    queue.Enqueue(adj);
                }
            }
        }

        _adjacentRobots = new BlockingCollection<Robot>();
        foreach (var v in visited) {
            _adjacentRobots.Add(Robots.First(r => r.Position == v));
        }

        return count;
    }

    private BlockingCollection<Robot> _adjacentRobots = new();

    private int GetSafetyFactor() {
        // get count of robots in each quadrant (top-left, top-right, bottom-left, bottom-right) and multiply them
        var topLeft = Robots.Count(r => r.Position.X < Robot.SpaceWidth / 2 && r.Position.Y < Robot.SpaceHeight / 2);
        var topRight = Robots.Count(r => r.Position.X > Robot.SpaceWidth / 2 && r.Position.Y < Robot.SpaceHeight / 2);
        var bottomLeft = Robots.Count(r => r.Position.X < Robot.SpaceWidth / 2 && r.Position.Y > Robot.SpaceHeight / 2);
        var bottomRight =
            Robots.Count(r => r.Position.X > Robot.SpaceWidth / 2 && r.Position.Y > Robot.SpaceHeight / 2);
        Console.WriteLine(
            $"Top-left: {topLeft}, Top-right: {topRight}, Bottom-left: {bottomLeft}, Bottom-right: {bottomRight}");
        return topLeft * topRight * bottomLeft * bottomRight;
    }

    private void PrintSpace() {
        var sb = new StringBuilder();
        for (var y = 0; y < Robot.SpaceHeight; y++) {
            for (var x = 0; x < Robot.SpaceWidth; x++) {
                var pos = new Vector2(x, y);
                var robotCount = Robots.Count(r => r.Position == pos);
                // . or robotCount
                sb.Append(robotCount > 0 ? robotCount.ToString() : '.');
            }

            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }

    public override void PuzzleTwo() {
        ResetInput();
        ReadInput();
        DrawLoop();
    }
}

public class Robot {
    public Vector2 Position { get; set; }

    public Vector2 Velocity { get; set; }

    public static int SpaceWidth { get; set; } = 101;
    public static int SpaceHeight { get; set; } = 103;

    public Robot(Vector2 position, Vector2 velocity) {
        Position = position;
        Velocity = velocity;
    }

    public Robot(string position, string velocity) {
        Position = ParseVector(position);
        Velocity = ParseVector(velocity);
    }

    private static Vector2 ParseVector(string s) {
        s = s.Replace("p=", "").Replace("v=", "");
        var tokens = s.Split(',');
        return new Vector2(int.Parse(tokens[0]), int.Parse(tokens[1]));
    }

    public void Move() {
        // if the robot moves out of the space, it teleports to the other side
        Position += Velocity;
        var posX = (Position.X + SpaceWidth) % SpaceWidth;
        var posY = (Position.Y + SpaceHeight) % SpaceHeight;
        Position = new Vector2(posX, posY);
    }
}