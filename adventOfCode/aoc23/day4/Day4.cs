using aocTools;

namespace aoc23.day4;

public class Day4 : AAocDay {
    public Day4() : base(true) {
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
        ReadInput();
    }

    private readonly List<Card> _cards = new();

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            InputTokens.Remove(1);
            var card = new Card {
                Id = int.Parse(InputTokens.Read()[..^1])
            };
            while (InputTokens.JustRead() != "|") {
                card.WinningNumbers.Add(InputTokens.ReadInt());
            }

            InputTokens.Remove(1);
            while (InputTokens.JustRead() != "Card") {
                card.PlayedNumbers.Add(InputTokens.ReadInt());
                if (InputTokens.Count == 0) break;
            }

            _cards.Add(card);
        }
    }

    public override void PuzzleOne() {
        var sum = _cards.Select(c => c.PlayedNumbers.Intersect(c.WinningNumbers).Count())
            .Select(wins => (wins == 0) ? 0 : Math.Pow(2, wins - 1))
            .Select(points => (int)points).Sum();

        Console.WriteLine(sum);
    }

    private readonly Dictionary<Card, int> _cardCount = new();

    public override void PuzzleTwo() {
        _cards.ForEach(c => _cardCount.Add(c, 1));

        foreach (var (c, cCount) in _cardCount) {
            var wins = c.PlayedNumbers.Intersect(c.WinningNumbers).Count();
            for (var i = c.Id + 1; i <= c.Id + wins; i++) {
                _cardCount[_cards[i - 1]] += cCount;
            }
        }

        Console.WriteLine(_cardCount.Values.Sum());
    }
}

class Card {
    public int Id { get; set; }
    public List<int> WinningNumbers { get; set; } = new();
    public List<int> PlayedNumbers { get; set; } = new();
}