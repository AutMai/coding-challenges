/*using System.Text;
using CodingHelper;

var r = new InputReader();
//r.ReadFolder("files/level4");
//r.ReadWholeFile("files/level4/level4_example.in");
r.ReadZipFile("files/level5.zip");

foreach (var l in r.GetInputs()) {
    l.SetOutput();

    var messageCount = l.ReadInt();

    Message message = new Message();


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

    var decryptionTables = new List<DecryptionTable>();

    for (var i = 0; i < descriptionTableCount; i++) {
        var decryptionTableId = l.ReadInt();
        decryptionTables.Add(new DecryptionTable(decryptionTableId));
        for (var j = 0; j < alphabetCount; j++) {
            decryptionTables.Single(dt => dt.Id == decryptionTableId).Add(l.ReadChar(), l.ReadChar());
        }
    }

    foreach (var decryptionTable in decryptionTables) {
        var decryptedMessage = message.Decrypt(decryptionTable);
        if (decryptedMessage.IsValid(words)) {
            Console.WriteLine(decryptionTable.Id);
            break;
        }
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
}

public class DecryptionTable : Dictionary<char, char> {
    public int Id { get; set; }

    public DecryptionTable(int id) {
        Id = id;
    }
}*/