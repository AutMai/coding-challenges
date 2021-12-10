namespace day4;

public class BingoBoard {
    public BingoBoard(BingoNumber[,] bingoBoard) {
        this.bingoBoard = bingoBoard;
    }

    private BingoNumber[,] bingoBoard { get; set; }
}