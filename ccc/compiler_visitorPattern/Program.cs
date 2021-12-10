using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using compiler_visitorPattern.tokens;

namespace compiler_visitorPattern; 

internal static class Program {
    private static void Main() {
        var words = File.ReadAllText("../../../files/level4/level4_5.in").Replace("\r", "")
            .Split(' ', '\n');

        var token = CreateTokenTree(words);

        var tokenVisitor = new TokenVisitor();
        // Visit the tree with visitor
        token.Accept(tokenVisitor);
        // Output the result
        Console.WriteLine(tokenVisitor.Output);
    }

    private static BaseToken CreateTokenTree(IReadOnlyList<string> words) {
        BaseToken currentToken = new RootToken(null);

        var result = currentToken;


        for (var i = 1; i < words.Count; i++) {
            switch (words[i]) {
                case "start":
                    currentToken.Children.Add(currentToken =
                        new StartToken(currentToken));
                    break;
                case "print":
                    currentToken.Children.Add(new PrintToken(words[i + 1], currentToken));
                    break;
                case "if":
                    currentToken.Children.Add(currentToken =
                        new IfToken(words[i + 1], currentToken, new List<BaseToken>()));
                    break;
                case "else":
                    if (currentToken.Children.Last() is IfToken) {
                        var ifToken = (IfToken)currentToken.Children[^1];
                        var elseToken = new ElseToken(ifToken.ParentToken);
                        currentToken.Children[^1] = new IfToken(ifToken.Value, ifToken.ParentToken,
                            ifToken.Children, elseToken);
                        currentToken = elseToken;
                    }
                    else {
                        Console.WriteLine("NO IF BEFORE ELSE");
                    }

                    break;
                case "postpone":
                    currentToken.Children.Add(currentToken = new PostponeToken(currentToken));
                    break;
                case "return":
                    currentToken.Children.Add(new ReturnToken(words[i + 1], currentToken));
                    break;
                case "var":
                    currentToken.Children.Add(new VarToken(words[i + 1], words[i + 2], currentToken));
                    break;
                case "set":
                    currentToken.Children.Add(new SetToken(words[i + 1], words[i + 2], currentToken));
                    break;
                case "end":
                    currentToken = currentToken.ParentToken;
                    break;
            }
        }

        return result;
    }
}