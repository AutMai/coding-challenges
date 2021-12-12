namespace day10; 

public class Bracket {
    public char OpenBracket { get; set; }
    public char CloseBracket { get; set; }

    public List<Bracket> ChildrenBrackets = new List<Bracket>();

    public Bracket ParentBracket { get; set; }
    
    public Bracket(char openBracket, Bracket parentBracket) {
        OpenBracket = openBracket;
        ParentBracket = parentBracket;
    }
}