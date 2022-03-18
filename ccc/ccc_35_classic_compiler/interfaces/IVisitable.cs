namespace compiler_visitorPattern.interfaces; 

public interface IVisitable {
    void Accept(IVisitor visitor);
}