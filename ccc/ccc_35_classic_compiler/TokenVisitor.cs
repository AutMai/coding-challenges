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

    public string Output2 => _functionOutputBuilder.ToString();

    public string Visit(RootToken token) {
        return VisitChildren(token.Children);
    }

    public string Visit(StartToken token, bool called = false) {
        try {
            var returnValue = VisitChildren(token.Children);
            if (called == false) {
                _result.Append(_functionOutputBuilder);
                _result.Append('\n');
                _functionOutputBuilder.Clear();
            }

            return returnValue;
        }
        catch (ReturnException re) {
            _result.Append(_functionOutputBuilder);
            if (called == false) _result.Append('\n');
            _functionOutputBuilder.Clear();
            return re.Value;
        }
        catch (Exception e) {
            _result.Append("ERROR");
            _result.Append('\n');
            _functionOutputBuilder.Clear();
            return e.Message;
        }
    }

    private string VisitChildren(IReadOnlyList<BaseToken> children, int startIndex = 0) {
        for (var i = startIndex; i < children.Count; i++) {
            var returnValue = children[i].Accept(this);
            if (returnValue != "" && children[i] is ReturnToken) return returnValue;
        }

        return "true";
    }

    public string Visit(IfToken token) => EvaluateValue(token) is not bool expressionBool
        ? throw new Exception("ValueError")
        : VisitChildren(expressionBool ? token.Children : token.ElseToken.Children);


    public string Visit(ElseToken token) => VisitChildren(token.Children);

    public string Visit(ReturnToken token) {
        var value = EvaluateValue(token);
        if (value is bool) value = value.ToString()?.ToLower();
        throw new ReturnException(value.ToString());
    }

    public string Visit(PrintToken token) {
        var value = EvaluateValue(token);

        value = value is bool ? value.ToString()?.ToLower() : value.ToString();
        _functionOutputBuilder.Append(value);
        return "";
    }

    public string Visit(VarToken token) {
        var startToken = (StartToken)token.GetFunctionToken();
        if (startToken.Variables.Exists(v => v.Name == token.Name)) {
            throw new Exception("UnknownVariable");
        }

        var value = EvaluateValue(token);
        startToken.Variables.Add(new Variable { Name = token.Name, Value = value });
        return "";
    }

    public string Visit(SetToken token) {
        var startToken = (StartToken)token.GetFunctionToken();
        var variable = startToken.Variables.SingleOrDefault(v => v.Name == token.Name);
        if (variable == null) throw new Exception("UnknownVariable");
        var value = EvaluateValue(token);
        variable.Value = value;
        return "";
    }

    public string Visit(PostponeToken token) {
        AppendTokens(token.ParentToken, token.Children);
        return "";
    }

    public string Visit(CallToken token) {
        var value = EvaluateValue(token);
        if (!int.TryParse(value.ToString(), out _)) {
            throw new Exception("ValueError");
        }

        var functionToken = token.GetRootToken().Children[Convert.ToInt32(value) - 1];
        return Visit((StartToken)functionToken, true);
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