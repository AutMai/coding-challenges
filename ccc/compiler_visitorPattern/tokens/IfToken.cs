using System.Collections.Generic;
using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class IfToken : BaseToken {
    public ElseToken ElseToken { get; }


    public override EStatus Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public IfToken(string value, BaseToken parentToken, List<BaseToken> children = null,
        ElseToken elseToken = null) : base(value, parentToken) {
        Children = children;
        ElseToken = elseToken;
    }
}