using System.Collections.Concurrent;
using System.Numerics;
using aocTools;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.MouseButton;

namespace aoc22.day14;

public class Day14 : AAocDay {
    public static List<Tuple<Vector2, Vector2>> Lines { get; set; } = new();
    public static HashSet<Vector2> Wall { get; set; } = new();
    public static BlockingCollection<Sand> Sand { get; set; } = new();

    public static HashSet<Vector2> Occupied = new HashSet<Vector2>();

    public static Vector2 LowestPoint => Wall.MaxBy(w => w.Y);

    public void ReadInputLines() {
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            var vectors = line.Replace("-> ", "").Split(" ");
            for (int i = 0; i < vectors.Length - 1; i++) {
                var x1 = vectors[i].Split(",")[0].ToInt();
                var y1 = vectors[i].Split(",")[1].ToInt();
                var x2 = vectors[i + 1].Split(",")[0].ToInt();
                var y2 = vectors[i + 1].Split(",")[1].ToInt();
                Lines.Add(new Tuple<Vector2, Vector2>(new Vector2(x1, y1), new Vector2(x2, y2)));

                // Add all points to wall
                // if horizontal line
                if (y1 == y2) {
                    for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++) {
                        Wall.Add(new Vector2(x, y1));
                    }
                }
                // if vertical line
                else {
                    for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++) {
                        Wall.Add(new Vector2(x1, y));
                    }
                }
            }
        }

        Occupied = new HashSet<Vector2>(Wall);
    }

    public override void PuzzleOne() {
        ReadInputLines();
        DrawLoop();
    }

    private void DrawLoop() {
        new Thread(GenerateSandLoop).Start();

        InitWindow(DisplayWidth, DisplayHeight, "Day14");
        var cam = new Camera2D();
        cam.target = new Vector2(450, 0);
        cam.offset = new Vector2(450, 0);
        cam.rotation = 0.0f;
        cam.zoom = 5.0f;

        while (!WindowShouldClose()) {
            // zoom based on wheel
            float wheel = GetMouseWheelMove();
            if (wheel != 0) {
                // get the world point that is under the mouse
                Vector2 mouseWorldPos = GetScreenToWorld2D(GetMousePosition(), cam);

                // set the offset to where the mouse is
                cam.offset = GetMousePosition();

                // set the target to match, so that the camera maps the world space point under the cursor to the screen space point under the cursor at any zoom
                cam.target = mouseWorldPos;

                // zoom
                cam.zoom += wheel * 0.125f;
                if (cam.zoom < 0.125f)
                    cam.zoom = 0.125f;
            }

            // move camera based on arrow keys
            if (IsKeyDown(KeyboardKey.KEY_RIGHT))
                cam.target.X += 0.1f;

            if (IsKeyDown(KeyboardKey.KEY_LEFT))
                cam.target.X -= 0.1f;

            if (IsKeyDown(KeyboardKey.KEY_UP))
                cam.target.Y -= 0.1f;

            if (IsKeyDown(KeyboardKey.KEY_DOWN))
                cam.target.Y += 0.1f;

            // draw head and tail
            BeginDrawing();
            ClearBackground(Color.WHITE);
            BeginMode2D(cam);

            foreach (var line in Lines) {
                // draw vertical and horizontal lines as rectangles to make them easier to see
                if (line.Item1.X == line.Item2.X) {
                    DrawRectangle((int) line.Item1.X, (int) Math.Min(line.Item1.Y, line.Item2.Y), 1,
                        (int) Math.Abs(line.Item2.Y - line.Item1.Y) + 1, Color.BLACK);
                }
                else if (line.Item1.Y == line.Item2.Y) {
                    DrawRectangle((int) Math.Min(line.Item1.X, line.Item2.X), (int) line.Item1.Y,
                        (int) Math.Abs(line.Item2.X - line.Item1.X) + 1, 1, Color.BLACK);
                }
                else {
                    DrawLineV(line.Item1, line.Item2, Color.BLACK);
                }
            }

            foreach (var sand in Sand) {
                DrawRectangle((int) sand.Position.X, (int) sand.Position.Y, 1, 1, Color.GOLD);
            }

            //Thread.Sleep(1);

            EndMode2D();
            EndDrawing();
        }
    }

    private bool dropSand = true;

    private void GenerateSandLoop() {
        while (true) {
            GenerateSand();
            if (Sand.Any(s => s.Position.X == 500 && s.Position.Y == 0)) {
                Console.WriteLine(Sand.Count);
                return;
            }
        }
    }

    private void GenerateSand() {
        if (dropSand) {
            Sand.Add(new Sand(new Vector2(500, 0)));
        }

        dropSand = !dropSand;

        Sand.Where(s => !s.Resting).ToList().ForEach(s => s.Move());
    }

    private const int DisplayHeight = 900;

    private const int DisplayWidth = 1700;

    public override void PuzzleTwo() {
        ReadInputLines();
        Sand = new BlockingCollection<Sand>();
        var lowestY = LowestPoint.Y;
        Lines.Add(new Tuple<Vector2, Vector2>(new Vector2(0, lowestY + 2), new Vector2(DisplayWidth, lowestY + 2)));
        // add line points to wall
        for (int x = 0; x < DisplayWidth; x++) {
            Wall.Add(new Vector2(x, lowestY + 2));
        }

        DrawLoop();
    }
}

public class Sand {
    public Vector2 Position { get; set; }
    public bool Resting { get; set; }

    public Sand(Vector2 position) {
        Position = position;
    }

    public Sand(int x, int y) {
        Position = new Vector2(x, y);
    }

    public void Move() {
        if (Resting) {
            return;
        }
        // move down if not hitting a wall or sand
        if (!Day14.Wall.Contains(Position + Vector2.UnitY) &&
            Day14.Sand.All(s => s.Position != Position + Vector2.UnitY)) {
            Position += Vector2.UnitY;
        }
        else {
            // if hitting a wall on both sides, stop
            if (!Day14.Wall.Contains(Position + new Vector2(-1, 1)) &&
                Day14.Sand.All(s => s.Position != Position + new Vector2(-1, 1))) {
                Position += new Vector2(-1, 1);
            }
            else if (!Day14.Wall.Contains(Position + new Vector2(1, 1)) &&
                     Day14.Sand.All(s => s.Position != Position + new Vector2(1, 1))) {
                Position += new Vector2(1, 1);
            }
            else {
                Resting = true;
                // remove sand under this sand from blocking collection
                //Day14.Sand.Where(s => s.Position.Y == Position.Y + 1 && s.Position.X == Position.X && s.Resting).ToList().ForEach(s => Day14.Sand.TryTake(out s));
            }
        }
    }
}