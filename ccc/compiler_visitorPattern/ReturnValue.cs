using System;

namespace compiler_visitorPattern; 

public class ReturnException : Exception{
    public string Value { get; }

    public ReturnException(string value) {
        Value = value;
    }
}