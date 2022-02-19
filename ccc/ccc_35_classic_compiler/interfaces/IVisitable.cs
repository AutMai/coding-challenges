namespace compiler_visitorPattern.interfaces; 

public interface IVisitable {
    string Accept(IVisitor visitor);
}