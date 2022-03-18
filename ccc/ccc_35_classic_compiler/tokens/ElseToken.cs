using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class ElseToken : BaseToken {
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public ElseToken(BaseToken parentToken) : base(parentToken) {
    }
}