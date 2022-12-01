using System.Collections.Generic;
using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class IfToken : BaseToken {
    public ElseToken ElseToken { get; }


    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public IfToken(object value, BaseToken parentToken, List<BaseToken> children = null,
        ElseToken elseToken = null) : base(value, parentToken) {
        Children = children;
        ElseToken = elseToken;
    }
}