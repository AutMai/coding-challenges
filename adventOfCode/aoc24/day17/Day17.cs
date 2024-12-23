using System.Collections.Concurrent;
using System.Numerics;
using System.Text;
using aocTools;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

namespace aoc24.day17;

public class Day17 : AAocDay {
    public Day17() : base(false, true) { }

    private void ReadInput() {
        var a = InputTokens.Read().Replace("Register A: ", "");
        var b = InputTokens.Read().Replace("Register B: ", "");
        var c = InputTokens.Read().Replace("Register C: ", "");
        var i = InputTokens.Read().Replace("Program: ", "").Split(",");

        Computer.RegisterA = int.Parse(a);
        Computer.RegisterB = int.Parse(b);
        Computer.RegisterC = int.Parse(c);
        Computer.Instructions = new int[i.Length];
        for (var j = 0; j < i.Length; j++) {
            Computer.Instructions[j] = int.Parse(i[j]);
        }
    }

    public override void PuzzleOne() {
        ReadInput();
        Computer.Execute();
        Console.WriteLine(string.Join(",", Computer.Output));
    }

    public override void PuzzleTwo() {
        Computer.Reset();
        ResetInput();
        ReadInput();
        
        var desiredOutString = string.Join(",", Computer.Instructions);
        var registerA = 3000000;
        
        while (true) {
            Computer.RegisterA = registerA;
            Console.WriteLine($"Trying {registerA}");
            Computer.Execute();
            var outString = string.Join(",", Computer.Output);
            
            if (outString == desiredOutString) {
                break;
            }
            
            Computer.Reset();
            registerA++;
        }
        
        Console.WriteLine(registerA);
    }
}

public static class Computer {
    public static int RegisterA { get; set; }
    public static int RegisterB { get; set; }
    public static int RegisterC { get; set; }

    public static int InstructionPointer { get; set; }
    public static int[] Instructions { get; set; }
    
    public static List<int> Output { get; set; } = new List<int>();

    public static void Reset() {
        RegisterA = 0;
        RegisterB = 0;
        RegisterC = 0;
        InstructionPointer = 0;
        Output.Clear();
    }

    public static void Execute() {
        while (InstructionPointer < Instructions.Length) {
            var instruction = Instructions[InstructionPointer];
            AInstruction aInstruction = instruction switch {
                0 => new Adv(),
                1 => new Bxl(),
                2 => new Bst(),
                3 => new Jnz(),
                4 => new Bxc(),
                5 => new Out(),
                6 => new Bdv(),
                7 => new Cdv(),
                _ => throw new ArgumentOutOfRangeException()
            };
            aInstruction.Execute(GetLiteralOperand(), GetComboOperand());
        }
    }


    public static int GetComboOperand() {
        var o = GetLiteralOperand();
        return o switch {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => RegisterA,
            5 => RegisterB,
            6 => RegisterC,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static int GetLiteralOperand() {
        return Instructions[InstructionPointer + 1];
    }
}

public abstract class AInstruction {
    public abstract void Execute(int literalOperand, int comboOperand);
}

public class Adv : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        var denominator = (int)Math.Pow(2, comboOperand);
        Computer.RegisterA = (int)Math.Truncate((double)(Computer.RegisterA / denominator));
        
        Computer.InstructionPointer += 2;
    }
}

public class Bxl : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        Computer.RegisterB = Computer.RegisterB ^ literalOperand;

        Computer.InstructionPointer += 2;
    }
}

public class Bst : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        Computer.RegisterB = comboOperand % 8;

        Computer.InstructionPointer += 2;
    }
}

public class Jnz : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        if (Computer.RegisterA != 0) {
            Computer.InstructionPointer = literalOperand;
        }
        else {
            Computer.InstructionPointer += 2;
        }
    }
}

public class Bxc : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        Computer.RegisterB = Computer.RegisterB ^ Computer.RegisterC;

        Computer.InstructionPointer += 2;
    }
}

public class Out : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        Computer.Output.Add(comboOperand % 8);

        Computer.InstructionPointer += 2;
    }
}

public class Bdv : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        var denominator = (int)Math.Pow(2, comboOperand);
        Computer.RegisterB = (int)Math.Truncate((double)(Computer.RegisterA / denominator));

        Computer.InstructionPointer += 2;
    }
}

public class Cdv : AInstruction {
    public override void Execute(int literalOperand, int comboOperand) {
        var denominator = (int)Math.Pow(2, comboOperand);
        Computer.RegisterC = (int)Math.Truncate((double)(Computer.RegisterA / denominator));

        Computer.InstructionPointer += 2;
    }
}