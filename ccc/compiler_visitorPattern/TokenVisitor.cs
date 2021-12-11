using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compiler_visitorPattern.interfaces;
using compiler_visitorPattern.tokens;

namespace compiler_visitorPattern;

public class TokenVisitor : IVisitor {
    private readonly StringBuilder _result = new();
    private readonly StringBuilder _functionOutputBuilder = new();

    public string Output => _result.ToString();

    public string Visit(RootToken token) {
        return VisitChildren(token.Children);
    }

    public string Visit(StartToken token) {
        var returnValue = VisitChildren(token.Children);
        if (returnValue == "Error")
            _result.Append("ERROR");
        else {
            _result.Append(_functionOutputBuilder);
            
            _functionOutputBuilder.Clear();
        }
        
        return returnValue;
    }

    private string VisitChildren(IReadOnlyList<BaseToken> children, int startIndex = 0, bool called = false) {
        for (var i = startIndex; i < children.Count; i++) {
            var returnValue = children[i].Accept(this);
            if (children[i].ParentToken is RootToken) {
                _result.Append('\n');
                _functionOutputBuilder.Clear();
            }

            switch (returnValue) {
                case "Error":
                    _result.Append("ERROR");
                    _functionOutputBuilder.Clear();
                    if (called) return EStatus.Error.ToString();
                    break;
                case "Continue":
                    break;
                default:
                    if (children[i] is ReturnToken) return returnValue;
                    if (called) return returnValue;
                    break;
            }
        }

        return "true";
    }

    public string Visit(IfToken token) => EvaluateValue(token) is not bool expressionBool
        ? EStatus.Error.ToString()
        : VisitChildren(expressionBool ? token.Children : token.ElseToken.Children);


    public string Visit(ElseToken token) => VisitChildren(token.Children);

    public string Visit(ReturnToken token) {
        var value = EvaluateValue(token);
        value = value is bool ? value.ToString()?.ToLower() : value.ToString();
        return value.ToString();
    }

    public string Visit(PrintToken token) {
        var value = EvaluateValue(token);

        value = value is bool ? value.ToString()?.ToLower() : value.ToString();
        _functionOutputBuilder.Append(value);
        return EStatus.Continue.ToString();
    }

    public string Visit(VarToken token) {
        var startToken = (StartToken)token.GetFunctionToken();
        if (startToken.Variables.Exists(v => v.Name == token.Name)) {
            return EStatus.Error.ToString();
        }

        var value = EvaluateValue(token);
        startToken.Variables.Add(new Variable { Name = token.Name, Value = value });
        return EStatus.Continue.ToString();
    }

    public string Visit(SetToken token) {
        var startToken = (StartToken)token.GetFunctionToken();
        var variable = startToken.Variables.SingleOrDefault(v => v.Name == token.Name);
        if (variable == null) return EStatus.Error.ToString();
        var value = EvaluateValue(token);
        variable.Value = value;
        return EStatus.Continue.ToString();
    }

    public string Visit(PostponeToken token) {
        AppendTokens(token.ParentToken, token.Children);
        return EStatus.Continue.ToString();
    }

    public string Visit(CallToken token) {
        var value = EvaluateValue(token);
        if (!int.TryParse(value.ToString(), out _)) {
            return EStatus.Error.ToString();
        }
        var functionToken = token.GetRootToken().Children[Convert.ToInt32(value) - 1];
        return VisitChildren(functionToken.Children, called: true);
    }

    private void AppendTokens(BaseToken parentToken, List<BaseToken> childTokens) {
        childTokens.ForEach(t => t.ParentToken = parentToken);
        parentToken.Children.AddRange(childTokens);
    }

    private object EvaluateValue(BaseToken token) {
        var value = token.Value;
        var startToken = (StartToken)token.GetFunctionToken();
        

        if (startToken.Variables.Exists(v => v.Name == value.ToString())) 
            return startToken.Variables.SingleOrDefault(v => v.Name == (string)value)?.Value;
        if (int.TryParse(value.ToString(), out _))
            return Convert.ToInt32(value);

        if (value is CallToken callToken) {
            value = callToken.Accept(this);
        }
        
        switch (value) {
            case "true":
                return true;
            case "false":
                return false;
        }
        
        return value;

    }



}