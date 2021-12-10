using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class ElseToken : BaseToken {
    public override EStatus Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public ElseToken(BaseToken parentToken) : base(parentToken) {
    }
}