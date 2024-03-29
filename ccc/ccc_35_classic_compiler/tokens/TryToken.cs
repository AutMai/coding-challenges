﻿using System.Collections.Generic;
using ccc_35_classic_compiler.interfaces;

namespace ccc_35_classic_compiler.tokens; 

public class TryToken : BaseToken {
    public CatchToken CatchToken { get; }


    public override void Accept(IVisitor visitor) {
        visitor.Visit(this);
    }

    public TryToken(BaseToken parentToken, List<BaseToken> children = null,
        CatchToken catchToken = null) : base(parentToken) {
        Children = children;
        CatchToken = catchToken;
    }
}