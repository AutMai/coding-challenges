using day13;

var input = File
    .ReadAllText(
        @"C:\Users\Sebastian\OneDrive\coding\coding-challenges\adventOfCode\day13\input.txt")
    .Replace("\r", "");

var foldInstructionList = input.Split("\n\n")[1].Replace("fold along ", "").Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

var dotCoords = input.Split("\n\n")[0].Split('\n');

TransparentPaper.DrawPaper(dotCoords);
//TransparentPaper.PrintPaper();

foreach (var foldInstr in foldInstructionList) {
    var instr = foldInstr.Split('=');
    if (instr[0] == "y")
        TransparentPaper.FoldPaperH(Convert.ToInt32(instr[1]));
    else
        TransparentPaper.FoldPaperV(Convert.ToInt32(instr[1]));
}

TransparentPaper.PrintPaper();
Console.WriteLine(TransparentPaper.CountDots());

Console.WriteLine();