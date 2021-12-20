// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using aocTools;
using day20;

var input = Helper.ReadFile("input.txt").Split("\n\n");;

var algorithm = input[0];

var inputImage = input[1];

Solver.ReadAlgorithm(algorithm);
Solver.ReadImageInput(inputImage);
Console.WriteLine(Solver.Algorithm);

for (int i = 0; i < 50; i++) {
    Solver.ConvertImage(i);
}
Console.WriteLine(Solver.CountLitPixels());