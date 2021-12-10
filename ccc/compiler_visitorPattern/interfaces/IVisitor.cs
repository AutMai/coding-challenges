

using compiler_visitorPattern.tokens;

namespace compiler_visitorPattern.interfaces; 

public interface IVisitor {
    EStatus Visit(RootToken token);
    EStatus Visit(StartToken token);
    EStatus Visit(IfToken token);
    EStatus Visit(ElseToken token);
    EStatus Visit(ReturnToken token);
    EStatus Visit(PrintToken token);
    EStatus Visit(VarToken token);
    EStatus Visit(SetToken token);
    EStatus Visit(PostponeToken token);
}