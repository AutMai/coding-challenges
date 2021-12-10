using System.Runtime.CompilerServices;
using day4;

List<BingoNumber[,]> bingoBoardList = new List<BingoNumber[,]>();


string input =
    File.ReadAllText(@"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\adventOfCode\day4\input.txt")[..^2];

var bingoInput = input.Split("\n")[0].Split(',');
var bingoInputInt = Array.ConvertAll(bingoInput, Convert.ToInt32);

var bingoBoardsAr = input.Split("\n\n").Skip(1).ToArray();


foreach (var bingoBoard in bingoBoardsAr) {
    BingoNumber[,] bingoBoardFinal = new BingoNumber[5, 5];

    var boardAr = bingoBoard.Replace("  ", " ").Split(' ', '\n').Where(s => s != "").ToArray();
    int counter = 0;
    for (int x = 0; x < 5; x++) {
        for (int y = 0; y < 5; y++) {
            bingoBoardFinal[x, y] = new BingoNumber(boardAr[counter]);
            counter++;
        }
    }

    bingoBoardList.Add(bingoBoardFinal);
}


foreach (var bingoDraw in bingoInputInt) {
    for (int i = 0; i < bingoBoardList.Count; i++) {
        for (int y = 0; y < 5; y++) {
            for (int x = 0; x < 5; x++) {
                if (bingoBoardList[i][y, x].Number == bingoDraw)
                    bingoBoardList[i][y, x].Checked = true;

                if (CheckWin(bingoBoardList[i])) {
                    if (bingoBoardList.Count == 1)
                        GetBoardScore(bingoBoardList.First(), bingoDraw);
                    bingoBoardList.RemoveAt(i);
                    y = 6;
                    x = 6;
                    i--;
                }
            }
        }
    }
}


bool CheckWin(BingoNumber[,] bingoBoard) {
    for (int y = 0; y < 5; y++) {
        bool isHorWin = true;

        for (int x = 0; x < 5; x++) {
            if (bingoBoard[y, x].Checked == false) {
                isHorWin = false;
            }
        }

        if (isHorWin) {
            return true;
        }
    }


    for (int x = 0; x < 5; x++) {
        bool isVerWin = true;

        for (int y = 0; y < 5; y++) {
            if (bingoBoard[y, x].Checked == false) {
                isVerWin = false;
            }
        }

        if (isVerWin) {
            return true;
        }
    }

    return false;
}

void GetBoardScore(BingoNumber[,] bingoBoard, int winDraw) {
    var unmarkedNums = bingoBoard.Cast<BingoNumber>().Where(field => field.Checked == false).ToArray();
    var score = unmarkedNums.Sum(field => field.Number) * winDraw;
    Console.WriteLine("FINAL: " + score);

    //Environment.Exit(0);
}