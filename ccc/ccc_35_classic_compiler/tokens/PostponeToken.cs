using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class PostponeToken: BaseToken{
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public PostponeToken(BaseToken parentToken) : base(parentToken) {
    }
}