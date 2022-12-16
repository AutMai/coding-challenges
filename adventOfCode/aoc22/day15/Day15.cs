using System.Collections.Concurrent;
using System.Numerics;
using System.Text.RegularExpressions;
using aocTools;
using Raylib_cs;

namespace aoc22.day15;

public class Day15 : AAocDay {
    const int ResultRow = 10;

    public Day15() : base(true, true) {
    }

    public HashSet<Sensor> Sensors { get; set; } = new();
    public HashSet<Beacon> Beacons { get; set; } = new();
    public HashSet<Vector2> Signal { get; set; } = new();

    public static float ManhattanDistance(Vector2 v1, Vector2 v2) {
        return Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
    }

    private bool SensorHasInRange(Sensor s, Vector2 v) {
        return s.ClosestDistance() >= ManhattanDistance(s.Position, v);
    }

    public override void PuzzleOne() {
        ReadInput();

        foreach (var sensor in Sensors) {
            var sensorY = sensor.Position.Y;
            var sensorX = sensor.Position.X;
            var beaconX = sensor.ClosestBeacon.Position.X;
            var beaconY = sensor.ClosestBeacon.Position.Y;

            var signalRadius = ManhattanDistance(sensor.Position, sensor.ClosestBeacon.Position);

            if (ResultRow > signalRadius + sensorY && ResultRow < sensorY - signalRadius)
                continue;

            var rowDistance = ResultRow > sensorY
                ? (sensorY + signalRadius) - ResultRow // result row is below sensor
                : ResultRow - (sensorY - signalRadius); // result row is above sensor

            for (var x = sensorX - rowDistance; x < sensorX + rowDistance; x++)
                Signal.Add(new Vector2(x, ResultRow));
        }


        Console.WriteLine(Signal.Count);
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            InputTokens.Remove(2);
            var sensor = new Sensor(InputTokens.Read().Split("=")[1].Replace(",", ""),
                InputTokens.Read().Split("=")[1].Replace(":", ""));
            InputTokens.Remove(4);
            var beacon = new Beacon(InputTokens.Read().Split("=")[1].Replace(",", ""),
                InputTokens.Read().Split("=")[1]);
            sensor.ClosestBeacon = beacon;
            Beacons.Add(beacon);
            Sensors.Add(sensor);
        }
    }

    public override void PuzzleTwo() {
        int maxX = 4000000;
        int maxY = 4000000;
    }
}

public class Sensor {
    public Vector2 Position { get; set; }
    public Beacon ClosestBeacon { get; set; }

    public float ClosestDistance() => Day15.ManhattanDistance(Position, ClosestBeacon.Position);

    public Sensor(string x, string y) {
        Position = new Vector2(float.Parse(x), float.Parse(y));
    }
}

public class Beacon {
    public Vector2 Position { get; set; }

    public Beacon(string x, string y) {
        Position = new Vector2(float.Parse(x), float.Parse(y));
    }
}