namespace day14;

public static class Polymer {
    public static Dictionary<string, long> Polymers = new();

    public static Dictionary<string, string> Operations = new();

    public static void CreateOperations(string[] operations) {
        foreach (var operation in operations) {
            var o = operation.Split(" -> ");
            var result = o[0][0] + o[1] + o[0][1];
            Operations.Add(o[0], result);
        }
    }

    public static void ReadPolymers(string input) {
        for (int i = 0; i < input.Length - 1; i++) {
            if (Polymers.ContainsKey(input[i..(i + 2)])) {
                Polymers[input[i..(i + 2)]]++;
            }
            else {
                Polymers[input[i..(i + 2)]] = 1;
            }
        }
    }

    public static void ProcessPolymers(int iterations) {
        for (int i = 0; i < iterations; i++) {
            var newPolymers = new Dictionary<string, long>();
            foreach (var polymer in Polymers) {
                for (int j = 0; j < polymer.Value; j++) {
                    var newPolymer = Operations[polymer.Key];

                    if (newPolymers.ContainsKey(newPolymer[..2]))
                        newPolymers[newPolymer[..2]]++;
                    else
                        newPolymers[newPolymer[..2]] = 1;

                    if (newPolymers.ContainsKey(newPolymer[1..3]))
                        newPolymers[newPolymer[1..3]]++;
                    else
                        newPolymers[newPolymer[1..3]] = 1;
                }
            }

            Polymers = newPolymers;
        }
    }

    public static Dictionary<char, long> Monomers = new Dictionary<char, long>();

    public static void ConvertToMonomers() {
        foreach (var polymer in Polymers) {
            for (int i = 0; i < polymer.Value; i++) {
                for (int j = 0; j < 2; j++) {
                    if (Monomers.ContainsKey(polymer.Key[j]))
                        Monomers[polymer.Key[j]]++;
                    else
                        Monomers[polymer.Key[j]] = 1;
                }
            }
        }
    }

    public static void GetResult() {
        ConvertToMonomers();
        Console.WriteLine(Monomers.Values.Max() / 2 - Monomers.Values.Min() / 2);
    }
}