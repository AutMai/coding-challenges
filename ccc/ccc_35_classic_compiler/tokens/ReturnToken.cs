using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class ReturnToken : BaseToken {
        
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public ReturnToken(string value, BaseToken parentToken) : base(value, parentToken) {
    }
}