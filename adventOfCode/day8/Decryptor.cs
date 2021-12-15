namespace day8;

public static class Decryptor {
    public static object PartTwo(string input) => Solve(input);


    public static int Solve(string input) {
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int result = 0;

        foreach (var line in lines) {
            var signalPatterns = DecodeSignalPatterns(line);

            var output = line.Split(" | ")[1].Split(' ');
            output = SortStringInAr(output);

            string outputValue = "";

            foreach (var p in output) {
                outputValue += signalPatterns.SingleOrDefault(x => x.Value == p).Key.ToString();
            }

            result += Convert.ToInt32(outputValue);
        }
        return result;
    }

    private static string[] SortStringInAr(string[] stringAr) {
        for (int i = 0; i < stringAr.Length; i++) {
            stringAr[i] = String.Concat(stringAr[i].OrderBy(c => c));
        }

        return stringAr;
    }

    private static Dictionary<int, string> DecodeSignalPatterns(string line) {
        Dictionary<int, string?> signalPatterns = new Dictionary<int, string?>();

        var encodedPatterns = line.Split(" | ")[0].Split(' ');

        encodedPatterns = SortStringInAr(encodedPatterns);

        signalPatterns[1] = encodedPatterns.SingleOrDefault(p => p.Length == 2);
        signalPatterns[4] = encodedPatterns.SingleOrDefault(p => p.Length == 4);
        signalPatterns[7] = encodedPatterns.SingleOrDefault(p => p.Length == 3);
        signalPatterns[8] = encodedPatterns.SingleOrDefault(p => p.Length == 7);


        signalPatterns[9] = encodedPatterns.SingleOrDefault(p =>
            p.Length == 6 && p.Except(signalPatterns[7]).Except(signalPatterns[4]).Count() == 1);
        signalPatterns[6] = encodedPatterns.SingleOrDefault(p =>
            p.Length == 6 && p != signalPatterns[9] && signalPatterns[1].Except(p).Count() == 1);
        signalPatterns[0] =
            encodedPatterns.SingleOrDefault(p => p.Length == 6 && p != signalPatterns[9] && p != signalPatterns[6]);


        signalPatterns[3] =
            encodedPatterns.SingleOrDefault(p => p.Length == 5 && p.Except(signalPatterns[7]).Count() == 2);

        signalPatterns[5] = encodedPatterns.SingleOrDefault(p => p.Length == 5 &&
                                                                 p.Except(signalPatterns[7]).Count() == 3 &&
                                                                 signalPatterns[4].Except(p).Count() == 1);
        signalPatterns[2] = encodedPatterns.SingleOrDefault(p => p.Length == 5 &&
                                                                 p.Except(signalPatterns[7]).Count() == 3 &&
                                                                 signalPatterns[4].Except(p).Count() == 2);

        return signalPatterns;
    }
}