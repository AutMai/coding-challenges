using System.Linq;
using aocTools;

string input = Helper.ReadFile("input.txt");

var x = input.Split('\n');


var numRows = x.Select(e =>
    e.Split("|", StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();


Dictionary<string, int> numMapping = new Dictionary<string, int>() {
    { "acedgfb", 8 },
    { "cdfbe", 5 },
    { "gcdfa", 2 },
    { "fbcad", 3 },
    { "dab", 7 },
    { "cefabd", 9 },
    { "cdfgeb", 6 },
    { "eafb", 4 },
    { "cagedb", 0 },
    { "ab", 1 }
};

int output = 0;
foreach (var numRow in numRows) {
    string s = "";
    foreach (var num in numRow) {
        s += numMapping[num];
    }

    output += Convert.ToInt32(s);
}

Console.WriteLine(output);

/*

var uniqueNumLengths = new[] { 2, 3, 4, 7 };
// 1=2, 4=4, 7=3, 8=7
var sumP1 = newList.Count(s => uniqueNumLengths.Contains(s.Length));
Console.WriteLine(sumP1);*/