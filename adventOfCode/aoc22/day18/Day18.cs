using System.Collections.Concurrent;
using System.Numerics;
using System.Text.RegularExpressions;
using aocTools;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.CameraMode;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Color;

namespace aoc22.day18;

public class Day18 : AAocDay {
    public HashSet<Vector3> Cubes = new();
    public BlockingCollection<Vector3> LavaCubes = new();
    bool _lavaCubesCompleted = new();
    int _cubeSidesTouchingLava = 0;

    const int screenWidth = 800;
    const int screenHeight = 450;

    Camera3D camera;


    public override void PuzzleOne() {
        ReadInput();
        RaylibInit();


        // Main game loop
        while (!WindowShouldClose()) // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            UpdateCamera(ref camera); // Update camera

            if (IsKeyDown(KEY_Z))
                camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(RAYWHITE);

            BeginMode3D(camera);

            foreach (var cube in Cubes) {
                DrawCubeV(cube, Vector3.One, LIGHTGRAY);
                DrawCubeWiresV(cube, Vector3.One, BLACK);
            }

            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawText("Exposed Sides: " + GetCubeExposedSideCount(), 10, 10, 20, DARKGRAY);

            EndDrawing();
        }

        CloseWindow(); // Close window and OpenGL context
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read().Split(",");
            Cubes.Add(new Vector3(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2])));
        }
    }

    private void RaylibInit() {
        InitWindow(screenWidth, screenHeight, "Day18");

        camera.position = new Vector3(10.0f, 10.0f, 10.0f); // Camera3D position
        camera.target = new Vector3(0.0f, 0.0f, 0.0f); // Camera3D looking at point
        camera.up = new Vector3(0.0f, 1.0f, 0.0f); // Camera3D up vector (rotation towards target)
        camera.fovy = 45.0f; // Camera3D field-of-view Y
        camera.projection = CAMERA_PERSPECTIVE;

        Vector3 cubePosition = new Vector3(0.0f, 0.0f, 0.0f);

        SetCameraMode(camera, CAMERA_FREE); // Set a free camera mode

        SetTargetFPS(165); // Set our game to run at 60 frames-per-second
    }

    private int GetCubeExposedSideCount(bool onlySeenFromOutside = false) {
        // count all sides of all cubes that are not adjacent to another cube
        var exposedSides = 0;

        foreach (var cube in Cubes) {
            exposedSides += GetCubeExposedSideCount(cube);
        }

        return exposedSides;
    }

    private int GetCubeExposedSideCount(Vector3 cube) {
        var exposedSides = 0;

        if (!Cubes.Contains(cube + Vector3.UnitX))
            exposedSides++;
        if (!Cubes.Contains(cube - Vector3.UnitX))
            exposedSides++;
        if (!Cubes.Contains(cube + Vector3.UnitY))
            exposedSides++;
        if (!Cubes.Contains(cube - Vector3.UnitY))
            exposedSides++;
        if (!Cubes.Contains(cube + Vector3.UnitZ))
            exposedSides++;
        if (!Cubes.Contains(cube - Vector3.UnitZ))
            exposedSides++;

        return exposedSides;
    }


    public override void PuzzleTwo() {

        ReadInput();
        RaylibInit();
        Texture2D lavaTexture = LoadTexture(@"../../../day18/lava.png");

        new Thread(FillLavaCubesOutside).Start();

        // Main game loop
        while (!WindowShouldClose()) // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            UpdateCamera(ref camera); // Update camera

            if (IsKeyDown(KEY_Z))
                // set camera to look at the center of the cubes
                camera.target = new Vector3(
                    (Cubes.Max(c => c.X) - Cubes.Min(c => c.X)) / 2,
                    (Cubes.Max(c => c.Y) - Cubes.Min(c => c.Y)) / 2,
                    (Cubes.Max(c => c.Z) - Cubes.Min(c => c.Z)) / 2
                );
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(RAYWHITE);

            BeginMode3D(camera);

            foreach (var cube in Cubes) {
                DrawCubeV(cube, Vector3.One, LIGHTGRAY);
                DrawCubeWiresV(cube, Vector3.One, BLACK);
            }

            foreach (var l in LavaCubes) {
                //DrawCubeV(l, Vector3.One, ORANGE);
                DrawCubeWiresV(l, Vector3.One, ORANGE);
                DrawCubeTexture(lavaTexture, l, 1.0f, 1.0f, 1.0f, WHITE);
            }

            DrawGrid(10, 1.0f);

            EndMode3D();
            if (_lavaCubesCompleted && _cubeSidesTouchingLava == 0) {
                new Task(GetCubesTouchingLava).Start();
            }
            
            DrawText(
                "Sides Touching Lava: " +
                (_cubeSidesTouchingLava == 0 ? "Waiting for Lava to generate" : _cubeSidesTouchingLava), 10, 10, 20,
                DARKGRAY);

            EndDrawing();
        }

        CloseWindow(); // Close window and OpenGL context
    }

    private void GetCubesTouchingLava() {
        // count all sides of all cubes that are adjacent to a lava cube
        foreach (var cube in Cubes) {
            if (IsTouchingLava(cube))
                _cubeSidesTouchingLava += GetSidesTouchingLava(cube);
        }
    }

    private bool IsTouchingLava(Vector3 cube) =>
        GetAdjacentCubes(cube).Any(adjacentCube => LavaCubes.Contains(adjacentCube));

    private int GetSidesTouchingLava(Vector3 cube) =>
        GetAdjacentCubes(cube).Count(adjacentCube => LavaCubes.Contains(adjacentCube));

    private void FillLavaCubesOutside() {
        var min = new Vector3(Cubes.Min(c => c.X), Cubes.Min(c => c.Y), Cubes.Min(c => c.Z)) - Vector3.One;
        var max = new Vector3(Cubes.Max(c => c.X), Cubes.Max(c => c.Y), Cubes.Max(c => c.Z)) + Vector3.One;
        var visited = new HashSet<Vector3>() {min};
        LavaCubes.Add(min);
        Dfs(min, min, max);
        _lavaCubesCompleted = true;
    }

    private void Dfs(Vector3 cube, Vector3 min, Vector3 max) {
        // starting cube 0,0,0
        // add all adjacent cubes to stack
        // if cube is not visited, add to visited
        // if cube is visited, do not add to stack
        var adjacentCubes = GetAdjacentCubes(cube, min, max);
        // add all adjacent cubes to lava cubes
        foreach (var adjacentCube in adjacentCubes) {
            LavaCubes.Add(adjacentCube);
        }

        foreach (var adjacentCube in adjacentCubes) {
            //Thread.Sleep(1);
            Dfs(adjacentCube, min, max);
        }
    }

    private HashSet<Vector3> GetAdjacentCubes(Vector3 cube) {
        return new HashSet<Vector3>() {
            cube + Vector3.UnitX,
            cube - Vector3.UnitX,
            cube + Vector3.UnitY,
            cube - Vector3.UnitY,
            cube + Vector3.UnitZ,
            cube - Vector3.UnitZ
        };
    }

    private HashSet<Vector3> GetAdjacentCubes(Vector3 cube, Vector3 min, Vector3 max) {
        // return all adjacent cubes that are not visited and are within min and max
        var adjacentCubes = new HashSet<Vector3>();

        if (cube.X + 1 <= max.X && !LavaCubes.Contains(cube + Vector3.UnitX) && !Cubes.Contains(cube + Vector3.UnitX))
            adjacentCubes.Add(cube + Vector3.UnitX);

        if (cube.X - 1 >= min.X && !LavaCubes.Contains(cube - Vector3.UnitX) && !Cubes.Contains(cube - Vector3.UnitX))
            adjacentCubes.Add(cube - Vector3.UnitX);

        if (cube.Y + 1 <= max.Y && !LavaCubes.Contains(cube + Vector3.UnitY) && !Cubes.Contains(cube + Vector3.UnitY))
            adjacentCubes.Add(cube + Vector3.UnitY);

        if (cube.Y - 1 >= min.Y && !LavaCubes.Contains(cube - Vector3.UnitY) && !Cubes.Contains(cube - Vector3.UnitY))
            adjacentCubes.Add(cube - Vector3.UnitY);

        if (cube.Z + 1 <= max.Z && !LavaCubes.Contains(cube + Vector3.UnitZ) && !Cubes.Contains(cube + Vector3.UnitZ))
            adjacentCubes.Add(cube + Vector3.UnitZ);

        if (cube.Z - 1 >= min.Z && !LavaCubes.Contains(cube - Vector3.UnitZ) && !Cubes.Contains(cube - Vector3.UnitZ))
            adjacentCubes.Add(cube - Vector3.UnitZ);

        return adjacentCubes;
    }
}