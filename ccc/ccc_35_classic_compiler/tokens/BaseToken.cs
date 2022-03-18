using System.Collections.Generic;
using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens;

public abstract class BaseToken : IVisitable {
    public object Value { get; set; }

    protected BaseToken(BaseToken parentToken) {
        ParentToken = parentToken;
    }

    protected BaseToken(object value, BaseToken parentToken) {
        Value = value;
        ParentToken = parentToken;
    }

    public BaseToken GetRootToken() {
        var token = this;
        if (token is RootToken rootToken) return rootToken;
        

        token = token.ParentToken.GetRootToken();
        return token;
    }

    public BaseToken GetFunctionToken() {
        var token = this;
        if (token is StartToken startToken) return startToken;

        token = token.ParentToken.GetFunctionToken();
        return token;
    }
    
    public void AddChildren(List<BaseToken> childTokens) {
        childTokens.ForEach(t => t.ParentToken = this);
        this.Children.AddRange(childTokens);
    }

    public BaseToken ParentToken { get; set; }

    public List<BaseToken> Children { get; protected init; } = new();
    public abstract void Accept(IVisitor visitor);
    
}