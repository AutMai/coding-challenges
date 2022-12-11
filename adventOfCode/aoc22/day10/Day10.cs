using System.Numerics;
using aocTools;
using Raylib_cs;

namespace aoc22.day10;

public class Day10 : AAocDay {
    public static int X = 1;
    public static int CompletedCycles = 0;
    public static Dictionary<int, int> CycleRegister = new();
    public static char[] CRT = new char[280];
    public static AInstruction? RunningInstruction = null;
    public static SemaphoreSlim InstructionFinishedCycle = new(0);
    public List<AInstruction> Instructions { get; set; } = new();

    public Day10() : base(true) {
    }

    public override void PuzzleOne() {
        ReadInput();
        /*
        ExecuteInstructions();
        PrintCycleRegister();
        Console.WriteLine(GetTotalSignalStrength());*/
    }

    public void CrtDraw(int pos) {
        CRT[pos] = Math.Abs(pos % 40 - X) <= 1 ? '█' : ' ';
    }

    private int GetTotalSignalStrength() {
        var total = 0;
        for (int i = 20; i <= 220; i += 40) {
            total += GetSignalStrength(i);
        }

        return total;
    }

    private int GetSignalStrength(int cycle) {
        return CycleRegister[cycle] * cycle;
    }

    private void ExecuteInstructions() {
        foreach (var instruction in Instructions) {
            instruction.Execute();
        }
    }

    private void PrintCycleRegister() {
        foreach (var (key, value) in CycleRegister) {
            Console.WriteLine($"Cycle {key} - {value}");
        }
    }

    public static void StoreCycleAndRegisterValue() {
        CycleRegister.Add(CompletedCycles, X);
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            Instructions.Add(InputTokens.Read() switch {
                "addx" => new AddInstruction(InputTokens.ReadInt()),
                "noop" => new NoopInstruction(),
                _ => throw new Exception("Unknown instruction")
            });
        }
    }

    public override void PuzzleTwo() {
        //ReadInput();
        while (Instructions.Count > 0) {
            DoCycle(CompletedCycles + 1);
            //Console.WriteLine($"Cycle {CompletedCycles} - {X}");
        }

        PrintCrt();
    }

    private void DoCycle(int cycle) {
        if (RunningInstruction is null) {
            Console.WriteLine($"Start cycle\t{cycle}: begin executing {Instructions.First()}");
            new Thread(Instructions.First().Execute).Start();
            RunningInstruction = Instructions.First();
            Instructions.RemoveAt(0);
        }

        CrtDraw(cycle - 1);
        Console.WriteLine($"During cycle\t{cycle}: CRT draws pixel in position {cycle - 1}");
        Console.WriteLine($"Current CRT row: {new string(CRT)}");

        CompletedCycles++;
        RunningInstruction.Cycle.Release();
        InstructionFinishedCycle.Wait();
        Console.WriteLine();
    }

    private void PrintCrt() {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 40; j++) {
                Console.Write(CRT[i * 40 + j]);
                Console.Write(CRT[i * 40 + j]);
                Console.Write(CRT[i * 40 + j]);
            }

            Console.WriteLine();
        }
    }
}

public abstract class AInstruction {
    public readonly SemaphoreSlim Cycle = new(0);

    public virtual void Execute() {
        Console.WriteLine(
            $"End of cycle\t{Day10.CompletedCycles}: finish executing {this} (Register X is now {Day10.X})");
        Day10.RunningInstruction = null;
        Day10.InstructionFinishedCycle.Release();
    }

    public override string ToString() {
        if (GetType() == typeof(AddInstruction)) 
            return $"addx {((AddInstruction) this).Amount}";
        
        if (GetType() == typeof(NoopInstruction)) 
            return "noop";

        throw new Exception("Unknown instruction");
    }
}

public class AddInstruction : AInstruction {
    public int Amount { get; set; }

    public override void Execute() {
        Cycle.Wait();
        Day10.InstructionFinishedCycle.Release();
        Cycle.Wait();
        Day10.X += Amount;
        base.Execute();
    }

    public AddInstruction(int amount) {
        Amount = amount;
    }
}

public class NoopInstruction : AInstruction {
    public override void Execute() {
        Cycle.Wait();
        base.Execute();
    }
}