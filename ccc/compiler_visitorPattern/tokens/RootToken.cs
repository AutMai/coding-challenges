using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class RootToken : BaseToken {
    public override EStatus Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }


    public RootToken(BaseToken parentToken) : base(parentToken) {
    }
}