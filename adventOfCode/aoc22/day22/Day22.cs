using System.Collections;
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

namespace aoc22.day22;

public class Day22 : AAocDay {
    public static Tile?[,] Map = new Tile[1000, 1000];
    public List<string> Instructions = new List<string>();
    public Player p;


    public override void PuzzleOne() {
        ReadInput();
        p = new Player();
        RaylibDraw();
        //PrintMap();
    }

    private void PlayerMove() {
        while (Instructions.Count > 0) {
            var i = Instructions.First();
            // if instruction is integer, then it is a move instruction
            if (int.TryParse(i, out var move)) {
                p.Move(move);
            }
            else {
                // otherwise it is a turn instruction
                p.Rotate(i[0]);
            }

            Instructions.RemoveAt(0);
        }

        Console.WriteLine((p.Y + 1) * 1000 + (p.X + 1) * 4 + p.IntDirection);
    }

    private void RaylibDraw() {
        new Thread(PlayerMove).Start();
        InitWindow(2300, 1300, "Day 22");
        const int scale = 6;
        while (!WindowShouldClose()) {
            BeginDrawing();
            ClearBackground(RAYWHITE);
            // draw map
            for (var y = 0; y < Map.GetLength(1); y++) {
                for (var x = 0; x < Map.GetLength(0); x++) {
                    if (Map[x, y] == null) {
                        DrawRectangle(x * scale, y * scale, scale, scale, BLACK);
                    }
                    else {
                        if (Map[x, y].IsWall) {
                            DrawRectangle(x * scale, y * scale, scale, scale, RED);
                        }
                        else {
                            DrawRectangle(x * scale, y * scale, scale, scale, WHITE);
                        }
                    }
                }
            }

            // draw player
            DrawRectangle(p.X * scale, p.Y * scale, scale, scale, BLUE);

            // draw player direction as arrow
            var dir = p.Direction;
            var dirX = p.X * scale + scale / 2;
            var dirY = p.Y * scale + scale / 2;
            DrawLine(dirX, dirY, dirX + p.XDirection[dir] * scale * 2, dirY + p.YDirection[dir] * scale * 2, BLUE);

            // write instructions
            DrawText("Instructions:", 2000, 50, 20, BLACK);
            for (var i = 0; i < Instructions.Count; i++) {
                DrawText(Instructions[i], 2000, 100 + i * 30, 20, BLACK);
            }


            EndDrawing();
        }
    }

    public override void PuzzleTwo() {
        Console.WriteLine();
    }

    private void ReadInput() {
        // split InputTokens at empty line
        var emptyLine = InputTokens.IndexOf("");
        var mapInput = InputTokens.GetRange(0, emptyLine);
        var instructions = InputTokens[emptyLine + 1];

        var mapHeight = mapInput.Count;
        var mapWidth = mapInput.Max(x => x.Length);
        Map = new Tile[mapWidth, mapHeight];

        for (var y = 0; y < mapInput.Count; y++) {
            for (var x = 0; x < mapInput[y].Length; x++) {
                if (mapInput[y][x] == ' ') {
                    continue;
                }

                Map[x, y] = new Tile() {
                    X = x,
                    Y = y,
                    IsWall = mapInput[y][x] == '#'
                };
            }
        }

        // split instructions using regex split
        var regex = new Regex(@"(\d+)");
        Instructions = regex.Split(instructions).ToList();
        Instructions.RemoveAll(x => x == "");
    }

    private void PrintMap() {
        // print map
        for (var y = 0; y < Map.GetLength(1); y++) {
            for (var x = 0; x < Map.GetLength(0); x++) {
                if (Map[x, y] == null) {
                    Console.Write(" ");
                }
                else if (Map[x, y]!.IsWall) {
                    Console.Write("#");
                }
                else {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }
    }
}

public class Tile {
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsWall { get; set; } = false;
}

public class Player {
    public int X { get; set; }
    public int Y { get; set; }
    public char Direction { get; set; } = 'R';
    public int IntDirection => DirectionToNum[Direction];

    public Player() {
        Y = 0;
        X = GetMinX().X;
    }

    public void Rotate(char clockwise) {
        if (clockwise == 'R') {
            var dirNum = IntDirection + 1;
            if (dirNum > 3) {
                dirNum = 0;
            }

            Direction = NumToDirection[dirNum];
        }
        else {
            var dirNum = IntDirection - 1;
            if (dirNum < 0) {
                dirNum = 3;
            }

            Direction = NumToDirection[dirNum];
        }

       // Thread.Sleep(100);
    }

    public void Move(int distance) {
        // move distance
        for (var i = 0; i < distance; i++) {
            // if next tile is out of bounds, reappear on other side
            if (X + XDirection[Direction] < GetMinX().X) {
                if (GetMaxX().IsWall) break;
                X = GetMaxX().X;
            }
            else if (X + XDirection[Direction] > GetMaxX().X) {
                if (GetMinX().IsWall) break;
                X = GetMinX().X;
            }
            else if (Y + YDirection[Direction] < GetMinY().Y) {
                if (GetMaxY().IsWall) break;
                Y = GetMaxY().Y;
            }
            else if (Y + YDirection[Direction] > GetMaxY().Y) {
                if (GetMinY().IsWall) break;
                Y = GetMinY().Y;
            }
            // if next tile is wall, stop
            else if (Day22.Map[X + XDirection[Direction], Y + YDirection[Direction]]!.IsWall) {
                break;
            }
            // else move
            else {
                X += XDirection[Direction];
                Y += YDirection[Direction];
            }

            //Thread.Sleep(100);
        }
    }

    public Dictionary<char, int> DirectionToNum = new() {
        {'R', 0},
        {'D', 1},
        {'L', 2},
        {'U', 3},
    };

    public Dictionary<int, char> NumToDirection = new() {
        {0, 'R'},
        {1, 'D'},
        {2, 'L'},
        {3, 'U'},
    };

    public Dictionary<char, int> XDirection = new Dictionary<char, int>() {
        {'R', 1},
        {'D', 0},
        {'L', -1},
        {'U', 0},
    };

    public Dictionary<char, int> YDirection = new Dictionary<char, int>() {
        {'R', 0},
        {'D', 1},
        {'L', 0},
        {'U', -1},
    };

    private Tile GetMinX() => GetRow(Y).Where(t => t is not null).MinBy(t => t.X);
    private Tile GetMaxX() => GetRow(Y).Where(t => t is not null).MaxBy(t => t.X);
    private Tile GetMinY() => GetColumn(X).Where(t => t is not null).MinBy(t => t.Y);
    private Tile GetMaxY() => GetColumn(X).Where(t => t is not null).MaxBy(t => t.Y);

    public List<Tile?> GetColumn(int columnNumber) =>
        Enumerable.Range(0, Day22.Map.GetLength(1))
            .Select(x => Day22.Map[columnNumber, x])
            .ToList();

    public List<Tile?> GetRow(int rowNumber) =>
        Enumerable.Range(0, Day22.Map.GetLength(0))
            .Select(x => Day22.Map[x, rowNumber])
            .ToList();
}