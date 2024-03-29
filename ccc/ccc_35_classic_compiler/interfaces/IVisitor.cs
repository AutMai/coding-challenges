﻿using ccc_35_classic_compiler.tokens;

namespace ccc_35_classic_compiler.interfaces; 

public interface IVisitor {
    void Visit(RootToken token);
    string Visit(StartToken token, bool called = false);
    void Visit(IfToken token);
    void Visit(ReturnToken token);
    void Visit(PrintToken token);
    void Visit(VarToken token);
    void Visit(SetToken token);
    void Visit(PostponeToken token);
    string Visit(CallToken token);
    void Visit(TryToken token);
}