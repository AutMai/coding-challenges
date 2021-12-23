using aocTools;
using day21;

// Player 1 starting position: 8
// Player 2 starting position: 6


Dictionary<int, ulong> PossibleRollOutComes = new Dictionary<int, ulong>();
ulong player1Victories = 0;
ulong player2Victories = 0;

Solve();

void Solve() {
    Part2();
}

void Part2() {
    for (int x = 1; x <= 3; x++) {
        for (int y = 1; y <= 3; y++) {
            for (int z = 1; z <= 3; z++) {
                int total = x + y + z;
                if (!PossibleRollOutComes.ContainsKey(total)) {
                    PossibleRollOutComes.Add(total, 1);
                }
                else {
                    PossibleRollOutComes[total] += 1;
                }
            }
        }
    }


    RollDiracDie(0, 0, 8, 6, 1, 1);

    if (player1Victories > player2Victories) {
        Console.WriteLine(player1Victories);
    }
    else {
        Console.WriteLine(player2Victories);
    }
}

 void RollDiracDie(int player1Points, int player2Points, int player1Pos, int player2Pos, int playerTurn,
    ulong universes) {
    if (player1Points > 21 || player2Points > 21) {
        return;
    }

    if (playerTurn == 1) {
        foreach (KeyValuePair<int, ulong> kvp in PossibleRollOutComes) {
            int pts = MoveSpaces(kvp.Key, player1Pos);
            if (player1Points + pts < 21) {
                RollDiracDie(player1Points + pts, player2Points, pts, player2Pos, 2, (kvp.Value * universes));
            }
            else {
                player1Victories += universes * kvp.Value;
            }
        }
    }
    else {
        foreach (KeyValuePair<int, ulong> kvp in PossibleRollOutComes) {
            int pts = MoveSpaces(kvp.Key, player2Pos);
            if (player2Points + pts < 21) {
                RollDiracDie(player1Points, player2Points + pts, player1Pos, pts, 1, (kvp.Value * universes));
            }
            else {
                player2Victories += universes * kvp.Value;
            }
        }
    }
}

 int MoveSpaces(int numSpaces, int currentSpace) {
    int spaceLandOn = 0;
    int toAdd = 0;
    if (numSpaces > 10) {
        toAdd = numSpaces % 10;
    }
    else {
        toAdd = numSpaces;
    }

    if (currentSpace + toAdd > 10) {
        spaceLandOn = (currentSpace + toAdd) % 10;
    }
    else {
        spaceLandOn = currentSpace + toAdd;
    }

    return spaceLandOn;
}