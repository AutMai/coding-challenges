using aocTools;

namespace aoc22.day1; 

public class Day1 : AAocDay{
    public Day1() {
        GetElves();
    }
    public override void PuzzleOne() {
        Console.WriteLine(_elves.Max(e=>e.CalorieCount));
    }

    public override void PuzzleTwo() {
        var top3 = _elves.OrderByDescending(e=>e.CalorieCount).Take(3).Sum(e=>e.CalorieCount);
        Console.WriteLine(top3);
    }
    private readonly List<Elf> _elves = new();
    private List<Elf> GetElves() {
        while (InputTokens.HasMoreTokens()) {
            var elf = new Elf();
            _elves.Add(elf);
            while (InputTokens.JustRead() != "") {
                elf.Calories.Add(InputTokens.ReadInt());
                if (!InputTokens.HasMoreTokens()) break;
            }

            if (InputTokens.HasMoreTokens())
                InputTokens.Read();
        }

        return _elves;
    }
}