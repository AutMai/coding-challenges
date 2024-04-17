using CodingHelper;

var r = new InputReader(4);


foreach (var l in r.GetInputs()) {
    l.SetOutput();
    var matchmaking = new Matchmaking();
    matchmaking.WinIncrement = l.ReadInt();
    matchmaking.LossDecrement = l.ReadInt();
    matchmaking.GameCount = l.ReadInt();
    matchmaking.PlayerCount = l.ReadInt();

    for (int i = 0; i < matchmaking.GameCount; i++) {
        matchmaking.Matches.Add(new Match());
        matchmaking.Matches[i].Player1 = new Match.Player();
        matchmaking.Matches[i].Player2 = new Match.Player();
        matchmaking.Matches[i].Player1.Id = l.ReadInt();
        matchmaking.Matches[i].Player1.Score = l.ReadInt();
        matchmaking.Matches[i].Player2.Id = l.ReadInt();
        matchmaking.Matches[i].Player2.Score = l.ReadInt();
    }
    
    var playerWins = new Dictionary<int, int>();
    
    // populate the dictionary with all players and 0 wins
    var playerIds = matchmaking.Matches
        .SelectMany(m => new[] { m.Player1, m.Player2 })
        .Select(p => p.Id)
        .Distinct()
        .OrderBy(p => p);
    
    // add all players to the dictionary
    foreach (var playerId in playerIds) {
        playerWins.Add(playerId, 0);
    }
    
    foreach (var match in matchmaking.Matches) {
        // if player1 has a higher score than player2, player1 wins
        if (match.Player1.Score > match.Player2.Score) {
            // increment the win count of player1 by winIncrement
            playerWins[match.Player1.Id] += matchmaking.WinIncrement;
            // decrement the win count of player2 by lossDecrement
            playerWins[match.Player2.Id] -= matchmaking.LossDecrement;
        }
        else {
            // increment the win count of player2 by winIncrement
            playerWins[match.Player2.Id] += matchmaking.WinIncrement;
            // decrement the win count of player1 by lossDecrement
            playerWins[match.Player1.Id] -= matchmaking.LossDecrement;
        }
    }
    
    playerWins = playerWins.OrderByDescending(p => p.Value).ThenBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
    
    Console.WriteLine(string.Join("\n", playerWins.Select(p => $"{p.Key} {p.Value}")));
}

class Matchmaking {
    public int GameCount { get; set; }
    public int PlayerCount { get; set; }
    
    public int WinIncrement { get; set; }
    
    public int LossDecrement { get; set; }

    public List<Match> Matches { get; set; } = new List<Match>();
}

class Match {
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }

    internal class Player {
        public int Id { get; set; }
        public int Score { get; set; }
    }
}