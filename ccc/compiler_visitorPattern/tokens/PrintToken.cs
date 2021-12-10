using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class PrintToken : BaseToken {
    public override EStatus Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public PrintToken(string value, BaseToken parentToken) : base(value, parentToken) {
    }
}