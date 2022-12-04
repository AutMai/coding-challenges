using aocTools;

namespace aoc22.day2;

public class Day2 : AAocDay {
    public Day2() {
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
    }

    public override void PuzzleOne() {
        var score = 0;
        while (InputTokens.HasMoreTokens()) {
            var game = InputTokens.Read();

            var myChoice = game.Split(" ")[1];
            score += GetResult(game.Split(" ")[0], myChoice);
            score += ShapeScore(myChoice);
        }

        Console.WriteLine(score);
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            var game = InputTokens.Read();
            GetResult(game.Split(" ")[0], game.Split(" ")[1]);
        }
    }

    public override void PuzzleTwo() {
        ResetInput();
        InputTokens = InputTokens.RemoveEmptyTokens().ToTokenList();
        var score = 0;
        while (InputTokens.HasMoreTokens()) {
            var game = InputTokens.Read();

            var gameResult = game.Split(" ")[1];
            var otherChoice = game.Split(" ")[0];
            var myMove = GetMove(otherChoice, gameResult);
            score += GetResult(otherChoice, myMove.ToString());
            score += ShapeScore(myMove.ToString());
        }

        Console.WriteLine(score);
    }

    private int GetResult(string otherChoice, string myChoice) {
        var my = GetRPS(myChoice);
        var other = GetRPS(otherChoice);
        if (my == other)
            return 3;

        switch (my) {
            case 'R' when other == 'S':
            case 'S' when other == 'P':
            case 'P' when other == 'R':
                return 6;
            default:
                return 0;
        }
    }

    private char GetMove(string otherChoice, string gameResult) {
        var other = GetRPS(otherChoice);
        char my = ' ';
        switch (gameResult) {
            case "X":
                my = _losingMove[other];
                break;
            case "Y":
                my = (char) (otherChoice[0]+23);
                break;
            case "Z":
                my = _winningMove[other];
                break;
        }

        return my;
    }

    private Dictionary<char, char> _losingMove = new Dictionary<char, char> {
        {'R', 'Z'},
        {'S', 'Y'},
        {'P', 'X'}
    };

    private Dictionary<char, char> _winningMove = new Dictionary<char, char>() {
        {'R', 'Y'},
        {'S', 'X'},
        {'P', 'Z'}
    };

    private char MoveToWin(char other) {
        switch (other) {
            case 'R':
                return 'P';
            case 'P':
                return 'S';
            case 'S':
                return 'R';
            default:
                return ' ';
        }
    }

    private char MoveToLose(char other) {
        switch (other) {
            case 'R':
                return 'S';
            case 'P':
                return 'S';
            case 'S':
                return 'R';
            default:
                return ' ';
        }
    }

    public char GetRPS(string input) {
        switch (input) {
            case "A":
            case "X":
                return 'R';
            case "B":
            case "Y":
                return 'P';
            case "C":
            case "Z":
                return 'S';
            default:
                return ' ';
        }
    }

    private int ShapeScore(string shape) {
        return shape switch {
            "X" => 1,
            "Y" => 2,
            "Z" => 3,
            _ => 0
        };
    }
}