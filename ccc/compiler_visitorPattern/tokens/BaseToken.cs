using System.Collections.Generic;
using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public abstract class BaseToken : IVisitable {
    public string Value { get; }
    protected BaseToken(BaseToken parentToken) {
        ParentToken = parentToken;
    }
    protected BaseToken(string value, BaseToken parentToken) {
        Value = value;
        ParentToken = parentToken;
    }


    public BaseToken ParentToken { get; set; }

    public List<BaseToken> Children { get; protected init; } = new();
    public abstract EStatus Accept(IVisitor visitor);
}