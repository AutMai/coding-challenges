using System;
using System.Collections.Generic;
using System.Linq;

namespace TransformTheString {
    class Program {
        static List<Case> cases = new List<Case>();

        static void Main(string[] args) {
            GenerateCase();
            EvaluateCases();
        }

        static void EvaluateCases() {
            foreach (var @case in cases) {
                int totalChangesCount = 0;
                foreach (var letter in @case.Padlock) {
                    int smallestDiff = int.MaxValue;
                    if (!@case.PadlockAllowedLetters.Contains(letter)) { // letter not allowed
                        foreach (var allowedLetter in @case.PadlockAllowedLetters) {
                            if (GetLettersDiff(letter, allowedLetter) < smallestDiff)
                                smallestDiff = GetLettersDiff(letter, allowedLetter);
                        }
                        totalChangesCount += smallestDiff;
                    }
                }

                Console.WriteLine($"Case #{@case.Index}: {totalChangesCount}");
            }
            
        }

        static int GetLettersDiff(char l1, char l2) {
            int diff = Math.Abs(l1 - l2); // negative 25
            if (diff > 13) {
                diff = 26 - diff;
            }

            return diff;
        }

        static void GenerateCase() {
            int testCases = Convert.ToInt32(Console.ReadLine());

            for (int i = 2; i < testCases * 2 + 1; i += 2) {
                var padlock = Console.ReadLine();

                var padlockLetters = Console.ReadLine();

                cases.Add(new Case(i / 2, padlock, padlockLetters));
            }
        }
    }

    public class Case {
        public int Index { get; set; }
        public string Padlock { get; set; }
        public string PadlockAllowedLetters { get; set; }

        public Case(int index, string padlock, string padlockAllowedLetters) {
            Index = index;
            Padlock = padlock;
            PadlockAllowedLetters = padlockAllowedLetters;
        }
    }
}