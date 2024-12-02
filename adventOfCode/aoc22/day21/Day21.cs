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

namespace aoc22.day21;

public class Day21 : AAocDay {
    public override void PuzzleOne() {
        Console.WriteLine();
    }

    public override void PuzzleTwo() {
        Console.WriteLine();
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
        }
    }
}

public class CircularList<T> : List<T> {
    public void Move(T item, int steps) {
        var itemIndex = this.IndexOf(item);
        var newIndex = itemIndex + steps;
        if (newIndex < 0) {
            newIndex = this.Count + newIndex;
        }
        else if (newIndex >= this.Count) {
            newIndex = newIndex - this.Count;
        }
        this.Remove(item);
    }
}