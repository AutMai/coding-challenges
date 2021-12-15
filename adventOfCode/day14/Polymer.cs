namespace day14;

public static class Polymer {
    public static Dictionary<string, long> Polymers = new();

    public static Dictionary<string, Tuple<string, string, char>> Operations = new();
    public static Dictionary<char, long> Monomers = new();


    public static void CreateOperations(string[] operations) {
        foreach (var operation in operations) {
            var o = operation.Split(" -> ");
            var c1 = o[0][0] + o[1];
            var c2 = o[1] + o[0][1];
            Operations.Add(o[0], new Tuple<string, string, char>(c1, c2, o[1].ToCharArray()[0]));
        }
    }

    public static void ReadPolymers(string input) {
        for (int i = 0; i < input.Length - 1; i++) {
            if (Polymers.ContainsKey(input[i..(i + 2)]))
                Polymers[input[i..(i + 2)]]++;
            else
                Polymers[input[i..(i + 2)]] = 1;
        }

        foreach (var m in input) {
            if (Monomers.ContainsKey(m))
                Monomers[m]++;
            else
                Monomers[m] = 1;
        }
    }

    public static void ProcessPolymers(int iterations) {
        for (int i = 0; i < iterations; i++) {
            foreach (var polymer in new Dictionary<string, long>(Polymers)) {
                for (int j = 0; j < polymer.Value; j++) {
                    var temp = Operations[polymer.Key];

                    var a = polymer.Key;
                    var b = temp.Item1;
                    var c = temp.Item2;

                    var m = temp.Item3;

                    Polymers[polymer.Key]--;

                    if (Polymers.ContainsKey(b))
                        Polymers[b]++;
                    else
                        Polymers[b] = 1;

                    if (Polymers.ContainsKey(c))
                        Polymers[c]++;
                    else
                        Polymers[c] = 1;

                    if (Monomers.ContainsKey(m))
                        Monomers[m]++;
                    else
                        Monomers[m] = 1;
                }
            }
        }
    }


    public static void GetResult() {
        Console.WriteLine(Monomers.Values.Max() - Monomers.Values.Min());
    }
}