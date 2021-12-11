namespace compiler_visitorPattern; 

public interface IReturnValue {
}

public class ReturnValue:IReturnValue {
    public string Value { get; set; }
}