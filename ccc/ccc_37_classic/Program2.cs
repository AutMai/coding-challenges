using System.Text;
using CodingHelper;

var r = new InputReader();

r.ReadZipFile("files/level3.zip", " ");



foreach (var l in r.GetInputs()) {
    l.SetOutput();
    var tournamentCount = l.ReadInt();
    var playersPerTournament = l.ReadInt();

    for (int i = 0; i < tournamentCount; i++) {
        var rocks = l.Read();
        var papers = l.Read();
        var scissors = l.Read();
        var rockAmount = rocks.Replace("R", "").ToInt32();
        var paperAmount = papers.Replace("P", "").ToInt32();
        var scissorsAmount = scissors.Replace("S", "").ToInt32();
        
        var playerList = new List<Player>();
        for (int j = 0; j < rockAmount; j++) {
            playerList.Add(new Player(FightingStyle.Rock));
        }
        for (int j = 0; j < paperAmount; j++) {
            playerList.Add(new Player(FightingStyle.Paper));
        }
        for (int j = 0; j < scissorsAmount; j++) {
            playerList.Add(new Player(FightingStyle.Scissors));
        }
        

        var tournament = new Tournament(playerList);

        tournament.ArrangePlayers();
    }
}


class Player {
    Dictionary<FightingStyle, FightingStyle> RockPaperScissors = new Dictionary<FightingStyle, FightingStyle>() {
        { FightingStyle.Rock, FightingStyle.Scissors },
        { FightingStyle.Paper, FightingStyle.Rock },
        { FightingStyle.Scissors, FightingStyle.Paper }
    };

    public FightingStyle FightingStyle { get; set; }

    public Player(char input) {
        FightingStyle = input switch {
            'R' => FightingStyle.Rock,
            'P' => FightingStyle.Paper,
            'S' => FightingStyle.Scissors,
            _ => FightingStyle
        };
    }

    public Player(FightingStyle fightingStyle) {
        FightingStyle = fightingStyle;
    }

    public Player Fight(Player otherPlayer) {
        // return winner

        if (RockPaperScissors[FightingStyle] == otherPlayer.FightingStyle) {
            return this;
        }
        else if (RockPaperScissors[otherPlayer.FightingStyle] == FightingStyle) {
            return otherPlayer;
        }
        else {
            return this;
        }
    }

    public override string ToString() {
        // return P,S,R for output
        return FightingStyle switch {
            FightingStyle.Rock => "R",
            FightingStyle.Paper => "P",
            FightingStyle.Scissors => "S",
            _ => ""
        };
    }
}


enum FightingStyle {
    Rock,
    Paper,
    Scissors
}

class Tournament {
    public List<Player> Players { get; set; }

    public Tournament(List<Player> players) {
        Players = players;
    }

    public List<Player> PlayRounds(int rounds) {
        // return winners remaining players

        var winners = new List<Player>();
        for (int i = 0; i < rounds; i++) {
            winners = PlayRound();
            Players = winners;
        }

        return Players;
    }

    private List<Player> PlayRound() {
        var winners = new List<Player>();
        for (int i = 0; i < Players.Count; i += 2) {
            var winner = Players[i].Fight(Players[i + 1]);
            if (winner != null) {
                winners.Add(winner);
            }
        }

        return winners;
    }

    public void ArrangePlayers() {
        // arrange players that after two rounds there are no rock fighters left in the tournament

        int rockPlayers() => Players.Count(p => p.FightingStyle == FightingStyle.Rock);
        int paperPlayers() => Players.Where(p => p.FightingStyle == FightingStyle.Paper).Count();
        int scissorsPlayers() => Players.Where(p => p.FightingStyle == FightingStyle.Scissors).Count();

        var arrangement = new StringBuilder();

        while (rockPlayers() + paperPlayers() + scissorsPlayers() > 0) {
            // arrange players according to pattern

            Execute();

            void Execute() {
                foreach (var p in Patterns) {
                    if (p.CanBuild(Players)) {
                        ApplyPattern(p);
                        return;
                    }

                    void ApplyPattern(Pattern pattern) {
                        arrangement.Append(string.Join("", pattern.P));
                        pattern.P.ForEach(c => Players.Remove(Players.First(p => p.ToString() == c.ToString())));

                        if (pattern.NextPattern != null) {
                            if (pattern.NextPattern.CanBuild(Players)) {
                                ApplyPattern(pattern.NextPattern);
                            }
                        }
                    }
                }
            }
        }
        
        Console.WriteLine(arrangement.ToString());
    }

    private static List<Pattern> Patterns = new List<Pattern>() {
        new Pattern("SS") {
            NextPattern = new Pattern("SP") {
            }
        },
        new Pattern("PP"),
        new Pattern("RR") {
            NextPattern = new Pattern("RP")
        },
        new Pattern("RP"),
        new Pattern("SP"),
        new Pattern("PP"),
        new Pattern("RS"),
    };
}

class Pattern {
    public List<char> P { get; set; }
    public Pattern NextPattern { get; set; }

    public Pattern(string s) {
        P = s.ToList();
    }

    public bool CanBuild(List<Player> players) {
        var playersChars = ToCharList(players);

        var intersectWithCounts = IntersectWithCounts(string.Join("", P), string.Join("", playersChars));
        
        return intersectWithCounts.Length == P.Count;
    }
    
    public static string IntersectWithCounts(string a, string b)
    {
        var aCounts = a.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        var bCounts = b.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

        var result = new StringBuilder();

        foreach (var pair in aCounts)
        {
            char character = pair.Key;
            int count = Math.Min(pair.Value, bCounts.GetValueOrDefault(character, 0));
            result.Append(new string(character, count));
        }

        return result.ToString();
    }

    public List<char> ToCharList(List<Player> players) {
        var list = new List<char>();
        foreach (var p in players) {
            list.Add(p.FightingStyle switch {
                FightingStyle.Rock => 'R',
                FightingStyle.Paper => 'P',
                FightingStyle.Scissors => 'S',
                _ => ' '
            });
        }

        return list;
    }

    public void Apply(StringBuilder arrangement, List<Player> players) {
        if (CanBuild(players)) {
        }
    }
}