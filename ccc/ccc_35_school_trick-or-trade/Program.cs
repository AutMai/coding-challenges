using ccc_35_school_trick_or_trade;
using CodingHelper;

#region Level1
/*
var input = File.ReadAllText($"../../../files/level1/level1_example.in").Replace("\r", "").Split('\n').ToList();

var economistCount = input.TakeAndRemove(1);

List<Economist> economists = new List<Economist>();

foreach (var l in input) {
    var line = l.Split(' ').ToList().ToInt();
    Economist economist;
    economists.Add(economist = new Economist(){Id = line.TakeAndRemove(1).First(), Sweets = new List<Sweet>()});
    for (int i = 0; i < line.Count; i++) {
        economist.Sweets.Add(new Sweet(){Economist = economist, Value = line[i]});
    }
}

foreach (var economist in economists) {
    Console.WriteLine(economist.Sweets.Sum(s=> s.Value));
}
*/
#endregion

#region Level2
/*
var input = File.ReadAllText($"../../../files/level2/level2_example.in").Replace("\r", "").Split('\n').ToList();

var economistCount = input.TakeAndRemove(1);

var economists = new List<Economist>();
var sweets = new List<Sweet>();

foreach (var line in input.Select(l => l.Split(' ').ToList().ToInt())) {
    Economist economist;
    economists.Add(economist = new Economist(){Id = line.TakeAndRemove(1).First(), Sweets = new List<Sweet>()});
    for (int i = 0; i < line.Count; i++) {
        var sweet = new Sweet() {Economist = economist, Value = line[i]};
        sweets.Add(sweet);
        economist.Sweets.Add(sweet);
    }
}

foreach (var eco in economists) {
    var ownMinSweet = eco.Sweets.Min(s=>s.Value);
    var otherMaxSweet = sweets.Except(eco.Sweets).Max(s=>s.Value);
    Console.WriteLine(otherMaxSweet > ownMinSweet
        ? $"{eco.Id} {ownMinSweet} {sweets.First(s => s.Value == otherMaxSweet).Economist.Id} {otherMaxSweet}"
        : "NO TRADE");
}
*/
#endregion

#region Level3
/*var input = File.ReadAllText($"../../../files/level2/level2_example.in").Replace("\r", "").Split('\n').ToList();

var economistCount = input.TakeAndRemove(1);

var economists = new List<Economist>();
var sweets = new List<Sweet>();

foreach (var line in input.Select(l => l.Split(' ').ToList().ToInt())) {
    Economist economist;
    economists.Add(economist = new Economist(){Id = line.TakeAndRemove(1).First(), Sweets = new List<Sweet>()});
    for (int i = 0; i < line.Count; i++) {
        var sweet = new Sweet() {Economist = economist, Value = line[i]};
        sweets.Add(sweet);
        economist.Sweets.Add(sweet);
    }
}

foreach (var eco in economists) {
    var ownMinSweet = eco.Sweets.Min(s=>s.Value);
    var otherMaxSweet = sweets.Except(eco.Sweets).Max(s=>s.Value);
    Console.WriteLine(otherMaxSweet > ownMinSweet
        ? $"{eco.Id} {ownMinSweet} {sweets.First(s => s.Value == otherMaxSweet).Economist.Id} {otherMaxSweet}"
        : "NO TRADE");
}*/
#endregion


Console.WriteLine();
