using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class VarToken : BaseToken {
    public string Name { get; }

    public override string Accept(IVisitor visitor) {
        return visitor.Visit(this);
    }

    public VarToken(string name, string value, BaseToken parentToken) : base(value, parentToken) {
        Name = name;
    }
}