// See https://aka.ms/new-console-template for more information

using CodingHelper;

var r = new InputReader(1, true, " ", true);

foreach (var l in r.GetInputs()) {
    //l.SetOutput();
    var pieceCount = l.ReadInt();

    var pieces = new List<PuzzlePiece>();

    for (int i = 0; i < pieceCount; i++) {
        var sides = new EPieceSideType[4];
        var line = l.Read();
        line = line.Replace(",", "");
        for (int j = 0; j < 4; j++) {
            sides[j] = line[j] switch {
                'H' => EPieceSideType.Hole,
                'K' => EPieceSideType.Knob,
                _ => EPieceSideType.Hole
            };
        }
        pieces.Add(new PuzzlePiece() { Sides = sides });
    }
    
}

class PuzzlePiece {
    public EPieceSideType[] Sides { get; set; }
    
    // value compare for sides
    public bool Equals(PuzzlePiece other) {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Sides.SequenceEqual(other.Sides);
    }
    
    public override string ToString() {
        return string.Join("", Sides.Select(s => s switch {
            EPieceSideType.Hole => "H",
            EPieceSideType.Knob => "K",
            _ => "H"
        }));
    }
}


enum EPieceSideType {
    Hole,
    Knob
}