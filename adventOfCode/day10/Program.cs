using day10;


var inputLines =
    File.ReadAllText(
            @"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\coding-challenges\adventOfCode\day10\input.txt")
        .Replace("\r", "")
        .Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();


var openBrackets = new List<char> { '{', '(', '[', '<' };
var closeBrackets = new List<char> { '}', ')', ']', '>' };


Dictionary<char, int> illegalBracketsValues = new Dictionary<char, int>()
    { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };

List<char> illegalBrackets = new List<char>();
List<Bracket> bracketLines = new List<Bracket>();

foreach (var inputLine in inputLines) {
    var currentBracket = new Bracket(' ', null);
    var rootBracket = currentBracket;

    foreach (var bracket in inputLine) {
        if (openBrackets.Contains(bracket))
            currentBracket.ChildrenBrackets.Add(currentBracket = new Bracket(bracket, currentBracket));
        else if (closeBrackets.Contains(bracket)) {
            currentBracket.CloseBracket = bracket;
            currentBracket = currentBracket.ParentBracket;
        }
        else {
            Console.WriteLine("ERROR");
        }
    }

    if (!CheckBrackets(rootBracket.ChildrenBrackets))
        bracketLines.Add(rootBracket);
}

List<string> missingBracketsList = new List<string>();

for (int i = 0; i < bracketLines.Count; i++) {
    FindMissingBrackets(bracketLines[i].ChildrenBrackets, i);
}

Dictionary<char, int> missingBracketsValues = new Dictionary<char, int>()
    { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 } };

List<long> missingBracketScores = new List<long>();

foreach (var missingBrackets in missingBracketsList) {
    long missingBracketScore = 0;
    foreach (var missingBracket in missingBrackets) {
        missingBracketScore *= 5;
        missingBracketScore += missingBracketsValues[missingBracket];
    }

    missingBracketScores.Add(missingBracketScore);
}

Console.WriteLine(GetMedian(missingBracketScores));


long GetMedian(List<long> list) {
    List<long> tempList = list;

    int count = tempList.Count;


    tempList.Sort();


    decimal medianValue = 0;


    if (count % 2 == 0) {
        // count is even, need to get the middle two elements, add them together, then divide by 2

        long middleElement1 = tempList[(count / 2) - 1];

        long middleElement2 = tempList[(count / 2)];

        medianValue = (middleElement1 + middleElement2) / 2;
    }

    else {
        // count is odd, simply get the middle element.

        medianValue = tempList[(count / 2)];
    }

    return (long)Math.Round(medianValue, 0);
}

bool FindMissingBrackets(List<Bracket> brackets, int index) {
    foreach (var bracket in brackets) {
        if (bracket.ChildrenBrackets.Count != 0) {
            var found = FindMissingBrackets(bracket.ChildrenBrackets, index);
            if (found)
                return true;
        }

        if (bracket.CloseBracket == '\0') {
            if (missingBracketsList.ElementAtOrDefault(index) != null) {
                missingBracketsList[index] += closeBrackets[openBrackets.FindIndex(c => c == bracket.OpenBracket)];
            }
            else {
                missingBracketsList.Add(closeBrackets[openBrackets.FindIndex(c => c == bracket.OpenBracket)]
                    .ToString());
            }
        }
    }

    return false;
}


bool CheckBrackets(List<Bracket> brackets) {
    foreach (var bracket in brackets) {
        if (bracket.ChildrenBrackets.Count != 0) {
            var found = CheckBrackets(bracket.ChildrenBrackets);
            if (found)
                return true;
        }

        if (IsIllegalBracket(bracket)) {
            illegalBrackets.Add(bracket.CloseBracket);
            bool found = true;
            return found;
        }
    }

    return false;
}

bool IsIllegalBracket(Bracket b) {
    if (b.CloseBracket == '\0')
        return false;
    if (closeBrackets[openBrackets.FindIndex(c => c == b.OpenBracket)] == b.CloseBracket)
        return false;
    return true;
}


/* PART 1
var inputLines =
    File.ReadAllText(
            @"C:\Users\Sebastian\OneDrive - Personal\OneDrive\coding\coding-challenges\adventOfCode\day10\input.txt")
        .Replace("\r", "")
        .Split('\n', StringSplitOptions.RemoveEmptyEntries);


var openBrackets = new List<char> { '{', '(', '[', '<' };
var closeBrackets = new List<char> { '}', ')', ']', '>' };


Dictionary<char, int> illegalBracketsValues = new Dictionary<char, int>()
    { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };

List<char> illegalBrackets = new List<char>();


foreach (var inputLine in inputLines) {
    var currentBracket = new Bracket(' ', null);
    var rootBracket = currentBracket;
    foreach (var bracket in inputLine) {
        if (openBrackets.Contains(bracket))
            currentBracket.ChildrenBrackets.Add(currentBracket = new Bracket(bracket, currentBracket));
        else if (closeBrackets.Contains(bracket)) {
            currentBracket.CloseBracket = bracket;
            currentBracket = currentBracket.ParentBracket;
        }
        else {
            Console.WriteLine("ERROR");
        }
    }

    CheckBrackets(rootBracket.ChildrenBrackets);
}

int syntaxErrorScore = 0;

illegalBrackets.ForEach(b => syntaxErrorScore += illegalBracketsValues[b]);

Console.WriteLine(syntaxErrorScore);


bool CheckBrackets(List<Bracket> brackets) {
    foreach (var bracket in brackets) {
        if (bracket.ChildrenBrackets.Count != 0) {
            var found = CheckBrackets(bracket.ChildrenBrackets);
            if (found)
                return true;
        }

        if (IsIllegalBracket(bracket)) {
            illegalBrackets.Add(bracket.CloseBracket);
            bool found = true;
            return found;
        }
    }

    return false;
}

bool IsIllegalBracket(Bracket b) {
    if (b.CloseBracket == '\0')
        return false;
    if (closeBrackets[openBrackets.FindIndex(c => c == b.OpenBracket)] == b.CloseBracket)
        return false;
    return true;
}

*/