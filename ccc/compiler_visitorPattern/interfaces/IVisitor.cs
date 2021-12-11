

using compiler_visitorPattern.tokens;

namespace compiler_visitorPattern.interfaces; 

public interface IVisitor {
    string Visit(RootToken token);
    string Visit(StartToken token);
    string Visit(IfToken token);
    string Visit(ElseToken token);
    string Visit(ReturnToken token);
    string Visit(PrintToken token);
    string Visit(VarToken token);
    string Visit(SetToken token);
    string Visit(PostponeToken token);
    string Visit(CallToken token);
}