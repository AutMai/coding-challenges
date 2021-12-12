using day12;

var inputLines = File
    .ReadAllText(
        @"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\coding-challenges\adventOfCode\day12\input.txt")
    .Replace("\r", "")
    .Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

CaveSystem.CreateCaves(inputLines);
//CaveSystem.CreatePaths();

// aardvark1231
CaveSystem.CreatePaths2();

Console.WriteLine(CaveSystem.Paths.Count);



