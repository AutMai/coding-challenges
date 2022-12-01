using System;

namespace ccc_35_classic_compiler; 

public class ReturnException : Exception{
    public string Value { get; }

    public ReturnException(string value) {
        Value = value;
    }
}