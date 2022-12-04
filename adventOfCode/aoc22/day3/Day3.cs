using aocTools;

namespace aoc22.day3;

public class Day3 : AAocDay {
    public Day3() {
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
    }
    public override void PuzzleOne() {
        var rucksacks = new List<Rucksack>();
        while (InputTokens.HasMoreTokens()) {
            rucksacks.Add(new Rucksack(InputTokens.Read()));
        }

        var prioritySum = 0;
        foreach (var rucksack in rucksacks) {
            prioritySum += rucksack.GetCommonItem().GetPriority();
        }

        Console.WriteLine(prioritySum);
    }

    public override void PuzzleTwo() {
        ResetInput();
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();

        var groups = new List<Group>();
        while (InputTokens.HasMoreTokens()) {
            var r1items = InputTokens.Read();
            var r2items = InputTokens.Read();
            var r3items = InputTokens.Read();
            groups.Add(new Group(r1items, r2items, r3items));
        }

        var prioritySum = 0;
        foreach (var group in groups) {
            prioritySum += group.GetCommonItem().GetPriority();
        }

        Console.WriteLine(prioritySum);
    }
}

public class Group {
    public Rucksack Rucksack1{ get; set; }
    public Rucksack Rucksack2{ get; set; }
    public Rucksack Rucksack3{ get; set; }
    
    public Group(string r1items, string r2items, string r3items) {
        Rucksack1 = new Rucksack(r1items);
        Rucksack2 = new Rucksack(r2items);
        Rucksack3 = new Rucksack(r3items);
    }

    public char GetCommonItem() {
        return Rucksack1.Items.Intersect(Rucksack2.Items).Intersect(Rucksack3.Items).Single();
    }
} 

public class Rucksack {
    public Compartment Compartment1 { get; set; }
    public Compartment Compartment2 { get; set; }

    public string Items { get; set; }
    public Rucksack(string items) {
        Items = items;
        Compartment1 = new Compartment(items[..(items.Length / 2)]);
        Compartment2 = new Compartment(items[(items.Length / 2)..]);
    }
    
    public char GetCommonItem() {
        return Compartment1.Items.Intersect(Compartment2.Items).Single();
    }
}

public class Compartment {
    public string Items { get; set; }

    public Compartment(string items) {
        Items = items;
    }
}

public static class ItemExtension {
    public static int GetPriority(this char item) {
        // if item is lowercase
        if (item >= 97 && item <= 122)
            return item - 96;

        // if item is uppercase
        if (item >= 65 && item <= 90)
            return item - 38;
        
        return 0;
    }
}