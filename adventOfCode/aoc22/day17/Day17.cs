using System.Collections.Concurrent;
using System.Numerics;
using System.Text.RegularExpressions;
using aocTools;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.MouseButton;

namespace aoc22.day17;

public class Day17 : AAocDay {
    private static List<int> Jets = new();
    public static int JetIndex = 0;
    public static HashSet<Tuple<int, int>> JetIndexAccordingShape = new();
    public static bool DrawingStop = false;
    public static int LoopRockShape { get; set; }
    public static int LoopJetIndex { get; set; }

    public static bool LoopFound { get; set; }
    public static int LoopStartHeight { get; set; }
    public static int LoopStartRockCount { get; set; }
    public static int LoopEndRockCount { get; set; }
    public static int LoopEndHeight { get; set; }

    public static int GetJetValue() {
        var result = Jets[JetIndex++];
        if (JetIndex >= Jets.Count) JetIndex = 0;
        return result;
    }

    public static int GetHighestRock() {
        return (int) Math.Abs(Rocks.Min(r => r.Shape.Min(s => s.Y)));
    }

    public static HashSet<Vector2> Wall = new();
    public static BlockingCollection<Rock> Rocks = new();
    public Camera2D cam = new();


    public override void PuzzleOne() {
        ReadInput();
        new Thread(GenerateRocksLoop).Start();
        DrawLoop();
        Console.WriteLine();
    }

    private void GenerateRocksLoop() {
        while (true) {
            GenerateRocks();
        }
    }

    private void DrawLoop() {
        Wall = new HashSet<Vector2>() {
            new(0, 0),
            new(1, 0),
            new(2, 0),
            new(3, 0),
            new(4, 0),
            new(5, 0),
            new(6, 0),
            new(7, 0),
            new(8, 0)
        };
        for (int i = 0; i > -1000; i--) {
            Wall.Add(new Vector2(0, i));
            Wall.Add(new Vector2(8, i));
        }

        Raylib.InitWindow(2300, 1300, "Day 17");
        cam.target = new Vector2(-350, -400);
        cam.offset = new Vector2(-350, -400);
        cam.rotation = 0.0f;
        cam.zoom = 4.0f;
        var counter = 0;

        while (!Raylib.WindowShouldClose()) {

            CamMovement();
            // draw wall
            BeginDrawing();
            BeginMode2D(cam);
            ClearBackground(Color.WHITE);
            foreach (var w in Wall) {
                DrawRectangleV(w, new Vector2(1, 1), Color.BROWN);
            }
            
            counter++;
            if (counter > 10000) {
                DrawingStop = false;
            }


            // draw rocks
            foreach (var r in Rocks) {
                foreach (var v in r.Shape) {
                    DrawRectangleV(v, new Vector2(1, 1), Color.RED);
                }
            }

            EndMode2D();
            EndDrawing();

            //Thread.Sleep(800);
        }
    }

    private bool GenerateRocks() {
        if (Rocks.All(r => r.Stopped)) {
            //Console.WriteLine(Rocks.Count);
            /*if (Rocks.Count == 2022) {
                //Console.WriteLine(Math.Abs(Rocks.Min(r => r.Shape.Min(v => v.Y))));
                return true;
            }
            */

            Rocks.Add(new Rock(GetSpawnXY()));
        }

        foreach (var r in Rocks.Where(r => !r.Stopped)) {
            r.Move();
        }

        return false;
    }

    private Vector2 GetSpawnXY() {
        var y = Rocks.Count == 0 ? -4 : Rocks.Min(r => r.Shape.Min(v => v.Y)) - 4;
        return new Vector2(3, y);
    }

    private void CamMovement() {
        cam.target.Y = -GetHighestRock() - 300;

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
    }

    private void ReadInput() {
        foreach (var c in InputTokens.Read()) {
            Jets.Add(c == '>' ? +1 : -1);
        }
    }


    public override void PuzzleTwo() {
        Console.WriteLine();
    }
}

public class Rock {
    public static int CurrentRockShape = 1;

    public static Dictionary<int, HashSet<Vector2>> Shapes = new() {
        {
            1,
            new HashSet<Vector2>() {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(3, 0),
            }
        }, {
            2,
            new HashSet<Vector2>() {
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(1, -1),
                new Vector2(2, -1),
                new Vector2(1, -2)
            }
        }, {
            3,

            new HashSet<Vector2>() {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(2, 0),
                new Vector2(2, -1),
                new Vector2(2, -2)
            }
        }, {
            4,

            new HashSet<Vector2>() {
                new Vector2(0, 0),
                new Vector2(0, -1),
                new Vector2(0, -2),
                new Vector2(0, -3)
            }
        }, {
            5,

            new HashSet<Vector2>() {
                new Vector2(0, 0),
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(1, -1)
            }
        }
    };
    // The five types of rocks have the following peculiar shapes, where # is rock and . is empty space:

    //####

    //.#.
    //###
    //.#.

    //..#
    //..#
    //###

    //#
    //#
    //#
    //#

    //##
    //##

    public HashSet<Vector2> Shape = new();
    public bool Stopped = false;

    public Rock(int x, int y) {
        var s = Shapes[CurrentRockShape];
        foreach (var v in s) {
            Shape.Add(new Vector2(v.X + x, v.Y + y));
        }
    }

    public void Move() {
        MoveHorizontal();
        MoveDown();
    }

    private void MoveHorizontal() {
        var x = Day17.GetJetValue();

        // check if leftmost part of rock is at left wall
        if (Shape.Min(v => v.X) == 1 && x == -1)
            return;


        // check if rightmost part of rock is at right wall
        if (Shape.Max(v => v.X) == 7 && x == 1)
            return;


        if (Shape.Any(s => Day17.Rocks.Except(new[] {this}).Any(r => r.Shape.Contains(s + new Vector2(x, 0)))))
            return;


        Shape = Shape.Select(s => s + new Vector2(x, 0)).ToHashSet();
    }

    public void MoveDown() {
        if (Shape.Count(s => s.Y >= -1) >= 1) { // hit bottom
            Stopped = true;
            return;
        }

        // hit other rock but exclude my own parts

        if (Shape.Any(s => Day17.Rocks.Except(new[] {this}).Any(r => r.Shape.Contains(s + new Vector2(0, 1))))) {
            if (!Day17.LoopFound) {
                if (Day17.JetIndexAccordingShape.Contains(new Tuple<int, int>(Day17.JetIndex - 1, CurrentRockShape))) {
                    Console.WriteLine("loop detected");
                    Console.WriteLine(Day17.JetIndex - 1);
                    Console.WriteLine(CurrentRockShape);
                    Day17.DrawingStop = true;
                    Day17.LoopFound = true;
                    Day17.LoopStartHeight = Day17.GetHighestRock();
                    Day17.LoopStartRockCount = Day17.Rocks.Count;
                    Day17.LoopJetIndex = Day17.JetIndex - 1;
                    Day17.LoopRockShape = CurrentRockShape;
                }
                else {
                    Day17.JetIndexAccordingShape.Add(new Tuple<int, int>(Day17.JetIndex - 1, CurrentRockShape));
                }
            }
            else {
                if (Day17.JetIndex - 1 == Day17.LoopJetIndex && CurrentRockShape == Day17.LoopRockShape) {
                    Day17.LoopEndHeight = Day17.GetHighestRock();
                    Day17.LoopEndRockCount = Day17.Rocks.Count;
                    Console.WriteLine("Result:");
                    var loopHeight = Day17.LoopEndHeight - Day17.LoopStartHeight;
                    var loopRockCount = Day17.LoopEndRockCount - Day17.LoopStartRockCount;
                    Console.WriteLine(Day17.LoopStartHeight +
                                      loopHeight * (1000000000000 - Day17.LoopStartRockCount) / loopRockCount);
                    // use modulo to get height of last loop which is not complete
                    var missingRockAmount = (1000000000000 - Day17.LoopStartRockCount) % loopRockCount;
                    // calculate height of last loop missing rocks
                    var missingHeight = Day17.LoopStartHeight + loopHeight * missingRockAmount / loopRockCount;
                    Console.WriteLine(missingHeight);
                    Day17.DrawingStop = true;
                }
            }

            Stopped = true;
            return;
        }

        // else move down
        Shape = Shape.Select(s => s + new Vector2(0, 1)).ToHashSet();
    }


    public Rock(Vector2 spawn) {
        var s = Shapes[CurrentRockShape++];
        if (CurrentRockShape > Shapes.Count) {
            CurrentRockShape = 1;
        }

        foreach (var v in s) {
            Shape.Add(new Vector2(v.X + spawn.X, v.Y + spawn.Y));
        }
    }
}