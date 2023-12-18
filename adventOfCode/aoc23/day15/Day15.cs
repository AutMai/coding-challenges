using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;
using aocTools.Neo4J;

namespace aoc23.day15;

public class Day15 : AAocDay {

    List<string> Values = new List<string>();
    
    public Day15() {
        ReadInput();
    }

    private void ReadInput() {
        Values = InputTokens.Read().Split(",").ToList();
    }

    public override void PuzzleOne() {
        var sum = Values.Sum(GetHash);
        Console.WriteLine(sum);
    }

    private int GetHash(string s) {
        int currentVal = 0;
        for (var i = 0; i < s.Length; i++) {
            currentVal += s[i];
            currentVal *= 17;
            currentVal %= 256;
        }
        return currentVal;
    }

    public HashSet<Box> Boxes { get; set; }
    
    public override void PuzzleTwo() {
        // create boxes 0..255
        Boxes = new HashSet<Box>();
        for (var i = 0; i < 256; i++) {
            var box = new Box {Nr = i, Lenses = new ListDictionary()};
            Boxes.Add(box);
        }

        foreach (var v in Values) {
            var label = v.Split(new[] { '=', '-' })[0];
            var box = Boxes.First(b => b.Nr == GetHash(label));

            // check if string contains = or -
            var add = v.Contains('=');
            if (add) {
                var value = int.Parse(v.Split('=')[1]);
                // box already contains label?
                if (box.Lenses.Contains(label)) {
                    // update value
                    box.Lenses[label] = value;
                }
                else {
                    // add label
                    box.Lenses.Add(label, value);
                }
            }
            else {
                // remove label from box if it exists
                box.Lenses.Remove(label);
            }
        }

        var res = GetResult();
        Console.WriteLine(res);
    }

    private int GetResult() {
        var sum = 0;
        foreach (var b in Boxes) {
            var lensNr = 1;
            foreach (var v in b.Lenses.Values) {
                sum += (b.Nr + 1) * (lensNr++) * (int) v;
            }
        }
        return sum;
    }
}

public class Box {
    public int Nr { get; set; }
    public ListDictionary Lenses { get; set; } = new ListDictionary();

    public override string ToString() {
        var sb = new StringBuilder();
        sb.Append($"Box {Nr}: ");
        foreach (DictionaryEntry entry in Lenses) {
            sb.Append($"[{entry.Key} {entry.Value}] ");
        }
        return sb.ToString();
    }
}

