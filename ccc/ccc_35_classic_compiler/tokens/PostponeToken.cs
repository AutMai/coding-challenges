using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class PostponeToken: BaseToken{
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public PostponeToken(BaseToken parentToken) : base(parentToken) {
    }
}