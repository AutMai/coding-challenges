using System.Collections.Concurrent;
using aocTools;
using aocTools.Interval;

namespace aoc23.day10;

public class Day10 : AAocDay {
    public Day10() {
        ReadInput();
        //Pipe.PrintPipes();
    }

    private void ReadInput() {
        var y = 0;
        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read();
            for (var x = 0; x < line.Length; x++) {
                var c = line[x];
                var p = new Pipe { X = x, Y = y, Value = c };
                Pipe.AllPipes.Add(p);
            }

            y++;
        }
    }

    public override void PuzzleOne() {
        var loop = GetLoop();
        //Pipe.PrintPipes(min.ToHashSet());
        Console.WriteLine(loop.Count / 2);
    }

    private List<Pipe> GetLoop() {
        var paths = new List<List<Pipe>>();
        var start = Pipe.AllPipes.FirstOrDefault(p => p.Value == 'S');
        foreach (var p in start.ConnectedPipes()) {
            var path = Pipe.DFS(p, start);
            if (path is null)
                continue;
            paths.Add(path);
        }

        // print path with least steps
        return paths.MinBy(p => p.Count);
    }

    public override void PuzzleTwo() {
        List<Pipe> enclosedPipes = new();
        var loop = GetLoop();
        Console.WriteLine(Pipe.Area(loop));
    }
}

class Pipe : Point {
    public static HashSet<Pipe> AllPipes { get; set; } = new();
    public char Value { get; set; }

    public List<Pipe> ConnectedPipes() {
        // depending on the value, we need to find the connected pipes
        var pipes = new List<Pipe>();
        switch (Value) {
            case '|':
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y - 1));
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y + 1));
                break;
            case '-':
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X - 1 && p.Y == Y));
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X + 1 && p.Y == Y));
                break;
            case 'L':
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y - 1));
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X + 1 && p.Y == Y));
                break;
            case 'J':
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y - 1));
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X - 1 && p.Y == Y));
                break;
            case '7':
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y + 1));
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X - 1 && p.Y == Y));
                break;
            case 'F':
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y + 1));
                pipes.Add(AllPipes.FirstOrDefault(p => p.X == X + 1 && p.Y == Y));
                break;
            case 'S':
                // return all pipes that are connected to this pipe
                var adjacentPipes = new List<Pipe>();
                adjacentPipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y - 1));
                adjacentPipes.Add(AllPipes.FirstOrDefault(p => p.X == X && p.Y == Y + 1));
                adjacentPipes.Add(AllPipes.FirstOrDefault(p => p.X == X - 1 && p.Y == Y));
                adjacentPipes.Add(AllPipes.FirstOrDefault(p => p.X == X + 1 && p.Y == Y));
                // remove nulls
                adjacentPipes.RemoveAll(p => p is null);
                // check if those pipes are open to this pipe
                foreach (var p in adjacentPipes) {
                    if (p.Value == 'S')
                        continue;
                    if (p.Value == ' ') {
                        pipes.Add(p);
                        continue;
                    }

                    if (p.Value == '|') {
                        if (p.X == X) {
                            pipes.Add(p);
                            continue;
                        }
                    }

                    if (p.Value == '-') {
                        if (p.Y == Y) {
                            pipes.Add(p);
                            continue;
                        }
                    }

                    if (p.Value == 'L') {
                        if (p.Y == Y + 1 || p.X == X - 1) {
                            pipes.Add(p);
                            continue;
                        }
                    }

                    if (p.Value == 'J') {
                        if (p.Y == Y + 1 || p.X == X + 1) {
                            pipes.Add(p);
                            continue;
                        }
                    }

                    if (p.Value == '7') {
                        if (p.Y == Y - 1 || p.X == X + 1) {
                            pipes.Add(p);
                            continue;
                        }
                    }

                    if (p.Value == 'F') {
                        if (p.Y == Y - 1 || p.X == X - 1) {
                            pipes.Add(p);
                            continue;
                        }
                    }
                }

                break;
            default:
                throw new Exception($"Unknown pipe value: {Value}");
        }

        return pipes;
    }

    public override string ToString() {
        var c = Value switch {
            '|' => '║',
            '-' => '═',
            'L' => '╚',
            'J' => '╝',
            '7' => '╗',
            'F' => '╔',
            'S' => 'S',
            _ => Value
        };
        return c.ToString();
    }

    public static void PrintPipes(HashSet<Pipe> highlight = null) {
        var minX = AllPipes.Min(p => p.X);
        var maxX = AllPipes.Max(p => p.X);
        var minY = AllPipes.Min(p => p.Y);
        var maxY = AllPipes.Max(p => p.Y);
        for (var y = minY; y <= maxY; y++) {
            for (var x = minX; x <= maxX; x++) {
                var p = AllPipes.FirstOrDefault(p => p.X == x && p.Y == y);
                if (highlight != null && highlight.Contains(p)) {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                if (p == null) {
                    Console.Write('.');
                    continue;
                }

                Console.Write(p);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }


    public static List<Pipe> DFS(Pipe start, Pipe end, List<Pipe> exclude = null) {
        var visited = new List<Pipe>();
        var stack = new System.Collections.Generic.Stack<Pipe>();
        stack.Push(start);
        while (stack.Count > 0) {
            var current = stack.Pop();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            if (current == end)
                return visited;
            foreach (var neighbor in current.ConnectedPipes()) {
                // if neighbor is start and current is the first step we continue because we dont wont to go back instantly
                if (neighbor == end && visited.Count == 1)
                    continue;
                if (neighbor is null)
                    continue;
                if (exclude is not null && exclude.Contains(neighbor))
                    continue;
                stack.Push(neighbor);
            }
        }

        return null;
    }

    public static int Area(List<Pipe> border) {
        return 0;
    }
}

class Point {
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString() {
        return $"({X},{Y})";
    }
}