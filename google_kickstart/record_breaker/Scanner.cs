namespace record_breaker; 

public class Scanner {
    private int CurrentPos { get; set; }

    private readonly List<int> _input;

    public Scanner(string input) {
        _input = input.Split(new [] {' ', '\r', '\n'}).Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
        CurrentPos = 0;
    }
    public int NextInt() {
        return _input[CurrentPos++];
    }
}