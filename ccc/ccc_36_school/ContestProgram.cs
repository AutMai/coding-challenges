/*using System.Text;
using CodingHelper;

var r = new InputReader();

r.ReadFolder("files/level4");
//r.ReadWholeFile("files/level4/level4_example.in");

foreach (var l in r.GetInputs()) {
    l.SetOutput();

    var messageCount = l.ReadInt();

    List<Word> message = new List<Word>();


    for (int i = 0; i < messageCount; i++) {
        message.Add(new Word() {Text = l.Read()});
    }

    var wordCount = l.ReadInt();

    List<Word> words = new List<Word>();
    for (int i = 0; i < wordCount; i++) {
        words.Add(new Word() {Text = l.Read()});
    }

    var alphabet = words.Select(s => s.Text[0]).Distinct();


    var descriptionTableCount = l.ReadInt();
    var alphabetCount = l.ReadInt();

    var decryptionTables = new List<Dictionary<char, char>>();

    for (int i = 0; i < descriptionTableCount; i++) {
        var descriptionTableId = l.ReadInt();
        decryptionTables.Add(new Dictionary<char, char>());
        for (int j = 0; j < alphabetCount; j++) {
            decryptionTables[i].Add(l.ReadChar(), l.ReadChar());
        }
    }

    var wordPatterns = new Dictionary<string, string>();
    foreach (var word in words) {
        var pattern = word.ToPattern();
        if (!wordPatterns.ContainsKey(pattern)) {
            wordPatterns.Add(pattern, word.Text);
        }
    }

    var messagePatterns = new Dictionary<string, string>();
    foreach (var word in message) {
        var pattern = word.ToPattern();
        if (!messagePatterns.ContainsKey(pattern)) {
            messagePatterns.Add(pattern, word.Text);
        }
    }

    var decryptionTable = new Dictionary<char, char>();
    foreach (var messagePattern in messagePatterns) {
        var sameWord = wordPatterns[messagePattern.Key];
        for (int i = 0; i < messagePattern.Value.Length; i++) {
            if (!decryptionTable.ContainsKey(messagePattern.Value[i]))
                decryptionTable.Add(messagePattern.Value[i], sameWord[i]);
        }
    }

    /*
    string messageString = string.Join("", words.ToArray());
    string decryptionString = string.Join("", message.ToArray());

    Dictionary<char, int> letterFrequency = new Dictionary<char, int>();
    foreach (char c in messageString) {
        if (letterFrequency.ContainsKey(c)) {
            letterFrequency[c]++;
        }
        else {
            letterFrequency.Add(c, 1);
        }
    }

    Dictionary<char, int> letterFrequency2 = new Dictionary<char, int>();
    foreach (char c in decryptionString) {
        if (letterFrequency2.ContainsKey(c)) {
            letterFrequency2[c]++;
        }
        else {
            letterFrequency2.Add(c, 1);
        }
    }

    var sortedLetFreq = letterFrequency.OrderBy(c => c.Value).ThenBy(c => c.Key);
    var sortedLetFreq2 = letterFrequency2.OrderBy(c => c.Value).ThenBy(c => c.Key);


    // zip letterFrequency and letterFrequency2 together
    var zipped = sortedLetFreq.Zip(sortedLetFreq2, (a, b) => new {a, b});


    foreach (var a in alphabet) {
        var o = zipped.First(x => x.b.Key == a);
        // decryptionTable.Add(o.b.Key, o.a.Key);
    }
    #1#
    // compare dictionary irrelevant of order

    var sortedDecTable = decryptionTable.OrderBy(x => x.Key);

    //compare dictionary irrelevant of order

    for (int i = 0; i < decryptionTables.Count; i++) {
        var sortedDecTable1 = decryptionTables[i].OrderBy(x => x.Key);

        bool correct = true;

        for (int j = 0; j < sortedDecTable.Count()-sortedDecTable.Count(); j++) {
            if (sortedDecTable.ElementAt(j).Value != sortedDecTable1.ElementAt(j).Value || sortedDecTable.ElementAt(j).Key != sortedDecTable1.ElementAt(j).Key) {
                correct = false;
            }
        }
        
        if (correct)
            Console.WriteLine(i);
        else {
            Console.WriteLine("wrong");
        }
    }
}

public class Word {
    public string Text { get; set; }

    public string ToPattern() {
        var pattern = new StringBuilder();
        var possibleChars = new StringBuilder("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#%&()[]{}*+-/.?:<>=_|");
        var decryptions = new Dictionary<char, char>();
        var num = 0;
        foreach (char c in Text) {
            if (!decryptions.ContainsKey(c)) {
                decryptions.Add(c, possibleChars[num]);
                num++;
            }

            pattern.Append(decryptions[c]);
        }

        return pattern.ToString();
    }

    public bool WordSame(Word w) {
        return this.ToPattern().Equals(w.ToPattern());
    }
}*/