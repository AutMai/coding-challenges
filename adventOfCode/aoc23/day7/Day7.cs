using System.Collections.Concurrent;
using aocTools;
using aocTools.Interval;

namespace aoc23.day7;

public class Day7 : AAocDay {
    public Day7() : base(true) {
        ReadInput();
    }

    private List<Tuple<PokerHand, int>> Hands { get; set; } = new();

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            Hands.Add(new Tuple<PokerHand, int>(new PokerHand(InputTokens.Read()), InputTokens.ReadInt()));
        }
    }

    public override void PuzzleOne() {
        // compare all poker hands and rank them
        // output each hand and its rank (best hand has highest rank)

        long bidSum = 0;
        foreach (var hand in Hands) {
            var rank = 1 + Hands.Count(otherHand => hand.Item1.CompareTo(otherHand.Item1) == 1);
            //Console.WriteLine($"{hand.Item1} {hand.Item2} {rank}");
            bidSum += rank * hand.Item2;
        }

        Console.WriteLine($"Puzzle 1: {bidSum}");
    }

    public override void PuzzleTwo() {
        PokerHand.CardsOrder = "J23456789TQKA";
        // J is now Joker, it can be any card that makes the best hand
        // rank hands with joker
        // output each hand and its rank (best hand has highest rank)
        var newHands = new Dictionary<PokerHand, Tuple<PokerHand, int>>();
        foreach (var h in Hands) {
            newHands.Add(h.Item1, new Tuple<PokerHand, int>(h.Item1.ConvertJoker(), h.Item2));
        }


        long bidSum = 0;
        foreach (var nh in newHands) {
            var oldHand = nh.Key;
            var newHand = nh.Value.Item1;
            var rank = 1;
            foreach (var otherHand in newHands) {
                if (oldHand.CompareTo(otherHand.Key, newHand, otherHand.Value.Item1) == 1) {
                    rank++;
                }
            }

            //Console.WriteLine($"{oldHand} {newHand} {nh.Value.Item2} {rank}");
            bidSum += rank * nh.Value.Item2;
        }

        Console.WriteLine($"Puzzle 2: {bidSum}");
    }
}

class PokerHand {
    public List<char> Cards { get; set; } = new();

    public PokerHand(string cards) {
        foreach (var card in cards) {
            Cards.Add(card);
        }
    }

    public bool IsFiveOfAKind() => Cards.All(c => c == Cards[0]);

    public bool IsFourOfAKind() => Cards.GroupBy(c => c).Any(g => g.Count() == 4);

    public bool IsFullHouse() =>
        Cards.GroupBy(c => c).Any(g => g.Count() == 3) && Cards.GroupBy(c => c).Any(g => g.Count() == 2);

    public bool IsThreeOfAKind() => Cards.GroupBy(c => c).Any(g => g.Count() == 3);

    public bool IsTwoPairs() => Cards.GroupBy(c => c).Count(g => g.Count() == 2) == 2;

    public bool IsOnePair() => Cards.GroupBy(c => c).Any(g => g.Count() == 2);

    public PokerHandType GetHandType() {
        if (IsFiveOfAKind())
            return PokerHandType.FiveOfAKind;
        if (IsFourOfAKind())
            return PokerHandType.FourOfAKind;
        if (IsFullHouse())
            return PokerHandType.FullHouse;
        if (IsThreeOfAKind())
            return PokerHandType.ThreeOfAKind;
        if (IsTwoPairs())
            return PokerHandType.TwoPairs;
        if (IsOnePair())
            return PokerHandType.OnePair;
        return PokerHandType.HighCard;
    }

    public static string CardsOrder { get; set; } = "23456789TJQKA";

    public int CompareTo(PokerHand other, PokerHand? jokerHandThis = null, PokerHand? jokerHandOther = null) {
        if (jokerHandThis is not null) {
            var thisHandType = jokerHandThis.GetHandType();
            var otherHandType = jokerHandOther.GetHandType();
            if (thisHandType > otherHandType)
                return 1;
            if (thisHandType < otherHandType)
                return -1;
        }
        else {
            var thisHandType = GetHandType();
            var otherHandType = other.GetHandType();
            if (thisHandType > otherHandType)
                return 1;
            if (thisHandType < otherHandType)
                return -1;
        }

        // draw: compare cards from first to last and check which one is higher
        for (var i = 0; i < Cards.Count; i++) {
            if (CardsOrder.IndexOf(Cards[i]) > CardsOrder.IndexOf(other.Cards[i]))
                return 1;
            if (CardsOrder.IndexOf(Cards[i]) < CardsOrder.IndexOf(other.Cards[i]))
                return -1;
        }

        return 0;
    }

    public override string ToString() {
        return string.Join("", Cards);
    }

    public PokerHand ConvertJoker() {
        var bestHand = new PokerHand("23456");

        // make all possible hands with jokers replaced by all possible cards
        var jokerCount = Cards.Count(c => c == 'J');
        var possibleCards = new List<char>();
        for (var i = 0; i < CardsOrder.Length; i++) {
            possibleCards.Add(CardsOrder[i]);
        }

        var possibleHands = new ConcurrentBag<PokerHand>();
        var possibleHandsCount = (int)Math.Pow(possibleCards.Count, jokerCount);
        Parallel.For(0, possibleHandsCount, i => {
            var hand = new PokerHand(ToString());
            var jokerIndex = 0;
            for (var j = 0; j < hand.Cards.Count; j++) {
                if (hand.Cards[j] == 'J') {
                    hand.Cards[j] =
                        possibleCards[(i / (int)Math.Pow(possibleCards.Count, jokerIndex)) % possibleCards.Count];
                    jokerIndex++;
                }
            }

            possibleHands.Add(hand);
        });

        // compare all possible hands and get the best one
        foreach (var possibleHand in possibleHands) {
            if (possibleHand.CompareTo(bestHand) == 1) {
                bestHand = possibleHand;
            }
        }

        return bestHand;
    }
}

internal enum PokerHandType {
    HighCard,
    OnePair,
    TwoPairs,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}