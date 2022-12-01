using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class RootToken : BaseToken {
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }


    public RootToken(BaseToken parentToken) : base(parentToken) {
    }
}