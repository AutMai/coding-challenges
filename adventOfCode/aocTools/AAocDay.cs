namespace aocTools;

public abstract class AAocDay : IAocDay {
    protected TokenList InputTokens { get; set; }
    protected readonly TokenList OriginalInputTokens;
    public abstract void PuzzleOne();

    public abstract void PuzzleTwo();

    protected AAocDay(bool splitSpaces = false) {
        if (splitSpaces) {
            InputTokens = Helper.ReadFile(Path.Join(GetType().Name.ToLower(), "input.txt")).Split(' ', '\r', '\n')
                .ToList().ToTokenList();
        }
        else {
            InputTokens = Helper.ReadFile(Path.Join(GetType().Name.ToLower(), "input.txt")).Split('\r', '\n')
                .ToList().ToTokenList();
        }

        OriginalInputTokens = new TokenList(InputTokens);
    }
    
    protected void ResetInput() {
        InputTokens = new TokenList(OriginalInputTokens);
    }
}