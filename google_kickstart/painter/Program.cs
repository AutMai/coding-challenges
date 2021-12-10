using System;
using System.Collections.Generic;
using System.Linq;

namespace painter {
    class Program {
        static List<Case> cases = new List<Case>();
        //test
        static Dictionary<char, string> colorMapping = new Dictionary<char, string>() {
            {'A', "RBY"},
            {'O', "RY"},
            {'G', "YB"},
            {'P', "RB"},
        };

        private static string allColors = "RBY";

        static void Main(string[] args) {
            GenerateCase();
            EvaluateCases();
            Console.WriteLine("");
        }


        static void GenerateCase() {
            int testCases = Convert.ToInt32(Console.ReadLine());

            for (int i = 2; i < testCases * 2 + 1; i += 2) {
                string paintLineLength = Console.ReadLine();
                string paintLine = Console.ReadLine();

                cases.Add(new Case(i / 2, paintLine));
            }
        }

        static void EvaluateCases() {
            foreach (var @case in cases) {
                int caseSteps = 0;
                foreach (var paintColor in allColors) {
                    caseSteps += MinNumForColorX(paintColor, @case.Colors);
                }

                Console.WriteLine(@case.Index + ": " + caseSteps);
            }
        }

        static int MinNumForColorX(char paintColor, string caseColors) {
            int minSteps = 0;
            int lastStrokePos = int.MinValue;

            for (int i = 0; i < caseColors.Length; i++) {
                char wantColor = caseColors[i];
                if (wantColor == 'U')
                    continue;
                
                if (wantColor == paintColor || ColorIsNeeded(paintColor, wantColor)) {
                    if (lastStrokePos != i - 1)
                        minSteps++;
                    lastStrokePos = i;
                }
            }       

            return minSteps;
        }

        static bool ColorIsNeeded(char color, char resultColor) {
            var colorMap = colorMapping.SingleOrDefault(pair => pair.Key == resultColor);
            if (colorMap.Value == null) {
                return false;
            }

            return colorMap.Value.Contains(color);
        }
    }

    public class Case {
        public int Index { get; set; }
        public string Colors { get; set; }

        public Case(int index, string colors) {
            Index = index;
            Colors = colors;
        }
    }
}