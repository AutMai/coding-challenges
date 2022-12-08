using aocTools;

namespace aoc22.day8;

public class Day8 : AAocDay {
    public static Tree[,] _trees;
    public override void PuzzleOne() {
        ReadTrees();
        PrintTrees();
        // get amount of visible trees
        int visibleTrees = 0;
        for (int y = 0; y < _trees.GetLength(1); y++) {
            for (int x = 0; x < _trees.GetLength(0); x++) {
                if (_trees[x, y].IsVisible()) {
                    visibleTrees++;
                }
            }
        }
        Console.WriteLine("Visible trees: " + visibleTrees);
    }

    private void ReadTrees() {
        _trees = new Tree[InputTokens.JustRead().Length, InputTokens.Count];

        for (int y = 0; y < OriginalInputTokens.Count; y++) {
            var line = InputTokens.Read();
            for (int x = 0; x < line.Length; x++) {
                _trees[x, y] = new Tree(line[x].ToInt(), x, y);
            }
        }
    }
    
    private void PrintTrees() {
        for (int y = 0; y < _trees.GetLength(1); y++) {
            for (int x = 0; x < _trees.GetLength(0); x++) {
                var tree = _trees[x, y];
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = tree.IsVisible() ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write(tree);
            }
            Console.WriteLine();
        }
        Console.BackgroundColor = ConsoleColor.Black;
    }
    
    public override void PuzzleTwo() {
        var scenicScore = 0;
        var bestTree = _trees[0, 0];
        for (int y = 0; y < _trees.GetLength(1); y++) {
            for (int x = 0; x < _trees.GetLength(0); x++) {
                if (_trees[x, y].GetScenicScore() > scenicScore) {
                    scenicScore = _trees[x, y].GetScenicScore();
                    bestTree = _trees[x, y];
                }
            }
        }
        
        Console.WriteLine("Scenic score: " + scenicScore);
        Console.WriteLine($"Best tree: X: {bestTree.X} | Y: {bestTree.Y}");
    }
}

public class Tree {
    public int Height { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Tree(int height, int x, int y) {
        Height = height;
        X = x;
        Y = y;
    }
    
    public override string ToString() {
        return Height.ToString();
    }
    
    public Tree Left => Day8._trees[X - 1, Y];
    public Tree Right => Day8._trees[X + 1, Y];
    public Tree Top => Day8._trees[X, Y - 1];
    public Tree Bottom => Day8._trees[X, Y + 1];

    private bool IsOnEdge() => X == 0 || X == Day8._trees.GetLength(0) - 1 || Y == 0 || Y == Day8._trees.GetLength(1) - 1;

    public int HighestRight() => Right.HighestRightR();
    public int HighestLeft() => Left.HighestLeftR();
    public int HighestTop() => Top.HighestTopR();
    public int HighestBottom() => Bottom.HighestBottomR();
    

    private int HighestRightR() => X == Day8._trees.GetLength(0) - 1 ? Height : Math.Max(Height, Right.HighestRightR());

    private int HighestLeftR() => X == 0 ? Height : Math.Max(Height, Left.HighestLeftR());

    private int HighestTopR() => Y == 0 ? Height : Math.Max(Height, Top.HighestTopR());

    private int HighestBottomR() => Y == Day8._trees.GetLength(1) - 1 ? Height : Math.Max(Height, Bottom.HighestBottomR());

    public bool IsVisible() {
        if (IsOnEdge()) return true;
        return Height > HighestLeft() || Height > HighestRight() || Height > HighestTop() || Height > HighestBottom();
    }

    public int GetScenicScore() {
        // multiply all viewingDistances
        return GetLeftViewingDistance() * GetRightViewingDistance() * GetTopViewingDistance() * GetBottomViewingDistance();
    }
    
    public int GetLeftViewingDistance() {
        var originalTree = this;
        var tree = this;
        int distance = 0;
        do {
            if (tree.X == 0) return distance;
            tree = tree.Left;
            distance++;
        } while (originalTree.Height > tree.Height);

        return distance;
    }
    
    public int GetRightViewingDistance() {
        var originalTree = this;
        var tree = this;
        int distance = 0;
        do {
            if (tree.X == Day8._trees.GetLength(0) - 1) return distance;
            tree = tree.Right;
            distance++;
        } while (originalTree.Height > tree.Height);

        return distance;
    }
    
    public int GetTopViewingDistance() {
        var originalTree = this;
        var tree = this;
        int distance = 0;
        do {
            if (tree.Y == 0) return distance;
            tree = tree.Top;
            distance++;
        } while (originalTree.Height > tree.Height);

        return distance;
    }
    
    public int GetBottomViewingDistance() {
        var originalTree = this;
        var tree = this;
        int distance = 0;
        do {
            if (tree.Y == Day8._trees.GetLength(1) - 1) return distance;
            tree = tree.Bottom;
            distance++;
        } while (originalTree.Height > tree.Height);

        return distance;
    }
}

