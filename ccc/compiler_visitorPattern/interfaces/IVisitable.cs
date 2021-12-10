namespace compiler_visitorPattern.interfaces; 

public interface IVisitable {
    EStatus Accept(IVisitor visitor);
}