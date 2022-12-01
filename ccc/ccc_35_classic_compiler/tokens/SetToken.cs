using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class SetToken : BaseToken {
    public string Name { get; }
    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public SetToken(string name, string value, BaseToken parentToken) : base(value, parentToken) {
        Name = name;
    }
}