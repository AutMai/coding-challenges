using System.IO.Compression;

namespace CodingHelper;

public class InputReader {
    private List<Input> _inputs { get; set; } = new();
    private int _currentInputIndex = 0;

    //list zip file und erstellt orderner mit allen dateien
    public void ReadZipFile(string zipFile, string splitValue = " ", bool lines = false) {
        UnzipFile(zipFile);
        ReadFolder(zipFile.Replace(".zip", ""), splitValue, lines);
    }

    public void UnzipFile(string zipFile) {
        ZipFile.ExtractToDirectory(GetCompletePath(zipFile), GetCompletePath(zipFile.Replace(".zip", "")), true);
    }

    //liest folder mit allen dateien ein
    private void ReadFolder(string folderName, string splitValue, bool lines) {
        foreach (string file in Directory.EnumerateFiles(GetCompletePath(folderName), "*.in")) {
            ReadWholeFile(file, splitValue, lines);
        }
    }

    public static string GetCompletePath(string fileName) {
        if (fileName.Contains("../")) return fileName;
        return "../../../" + fileName;
    }

    //list einen file ein und splittet nach splitValue
    public void ReadWholeFile(string fileName, string splitValue = " ", bool lines= false) {
        List<string> input = new();
        if (lines) {
            input = File.ReadAllLines(GetCompletePath(fileName)).ToList();
        }
        else {
            input = File.ReadAllLines(GetCompletePath(fileName)).Select(k => k.Split(splitValue)).SelectMany(k => k)
                .ToList();
        }

        _inputs.Add(new Input(fileName, input));
    }

    //liest file mit standart split value
    public void ReadFilePerLine(string filename) {
        ReadFilePerLine(filename, " ");
    }

    //liest file mit splitvalue
    public void ReadFilePerLine(string filename, string splitValue) {
        var input = File.ReadLines("../../../" + filename);
        foreach (var s in input) {
            _inputs.Add(
                new Input(filename, s.Split(splitValue, StringSplitOptions.RemoveEmptyEntries).ToList()));
        }
    }

    //wenn man ein file mit mehreren Trennern hat zb ein leerzeichen und ein komma
    public void ReadFilePerLine(string filename, List<string> splitValues) {
        if (splitValues.Count == 0) throw new Exception("There has to be at least 1 Split Value");

        var input = File.ReadLines("../../../" + filename).ToList();
        for (int i = 0; i < input.Count; i++) {
            foreach (var splitValue in splitValues) {
                input[i] = input[i].Replace(splitValue, splitValues[0]);
            }

            _inputs.Add(new Input(filename,
                input[i].Split(splitValues[0], StringSplitOptions.RemoveEmptyEntries).ToList()));
        }
    }


    //bekommst alle inputs also alle files
    public List<Input> GetInputs() {
        return _inputs;
    }

    //naechstes file sofern es eins gibt
    public Input? GetNextInput() {
        if (_currentInputIndex >= _inputs.Count) {
            return null;
        }

        return _inputs[_currentInputIndex++];
    }

    //bekommst input mit index
    public Input? GetInputAt(int index) {
        if (index >= _inputs.Count) {
            return null;
        }

        return _inputs[index];
    }

    //speichert den consolen output in eine datei
    public void InitOutputRedirection() {
        FileStream filestream = new FileStream("../../../out.txt", FileMode.Create);
        var streamwriter = new StreamWriter(filestream);
        streamwriter.AutoFlush = true;
        Console.SetOut(streamwriter);
        Console.SetError(streamwriter);
    }
}

public class Input {
    public List<string> Inputs;
    public int Index;
    public string FileName { get; set; }

    //speichert alle console.writeline outputs in ein file
    public void SetOutput() {
        FileStream filestream = new FileStream(
            GetOutputPath("outputs/" + FileName),
            FileMode.Create);
        var streamwriter = new StreamWriter(filestream);
        streamwriter.AutoFlush = true;
        Console.SetOut(streamwriter);
    }

    public Input(string fileName, List<string> inputs) {
        FileName = fileName;
        Inputs = inputs;
    }


    private string GetOutputPath(string fileName) {
        var path = fileName.Replace(".in", "")
            .Split(new string[] {"/", "\\"}, StringSplitOptions.RemoveEmptyEntries);
        path[^2] += "Output";
        Directory.CreateDirectory(InputReader.GetCompletePath(path[^2]));
        return "../../../" + path[^2] + "/" + path[^1] + ".out";
    }

    //gibt das naechste fragment aus
    public string Read() {
        return Inputs[Index++];
    }

    public string JustRead() {
        return Inputs[Index];
    }


    public int ReadInt() {
        return int.Parse(Read());
    }

    public char ReadChar() {
        return Convert.ToChar(Read());
    }


    public long ReadLong() {
        return long.Parse(Read());
    }

    public double ReadDouble() {
        return double.Parse(Read());
    }

    public bool ReadBool() {
        return bool.Parse(Read());
    }

    //gibt das naechste fragment aus (mit anzahl an naechsten fragmenten)
    public List<string> Read(int amount) {
        var x = Inputs.GetRange(Index, amount);
        Index += amount;
        return x;
    }

    //setzt den index des fragment lesers
    public void SetIndex(int index) {
        Index = index;
    }

    //ob file noch nicht fertig gelesen ist
    public bool HasNotEnded() => Index < Inputs.Count;


    //ob file fertig gelesen ist
    public bool HasEnded() => Index >= Inputs.Count;
}

public static class Extension {
    public static T Pop<T>(this List<T> list) {
        var item = list[0];
        list.RemoveAt(0);
        return item;
    }

    public static T Pop<T>(this List<T> list, int index) {
        var item = list[index];
        list.RemoveAt(index);
        return item;
    }


    public static List<Node> ToNeighbors(this Node[][] array) {
        var nodes = new List<Node>();

        for (int y = 0; y < array.Length; y++) {
            for (int x = 0; x < array[y].Length; x++) {
                var left = (x - 1 >= 0);
                var right = (x + 1 < array[y].Length);
                var top = (y - 1 >= 0);
                var bottom = (y + 1 < array.Length);

                if (left) array[y][x].Neighbors.Add(array[y][x - 1]);
                if (right) array[y][x].Neighbors.Add(array[y][x + 1]);
                if (top) array[y][x].Neighbors.Add(array[y - 1][x]);
                if (bottom) array[y][x].Neighbors.Add(array[y + 1][x]);

                nodes.Add(array[y][x]);
            }
        }

        return nodes;
    }

    public static List<Node> ToNeighborsFull(this Node[][] array) {
        var nodes = new List<Node>();

        for (int y = 0; y < array.Length; y++) {
            for (int x = 0; x < array[y].Length; x++) {
                var left = (x - 1 >= 0);
                var right = (x + 1 < array[y].Length);
                var top = (y - 1 >= 0);
                var bottom = (y + 1 < array.Length);

                if (left) array[y][x].Neighbors.Add(array[y][x - 1]);
                if (right) array[y][x].Neighbors.Add(array[y][x + 1]);
                if (top) array[y][x].Neighbors.Add(array[y - 1][x]);
                if (bottom) array[y][x].Neighbors.Add(array[y + 1][x]);
                if (left && top) array[y][x].Neighbors.Add(array[y - 1][x - 1]);
                if (right && top) array[y][x].Neighbors.Add(array[y - 1][x + 1]);
                if (left && bottom) array[y][x].Neighbors.Add(array[y + 1][x - 1]);
                if (right && bottom) array[y][x].Neighbors.Add(array[y + 1][x + 1]);

                nodes.Add(array[y][x]);
            }
        }

        return nodes;
    }

    public static int ToInt32(this string s) {
        return Convert.ToInt32(s);
    }

    public static long ToInt64(this string s) {
        return Convert.ToInt64(s);
    }

    public static bool ToBool(this string s) {
        return Convert.ToBoolean(s);
    }

    public static List<T> AddReturn<T>(this List<T> list, T element) {
        list.Add(element);
        return list;
    }

    public static List<T> GetFromTo<T>(this List<T> list, int from, int two) => list.GetRange(from, from - two);

    public static string PrintWithComma<T>(this IEnumerable<T> list) =>
        list.Select(k => Convert.ToString(k)).Aggregate((a, b) => a + "," + b)!;
}

public class Node {
    public List<Node> Neighbors { get; set; } = new List<Node>();

    public bool Visited { get; set; } = false;
    public int VisitedCount { get; set; } = 0;

    public bool Ghost = false;
    public int PosX { get; set; }
    public int PosY { get; set; }

    public char Type;

    public Node() {
    }

    public Node(char c) {
        Type = c;
    }
}