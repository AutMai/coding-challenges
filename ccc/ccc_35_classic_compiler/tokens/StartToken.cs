using System.Collections.Generic;
using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class StartToken : BaseToken {
    public List<Variable> Variables = new();

    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public StartToken(BaseToken parentToken) : base(parentToken) {
    }
}