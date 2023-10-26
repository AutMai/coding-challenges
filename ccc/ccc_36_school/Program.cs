using System.Text;
using System.Text.RegularExpressions;
using CodingHelper;

var r = new InputReader();
//r.ReadFolder("files/level4");
r.ReadWholeFile("files/level5/level5_1.in", " ", true);
//r.ReadZipFile("files/level5.zip", " ", true);

foreach (var l in r.GetInputs()) {
    //l.SetOutputToFile();

    var messageCount = l.ReadInt();

    List<Message> messages = new();


    for (int i = 0; i < messageCount; i++) {
        var message = new Message();
        message.AddRange(l.Read().Split(" ").Select(w => new Word() {Text = w}));
        messages.Add(message);
    }

    var wordCount = l.ReadInt();

    Message dictionary = new();
    for (int i = 0; i < wordCount; i++) {
        dictionary.Add(new Word() {Text = l.Read()});
    }

    var alphabet = dictionary.Select(s => s.Text[0]).Distinct();

    // dictionary get letter frequency
    var dictLetterFrequency = dictionary.GetLettersOrderedByFrequency();

    var decryptionTables = new List<DecryptionTable>();
    foreach (var message in messages) {
        DecryptionTable decryptionTable = new DecryptionTable(0);
        decryptionTables.Add(decryptionTable);
        var messageLetterFrequency = message.GetLettersOrderedByFrequency();
        for (int i = 0; i < 5; i++) {
            var messageLetter = messageLetterFrequency[i];
            var dictLetter = dictLetterFrequency[i];
            decryptionTable.Add(messageLetter, dictLetter);
        }

        var dictRegexPattern = $"[{string.Join("", decryptionTable.Values)}]";
        dictionary.ForEach(word => word.Text = Regex.Replace(word.Text, dictRegexPattern, ""));
        dictionary.RemoveAll(word => word.Text.Length == 0);
        
        var messageRegexPattern = $"[{string.Join("", decryptionTable.Keys)}]";
        message.ForEach(word => word.Text = Regex.Replace(word.Text, messageRegexPattern, ""));
        message.RemoveAll(word => word.Text.Length == 0);



        while (message.Count > 0) {
            var messagePatterns = message.Select(w => w.ToPattern()).OrderByDescending(w=>w.Length);
            var dictPatterns = dictionary.Select(w => w.ToPattern()).OrderByDescending(w=>w.Length);
            // find the longest pattern that matches
            var match = dictPatterns.First(dictPattern => messagePatterns.Any(messagePattern => dictPattern == messagePattern));
            var m = message.First(m=>m.ToPattern() == match);
            var d = dictionary.First(d=>d.ToPattern() == match);
            //Console.WriteLine($"Matched {m.Text} with {d.Text}");

            for (int i = 0; i < m.Text.Length; i++) {
                if (!decryptionTable.ContainsKey(m.Text[i]))
                    decryptionTable.Add(m.Text[i], d.Text[i]);
            }
            
            dictRegexPattern = $"[{string.Join("", decryptionTable.Values)}]";
            dictionary.ForEach(word => word.Text = Regex.Replace(word.Text, dictRegexPattern, ""));
            dictionary.RemoveAll(word => word.Text.Length == 0);
        
            messageRegexPattern = $"[{string.Join("", decryptionTable.Keys)}]";
            message.ForEach(word => word.Text = Regex.Replace(word.Text, messageRegexPattern, ""));
            message.RemoveAll(word => word.Text.Length == 0);
        }
        
        // output all words from dictionary
        //dictionary.OrderByDescending(w=>w.Text.Length).ThenBy(w=>w.Text).DistinctBy(w=>w.Text).ToList().ForEach(w=> Console.Write(w.Text + " "));
        /*var dictPatterns = dictionary
            .OrderByDescending(w => w.Text.Length)
            .ThenBy(w => w.Text)
            .DistinctBy(w => w.Text)
            .Select(w=>w.Text)
            .ToList();*/
            //.DistinctBy(w=>w.ToPattern())
            //.ToDictionary(w => w.ToPattern(), w=>w.Text);
        //message.OrderByDescending(w=>w.Text.Length).ThenBy(w=>w.Text).DistinctBy(w=>w.Text).ToList().ForEach(w=> Console.Write(w.Text + " "));
        /*var messagePatterns = message
            .OrderByDescending(w => w.Text.Length)
            .ThenBy(w => w.Text)
            .DistinctBy(w => w.Text)
            .Select(w=>w.Text)
            .ToList();*/
            //.DistinctBy(w=>w.ToPattern())
            //.ToDictionary(w => w.ToPattern(), w=>w.Text);
        
        /*foreach (var messagePattern in messagePatterns) {
            var sameWord = dictPatterns[messagePattern.Key];
            for (int i = 0; i < messagePattern.Value.Length; i++) {
                if (!decryptionTable.ContainsKey(messagePattern.Value[i]))
                    decryptionTable.Add(messagePattern.Value[i], sameWord[i]);
            }
        }*/

    }
}

public class Message : List<Word> {
    public Message Decrypt(DecryptionTable decryptionTable) {
        Message m = new Message();
        foreach (var word in this) {
            m.Add(word.Decrypt(decryptionTable));
        }

        return m;
    }

    public bool IsValid(List<Word> words) {
        return this.All(w => words.Any(w2 => w2.Text == w.Text));
    }

    public List<char> GetLettersOrderedByFrequency() {
        return this.SelectMany(s => s.Text).GroupBy(s => s).OrderByDescending(s => s.Count()).Select(s => s.Key)
            .ToList();
    }
}

public class Word {
    public string Text { get; set; }

    public Word Decrypt(DecryptionTable decryptionTable) {
        var decrypted = new StringBuilder();
        foreach (var c in Text) {
            decrypted.Append(decryptionTable[c]);
        }

        return new Word() {Text = decrypted.ToString()};
    }
    
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
}

public class DecryptionTable : Dictionary<char, char> {
    public int Id { get; set; }

    public DecryptionTable(int id) {
        Id = id;
    }
}