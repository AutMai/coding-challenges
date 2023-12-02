using aocTools;

namespace aoc23.day2;

public class Day2 : AAocDay {
    public Day2() : base() {
        ReadInput();
    }

    List<Game> Games { get; set; } = new List<Game>();

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            var split = line.Split(';', ':');
            var game = new Game();
            game.Id = split[0].Split(' ')[1].ToInt();

            // loop all other elements in split
            for (var i = 1; i < split.Length; i++) {
                var set = new Set();
                var cubes = split[i].Split(',');
                // trim all cubes
                cubes = cubes.Select(cube => cube.Trim()).ToArray();
                foreach (var cube in cubes) {
                    var color = cube.Split(' ')[1] switch {
                        "red" => EColor.Red,
                        "green" => EColor.Green,
                        "blue" => EColor.Blue,
                        _ => throw new Exception("Unknown color")
                    };
                    var count = cube.Split(' ')[0].ToInt();
                    set.Add(color, count);
                }

                game.Sets.Add(set);
            }

            Games.Add(game);
        }
    }


    public override void PuzzleOne() {
        var allowedAmounts = new Dictionary<EColor, int>() {
            { EColor.Red, 12 },
            { EColor.Green, 13 },
            { EColor.Blue, 14 }
        };

        var validGames = Games.Where(game => game.Sets.All(set => set.All(pair => pair.Value <= allowedAmounts[pair.Key]))).ToList();
        Console.WriteLine(validGames.Sum(vg => vg.Id));
    }

    public override void PuzzleTwo() =>
        Console.WriteLine(
            Games.Select(game => new Dictionary<EColor, int>() {
                { EColor.Red, game.Sets.Where(set => set.ContainsKey(EColor.Red)).Max(set => set[EColor.Red]) },
                { EColor.Green, game.Sets.Where(set => set.ContainsKey(EColor.Green)).Max(set => set[EColor.Green]) },
                { EColor.Blue, game.Sets.Where(set => set.ContainsKey(EColor.Blue)).Max(set => set[EColor.Blue]) }
            }).Select(minAmounts => minAmounts.Aggregate(1, (current, pair) => current * pair.Value)).Sum()
        );
}

public class Game {
    public int Id { get; set; }
    public List<Set> Sets { get; set; } = new List<Set>();
}

public class Set : Dictionary<EColor, int> {
}

public enum EColor {
    Red,
    Green,
    Blue
}