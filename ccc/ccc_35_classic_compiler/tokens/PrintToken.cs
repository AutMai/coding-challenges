using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class PrintToken : BaseToken {
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public PrintToken(string value, BaseToken parentToken) : base(value, parentToken) {
    }
}