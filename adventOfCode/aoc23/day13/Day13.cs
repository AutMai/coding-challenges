using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using aocTools.Interval;

namespace aoc23.day13;

public class Day13 : AAocDay {
    private List<Field> _fields = new();

    public Day13() {
        // split list into lists by empty lines
        while (InputTokens.HasMoreTokens()) {
            var rows = new List<string>();
            while (InputTokens.HasMoreTokens()) {
                var line = InputTokens.Read();
                if (line == "") {
                    break;
                }

                rows.Add(line);
            }

            _fields.Add(new Field(rows));
        }
    }


    public override void PuzzleOne() {
        var column = 0;
        var row = 0;
        foreach (var f in _fields) {
            var res = f.GetSymmetricAxis();
            if (res.IsHorizontal) {
                row += res.Index + 1;
            }
            else {
                column += res.Index + 1;
            }
        }

        Console.WriteLine(column + row * 100);
    }

    public override void PuzzleTwo() {
        var column = 0;
        var row = 0;
        foreach (var f in _fields) {
            var res = f.GetSymmetricAxis2();
            if (res.IsHorizontal) {
                row += res.Index + 1;
            }
            else {
                column += res.Index + 1;
            }
        }

        Console.WriteLine(column + row * 100);
    }
}

class Field {
    public Field(List<string> input) {
        Rows = input;
    }

    public List<string> Rows { get; set; }

    public List<string> Columns() {
        // map rows to columns
        var columns = new List<string>();
        for (var i = 0; i < Rows[0].Length; i++) {
            var sb = new StringBuilder();
            for (var j = 0; j < Rows.Count; j++) {
                sb.Append(Rows[j][i]);
            }

            columns.Add(sb.ToString());
        }

        return columns;
    }

    public SymmetricAxis GetSymmetricAxis() {
        var horizontalAxisIndex = GetSymmetricAxisIndex(Rows);
        if (horizontalAxisIndex != -1) return new SymmetricAxis(true, horizontalAxisIndex);

        var verticalAxisIndex = GetSymmetricAxisIndex(Columns());
        if (verticalAxisIndex != -1) return new SymmetricAxis(false, verticalAxisIndex);

        throw new Exception("No symmetric axis found");
    }

    public int GetSymmetricAxisIndex(List<string> list) {
        for (var i = 0; i < list.Count - 1; i++) {
            // check if row i is equal to next row
            if (list[i] == list[i + 1]) {
                var hasSymmetricAxis = true;
                var r2 = i + 1;
                for (var r1 = i; r1 >= 0 && r2 < list.Count; r1--) {
                    if (list[r1] != list[r2]) {
                        hasSymmetricAxis = false;
                        break;
                    }

                    r2++;
                }

                if (hasSymmetricAxis) return i;
            }
        }

        return -1;
    }
    
    public SymmetricAxis GetSymmetricAxis2() {
        var horizontalAxisIndex = GetSymmetricAxisIndex2(Rows);
        if (horizontalAxisIndex != -1) return new SymmetricAxis(true, horizontalAxisIndex);

        var verticalAxisIndex = GetSymmetricAxisIndex2(Columns());
        if (verticalAxisIndex != -1) return new SymmetricAxis(false, verticalAxisIndex);

        throw new Exception("No symmetric axis found");
    }

    
    public int GetSymmetricAxisIndex2(List<string> list) {
        var realSymmetricAxisIndex = GetSymmetricAxisIndex(list);

        for (var i = 0; i < list.Count - 1; i++) {
            // Skip the real symmetric axis
            if (i == realSymmetricAxisIndex) continue;

            // check if row i is equal to next row
            if (list[i] == list[i + 1] || StringsDifferByOneChar(list[i], list[i + 1])) {
                
                var hasSymmetricAxis = true;
                var jokerUsed = false;
                var r2 = i + 1;
                for (var r1 = i; r1 >= 0 && r2 < list.Count; r1--) {
                    if (StringsDifferByOneChar(list[r1], list[r2])) {
                        if (jokerUsed) {
                            hasSymmetricAxis = false;
                            break;
                        }

                        jokerUsed = true;
                    }
                    else if (list[r1] != list[r2]) {
                        hasSymmetricAxis = false;
                        break;
                    }

                    r2++;
                }

                if (hasSymmetricAxis) return i;
            }
        }
        
        // now check if the beginning two lines only differ by one char and further lines are equal
        

        return -1;
    }
    
    public bool StringsDifferByOneChar(string s1, string s2) => s1.Where((t, i) => t != s2[i]).Count() == 1;
}

record SymmetricAxis(bool IsHorizontal, int Index) {
    public override string ToString() {
        return $"{(IsHorizontal ? "Horizontal" : "Vertical")}, Index: {Index}";
    }
}