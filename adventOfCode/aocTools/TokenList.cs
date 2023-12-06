namespace aocTools;

public class TokenList : List<string> {
    public TokenList(IEnumerable<string> tokens) : base(tokens) {
    }

    public string JustRead() {
        return this[0];
    }
    
    public bool HasMoreTokens() {
        return Count > 0;
    }
    
    public string Read() {
        var token = this[0];
        this.RemoveAt(0);
        return token;
    }

    public int ReadInt() {
        return int.Parse(Read());
    }
    
    public long ReadLong() {
        return long.Parse(Read());
    }
    
    public void Remove(int count) {
        this.RemoveRange(0, count);
    }
}