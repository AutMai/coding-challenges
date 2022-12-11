using System.Xml;
using aoc22.day10;
using aocTools;
using Raylib_cs;

namespace aoc22.day11;

public class Day11 : AAocDay {
    public static readonly List<Monkey> Monkeys = new();

    public Day11() : base(true, true) {
    }

    public override void PuzzleOne() {
        ReadInput();

        for (int i = 0; i < 20; i++) {
            Monkeys.ForEach(m => m.MakeMove());
        }

        Monkeys.ForEach(m => Console.WriteLine($"Monkey {m.Number} - {m.InspectedItemCount}"));
        var monkeys = Monkeys.OrderByDescending(m => m.InspectedItemCount).Take(2);
        ulong res = monkeys.First().InspectedItemCount * monkeys.Last().InspectedItemCount;
        Console.WriteLine($"Puzzle one: {res}");
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            InputTokens.Remove(1);
            Monkeys.Add(new Monkey(InputTokens.Read().Replace(":", "")));
            InputTokens.Remove(2);
            while (InputTokens.JustRead() != "Operation:") {
                Monkeys.Last().Items.Add(new Item(InputTokens.Read().Replace(",", "")));
            }

            InputTokens.Remove(4);
            var oper = InputTokens.Read();
            var num = InputTokens.Read();

            Monkeys.Last().Operation = oper switch {
                "*" when num == "old" => i => i * i,
                "+" => i => i + long.Parse(num),
                "-" => i => i - long.Parse(num),
                "/" => i => i / long.Parse(num),
                "*" => i => i * long.Parse(num),
                _ => throw new ArgumentOutOfRangeException()
            };
            InputTokens.Remove(3);
            Monkeys.Last().TestDivisibleBy = InputTokens.ReadInt();
            InputTokens.Remove(5);
            Monkeys.Last().TargetMonkeyIfTestTrue = InputTokens.ReadInt();
            InputTokens.Remove(5);
            Monkeys.Last().TargetMonkeyIfTestFalse = InputTokens.ReadInt();
        }
    }

    public override void PuzzleTwo() {
        //ResetInput();
        ReadInput();
        for (int i = 0; i < 10000; i++) {
            Monkeys.ForEach(m => m.MakeMove());
        }

        Monkeys.ForEach(m => Console.WriteLine($"Monkey {m.Number} - {m.InspectedItemCount}"));
        var monkeys = Monkeys.OrderByDescending(m => m.InspectedItemCount).Take(2);
        Console.WriteLine($"Puzzle one: {monkeys.First().InspectedItemCount * monkeys.Last().InspectedItemCount}");
    }
}

public class Monkey {
    public int Number { get; set; }
    public List<Item> Items { get; set; } = new();

    public Func<long, long> Operation { get; set; }
    public int TestDivisibleBy { get; set; }
    public int TargetMonkeyIfTestTrue { get; set; }
    public int TargetMonkeyIfTestFalse { get; set; }

    public ulong InspectedItemCount { get; set; } = 0;

    public void MakeMove() {
        InspectItems();
    }

    public Monkey(string number) {
        Number = int.Parse(number);
    }

    public Monkey(int number, List<Item> items, Func<long, long> operation) {
        Number = number;
        Items = items;
        Operation = operation;
    }


    private void InspectItems() {
        var newItems = new List<Item>(Items);
        foreach (var item in Items) {
            InspectedItemCount++;
            item.WorryLevel = Operation(item.WorryLevel);
            //item.WorryLevel /= 3;

            item.WorryLevel %= Extensions.lcm(Day11.Monkeys.Select(m => m.TestDivisibleBy).ToArray());
            
            if (item.WorryLevel % TestDivisibleBy == 0) {
                Day11.Monkeys[TargetMonkeyIfTestTrue].Items.Add(item);
                newItems.Remove(item);
            }
            else {
                Day11.Monkeys[TargetMonkeyIfTestFalse].Items.Add(item);
                newItems.Remove(item);
            }
        }
        Items = newItems;
    }
}

public class Item {
    public long WorryLevel { get; set; }

    public Item(long worryLevel) {
        WorryLevel = worryLevel;
    }

    public Item(string worryLevel) {
        WorryLevel = long.Parse(worryLevel);
    }
    
    public override string ToString() {
        return WorryLevel.ToString();
    }
}