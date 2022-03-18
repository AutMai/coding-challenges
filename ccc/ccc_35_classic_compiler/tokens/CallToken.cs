using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class CallToken : BaseToken {
        
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public CallToken(string value, BaseToken parentToken) : base(value, parentToken) {
    }
}