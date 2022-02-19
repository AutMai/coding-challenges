using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class ReturnToken : BaseToken {
        
    public override string Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public ReturnToken(string value, BaseToken parentToken) : base(value, parentToken) {
    }
}