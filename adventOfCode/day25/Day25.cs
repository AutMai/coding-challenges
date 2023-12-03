using System.Numerics;
using aocTools;

namespace day25;

public class Day25 : AAocDay {
    public Day25() : base() {
        ReadInput();
    }

    private void PrintSeaCucumbers() {
        for (int y = 0; y < Height; y++) {
            var line = "";
            for (int x = 0; x < Width; x++) {
                // find key value pair with key x,y
                line += SeaCucumbers.ContainsKey(new Vector2(x, y))
                    ? SeaCucumbers[new Vector2(x, y)]
                    : '.';
            }

            Console.WriteLine(line);
        }
    }

    Dictionary<Vector2, char> SeaCucumbers = new();
    private int Width = 0;
    private int Height = 0;

    private void ReadInput() {
        Width = InputTokens.JustRead().Length;
        var line = 0;
        while (InputTokens.HasMoreTokens()) {
            var inp = InputTokens.Read();
            for (int i = 0; i < inp.Length; i++) {
                if (inp[i] == '.') continue;
                SeaCucumbers.Add(new Vector2(i, line), inp[i]);
            }

            line++;
        }

        Height = line;
    }


    public override void PuzzleOne() {
        var i = 0;
        while (true) {
            var copy = new Dictionary<Vector2, char>(SeaCucumbers);
            MoveSeaCucumbers();
            i++;
            //Console.Clear();
            //PrintSeaCucumbers();
            //Console.WriteLine(SeaCucumbers.GetHashCode());
            //Console.WriteLine(i);
            // check if dictionary is the same as before
            if (SeaCucumbers.SequenceEqual(copy)) {
                Console.WriteLine($"Found a loop at {i}");
                break;
            }
        }
    }

    private void MoveSeaCucumbers() {
        var newSeaCucumbers = new Dictionary<Vector2, char>();
        foreach (var sc in SeaCucumbers.Where(s=>s.Value == '>')) {
            var nextPos = GetNextPosition(sc.Key, sc.Value);
            if (SeaCucumbers.ContainsKey(nextPos)) {  // already a sea cucumber there => add current sea cucumber to new list
                newSeaCucumbers.Add(sc.Key, sc.Value);
            }
            else { // no sea cucumber there => move current sea cucumber to new position
                newSeaCucumbers.Add(nextPos, '>');
            }
        }
        
        // write newSeaCucumbers to SeaCucumbers and add south facing sea cucumbers
        var southSeaCucumbers = SeaCucumbers.Where(kv => kv.Value == 'v').ToList();
        SeaCucumbers = new Dictionary<Vector2, char>(newSeaCucumbers);
        southSeaCucumbers.ForEach(sc => SeaCucumbers.Add(sc.Key, sc.Value));
        
        
        // now move south facing sea cucumbers
        newSeaCucumbers = new Dictionary<Vector2, char>();
        foreach (var sc in SeaCucumbers.Where(s=>s.Value == 'v')) {
            var nextPos = GetNextPosition(sc.Key, sc.Value);
            if (SeaCucumbers.ContainsKey(nextPos)) {  // already a sea cucumber there => add current sea cucumber to new list
                newSeaCucumbers.Add(sc.Key, sc.Value);
            }
            else { // no sea cucumber there => move current sea cucumber to new position
                newSeaCucumbers.Add(nextPos, 'v');
            }
        }
        
        // write newSeaCucumbers to SeaCucumbers and add east facing sea cucumbers
        var eastSeaCucumbers = SeaCucumbers.Where(kv => kv.Value == '>').ToList();
        SeaCucumbers = new Dictionary<Vector2, char>(newSeaCucumbers);
        eastSeaCucumbers.ForEach(sc => SeaCucumbers.Add(sc.Key, sc.Value));
    }

    private Vector2 GetNextPosition(Vector2 pos, char direction) {
        return direction switch {
            '>' => (pos.X + 1 >= Width) ? new Vector2(0, pos.Y) : new Vector2(pos.X + 1, pos.Y),
            'v' => (pos.Y + 1 >= Height) ? new Vector2(pos.X, 0) : new Vector2(pos.X, pos.Y + 1),
            _ => pos
        };
    }

    public override void PuzzleTwo() {
    }
}