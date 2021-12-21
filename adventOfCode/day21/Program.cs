using aocTools;

// Player 1 starting position: 8
// Player 2 starting position: 6

var p1Score = 0;
var p2Score = 0;

var p1Pos = 8;
var p2Pos = 6;

List<int> gameBoard = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var diceSide = 0;
int diceRolls = 0;
bool p1 = true;
while (true) {
    diceRolls += 3;

    var roll = Roll() + Roll() + Roll();

    MovePlayer(roll);
    p1 = !p1;
    if (p1Score >= 1000) {
        Console.WriteLine((p2Score * diceRolls));
        return;
    }

    if (p2Score >= 1000) {
        Console.WriteLine((p1Score * diceRolls));
        return;
    }
}


int Roll() {
    if (diceSide != 100)
        diceSide++;
    else
        diceSide = 1;

    return diceSide;
}

void MovePlayer(int steps) {
    if (p1) {
        if (((p1Pos + steps) % 10) == 0) {
            p1Pos = 10;
            p1Score += 10;
        }
        else {
            p1Score += (p1Pos = ((p1Pos + steps) % 10));
        }
    }
    else {
        if (((p2Pos + steps) % 10) == 0) {
            p2Pos = 10;
            p2Score += 10;
        }
        else {
            p2Score += (p2Pos = ((p2Pos + steps) % 10));
        }
    }
}