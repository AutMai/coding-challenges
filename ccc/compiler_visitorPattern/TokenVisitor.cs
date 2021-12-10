using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compiler_visitorPattern.interfaces;
using compiler_visitorPattern.tokens;

namespace compiler_visitorPattern;

public class TokenVisitor : IVisitor {
    private readonly StringBuilder _result = new();
    private readonly StringBuilder _builder = new();

    public string Output => _result.ToString();

    public EStatus Visit(RootToken token) {
        return VisitChildren(token.Children);
    }

    public EStatus Visit(StartToken token) {
        if (VisitChildren(token.Children) == EStatus.Error)
            _result.Append("ERROR\n");
        else {
            _builder.Append("\n");
            _result.Append(_builder.ToString());
        }

        _builder.Clear();

        return EStatus.Continue;
    }

    private EStatus VisitChildren(IReadOnlyList<BaseToken> children, int startIndex = 0) {
        for (var i = startIndex; i < children.Count; i++) {
            var status = children[i].Accept(this);
            switch (status) {
                case EStatus.ExitFunction:
                    return EStatus.ExitFunction;
                case EStatus.Error:
                    return EStatus.Error;
            }
        }

        return EStatus.Continue;
    }

    public EStatus Visit(IfToken token) => ConvertToDatatype(token) is not bool expressionBool
        ? EStatus.Error
        : VisitChildren(expressionBool ? token.Children : token.ElseToken.Children);


    public EStatus Visit(ElseToken token) => VisitChildren(token.Children);

    public EStatus Visit(ReturnToken token) => EStatus.ExitFunction;

    public EStatus Visit(PrintToken token) {
        var value = ConvertToDatatype(token);
        value = value is bool ? value.ToString()?.ToLower() : value.ToString();
        _builder.Append(value);
        return EStatus.Continue;
    }

    public EStatus Visit(VarToken token) {
        var startToken = (StartToken)GetFunctionToken(token);
        if (startToken.Variables.Exists(v => v.Name == token.Name)) {
            return EStatus.Error;
        }

        var value = ConvertToDatatype(token);
        startToken.Variables.Add(new Variable { Name = token.Name, Value = value });
        return EStatus.Continue;
    }

    private static BaseToken GetFunctionToken(BaseToken token) {
        if (token is StartToken) {
            return token;
        }

        token = token.ParentToken;
        token = GetFunctionToken(token);
        return token;
    }

    public EStatus Visit(SetToken token) {
        var startToken = (StartToken)GetFunctionToken(token);
        var variable = startToken.Variables.SingleOrDefault(v => v.Name == token.Name);
        if (variable == null) return EStatus.Error;
        var value = ConvertToDatatype(token);
        variable.Value = value;
        return EStatus.Continue;
    }

    public EStatus Visit(PostponeToken token) {
        AppendTokens(token.ParentToken, token.Children);
        return EStatus.Continue;
    }

    private void AppendTokens(BaseToken parentToken, List<BaseToken> childTokens) {
        childTokens.ForEach(t=>t.ParentToken = parentToken);
        parentToken.Children.AddRange(childTokens);
    }

    
    private object ConvertToDatatype(BaseToken token) {
        var input = token.Value;
        var startToken = (StartToken)GetFunctionToken(token);
        switch (input) {
            case "true":
                return true;
            case "false":
                return false;
        }

        if (startToken.Variables.Exists(v => v.Name == input))
            return startToken.Variables.SingleOrDefault(v => v.Name == input)?.Value;
        if (int.TryParse(input, out _))
            return Convert.ToInt32(input);
        return input;
    }
}