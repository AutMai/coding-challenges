using System.Threading.Channels;
using aocTools;

string input = Helper.ReadFile("input.txt");
var inputAr = input.Split("\n");
inputAr = inputAr.SkipLast(1).ToArray();

List<Tuple<string, int>> commands = new List<Tuple<string, int>>();

foreach (var row in inputAr) {
    var rowAr = row.Split(' ');
    commands.Add(new Tuple<string, int>(rowAr[0], Convert.ToInt32(rowAr[1])));
}

int depth = 0;
int distance = 0;
int aim = 0;

foreach (var command in commands) {
    switch (command.Item1) {
        case "forward":
            distance+= command.Item2;
            depth += (aim * command.Item2);
            break;
        case "up":
            aim -= command.Item2;
            break;
        case "down":
            aim += command.Item2;
            break;
        default:
            Console.WriteLine("Err");
            break;
    }
}

Console.WriteLine(depth*distance);