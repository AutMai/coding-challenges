using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class PostponeToken: BaseToken{
    public override string Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public PostponeToken(BaseToken parentToken) : base(parentToken) {
    }
}