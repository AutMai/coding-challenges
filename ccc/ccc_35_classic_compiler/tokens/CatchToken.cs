using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class CatchToken : BaseToken {

    public CatchToken(BaseToken parentToken) : base(parentToken) {
    }

    public override void Accept(IVisitor visitor) {
        throw new System.NotImplementedException();
    }
}