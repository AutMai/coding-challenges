using aocTools;

string input = Helper.ReadFile("input.txt");

var crabs1 = input.Split(',');


var crabs = Array.ConvertAll(crabs1, int.Parse);

Console.WriteLine(crabs.Average());

double goal = Math.Floor(crabs.Average());
double sum = 0;
foreach (var crab in crabs) {
    sum += (CalcFuelConsumption(Math.Abs(goal - crab)));
}

Console.WriteLine(sum);

int CalcFuelConsumption(double diff) {
    int sum = 0;
    for (int i = 1; i <= diff; i++) {
        sum += i;
    }
    return sum;
}


/*
int goal = GetMedian(crabs);
Console.WriteLine(goal);

goal = 362;

int sum = 0;
foreach (var crab in crabs) {
    sum += (Math.Abs(goal - crab));
}

Console.WriteLine(sum);
int GetMedian(int[] array) {
    int[] tempArray = array;

    int count = tempArray.Length;


    Array.Sort(tempArray);


    decimal medianValue = 0;


    if (count % 2 == 0) {
        // count is even, need to get the middle two elements, add them together, then divide by 2

        int middleElement1 = tempArray[(count / 2) - 1];

        int middleElement2 = tempArray[(count / 2)];

        medianValue = (middleElement1 + middleElement2) / 2;
    }

    else {
        // count is odd, simply get the middle element.

        medianValue = tempArray[(count / 2)];
    }

    return (int)Math.Round(medianValue,0);
}*/