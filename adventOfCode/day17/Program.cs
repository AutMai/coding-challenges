using System.Text.RegularExpressions;
using aocTools;

var input = Helper.ReadFile("input.txt");

Console.WriteLine(GetMaxHeight());

double GetMaxHeight() {
    var yCoords = input.Split("y=")[1].Split("..");
    var lowerY = Convert.ToInt32(yCoords.Min());

    // ReSharper disable once PossibleLossOfFraction
    return (lowerY + 1) * lowerY / 2;
}

DetermineTargetArea();
var vList = new List<(int x, int y)>();

Console.WriteLine(TryVelocities());

int left = 0;
int right = 0;
int top = 0;
int bottom = 0;



void DetermineTargetArea() {
    Regex r = new Regex(@"-?\d+");
    var areaNums = Array.ConvertAll(r.Matches(input).Select(match => match.Value).ToArray(), int.Parse);
    left = areaNums[0];
    right = areaNums[1];
    bottom = areaNums[2];
    top = areaNums[3];
}


long TryVelocities() {
    var minXVelocity = (int)Math.Floor((1 + Math.Sqrt(1 + left * 8)) / 2);

    long hitCount = 0;
    for (int x = 0; x <= right; x++) {
        for (int y = bottom; y <= -bottom; y++) {
            if (Shoot(x, y)) hitCount++;
        }
    }

    return hitCount;
}

bool Shoot(int vx, int vy) {
    int orig_vx = vx;
    int orig_vy = vy;
    var x = 0;
    var y = 0;

    while (true) {
        x += vx;
        y += vy;

        if (vx > 0)
            vx--;
        else if (vx < 0)
            vx++;

        vy--;

        if (InTargetArea(x, y)) {
            vList.Add((orig_vx,orig_vy));
            return true;
        }

        if (ToFar(x, y)) return false;
    }
}

bool InTargetArea(int x, int y) => x >= left && x <= right && y >= bottom && y <= top;
bool ToFar(int x, int y) => y < bottom || x > right;