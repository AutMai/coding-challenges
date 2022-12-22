using System.Collections;
using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using aocTools;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.CameraMode;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Color;

namespace aoc22.day20;

public class Day20 : AAocDay {
    public CircularList<(int Index, long Value)> ListClone = new();

    public override void PuzzleOne() {
        ReadInput();
        CircularList<(int Index, long Value)> List = new(ListClone);
        foreach (var i in ListClone) {
            var index = List.IndexOf((i.Index, i.Value));
            var newIndex = (int) ((index + i.Value) % (List.Count - 1));
            if (newIndex < 0) {
                newIndex += List.Count - 1;
            }

            List.RemoveAt(index);
            List.Insert(newIndex, i);
        }

        var zero = List.FindIndex(i => i.Value == 0);

        // get 1000th number after element with value 0
        var num1000 = List.WrappingIndex(zero + 1000).Value;
        var num2000 = List.WrappingIndex(zero + 2000).Value;
        var num3000 = List.WrappingIndex(zero + 3000).Value;
        //sum 
        Console.WriteLine(num1000 + num2000 + num3000);
    }

    public override void PuzzleTwo() {
        // multiply each element in list by 1000000
        ListClone = new(ListClone.Select(i => (i.Index, i.Value * 811589153)).ToList());
        CircularList<(int Index, long Value)> List = new(ListClone);

        for (int x = 0; x < 10; x++) {
            foreach (var i in ListClone) {
                var index = List.IndexOf((i.Index, i.Value));
                var newIndex = (int) ((index + i.Value) % (List.Count - 1));
                if (newIndex < 0) {
                    newIndex += List.Count - 1;
                }

                List.RemoveAt(index);
                List.Insert(newIndex, i);
            }
        }

        var zero = List.FindIndex(i => i.Value == 0);

        // get 1000th number after element with value 0
        var num1000 = List.WrappingIndex(zero + 1000).Value;
        var num2000 = List.WrappingIndex(zero + 2000).Value;
        var num3000 = List.WrappingIndex(zero + 3000).Value;
        //sum 
        Console.WriteLine(num1000 + num2000 + num3000);
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            ListClone.Add((ListClone.Count, InputTokens.ReadInt()));
        }
    }
}

public class CircularList<T> : List<T> {
    public CircularList() {
    }

    public CircularList(List<T> t) : base(t) {
    }

    public void Move(T item, int steps) {
        var itemIndex = this.IndexOf(item);
        var newIndex = (itemIndex + steps) % (Count - 1);
        if (newIndex < 0) {
            newIndex += (Count - 1);
        }

        this.RemoveAt(itemIndex);
        this.Insert(newIndex, item);
    }

    public override string ToString() {
        var sb = new StringBuilder();
        foreach (var t in this) {
            sb.Append(t + ", ");
        }

        return sb.ToString();
    }

    public T WrappingIndex(int index) {
        index = index % (Count);
        /*if (index < 0) {
            index = this.Count + index;
        }*/

        return this[index];
    }
}