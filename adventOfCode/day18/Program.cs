using aocTools;
using day18;

var numbers = Helper.ReadFile("input_small.txt").Split('\n');
var currentPair = new Pair(null, null, null);
Pair? rootPair = null;
CreatePairs();
rootPair.ParentPair = null;
rootPair!.Reduce(new List<Pair>());
Console.WriteLine(rootPair);


void CreatePairs() {

    foreach (var num in numbers) {
        foreach (var c in num) {
            switch (c) {
                case '[':
                    currentPair.SetItem((currentPair = new Pair(null, null, currentPair)));
                    rootPair ??= currentPair;
                    break;
                case ']':
                    currentPair = currentPair.ParentPair;
                    break;
                case var c2 when char.IsNumber(c2):
                    currentPair.SetItem(c.ToInt());
                    break;
            }
        }

        Console.WriteLine(rootPair);
    }
}