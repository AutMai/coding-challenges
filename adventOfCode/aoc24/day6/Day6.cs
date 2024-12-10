using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using aocTools;
using aocTools.Neo4J;

namespace aoc24.day6;

public class Day6 : AAocDay {
    private NodeMap<char> _map;
    private Guard _guard;
    private HashSet<Node<char>> _path = [];

    public Day6() : base() {
        _map = new NodeMap<char>(InputTokens);
    }


    public override void PuzzleOne() {
        var guardNode = _map.NodeList.First(n => n.Value == '^');
        guardNode.Value = '.';
        _guard = new Guard(new Vector2(guardNode.PosX, guardNode.PosY), EDirection.North);

        MoveGuard();
        
        Console.WriteLine($"{_path.Count} steps taken");
    }

    private void MoveGuard() {
        _path = [];
        _map.NodeList.Where(n => n.Value != '#').ToList().ForEach(n => n.Value = '.');

        // move guard in current direction until wall or obstacle (#) is hit
        // if wall or obstacle is hit, turn right

        var directionVector = new Direction(_guard.Direction).ToVector2();

        while (true) {
            var nextPos = _guard.Position + directionVector;
            // if nextPos is out of bounds, break
            if (nextPos.X < 0 || nextPos.X >= _map.Width || nextPos.Y < 0 || nextPos.Y >= _map.Height) {
                break;
            }
            var nextNode = _map.GetNode(_guard.Position + directionVector);

            if (nextNode.Value == '#') {
                _guard.Direction = _guard.Direction switch {
                    EDirection.North => EDirection.East,
                    EDirection.East => EDirection.South,
                    EDirection.South => EDirection.West,
                    EDirection.West => EDirection.North,
                    _ => throw new ArgumentOutOfRangeException()
                };
                directionVector = new Direction(_guard.Direction).ToVector2();
            }
            else {
                // if we already know this state (position and directon), we have found a loop
                var positionString = _guard.Direction switch {
                    EDirection.North => '^',
                    EDirection.East => '>',
                    EDirection.South => 'v',
                    EDirection.West => '<',
                    _ => throw new ArgumentOutOfRangeException()
                };
                if (_path.Contains(nextNode) && nextNode.Value == positionString) {
                    Console.WriteLine("Loop found");
                    throw new LoopException("Loop found");
                }
                
                // set node value to direction character: ^, >, v, <
                nextNode.Value = positionString;
                _path.Add(nextNode);
                _guard.Position += directionVector;
            }

            Console.Clear();
            _map.PrintPath(_path.ToList());
            
            Thread.Sleep(1);
        }
    }

    public override void PuzzleTwo() {
        var loopObstacleCount = 0;
        ResetInput();
        _map = new NodeMap<char>(InputTokens);
        var obstaclePositions = _map.NodeList.Where(n => n.Value == '.');

        var guardNode = _map.NodeList.First(n => n.Value == '^');
        guardNode.Value = '.';


        foreach (var pos in obstaclePositions) {
            _map.NodeList.First(n => n.PosX == pos.PosX && n.PosY == pos.PosY).Value = '#';
            try {
                _guard = new Guard(new Vector2(guardNode.PosX, guardNode.PosY), EDirection.North);
                MoveGuard();
            }
            catch (LoopException e) {
                Console.WriteLine("Loop found");
                loopObstacleCount++;
            }
            _map.NodeList.First(n => n.PosX == pos.PosX && n.PosY == pos.PosY).Value = '.';
        }
        
        Console.WriteLine($"{loopObstacleCount} obstacles found that create a loop");
    }

    private List<Vector2> FindAllPossibleLoops() {
        return new List<Vector2>();
    }
}

public class Guard {
    public Vector2 Position { get; set; }
    public EDirection Direction { get; set; }

    public Guard(Vector2 pos, EDirection direction) {
        Position = pos;
        Direction = direction;
    }
}

public class LoopException : Exception {
    public LoopException(string message) : base(message) {
    }
}