using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccc_35_classic_compiler.interfaces;
using ccc_35_classic_compiler.tokens;

namespace ccc_35_classic_compiler;

public class TokenVisitor : IVisitor {
    private readonly StringBuilder _result = new();
    private readonly StringBuilder _functionOutputBuilder = new();

    public string Output => _result.ToString();
    
    public void Visit(RootToken token) {
        VisitChildren(token.Children);
    }

    public string Visit(StartToken token, bool called = false) {
        try {
            VisitChildren(token.Children);
        }
        catch (ReturnException re) {
            _result.Append(_functionOutputBuilder);
            if (called == false) 
                _result.Append('\n');
            _functionOutputBuilder.Clear();
            return re.Value;
        }
        catch (Exception e) {
            _result.Append("ERROR");
            _result.Append('\n');
            _functionOutputBuilder.Clear();
        }

        if (called) return "";
        _result.Append(_functionOutputBuilder);
        _result.Append('\n');
        _functionOutputBuilder.Clear();

        return "";
    }

    private void VisitChildren(IReadOnlyList<BaseToken> children, int startIndex = 0) {
        for (var i = startIndex; i < children.Count; i++) {
            children[i].Accept(this);
        }
    }

    public void Visit(IfToken token) {
        if (EvaluateValue(token) is not bool expressionBool)
            throw new Exception("ValueError");
        
        VisitChildren(expressionBool ? token.Children : token.ElseToken.Children);
    }
    
    public void Visit(ReturnToken token) {
        var value = EvaluateValue(token);
        if (value is bool) value = value.ToString()?.ToLower();
        throw new ReturnException(value.ToString());
    }

    public void Visit(PrintToken token) {
        var value = EvaluateValue(token);

        value = value is bool ? value.ToString()?.ToLower() : value.ToString(); // write true and false in lowercase
        _functionOutputBuilder.Append(value);
    }

    public void Visit(VarToken token) {
        var startToken = (StartToken) token.GetFunctionToken();
        if (startToken.Variables.Exists(v => v.Name == token.Name)) {
            throw new Exception("VariableAlreadyExists");
        }

        startToken.Variables.Add(new Variable {Name = token.Name, Value = EvaluateValue(token)});
    }

    public void Visit(SetToken token) {
        var startToken = (StartToken) token.GetFunctionToken();
        var variable = startToken.Variables.SingleOrDefault(v => v.Name == token.Name);
        if (variable == null) throw new Exception("UnknownVariable");
        var value = EvaluateValue(token);
        variable.Value = value;
    }

    public void Visit(PostponeToken token) {
        token.ParentToken.AddChildren(token.Children);
    }

    public string Visit(CallToken token) {
        var value = EvaluateValue(token);
        if (!int.TryParse(value.ToString(), out _)) {
            throw new Exception("ValueError");
        }

        var functionTokenToCall = (StartToken) token.GetRootToken().Children[Convert.ToInt32(value) - 1];
        var variablesBefore = new List<Variable>(functionTokenToCall.Variables);
        functionTokenToCall.Variables.AddRange(((StartToken) token.GetFunctionToken()).Variables);
        var returnValue = Visit(functionTokenToCall, true);
        functionTokenToCall.Variables = variablesBefore;

        return returnValue;
    }

    public void Visit(TryToken token) {
        try {
            VisitChildren(token.Children);
        }
        catch (Exception e) {
            VisitChildren(token.CatchToken.Children);
        }
    }

    public string Visit(CatchToken token) {
        throw new NotImplementedException();
    }

    private object EvaluateValue(BaseToken token) {
        var value = token.Value;
        var startToken = (StartToken) token.GetFunctionToken();


        if (startToken.Variables.Exists(v => v.Name == value.ToString()))
            return startToken.Variables.SingleOrDefault(v => v.Name == (string) value)?.Value;
        if (int.TryParse(value.ToString(), out _))
            return Convert.ToInt32(value);

        if (value is CallToken callToken) {
            value = Visit(callToken);
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