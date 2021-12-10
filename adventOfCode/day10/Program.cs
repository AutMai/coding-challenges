var chunksAr = File.ReadAllLines(@"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\adventOfCode\day10\input.txt");

var chunkBraces = new List<string> { "{}", "()", "[]", "<>" };

var x = RemoveValidChunks();

Console.WriteLine("twst");

List<string> RemoveValidChunks() {
    var output = new List<string>();
    for (int i = 0; i < chunksAr.Length; i++) {
        var input2 = chunksAr[i];
        while (chunkBraces.Any(x => input2.Contains(x))) {
            chunkBraces.ForEach(y => { input2 = input2.Replace(y, ""); });
        }

        output.Add(input2);
    }

    return output;
}