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

namespace aoc22.day23;

public class Day23 : AAocDay {
    public static HashSet<Elf> elves = new HashSet<Elf>();

    public override void PuzzleOne() {
        ReadInput();
        //DrawElves();
        for (var i = 0; i < 10; i++) {
            foreach (var e in elves) {
                e.ProposeMove();
            }

            Elf.RemoveProposedPositionIfTwoElvesHaveTheSamePosition();
            foreach (var e in elves) {
                e.Move();
            }

            Elf.ChangeDirection();
            //DrawElves();

            // Thread.Sleep(1000);
        }

        Console.WriteLine();
        var min = new Vector2(elves.Min(e => e.Position.Value.X), elves.Min(e => e.Position.Value.Y));
        var max = new Vector2(elves.Max(e => e.Position.Value.X), elves.Max(e => e.Position.Value.Y));
        var width = (int) (max.X - min.X + 1);
        var height = (int) (max.Y - min.Y + 1);

        var surface = width * height;
        Console.WriteLine($"Surface: {surface}");
        Console.WriteLine(surface - elves.Count);
    }

    private void DrawElves() {
        // draw elves in console but watch out becuase elves positions are relative and could be negative
        var min = new Vector2(elves.Min(e => e.Position.Value.X), elves.Min(e => e.Position.Value.Y));
        var max = new Vector2(elves.Max(e => e.Position.Value.X), elves.Max(e => e.Position.Value.Y));
        var width = (int) (max.X - min.X + 1);
        var height = (int) (max.Y - min.Y + 1);
        var map = new char[width, height];
        foreach (var e in elves) {
            var x = (int) (e.Position.Value.X - min.X);
            var y = (int) (e.Position.Value.Y - min.Y);
            map[x, y] = e.Id.ToString()[0];
        }

        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                Console.Write(map[x, y] == '\0' ? ". " : map[x, y] + " ");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public override void PuzzleTwo() {
        ReadInput();
        //DrawElves();
        var round = 1;
        while (true) {
            // propose moves parallel foreach
            Parallel.ForEach(elves, e => e.ProposeMove());

            Elf.RemoveProposedPositionIfTwoElvesHaveTheSamePosition();

            var elvesThatMoveCount = elves.Count(e => e.ProposedPosition != e.Position);
            var elvesThatMove = elves.Where(e => e.ProposedPosition != e.Position).ToList();
            Console.WriteLine($"Elves that move: {elvesThatMoveCount}");
            if (elvesThatMoveCount == 0) {
                break;
            }

            foreach (var e in elves) {
                e.Move();
            }

            Elf.ChangeDirection();

            // DrawElves();
            round++;

            //Thread.Sleep(1000);
        }

        Console.WriteLine(round);
    }

    private void ReadInput() {
        int id = 0;
        var labels = "0123456789abcdefghijklmnopqrstuvwxyz";
        for (int line = 0; line < InputTokens.Count; line++) {
            for (int i = 0; i < InputTokens[0].Length; i++) {
                if (InputTokens[line][i] == '#') {
                    elves.Add(new Elf(i, line) {Id = labels[id++]});
                    if (id == labels.Length) {
                        id = 0;
                    }
                }
            }
        }
    }
}

public class Elf {
    public char Id { get; set; }

    public static Vector2[][] Directions = new[] {
        // north, northeast and northwest
        new[] {new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, -1)},
        // south, southeast and southwest
        new[] {new Vector2(0, 1), new Vector2(1, 1), new Vector2(-1, 1)},
        // west, northwest and southwest
        new[] {new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(-1, 1)},
        // east, northeast and southeast
        new[] {new Vector2(1, 0), new Vector2(1, -1), new Vector2(1, 1)}
    };

    public static int GlobalDirection = 0;
    public int CurrentzDirection = 0;

    public Vector2? Position { get; set; }
    public static HashSet<Vector2> Positions = new();
    public Vector2 ProposedPosition { get; set; }

    public Elf(int x, int y) {
        Position = new Vector2(x, y);

        Positions.Add(Position.Value);
    }

    public void ProposeMove() {
        CurrentzDirection = GlobalDirection;
        // if all neighbor fields are empty do nothing
        if (IsAllNeighborFieldsEmpty()) {
            return;
        }

        ProposeMoveR();
    }

    private bool IsAllNeighborFieldsEmpty() {
        var neighbors = new HashSet<Vector2>() {
            Position.Value + new Vector2(-1, -1),
            Position.Value + new Vector2(-1, 0),
            Position.Value + new Vector2(-1, 1),
            Position.Value + new Vector2(0, -1),
            Position.Value + new Vector2(0, 1),
            Position.Value + new Vector2(1, -1),
            Position.Value + new Vector2(1, 0),
            Position.Value + new Vector2(1, 1),
        };

        // check if all neighbors are empty but do not use linq any and do not use foreach because it is slow
        if (neighbors.All(n => !Positions.Contains(n))) {
            return true;
        }

        return false;
    }


    private void ProposeMoveR() {
        var direction = Directions[CurrentzDirection];
        var proposedPositions = new[] {
            Position.Value + direction[0],
            Position.Value + direction[1],
            Position.Value + direction[2]
        };

        if (proposedPositions.Any(proposedPosition => Elf.Positions.Contains(proposedPosition))) {
            CurrentzDirection = (CurrentzDirection + 1) % 4;
            if (CurrentzDirection == GlobalDirection) {
                ProposedPosition = Position.Value;
                return;
            }

            ProposeMoveR();
        }
        else {
            ProposedPosition = proposedPositions[0];
            return;
        }
    }

    public void Move() {
        if (ProposedPosition != Position) {
            Positions.Remove(Position.Value);
            Positions.Add(ProposedPosition);
            Position = ProposedPosition;
        }
    }

    public static void ChangeDirection() {
        GlobalDirection = (GlobalDirection + 1) % 4;
    }

    public static void RemoveProposedPositionIfTwoElvesHaveTheSamePosition() {
        // get all elves with the same proposed position
        var x = Day23.elves.Select(e => e.ProposedPosition).GroupBy(x => x).Where(g => g.Count() > 1)
            .Select(g => g.Key).ToList();

        var elvesWithSameProposedPosition = Day23.elves.Where(e => x.Contains(e.ProposedPosition)).ToList();

        elvesWithSameProposedPosition.ForEach(e => e.ProposedPosition = e.Position.Value);
    }
}