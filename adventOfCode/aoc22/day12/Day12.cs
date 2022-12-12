using aocTools;
using aocTools.Neo4J;

namespace aoc22.day12;

public class Day12 : AAocDay {
    public override void PuzzleOne() {
        FillMatrix();
        using var greeter = new Neo4JConnection();
        //greeter.DropVirtualGraph("graph");
        //greeter.CreateVirtualGraph("graph", "Node", "CONNECTED");
        Console.WriteLine(greeter.ShortestPathSourceTarget());
        Console.WriteLine();
    }

    private char[,] _matrix;

    private void FillMatrix() {
        _matrix = new char[InputTokens.JustRead().Length, InputTokens.Count];
        for (int y = 0; y < InputTokens.Count; y++) {
            for (int x = 0; x < InputTokens.JustRead().Length; x++) {
                _matrix[x, y] = InputTokens[y][x];
            }
        }
    }

    private void MatrixToNeo4j() {
        using var greeter = new Neo4JConnection();
        greeter.DeleteAll();

        // loop through matrix
        for (int y = 0; y < _matrix.GetLength(1); y++) {
            for (int x = 0; x < _matrix.GetLength(0); x++) {
                var neighbours = new List<(int x, int y)>();
                if (x > 0)
                    neighbours.Add((x - 1, y));
                if (x < _matrix.GetLength(0) - 1)
                    neighbours.Add((x + 1, y));
                if (y > 0)
                    neighbours.Add((x, y - 1));
                if (y < _matrix.GetLength(1) - 1)
                    neighbours.Add((x, y + 1));

                neighbours.ForEach(n => {
                    if (ValidNeighbor(_matrix[x, y], _matrix[n.x, n.y])) {
                        if (_matrix[n.x, n.y] is 'S' or 'E') {
                            greeter.CreateRel("Node", $"{x},{y}", $"{_matrix[x, y]}", "Node", $"{_matrix[n.x, n.y]}",
                                $"{_matrix[n.x, n.y]}", "CONNECTED", $"r{x},{y},{n.x},{n.y}");
                        }
                        else if (_matrix[x, y] is 'S' or 'E') {
                            greeter.CreateRel("Node", $"{_matrix[x, y]}", $"{_matrix[x, y]}", "Node", $"{n.x},{n.y}",
                                $"{_matrix[n.x, n.y]}", "CONNECTED", $"r{x},{y},{n.x},{n.y}");
                        }
                        else {
                            greeter.CreateRel("Node", $"{x},{y}", $"{_matrix[x, y]}", "Node", $"{n.x},{n.y}",
                                $"{_matrix[n.x, n.y]}", "CONNECTED", $"r{x},{y},{n.x},{n.y}");
                        }
                    }
                });
            }
        }
    }

    private bool ValidNeighbor(char s, char n) {
        switch (s) {
            case 'E':
                return false;
            case 'S' when n is 'a' or 'b':
                return true;
        }

        switch (n) {
            case 'S':
            case 'E' when s is 'y' or 'z':
                return true;
            case 'E' when s < 'y':
                return false;
        }

        return n <= s + 1;
    }

    public override void PuzzleTwo() {
        using var greeter = new Neo4JConnection();
        // loop through matrix
        int shortest = int.MaxValue;
        for (int y = 0; y < _matrix.GetLength(1); y++) {
            for (int x = 0; x < _matrix.GetLength(0); x++) {
                if (_matrix[x, y] is 'a') {
                    var steps = greeter.ShortestPathSourceTarget($"{x},{y}", "E");
                    if (steps < shortest)
                        shortest = steps;
                }
                if (_matrix[x, y] is 'S') {
                    var steps = greeter.ShortestPathSourceTarget();
                    if (steps < shortest)
                        shortest = steps;
                }
            }
        }

        Console.WriteLine(shortest);
        Console.WriteLine();
    }
}