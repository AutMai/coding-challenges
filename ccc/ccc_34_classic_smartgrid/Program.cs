using System.Text;
using CodingHelper;

#region Level1

/*
for (int i = 1; i <= 5; i++)
{
    var input = File.ReadAllText($"../../../files/level1/level1_{i}.in").Replace("\r", "").Split('\n').ToList();
    input.RemoveAt(0);
    input.RemoveAt(input.Count-1);

    var minPrice = input.Min();
    var minute = input.IndexOf(minPrice);
    
    File.WriteAllText($"../../../files/level1/level1_{i}.out", minute.ToString());
}
*/

#endregion

#region Level2
/*
for (int l = 1; l <= 5; l++) {
    var output = new StringBuilder();

    var input = File.ReadAllText($"../../../files/level2/level2_{l}.in").Replace("\r", "").Split('\n').ToList();
    var minutesCount = input.TakeAndRemove(1).First().ToInt();
    var powerPrices = input.TakeAndRemove(minutesCount).ToInt();

    int tasksCount;
    output.Append(tasksCount = input.TakeAndRemove(1).First().ToInt());
    var tasks = input.TakeAndRemove(tasksCount);

    for (int t = 1; t <= tasks.Count; t++) {
        var taskLength = tasks[t-1].Split(' ')[1].ToInt();
        var minTotalPowerPrice = int.MaxValue;
        var startMin = 0;
        
        for (int pp = 0; pp <= minutesCount - taskLength; pp++) {
            if (powerPrices.Skip(pp).Take(taskLength).Sum() < minTotalPowerPrice) {
                startMin = pp;
                minTotalPowerPrice = powerPrices.Skip(pp).Take(taskLength).Sum();
            }
        }

        output.Append($"\n{t} {startMin}");
    }

    File.WriteAllText($"../../../files/level2/level2_{l}.out", output.ToString());
}
*/
#endregion


#region Level3

for (int l = 1; l <= 5; l++) {  
    var output = new StringBuilder();

    var input = File.ReadAllText($"../../../files/level3/level3_{l}.in").Replace("\r", "").Split('\n').ToList();
    var minutesCount = input.TakeAndRemove(1).First().ToInt();
    var powerPrices = input.TakeAndRemove(minutesCount).ToInt();

    int tasksCount;
    output.Append(tasksCount = input.TakeAndRemove(1).First().ToInt());
    var tasks = input.TakeAndRemove(tasksCount);

    for (int t = 0; t < tasks.Count; t++) {
        var taskPower = tasks[t].Split(' ')[1].ToInt();
        var taskStart = tasks[t].Split(' ')[2].ToInt();
        var taskEnd = tasks[t].Split(' ')[3].ToInt();

        var powerMinute = powerPrices.IndexOf(powerPrices.GetRange(taskStart, taskEnd - taskStart + 1).Min());

        output.Append($"\n{t} {powerMinute} {taskPower}");
    }

    File.WriteAllText($"../../../files/level3/level3_{l}.out", output.ToString());
}

#endregion