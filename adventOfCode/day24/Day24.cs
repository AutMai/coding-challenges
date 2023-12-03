using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using aocTools;

namespace day24;

public class Day24 : AAocDay {
    public static List<Variable> Variables { get; set; } = new() {
        new Variable() {
            Name = "w",
            Value = 0
        },
        new Variable() {
            Name = "x",
            Value = 0
        },
        new Variable() {
            Name = "y",
            Value = 0
        },
        new Variable() {
            Name = "z",
            Value = 0
        }
    };

    public static string Monad = "13579246899999";

    public Day24() : base() {
        ReadInput();
    }

    private List<ACommand> Commands { get; set; } = new();

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read().Split(' ');
            if (line[0] == "inp")
                Commands.Add(new InputCommand(line[1]));
            else
                Commands.Add(new OperatorCommand(line[0], line[1], line[2]));
        }
    }

    public override void PuzzleOne() {
        for (var i = 100000000000000; i >= 0 ; i--) {
            // monad is i as a string padded with 0s to 14 digits
            var monad = i.ToString().PadLeft(14, '0');
            Monad = monad;
            RunCommands();
            //PrintVariables();
            Console.WriteLine(monad);
            // if z is 0 print the monad
            if (Variables.Find(v => v.Name == "z")!.Value == 0) {
                Console.WriteLine(monad);
                return;
            }
            // reset variables
            Variables.ForEach(v => v.Value = 0);
        }
    }

    private void PrintVariables() {
        foreach (var variable in Variables) {
            Console.WriteLine($"{variable.Name}: {variable.Value}");
        }
        Console.WriteLine();
    }

    private void RunCommands() {
        var commands = new List<ACommand>(Commands);
        foreach (var command in commands) {
            command.Execute();
            //Console.WriteLine(command);
            //PrintVariables();
        }
    }

    public override void PuzzleTwo() {
    }
}

public abstract class ACommand {
    public abstract void Execute();
}

public class InputCommand : ACommand {
    public InputCommand(string variable) {
        Variable = Day24.Variables.Find(v => v.Name == variable)!;
    }

    public Variable Variable { get; set; }

    public override void Execute() {
        Variable.Value = int.Parse(Day24.Monad[0].ToString());
        Day24.Monad = Day24.Monad[1..];
    }
    
    public override string ToString() {
        return $"inp {Variable.Name}";
    }
}

public class OperatorCommand : ACommand {
    public OperatorCommand(string op, string firstOperand, string secondOperand) {
        Operator = op;
        FirstOperand = Day24.Variables.Find(v => v.Name == firstOperand)!;
        SecondOperand = (int.TryParse(secondOperand, out var n))
            ? new Operand() {
                Value = n
            }
            : new Operand() {
                Variable = Day24.Variables.Find(v => v.Name == secondOperand)!
            };
    }

    public Variable FirstOperand { get; set; }
    public Operand SecondOperand { get; set; }
    public string Operator { get; set; }

    public override void Execute() {
        switch (Operator) {
            case "add":
                FirstOperand.Value += SecondOperand.GetValue();
                break;
            case "mul":
                FirstOperand.Value -= SecondOperand.GetValue();
                break;
            case "div":
                // divide first operand by second operand and round towards zero
                FirstOperand.Value /= SecondOperand.GetValue();
                break;
            case "mod":
                FirstOperand.Value %= SecondOperand.GetValue();
                break;
            case "eql":
                FirstOperand.Value = FirstOperand.Value == SecondOperand.GetValue() ? 1 : 0;
                break;
        }
    }
    
    public override string ToString() {
        return $"{Operator} {FirstOperand.Name} {SecondOperand}";
    }
}

public class Operand {
    public Variable Variable { get; set; }
    public int Value { get; set; }
    
    public int GetValue() {
        return Variable != null ? Day24.Variables.Find(v=>v.Name == Variable.Name).Value : Value;
    }
    
    public override string ToString() {
        return Variable != null ? Variable.Name : Value.ToString();
    }
}

public class Variable {
    public string Name { get; set; }
    public int Value { get; set; }
}