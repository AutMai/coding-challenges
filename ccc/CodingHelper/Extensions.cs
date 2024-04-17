using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace CodingHelper;

public class InputReader {
    private List<Input> _inputs { get; set; } = new();
    private int _currentInputIndex = 0;

    private bool example = false;
    private string splitValue = " ";
    private bool lines = false;
    private string filename;
    private string filenameExlExt;

    public InputReader(string filename, bool example = false, string splitValue = " ", bool lines = false) {
        this.filename = filename;
        this.filenameExlExt = filename.Replace(".zip", "");
        this.example = example;
        this.splitValue = splitValue;
        this.lines = lines;
        ReadZipFile();
    }


    public InputReader(int level, bool example = false, string splitValue = " ", bool lines = false)
        : this($"files/level{level}.zip", example, splitValue, lines) {
    }
    //
    // public InputReader(string filename, bool example = false, string splitValue = " ", bool lines = false) : this(
    //     example, splitValue, lines) {
    //     ReadZipFile(filename);
    // }

    //list zip file und erstellt orderner mit allen dateien
    public void ReadZipFile() {
        UnzipFile();
        ReadFolder();
    }

    public void UnzipFile() {
        var zipFile = filename;
        ZipFile.ExtractToDirectory(GetCompletePath(zipFile), GetCompletePath(zipFile.Replace(".zip", "")), true);
    }

    //liest folder mit allen dateien ein
    private void ReadFolder() {
        var folderName = filenameExlExt;

        foreach (var file in Directory.EnumerateFiles(GetCompletePath(folderName), "*example.in")) {
            ReadWholeFile(file);
        }

        if (!example) {
            foreach (var file in Directory.EnumerateFiles(GetCompletePath(folderName), "*.in")) {
                ReadWholeFile(file);
            }
        }
    }

    public static string GetCompletePath(string fileName) {
        if (fileName.Contains("../")) return fileName;
        return "../../../" + fileName;
    }

    //list einen file ein und splittet nach splitValue
    public void ReadWholeFile(string fileName) {
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
    public void SetOutputToFile() {
        FileStream filestream = new FileStream(
            GetOutputPath("outputs/" + FileName),
            FileMode.Create);
        var streamwriter = new StreamWriter(filestream);
        streamwriter.AutoFlush = true;
        Console.SetOut(streamwriter);
    }

    public void SetOutput() {
        FileStream filestream = new FileStream(GetOutputPath("outputs/" + FileName), FileMode.Create);
        StreamWriter filewriter = new StreamWriter(filestream);
        filewriter.AutoFlush = true;

        // Create a new StreamWriter instance that writes to the console's standard output stream.
        StreamWriter consolewriter = new StreamWriter(Console.OpenStandardOutput());
        consolewriter.AutoFlush = true;

        // Combine the two writers into a single StreamWriter that will write to both the file and the console.
        var combinedwriter = new CombinedWriter(filewriter, consolewriter);

        Console.SetOut(combinedwriter);
    }

    public class CombinedWriter : TextWriter {
        private readonly List<TextWriter> writers;

        public CombinedWriter(params TextWriter[] writers) {
            this.writers = new List<TextWriter>(writers);
        }

        public override void Write(char value) {
            foreach (var writer in writers) {
                writer.Write(value);
            }
        }

        public override void Write(string value) {
            foreach (var writer in writers) {
                writer.Write(value);
            }
        }

        public override Encoding Encoding => Encoding.UTF8;
    }


    public Input(string fileName, List<string> inputs) {
        FileName = fileName;
        Inputs = inputs;
    }


    private string GetOutputPath(string fileName) {
        var path = fileName.Replace(".in", "")
            .Split(new string[] { "/", "\\" }, StringSplitOptions.RemoveEmptyEntries);
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


    public decimal ReadDecimal() {
        return Convert.ToDecimal(Read().Replace(",", "."), new CultureInfo("en-US"));
    }

    public char ReadChar() {
        return Convert.ToChar(Read());
    }


    public long ReadLong() {
        return long.Parse(Read());
    }

    public double ReadDouble() {
        return Convert.ToDouble(Read().Replace(",", "."), new CultureInfo("en-US"));
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
    public static IEnumerable<IList<TSource>> Split<TSource>(this IEnumerable<TSource> source,
        Func<TSource, bool> predicate) {
        var list = new List<TSource>();

        foreach (var element in source) {
            if (predicate(element)) {
                if (list.Count > 0) {
                    yield return list;
                    list = new List<TSource>();
                }
            }
            else {
                list.Add(element);
            }
        }

        if (list.Count > 0) {
            yield return list;
        }
    }

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


    /*public static List<NodeKonst> ToNeighbors(this NodeKonst[][] array) {
        var nodes = new List<NodeKonst>();

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
    }*/

    public static List<NodeKonst> ToNeighborsFull(this NodeKonst[][] array) {
        var nodes = new List<NodeKonst>();

        for (int y = 0; y < array.Length; y++) {
            for (int x = 0; x < array[y].Length; x++) {
                var left = (x - 1 >= 0);
                var right = (x + 1 < array[y].Length);
                var top = (y - 1 >= 0);
                var bottom = (y + 1 < array.Length);

                if (left) array[y][x].Neighbors.Add(new Connection(array[y][x - 1], EDirection.Left));
                if (right) array[y][x].Neighbors.Add(new Connection(array[y][x + 1], EDirection.Right));
                if (top) array[y][x].Neighbors.Add(new Connection(array[y - 1][x], EDirection.Up));
                if (bottom) array[y][x].Neighbors.Add(new Connection(array[y + 1][x], EDirection.Down));
                if (left && top) array[y][x].Neighbors.Add(new Connection(array[y - 1][x - 1], EDirection.LeftUp));
                if (right && top)
                    array[y][x].Neighbors.Add(new Connection(array[y - 1][x + 1], EDirection.RightUp));
                if (left && bottom)
                    array[y][x].Neighbors.Add(new Connection(array[y + 1][x - 1], EDirection.LeftDown));
                if (right && bottom)
                    array[y][x].Neighbors.Add(new Connection(array[y + 1][x + 1], EDirection.RightDown));

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

public enum EDirection {
    Left,
    Right,
    Up,
    Down,
    LeftUp,
    LeftDown,
    RightUp,
    RightDown
}

public class Connection {
    public NodeKonst NodeKonst { get; set; }
    public EDirection EDirection { get; set; }

    public Connection(NodeKonst nodeKonst, EDirection eDirection) {
        NodeKonst = nodeKonst;
        EDirection = eDirection;
    }
}

public class NodeKonst {
    public NodeKonst Previous { get; set; }
    public NodeKonst Parent { get; set; }

    public List<NodeKonst> GetNeighbors() {
        return Neighbors.Select(k => k.NodeKonst).ToList();
    }


    public List<Connection> Neighbors { get; set; } = new();
    public bool Visited { get; set; } = false;
    public int VisitedCount { get; set; } = 0;

    public bool Ghost = false;
    public int PosX { get; set; }
    public int PosY { get; set; }

    public char Type;

    public NodeKonst() {
    }

    public Connection GetDirection(EDirection dir) {
        if (Neighbors.Any(k => k.EDirection == dir))
            return this.Neighbors.First(k => k.EDirection == dir);
        else {
            return null;
        }
    }

    public NodeKonst(char c) {
        Type = c;
    }
}