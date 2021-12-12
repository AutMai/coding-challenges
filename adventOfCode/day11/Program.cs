using day11;

var inputLines = File
    .ReadAllText(
        @"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\coding-challenges\adventOfCode\day11\input.txt")
    .Replace("\r", "")
    .Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

OctopusField.CreateField(inputLines);

OctopusField.PrintField();

var allFlashStepNr = -1;


int stepNr = 1;
while (allFlashStepNr == -1) {
    OctopusField.IncreaseEnergyLevel();

    if (OctopusField.TryFlash()) {
        allFlashStepNr = stepNr;
    }

    stepNr++;
}

/* PART 1
for (int i = 1; i <= 200; i++) {
    OctopusField.IncreaseEnergyLevel();

    OctopusField.TryFlash();

    //OctopusField.PrintField();
}*/

OctopusField.PrintField();


Console.WriteLine("FlashCount: " + OctopusField.FlashCount);
Console.WriteLine("AllFlashed: " + allFlashStepNr);