using System.Threading.Channels;
using aocTools;

string input = Helper.ReadFile("input.txt");
var binaryNums = input.Split("\n");
binaryNums = binaryNums.SkipLast(1).ToArray();

string co2 = "";
string oxygen = "";

int numLength = binaryNums[0].Length;

List<string> binaryNumsNew = binaryNums.ToList();

for (int i = 0; i < numLength; i++) {
    int zeroCount = 0;
    int oneCount = 0;
    foreach (var binaryNum in binaryNumsNew) {
        if (binaryNum[i] == '1')
            oneCount++;
        else
            zeroCount++;
    }

    if (binaryNumsNew.Count == 1) break;


    binaryNumsNew = zeroCount > oneCount
        ? binaryNumsNew.Where(s => s[i] == '0').ToList()
        : binaryNumsNew.Where(s => s[i] == '1').ToList();
}

oxygen = binaryNumsNew[0];


binaryNumsNew = binaryNums.ToList();

for (int i = 0; i < numLength; i++) {
    int zeroCount = 0;
    int oneCount = 0;
    foreach (var binaryNum in binaryNumsNew) {
        if (binaryNum[i] == '1')
            oneCount++;
        else
            zeroCount++;
    }

    if (binaryNumsNew.Count == 1) break;


    binaryNumsNew = zeroCount > oneCount
        ? binaryNumsNew.Where(s => s[i] == '1').ToList()
        : binaryNumsNew.Where(s => s[i] == '0').ToList();
}

co2 = binaryNumsNew[0];

Console.WriteLine(Convert.ToInt32(oxygen, 2) * Convert.ToInt32(co2, 2));


/* PART 1

int numLength = binaryNums[0].Length;
string gamma = "";


for (int i = 0; i < numLength; i++) {
    int zeroCount = 0;
    int oneCount = 0;
    foreach (var binaryNum in binaryNums) {
        if (binaryNum[i] == '1')
            oneCount++;
        else
            zeroCount++;
    }

    if (zeroCount > oneCount) {
        gamma += "0";
    }
    else {
        gamma += "1";
    }
}



var epsilon = gamma.Replace("1", "2").Replace("0", "1").Replace("2", "0");





var gammaDec = Convert.ToInt64(gamma, 2);
var epsilonDec = Convert.ToInt64(epsilon, 2);


Console.WriteLine(gammaDec*epsilonDec);

*/