using System.Text;
using aocTools;

namespace aoc24.day11;

public class Day11 : AAocDay {
    private static Dictionary<long,long> _stones = [];
    private static Dictionary<long,long> _newStones = [];
    private static Dictionary<long,long> _originalStones = [];

    private void AddStone(long stone) {
        if (!_stones.TryAdd(stone, 1)) {
            _stones[stone]++;
        }
    }
    
    private void AddStones(long stone, long count) {
        if (!_stones.TryAdd(stone, count)) {
            _stones[stone] += count;
        }
    }
    
    private void AddNewStones(long stone, long count) {
        if (!_newStones.TryAdd(stone, count)) {
            _newStones[stone] += count;
        }
    }
    
    private long StoneCount() {
        return _stones.Values.Sum();
    }
    
    public Day11() : base() {
        ReadInput();
    }

    private void ReadInput() {
        var line = InputTokens.Read();
        var stones = line.Split(' ');
        foreach (var stone in stones) {
            AddStone(long.Parse(stone));
        }

        _originalStones = new Dictionary<long, long>(_stones);
    }

    public override void PuzzleOne() {
        PrintStones();
        for (int i = 0; i < 25; i++) {
            Console.WriteLine($"Blink {i}");
            Blink();
            //PrintStones();
        }

        Console.WriteLine($"There are {StoneCount()} stones after 25 blinks.");
    }

    private void Blink() {
        if (StateMem.StateExists(_stones)) {
            _stones = StateMem.GetState(_stones);
            return;
        }
        
        _newStones.Clear();
        
        foreach (var stone in _stones) {
            var newStonesTuple = StoneBlinker.GetStone(stone.Key);
            AddNewStones(newStonesTuple.Item1, stone.Value);
            if (newStonesTuple.Item2 != null) {
                AddNewStones(newStonesTuple.Item2.Value, stone.Value);
            }
        }
        
        StateMem.SaveState(_stones, _newStones);
        _stones = new Dictionary<long, long>(_newStones);
    }

    private void PrintStones() {
        var sb = new StringBuilder();
        foreach (var stone in _stones) {
            for (int i = 0; i < stone.Value; i++) {
                sb.Append($"{stone.Key} ");
            }
        }

        Console.WriteLine(sb.ToString());
    }

    public override void PuzzleTwo() {
        _stones = new Dictionary<long, long>(_originalStones);
        for (int i = 0; i < 75; i++) {
            Console.WriteLine($"Blink {i}");
            Blink();
            //PrintStones();
        }

        Console.WriteLine($"There are {StoneCount()} stones after 75 blinks.");
    }
}

public static class StateMem {
    public static readonly Dictionary<Dictionary<long,long>, Dictionary<long,long>> States = new();
    
    public static void SaveState(Dictionary<long,long> stones1, Dictionary<long,long> stones2) {
        // order them sorted to make sure we can compare them
        stones1 = stones1.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        stones2 = stones2.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        States[stones1] = stones2;
    }
    
    public static bool StateExists(Dictionary<long,long> stones) {
        // use sequence equal to compare the lists (order should not matter)
        // sort the stones to make sure we can compare them
        stones = stones.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        return States.Any(kvp => kvp.Key.SequenceEqual(stones));
    }
    
    public static Dictionary<long,long> GetState(Dictionary<long,long> stones) {
        // sort the stones to make sure we can compare them and get the correct state by sequence equal
        stones = stones.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        return States.First(kvp => kvp.Key.SequenceEqual(stones)).Value;
    }
}

public static class StoneBlinker {
    private static readonly Dictionary<long, Tuple<long,long?>> Memo = new();

    public static Tuple<long,long?> GetStone(long stone) {
        if (Memo.TryGetValue(stone, out var value)) {
             return value;
        }

        var newStones = Blink(stone);
        Memo[stone] = newStones;
        return newStones;
    }

    private static Tuple<long,long?> Blink(long stone) {
        // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
        if (stone == 0) {
            return new Tuple<long, long?>(1, null);
        }

        //  If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
        var len = Math.Floor(Math.Log10(stone)) + 1;
        if (len % 2 == 0) {
            var left = stone / (long)Math.Pow(10, len / 2);
            var right = stone % (long)Math.Pow(10, len / 2);
            return new Tuple<long, long?>(left, right);
        }

        // If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
        return new Tuple<long, long?>(stone * 2024, null);
    }
}