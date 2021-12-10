using System;
using System.Collections.Generic;

namespace silly_substitutions {
    class Program {
        static List<Case> cases = new List<Case>();

        static void Main(string[] args) {
            GenerateCase();

            EvaluateCases();
        }


        static void GenerateCase() {
            int testCases = Convert.ToInt32(Console.ReadLine());

            for (int i = 2; i < testCases * 2 + 1; i += 2) {
                string digitsLength = Console.ReadLine();
                string digits = Console.ReadLine();

                cases.Add(new Case(i / 2, digits));
            }
        }

        static void EvaluateCases() {
            foreach (var testcase in cases) {
                bool running = true;
                while (running) {
                    int nothingChanged = 0;
                    for (int i = 0; i < 10; i++) {
                        string oldValue = i.ToString() + (i + 1).ToString();
                        if (i == 9)
                            oldValue = "90";

                        int temp = oldValue[1] - '0';
                        string newValue = (temp + 1).ToString();
                        if (temp == 9)
                            newValue = "0";
                        if (testcase.Digits == testcase.Digits.Replace(oldValue, newValue)) {
                            nothingChanged++;
                            if (nothingChanged >= 10) {
                                running = false;
                            }
                        }
                        testcase.Digits = testcase.Digits.Replace(oldValue, newValue);
                    }
                }

                Console.WriteLine($"Case #{testcase.Index}: {testcase.Digits}");
            }
        }
    }

    public class Case {
        public int Index { get; set; }
        public string Digits { get; set; }

        public Case(int index, string digits) {
            Index = index;
            Digits = digits;
        }
    }
}