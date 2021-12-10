using System.Threading.Channels;
using day6;

string input =
    File.ReadAllText(@"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\adventOfCode\day6\input.txt");
// 3,4,3,1,2

//List<Fish> fishList = new List<Fish>();

Dictionary<int, ulong> fishDict = new Dictionary<int, ulong>()
    { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 } };

foreach (var entry in input.Split(',')) {
    fishDict[Convert.ToInt32(entry)] = (fishDict.ContainsKey(Convert.ToInt32(entry)))
        ? fishDict[Convert.ToInt32(entry)] += 1
        : fishDict[Convert.ToInt32(entry)] = 1;
}

for (int i = 0; i < 256; i++) {

    var finishCount = fishDict[0];

    fishDict[7] += fishDict[0]; // add zeros to 7
    fishDict[0] = 0; // remove zeros

    for (int age = 0; age < 8; age++) {
        fishDict[age] = fishDict[age + 1];
    }

    fishDict[8] = finishCount;
}


ulong sum = 0;
foreach (var entry in fishDict) {
    sum += (entry.Value);
}

Console.WriteLine(sum);


/*
foreach (var entry in input.Split(',')) {
    fishList.Add(Convert.ToInt32(entry));
}

for (int i = 0; i < 80; i++) {
    //PrintList(i);
    var finishCount = fishList.Count(f => f == 0);

    for (int j = 0; j < fishList.Count; j++) {
        if (fishList[j] == 0)
            fishList[j] = 7;
    }

    fishList = fishList.Select(f => f - 1).ToList();

    for (int j = 0; j < finishCount; j++) {
        fishList.Add(8);
    }
}

Console.WriteLine(fishList.Count);

void PrintList(int day) {
    Console.Write($"After {day} days:");
    foreach (var item in fishList) {
        Console.Write(item + ",");
    }

    Console.WriteLine();
}*/