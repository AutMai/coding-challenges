using aocTools;

namespace aoc22.day5;

public class Day5 : AAocDay {
    public Day5() {
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
    }

    private List<Stack> _stacks = new List<Stack>();

    public override void PuzzleOne() {
        for (int i = 0; i < 9; i++) {
            _stacks.Add(new Stack());
        }

        while (!int.TryParse(InputTokens.JustRead()[1].ToString(), out _)) {
            var x = InputTokens.Read().SplitChunks(3);
            for (int i = 0; i < 9; i++) {
                if (x.ElementAt(i)[1] != ' ') {
                    _stacks[i].Add(x.ElementAt(i)[1]);
                }
            }
        }

        InputTokens.Read();

        while (InputTokens.HasMoreTokens()) {
            var s = InputTokens.Read().Split(' ');
            var amount = int.Parse(s[1]);
            var from = int.Parse(s[3]) - 1;
            var to = int.Parse(s[5]) - 1;

            for (int i = 0; i < amount; i++) {
                _stacks[to].Insert(0, _stacks[from].First());
                _stacks[from].RemoveAt(0);
            }
        }

        _stacks.ForEach(s => Console.Write(s.First()));

        Console.WriteLine();
    }

    public override void PuzzleTwo() {
        ResetInput();
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
        _stacks = new List<Stack>();
        for (int i = 0; i < 9; i++) {
            _stacks.Add(new Stack());
        }

        while (!int.TryParse(InputTokens.JustRead()[1].ToString(), out _)) {
            var x = InputTokens.Read().SplitChunks(3);
            for (int i = 0; i < 9; i++) {
                if (x.ElementAt(i)[1] != ' ') {
                    _stacks[i].Add(x.ElementAt(i)[1]);
                }
            }
        }

        InputTokens.Read();

        while (InputTokens.HasMoreTokens()) {
            var s = InputTokens.Read().Split(' ');
            var amount = int.Parse(s[1]);
            var from = int.Parse(s[3]) - 1;
            var to = int.Parse(s[5]) - 1;

            _stacks[to].InsertRange(0, _stacks[from].Take(amount));
            _stacks[from].RemoveRange(0, amount);
        }

        _stacks.ForEach(s => Console.Write(s.First()));

        Console.WriteLine();
    }
}

public class Stack : List<char> {
}

public static class Extensions {
    public static IEnumerable<string> SplitChunks(this string str, int chunkSize) {
        var inc = chunkSize + 1;
        var list = new List<string>();
        for (int i = 0; i < 9; i++) {
            list.Add(str.Substring(i * inc, chunkSize));
            // first iteration: 0, 3
            // second iteration: 4, 3
            // third iteration: 8, 3
            // fourth iteration: 12, 3
            // fifth iteration: 16, 3
            // sixth iteration: 20, 3
            // seventh iteration: 24, 3
            // eighth iteration: 28, 3
            // ninth iteration: 32, 3
        }

        return list;
    }
}