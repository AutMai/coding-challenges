using System.Collections.Generic;
using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class StartToken : BaseToken {
    public List<Variable> Variables = new();

    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public StartToken(BaseToken parentToken) : base(parentToken) {
    }
}