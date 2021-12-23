namespace day21;

public class Universe {
    public bool p1 { get; set; }
    public int p1Score { get; set; }
    public int p1Pos { get; set; }
    public int p2Score { get; set; }
    public int p2Pos { get; set; }
    public bool winner { get; set; }

    public Universe(int p1Score, int p1Pos, int p2Score, int p2Pos, bool p1) {
        this.p1 = p1;
        this.p1Score = p1Score;
        this.p1Pos = p1Pos;
        this.p2Score = p2Score;
        this.p2Pos = p2Pos;
        winner = false;
    }
}