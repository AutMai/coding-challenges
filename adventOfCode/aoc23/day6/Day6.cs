using aocTools;

namespace aoc23.day6;

public class Day6 : AAocDay {
    public Day6() : base(true) {
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
        ReadInput();
    }

    List<Race> _races = new();
    private void ReadInput() {
        var times = new List<int>();
        var distances = new List<int>();
        InputTokens.Remove(1);
        while (InputTokens.JustRead() != "Distance:") {
            times.Add(InputTokens.ReadInt());
        }
        InputTokens.Remove(1);
        while (InputTokens.HasMoreTokens()) {
            distances.Add(InputTokens.ReadInt());
        }
        
        // generate races
        for (var i = 0; i < times.Count; i++) {
            _races.Add(new Race(){Time = times[i], Distance = distances[i]});
        }
    }

    public override void PuzzleOne() {
        var res = 1;
        foreach (var race in _races) {
            int waysToWin = 0;
            for (int i = 1; i < race.Time; i++) {
                var distance = i * (race.Time - i);
                if (distance > race.Distance) waysToWin++;
            }            
            res *= waysToWin;
        }

        Console.WriteLine(res);
    }

    public override void PuzzleTwo() {
        var r = GetPuzzleTwoInput();
        var b1 = (r.Time - Math.Sqrt((-4) * r.Distance + Math.Pow(r.Time, 2))) / 2;
        var b2 = (r.Time + Math.Sqrt((-4) * r.Distance + Math.Pow(r.Time, 2))) / 2;
        var diff = Math.Ceiling(b2) - Math.Ceiling(b1);
        Console.WriteLine(diff);
    }

    private Race GetPuzzleTwoInput() {
        Race race = new();
        foreach (var r in _races) {
            // concat all race times into a single one (e.g.: 63     78     94     68 => 63789468)
            var newTime = race.Time.ToString();
            newTime += r.Time;
            race.Time = long.Parse(newTime);
            
            // distance:
            var newDistance = race.Distance.ToString();
            newDistance += r.Distance;
            race.Distance = long.Parse(newDistance);
        }

        return race;
    }
}

class Race {
    public long Time { get; set; }
    public long Distance { get; set; }
}