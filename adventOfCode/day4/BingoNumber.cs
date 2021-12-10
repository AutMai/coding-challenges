namespace day4; 

public class BingoNumber {
    public int Number { get; set; }
    public bool Checked { get; set; }

    public BingoNumber(string number, bool @checked = false) {
        Number = Convert.ToInt32(number);
        Checked = @checked;
    }
}