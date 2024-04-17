using System;
using CodingHelper;

var r = new InputReader(6);

foreach (var level in r.GetInputs()) {
    level.SetOutputToFile();
    var maxPowerPerMinute = level.Read().ToInt32();
    var maxBill = level.Read().ToInt64();
    var maxTasksPerMinute = level.Read().ToInt64();
    var powerPriceAmount = level.Read().ToInt64();
    var powerPrices = new List<PowerPrice>();
    var tasks = new List<PowerTask>();

    for (int i = 0; i < powerPriceAmount; i++) {
        var p = level.Read();
        powerPrices.Add(new PowerPrice(p.ToInt64(), maxPowerPerMinute, i, maxTasksPerMinute));
    }

    var taskAmount = level.ReadInt();

    for (long i = 0; i < taskAmount; i++) {
        var tid = level.Read().ToInt64();
        var tpow = level.Read().ToInt32();
        var tstart = level.Read().ToInt32();
        var tend = level.Read().ToInt32();

        tasks.Add(new PowerTask(tid, tpow, tstart, tend, powerPriceAmount));
    }

    foreach (var task in tasks.OrderBy(t => t.IntervalLength)) {
        var powerRange = powerPrices.GetRange(task.StartInterval, task.EndInterval - task.StartInterval + 1);

        while (task.Power > 0) {
            var minPricePower = powerRange.Where(p => p.PowerUsagesLeft > 0 && p.TaskUsagesLeft > 0).MinBy(p => p.Price); // get minimum which has at least one usage
            minPricePower.TaskUsagesLeft--;
            var powerToUse = Math.Min(minPricePower.PowerUsagesLeft, task.Power);
            minPricePower.PowerUsagesLeft -= powerToUse;
            task.Power -= powerToUse;
            task.powerUsage.Add(minPricePower.Minute, powerToUse);
        }
    }

    Console.WriteLine(taskAmount);

    foreach (var task in tasks) {
        Console.WriteLine(task.Id + " " + string.Join(" ", task.powerUsage.Select(p => p.Key + " " + p.Value)));
    }

    

    long GetElectricityBill() {
        long bill = 0;
        
        foreach (var task in tasks) {
            bill += task.powerUsage.Sum(pu => powerPrices[pu.Key].Price * (1 + pu.Value / maxPowerPerMinute));
        }
        
        return bill;
    }
}





public class PowerTask {
    public Dictionary<int, int> powerUsage = new Dictionary<int, int>();

    public long Duration { get; set; }
    public long PowerTasksAmount { get; set; }
    public long Id { get; set; }
    public int Power { get; set; }

    public int StartInterval { get; set; }
    public int EndInterval { get; set; }

    public long IntervalLength => EndInterval - StartInterval + 1;

    public long Priority => (PowerTasksAmount - IntervalLength) * Power;

    public long Price { get; set; } = long.MaxValue;

    public long StartMinute { get; set; }

    public PowerTask(long id, int power, int startInterval, int endInterval, long powerTasksAmount) {
        Id = id;
        Power = power;
        StartInterval = startInterval;
        EndInterval = endInterval;
        PowerTasksAmount = powerTasksAmount;
    }
}

public class PowerPrice {
    public long Price { get; set; }
    public int Minute { get; set; }
    public int PowerUsagesLeft { get; set; }
    public long TaskUsagesLeft { get; set; }

    public PowerPrice(long price, int usagesLeft, int minute, long taskUsagesLeft) {
        Price = price;
        PowerUsagesLeft = usagesLeft;
        Minute = minute;
        TaskUsagesLeft = taskUsagesLeft;
    }

}