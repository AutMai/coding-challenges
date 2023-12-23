using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

namespace aoc23.day22;

public class Day22 : AAocDay {
    HashSet<Brick> Bricks = new();

    public Day22() {
        while (InputTokens.HasMoreTokens()) {
            Bricks.Add(new Brick(InputTokens.Read()));
        }
    }

    const int screenWidth = 1700;
    const int screenHeight = 900;

    Camera3D camera;

    private void RaylibInit() {
        InitWindow(screenWidth, screenHeight, "Day18");

        camera.Position = new Vector3(10.0f, 10.0f, 10.0f); // Camera3D position
        camera.Target = new Vector3(0.0f, 0.0f, 5.0f); // Camera3D looking at point
        camera.Up = new Vector3(0.0f, 0.0f, 1.0f); // Camera3D up vector (rotation towards target)
        camera.FovY = 45.0f; // Camera3D field-of-view Y
        camera.Projection = CameraProjection.CAMERA_PERSPECTIVE;


        // Set a free camera mode
        UpdateCamera(ref camera, CameraMode.CAMERA_CUSTOM); // Update camera

        SetTargetFPS(165); // Set our game to run at 60 frames-per-second
    }

    public override void PuzzleOne() {
        RaylibInit();
        new Thread(LetBricksFall).Start();

        // Main game loop
        while (!WindowShouldClose()) // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            UpdateCamera(ref camera, CameraMode.CAMERA_CUSTOM);

            if (IsKeyDown(KEY_Z))
                camera.Target = new Vector3(0.0f, 0.0f, 5.0f);
            if (IsKeyDown(KEY_X)) {
                // watch from x direction
                camera.Target = new Vector3(0.0f, 0.0f, 50.0f);
                camera.Position = new Vector3(50.0f, 0.0f, 50.0f);
            }

            if (IsKeyDown(KEY_Y)) {
                // watch from y direction
                camera.Target = new Vector3(0.0f, 0.0f, 50.0f);
                camera.Position = new Vector3(0.0f, 50.0f, 50.0f);
            }


            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(RAYWHITE);

            BeginMode3D(camera);

            foreach (var b in Bricks) {
                // add vector 1 to size to get a cube
                var color = b.Resting ? GREEN : RED;
                DrawCylinderEx(b.Pos1, b.Pos2, 0.5f, 0.5f, 8, color);
                DrawCylinderWiresEx(b.Pos1, b.Pos2, 0.5f, 0.5f, 8, LIGHTGRAY);
                DrawSphere(b.Pos1, 0.1f, BLACK);
            }

            // draw floor at z = 0
            DrawCubeV(new Vector3(0, 0, 0), new Vector3(100, 100, 0), LIGHTGRAY);

            // draw a vertical line at z = 0
            DrawLine3D(new Vector3(0, 0, 0), new Vector3(0, 0, 100), BLACK);

            /*foreach (var cube in Cubes) {
                DrawCubeV(cube, Vector3.One, LIGHTGRAY);
                DrawCubeWiresV(cube, Vector3.One, BLACK);
            }*/

            EndMode3D();

            DrawText("Exposed Sides: ", 10, 10, 20, DARKGRAY);

            EndDrawing();
        }

        CloseWindow(); // Close window and OpenGL context
    }

    private void LetBricksFall() {
        while (Bricks.Any(b => !b.Resting)) {
            foreach (var brick in Bricks.Where(b => !b.Resting).OrderBy(b => b.Pos1.Z)) {
                while (!brick.Resting) {
                    //Thread.Sleep(1);
                    // check if brick is resting (brick can be longer or shorter than other bricks so we also have to consider the vectors between pos1 and pos2)
                    if (CheckResting(brick)) {
                        brick.Resting = true;
                    }
                    else {
                        brick.Pos1 -= Vector3.UnitZ;
                        brick.Pos2 -= Vector3.UnitZ;
                    }
                }
            }
        }

        Console.WriteLine("All bricks are resting");
        Dictionary<Brick, HashSet<Brick>> SupportedBy = new();
        Dictionary<Brick, HashSet<Brick>> IsSupporting = new();
        // add each brick to the dictionary and all bricks that have at least one part under the brick
        foreach (var brick in Bricks.OrderBy(b => b.Pos1.Z)) {
            SupportedBy.Add(brick, new HashSet<Brick>());
            IsSupporting.Add(brick, new HashSet<Brick>());
            foreach (var otherBrick in Bricks) {
                if (otherBrick == brick) continue;
                if (otherBrick.GetAllParts().Any(p => brick.GetAllParts().Contains(p + Vector3.UnitZ))) {
                    SupportedBy[brick].Add(otherBrick);
                }

                if (otherBrick.GetAllParts().Any(p => brick.GetAllParts().Contains(p - Vector3.UnitZ))) {
                    IsSupporting[brick].Add(otherBrick);
                }
            }
        }

        var bricksThatCauseOtherBricksToFall = new HashSet<Brick>();

        var bricksToRemoveCount = 0;

        foreach (var (brick, otherbricks) in IsSupporting) {
            // if the brick is not supporting any other bricks, it can be removed
            if (otherbricks.Count == 0) {
                bricksToRemoveCount++;
                continue;
            }

            // if the brick is supporting other bricks, it can be removed if it supports a brick that is also supported by another brick
            var safeToRemove = true;
            foreach (var otherBrick in otherbricks) {
                if (SupportedBy[otherBrick].Count <= 1) {
                    safeToRemove = false;
                }
            }

            if (safeToRemove)
                bricksToRemoveCount++;
            else
                bricksThatCauseOtherBricksToFall.Add(brick);
        }

        Console.WriteLine($"Bricks to remove: {bricksToRemoveCount}");

        var fallCount = 0;
        var removeBrickFallCount = new Dictionary<Brick, int>();
        foreach (var brick in bricksThatCauseOtherBricksToFall) {
            var fc = GetFallCount(brick, IsSupporting, SupportedBy);
            //Console.WriteLine($"Brick {brick.Pos1} would cause {fc.Count} bricks to fall");
            fallCount += fc.Count;
        }

        Console.WriteLine(fallCount);
    }

    private HashSet<Brick> GetFallCount(Brick brick, Dictionary<Brick, HashSet<Brick>> isSupporting,
        Dictionary<Brick, HashSet<Brick>> supportedBy) {
        Queue<Brick> bricksToCheck = new();
        HashSet<Brick> fallingBricks = new();
        bricksToCheck.Enqueue(brick);
        fallingBricks.Add(brick);
        // check which bricks would fall if the bricks in fallingBricks are removed and add them to fallingBricks
        while (true) {
            var fb = bricksToCheck.Dequeue();
            foreach (var supportedBrick in isSupporting[fb]) {
                var support = supportedBy[supportedBrick];
                // check if the whole support hashset is in fallingBricks
                if (support.All(b => fallingBricks.Contains(b))) {
                    fallingBricks.Add(supportedBrick);
                    bricksToCheck.Enqueue(supportedBrick);
                }
            }

            if (bricksToCheck.Count == 0) {
                break;
            }
        }

        fallingBricks.Remove(brick);
        return fallingBricks;
    }

    private bool CheckResting(Brick brick) {
        var lowPoint = Math.Min(brick.Pos1.Z, brick.Pos2.Z);
        var bricksBelow = Bricks.Where(b => b.Pos1.Z == (lowPoint - 1) || b.Pos2.Z == (lowPoint - 1)).ToList();
        // check if any of the parts of the brick are resting on another brick
        var parts = brick.GetAllParts();
        // check if any of the parts are resting on another brick
        foreach (var part in parts) {
            if (bricksBelow.Any(b => b.GetAllParts().Contains(part - Vector3.UnitZ))) {
                return true;
            }

            if (part.Z == 0) {
                return true;
            }
        }

        return false;
    }

    public override void PuzzleTwo() {
        // move bricks -1 in z direction until they rest on another brick or the floor
    }
}

class Brick {
    public Vector3 Pos1 { get; set; }
    public Vector3 Pos2 { get; set; }

    public Vector3 Size { get; set; }

    public bool Resting { get; set; }

    public HashSet<Vector3> GetAllParts() {
        // if a brick is 1x1x1, it has 1 part
        // if a brick is 2x3x4, it has 24 parts
        var parts = new HashSet<Vector3>();
        for (int x = 0; x < Size.X; x++) {
            for (int y = 0; y < Size.Y; y++) {
                for (int z = 0; z < Size.Z; z++) {
                    parts.Add(new Vector3(Pos1.X + x, Pos1.Y + y, Pos1.Z + z));
                }
            }
        }

        return parts;
    }

    public Brick(string pos) {
        // string is in format "1,0,1~1,2,1"
        var split = pos.Split("~");
        var pos1 = split[0].Split(",");
        var pos2 = split[1].Split(",");
        Pos1 = new Vector3(int.Parse(pos1[0]), int.Parse(pos1[1]), int.Parse(pos1[2]));
        Pos2 = new Vector3(int.Parse(pos2[0]), int.Parse(pos2[1]), int.Parse(pos2[2]));
        Size = new Vector3(Math.Abs(Pos1.X - Pos2.X), Math.Abs(Pos1.Y - Pos2.Y), Math.Abs(Pos1.Z - Pos2.Z)) +
               Vector3.One;
    }
}