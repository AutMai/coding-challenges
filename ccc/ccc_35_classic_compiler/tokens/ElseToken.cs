﻿using compiler_visitorPattern.interfaces;

namespace compiler_visitorPattern.tokens; 

public class ElseToken : BaseToken {


    public ElseToken(BaseToken parentToken) : base(parentToken) {
    }

    public override void Accept(IVisitor visitor) {
        throw new System.NotImplementedException();
    }
}