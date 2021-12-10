using System.Threading.Channels;

string input = System.IO.File.ReadAllText(@"C:\Users\Sebastian\OneDrive\coding\adventOfCode\day1\input.txt");
var inputAr = input.Split("\n");
inputAr = inputAr.SkipLast(1).ToArray();
int[] inputArInt = Array.ConvertAll(inputAr, s => int.Parse(s));

int increasedCount = 0;

List<int> sums = new List<int>();

for (int i = 0; i < inputArInt.Length-2; i++) {
    int sum = inputArInt[i] + inputArInt[i + 1] + inputArInt[i + 2];
    sums.Add(sum);
}

for (int i = 1; i < sums.Count; i++) {
    if (sums[i] > sums[i - 1]) increasedCount++;
}

Console.WriteLine(increasedCount);