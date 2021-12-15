/*
string input = Helper.ReadFile("input.txt").Split("\n\n")[0];

var operationsString = Helper.ReadFile("input.txt").Split("\n\n")[1].Split('\n');

var operations = new Dictionary<string, string>();

foreach (var operation in operationsString) {
    var o = operation.Split(" -> ");
    var result = o[0][0] + o[1] + o[0][1];
    operations.Add(o[0], result);
}

var output = new StringBuilder();

for (int i = 0; i < 40; i++) {
    output.Clear();
    for (int j = 0; j < input.Length - 1; j++) {
        var partT = input[j..(j + 2)];
        output.Append(operations[partT]);
        if (j != input.Length - 2) 
            output.Remove(output.Length - 1, 1);
    };

    input = output.ToString();
}

//Console.WriteLine(input);

var results = new Dictionary<char, long>();

foreach (var i in input) {
    if (results.ContainsKey(i))
        results[i]++;
    else
        results.Add(i,1);
}

Console.WriteLine(results.Values.Max()-results.Values.Min());

*/