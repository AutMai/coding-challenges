using aocTools;

namespace aoc22.day6;

public class Day6 : AAocDay {

    public override void PuzzleOne() {
        var message = InputTokens.Read();
        PrintStartOfDistincSequence(message,4);
    }


    private void PrintStartOfDistincSequence(string message, int distinctAmount) {
        for (int i = 0; i <= message.Length - distinctAmount; i++) {
            // get message range from i to i+distinctAmount
            var range = message.Substring(i, distinctAmount);
            // check if range contains distinctAmount distinct characters
            if (range.Distinct().Count() == distinctAmount) {
                // print range
                Console.WriteLine(i+distinctAmount);
                break;
            }
        }
    }
    
    public override void PuzzleTwo() {
        ResetInput();
        var message = InputTokens.Read();
        
        PrintStartOfDistincSequence(message,14);
    }
}