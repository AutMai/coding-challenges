using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using compiler_visitorPattern.tokens;

namespace compiler_visitorPattern;

internal static class Program {
    private static void Main() {
        var words = File.ReadAllText("../../../files/level5/level5_example.in").Replace("\r", "")
            .Split(' ', '\n');

        var token = CreateTokenTree(words);
        
        var tokenVisitor = new TokenVisitor();
        // Visit the tree with visitor
        token.Accept(tokenVisitor);
        // Output the result
        Console.WriteLine(tokenVisitor.Output);
    }

    private static BaseToken CreateTokenTree(IReadOnlyList<string> words) {
        BaseToken currentContextToken = new RootToken(null);
        BaseToken lastToken = currentContextToken;

        var result = currentContextToken;
        
        for (var i = 1; i < words.Count; i++) {
            switch (words[i]) {
                case "start":
                    currentContextToken.Children.Add(currentContextToken = lastToken =
                        new StartToken(currentContextToken));
                    break;
                case "print":
                    currentContextToken.Children.Add(lastToken = new PrintToken(words[i + 1], currentContextToken));
                    break;
                case "if":
                    currentContextToken.Children.Add(currentContextToken = lastToken =
                        new IfToken(words[i + 1], currentContextToken, new List<BaseToken>()));
                    break;
                case "else":
                    if (currentContextToken.Children.Last() is IfToken ifToken) {
                        var elseToken = new ElseToken(ifToken.ParentToken);
                        currentContextToken.Children[^1] = new IfToken(ifToken.Value, ifToken.ParentToken,
                            ifToken.Children, elseToken);
                        currentContextToken = elseToken;
                    }
                    else {
                        Console.WriteLine("NO IF BEFORE ELSE");
                    }

                    break;
                case "postpone":
                    currentContextToken.Children.Add(currentContextToken = new PostponeToken(currentContextToken));
                    break;
                case "return":
                    currentContextToken.Children.Add(new ReturnToken(words[i + 1], currentContextToken));
                    break;
                case "var":
                    currentContextToken.Children.Add(new VarToken(words[i + 1], words[i + 2], currentContextToken));
                    break;
                case "set":
                    currentContextToken.Children.Add(new SetToken(words[i + 1], words[i + 2], currentContextToken));
                    break;
                case "end":
                    currentContextToken = currentContextToken.ParentToken;
                    break;
                case "call":
                    if (lastToken.Value == null) {
                        lastToken.Children.Add(lastToken = new CallToken(words[i + 1], currentContextToken));
                    }
                    else if (lastToken.Value.ToString() == "call") {
                        lastToken.Value = (lastToken = new CallToken(words[i + 1], currentContextToken));
                    }
                    else {
                        lastToken.Children.Add(lastToken = new CallToken(words[i + 1], currentContextToken));
                    }
                    break;
            }
        }

        return result;
    }
}