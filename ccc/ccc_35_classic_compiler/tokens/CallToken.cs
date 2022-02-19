using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class CallToken : BaseToken {
        
    public override string Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public CallToken(string value, BaseToken parentToken) : base(value, parentToken) {
    }
}