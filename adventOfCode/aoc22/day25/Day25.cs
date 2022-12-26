using System.Collections;
using System.Collections.Concurrent;
using System.Numerics;
using System.Text.RegularExpressions;
using aocTools;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.CameraMode;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.Color;

namespace aoc22.day25;

public class Day25 : AAocDay {
    public List<SnafuNumber> SnafuNumbers = new List<SnafuNumber>();

    public override void PuzzleOne() {
        ReadInput();
        long sum = SnafuNumbers.Sum(sn => sn.ToDecimal());
        Console.WriteLine(sum);

        Console.WriteLine(SnafuNumber.DecimalToSnafu(sum));
    }

    public override void PuzzleTwo() {
        Console.WriteLine();
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            SnafuNumbers.Add(new SnafuNumber(InputTokens.Read()));
        }
    }
}

public class SnafuNumber {
    public string Number { get; set; }

    public SnafuNumber(string number) {
        Number = number;
    }

    public static string DecimalToSnafu(long d) {
        if (d == 0) return "";

        var quotient = (d + 2) / 5;
        int remainder = (int) ((d + 2) % 5);
        return DecimalToSnafu(quotient) + "=-012"[remainder];
    }

    public string Add(SnafuNumber other) {
        // add snafu numbers
        var sum = ToDecimal() + other.ToDecimal();
        return DecimalToSnafu(sum);
    }

    public long ToDecimal() {
        var numberArray = new long[Number.Length];

        for (var i = 0; i < Number.Length; i++) {
            numberArray[i] = Number[i] switch {
                '=' => -2,
                '-' => -1,
                _ => long.Parse(Number[i].ToString())
            };
        }


        long result = 0;
        long power = 0;
        for (long i = numberArray.Length - 1; i >= 0; i--) {
            var digit = numberArray[i];

            long value = (long) System.Math.Pow(5, power);
            result += digit * value;

            power++;
        }

        return result;
    }
}