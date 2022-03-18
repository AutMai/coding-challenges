using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class RootToken : BaseToken {
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }


    public RootToken(BaseToken parentToken) : base(parentToken) {
    }
}