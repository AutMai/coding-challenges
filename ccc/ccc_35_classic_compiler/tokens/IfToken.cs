using System.Collections.Generic;
using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class IfToken : BaseToken {
    public ElseToken ElseToken { get; }


    public override string Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public IfToken(object value, BaseToken parentToken, List<BaseToken> children = null,
        ElseToken elseToken = null) : base(value, parentToken) {
        Children = children;
        ElseToken = elseToken;
    }
}