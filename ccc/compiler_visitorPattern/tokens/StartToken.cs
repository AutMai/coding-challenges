using System.Collections.Generic;
using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class StartToken : BaseToken {
    public readonly List<Variable> Variables = new();

    public override string Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public StartToken(BaseToken parentToken) : base(parentToken) {
    }
}