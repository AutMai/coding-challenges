namespace aocTools;

public abstract class AAocDay : IAocDay {
    protected TokenList InputTokens { get; set; }
    public abstract void PuzzleOne();

    public abstract void PuzzleTwo();

    protected AAocDay() {
        InputTokens = Helper.ReadLines(Path.Join(GetType().Name.ToLower(), "input.txt")).ToTokenList();
        Console.WriteLine();
    }
}