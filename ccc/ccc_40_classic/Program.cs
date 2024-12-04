using System.Numerics;
using ccc_40_classic;
using CodingHelper;

var r = new InputReader(5, true, " ", false);

var deskCollection1 = new HashSet<Desk>();
deskCollection1.Add(new Desk(1,2, new Vector2(0, 0), true));

var deskCollection2 = new HashSet<Desk>();
deskCollection2.Add(new Desk(1,2, new Vector2(0, 0), true));

var state1 = new State(deskCollection1, 0, 0);
var state2 = new State(deskCollection2, 0, 0);

Console.WriteLine(state1.Equals(state2));





foreach (var l in r.GetInputs()) {
    l.SetOutput();

    var nLawn = l.ReadInt();

    for (var i = 0; i < nLawn; i++) {
        var x = l.ReadInt();
        var y = l.ReadInt();
        var deskAmount = l.ReadInt();
        
        var room = new Room(x, y, deskAmount);
        
        Console.WriteLine(room.GetRoomMap());
    }
}