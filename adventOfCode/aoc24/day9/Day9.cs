using System.Text;
using aocTools;

namespace aoc24.day9;

public class Day9 : AAocDay {
    private List<long> _diskContent = [];

    public Day9() : base() {
        ReadInput();
    }

    private void PrintDiskContent() {
        var sb = new StringBuilder();
        foreach (var num in _diskContent) {
            // if -1, print .
            if (num == -1) {
                sb.Append(".");
            }
            else {
                sb.Append(num);
            }
        }

        Console.WriteLine(sb.ToString());
    }
    
    private void ReadInput() {
        _diskContent.Clear();
        var line = InputTokens.Read();

        var id = 0;
        for (var i = 0; i < line.Length; i++) {
            var num = long.Parse(line[i].ToString());
            // if odd index
            if (i % 2 == 0) {
                // append the id as many times as the number to the disk content
                for (var j = 0; j < num; j++) {
                    _diskContent.Add(id);
                }

                id++;
            }
            else {
                // append -1 as many times as the number to the disk content
                for (var j = 0; j < num; j++) {
                    _diskContent.Add(-1);
                }
            }
        }
    }

    public override void PuzzleOne() {
        CompressDiskContent();
        Console.WriteLine(CalculateCheckSum());
    }

    private void CompressDiskContent() {
        // take numbers from back and insert them in the front instead of first -1 found
        for (var i = _diskContent.Count - 1; i >= 0; i--) {
            if (_diskContent[i] == -1) {
                continue;
            }

            // else insert the current number at first -1
            var replaceNum = _diskContent.FindIndex(x => x == -1);

            if (replaceNum > i) {
                break;
            }

            _diskContent[replaceNum] = _diskContent[i];
            _diskContent[i] = -1;
        }
    }

    private long CalculateCheckSum() {
        // calculate the checksum by multiplying the number by the index and summing all up.
        // -1 is not counted
        long sum = 0;
        for (var i = 0; i < _diskContent.Count; i++) {
            if (_diskContent[i] == -1) {
                continue;
            }

            sum += _diskContent[i] * i;
        }

        return sum;
    }

    public override void PuzzleTwo() {
        ResetInput();
        ReadInput();

        CompressDiskContent2();
        Console.WriteLine(CalculateCheckSum());
    }

    private void CompressDiskContent2() {
        // take same number blocks from back and try to fit them in the front

        var highestId = _diskContent.Max();

        for (var i = highestId; i >= 0; i--) {
            Console.WriteLine("Countdown: " + i);
            var amount = _diskContent.Count(x => x == i);
            var firstIndexOfId = _diskContent.FindIndex(x => x == i);
            
            // check from the front if there is a sequence of amount times -1 (ex amount = 3, -1, -1, -1)
            var sequence = Enumerable.Repeat(long.Parse("-1"), amount).ToList();
            var index = _diskContent.ContainsSequence(sequence);
            if (index == -1 || index > firstIndexOfId) continue;
            
            // set all the currentId to -1 (because we move them to the front)
            for (var j = 0; j < amount; j++) {
                _diskContent[_diskContent.FindIndex(x => x == i)] = -1;
            }
            
            for (var j = index; j < index + amount; j++) {
                _diskContent[j] = i;
            }
        }
    }
}