namespace ccc_35_classic_compiler.interfaces; 

public interface IVisitable {
    void Accept(IVisitor visitor);
}