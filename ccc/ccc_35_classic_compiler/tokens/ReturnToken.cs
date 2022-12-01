using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class ReturnToken : BaseToken {
        
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public ReturnToken(string value, BaseToken parentToken) : base(value, parentToken) {
    }
}