using System.Numerics;
using System.Text.RegularExpressions;
using aocTools;
using Raylib_cs;

namespace aoc22.day16;

public class Day16 : AAocDay {
    public static Dictionary<string, Valve> Valves = new();

    public override void PuzzleOne() {
        ReadInput();
        //RaylibDraw();
        var x = Valves["AA"].GetDistances();
        Console.WriteLine(x);
    }

    private int DfsMaximumFlowrate(int timeRemaining) {
        return 0;
    }

    private void RaylibDraw() {
        // assign positions to valves so that connections don't overlap
        var positions = new Dictionary<string, Vector2>();
        var pos = new Vector2(100, 100);
        foreach (var valve in Valves.Values) {
            valve.Position = pos;
            pos.X += 100;
            if (pos.X > 400) {
                pos.X = 100;
                pos.Y += 100;
            }
        }

        // constant values for the window
        const int windowWidth = 2300;
        const int windowHeight = 1300;
        Raylib.InitWindow(windowWidth, windowHeight, "Day 16");
        Raylib.SetTargetFPS(60);


        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);
            Raylib.DrawText("Day 16", 10, 10, 20, Color.BLACK);


            foreach (var v in Valves) {
                // draw the valve
                Raylib.DrawCircleV(v.Value.Position, 20, Color.BLUE);
                Raylib.DrawText(v.Key, (int) v.Value.Position.X - 10, (int) v.Value.Position.Y - 5, 12, Color.BLACK);
                // draw the connections
                foreach (var c in v.Value.ConnectedValves) {
                    Raylib.DrawLineEx(v.Value.Position, c.Position, 2, Color.BLACK);
                }
            }

            Raylib.EndDrawing();
        }
    }

    private void ReadInput() {
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            // line looks like "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB" or "Valve BB has flow rate=13; tunnels lead to valves CC, AA"
            var x = line.Split("rate=");
            var mainValveFlowRate = x[1].Split(";")[0];
            var mainValveName = x[0].Split(" ")[1];
            if (!Valves.ContainsKey(mainValveName)) {
                Valves.Add(mainValveName, new Valve(mainValveName, int.Parse(mainValveFlowRate)));
            }
            else {
                Valves[mainValveName].FlowRate = int.Parse(mainValveFlowRate);
            }

            var connectedValves = Regex.Split(x[1], @"to valves? ")[1].Split(", ");
            foreach (var valve in connectedValves) {
                if (!Valves.ContainsKey(valve)) {
                    Valves.Add(valve, new Valve(valve));
                    Valves[mainValveName].ConnectedValves.Add(Valves[valve]);
                }

                Valves[valve].ConnectedValves.Add(Valves[mainValveName]);
            }
        }
    }

    public override void PuzzleTwo() {
        Console.WriteLine();
    }
}

public class Valve {
    public Vector2 Position { get; set; }
    public string Name { get; }
    public int FlowRate { get; set; }
    public HashSet<Valve> ConnectedValves { get; set; } = new();

    public bool IsOpen { get; set; }

    public Valve(string name) {
        Name = name;
    }

    public Valve(string name, int flowRate) {
        Name = name;
        FlowRate = flowRate;
    }

    // shortest path to each other valve
    public Dictionary<Valve, int> GetDistances(string valveName = "") {
        var distances = new Dictionary<Valve, int>();
        var visited = new HashSet<Valve>();
        var queue = new Queue<(Valve, int)>();
        queue.Enqueue((this, 0));
        while (queue.Count > 0) {
            var (valve, distance) = queue.Dequeue();
            if (visited.Contains(valve)) continue;
            visited.Add(valve);
            distances.Add(valve, distance);
            foreach (var connectedValve in valve.ConnectedValves) {
                queue.Enqueue((connectedValve, distance + 1));
            }
        }

        if (valveName != "") {
            return distances;
        }
        else {
            return distances.Where(d => d.Key.Name == valveName).ToDictionary(d => d.Key, d => d.Value);
        }
    }
}