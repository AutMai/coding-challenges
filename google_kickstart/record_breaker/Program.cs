using record_breaker;

int NumberOfRecordBreakingDays(int numberOfDays, List<int> visitors) {
    int recordBreakingDays = 0;

    for (var i = 0; i < numberOfDays; i++) {
        var currentMax = 0;
        try {
            currentMax = visitors.Take(i).Max();
        }
        catch (Exception e) {
            // ignored
        }

        if (i == 0 || visitors[i] > currentMax) {
            if (i == numberOfDays - 1 || visitors[i] > visitors[i + 1]) {
                recordBreakingDays++;
            }
        }
    }

    return recordBreakingDays;
}

Scanner scanner = new Scanner(File.ReadAllText("../../../input.txt"));
// Get number of test cases in input
int testCaseCount = scanner.NextInt();
// Iterate through test cases
for (int tc = 1; tc <= testCaseCount; ++tc) {
    // Get number of days for this test case
    int numberOfDays = scanner.NextInt();
    // Get attendance for each day
    int[] visitorsPerDay = new int[numberOfDays];
    for (int d = 0; d < numberOfDays; ++d) {
        visitorsPerDay[d] = scanner.NextInt();
    }

    // Print results
    int answer = NumberOfRecordBreakingDays(numberOfDays, visitorsPerDay.ToList());
    Console.WriteLine("Case #" + tc + ": " + answer);
}