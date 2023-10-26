using System.Text;
using CodingHelper;
/*
var r = new InputReader();

r.ReadZipFile("files/level4.zip");
//r.ReadWholeFile("files/level3/level3_7.in");

foreach (var l in r.GetInputs()) {
    //l.SetOutputToFile();

    var rowCount = l.ReadInt();

    var moveableObjects = new List<AMovable>();

    Node[][] map = new Node[rowCount][];

    for (int i = 0; i < rowCount; i++) {
        map[i] = l.Read().Replace('G', 'W').ToCharArray().Select((k, x) => new Node(k) {PosY = i, PosX = x}).ToArray();
    }

    /*
    map.ToNeighbors();
    #1#

    var cointCount = map.SelectMany(k => k).Count(k => k.Type == 'C');

    var gameMap = new GameMap(map);


    var posY = l.ReadInt() - 1;
    var posX = l.ReadInt() - 1;

    var maxSteps = l.ReadInt();

    var pos = map[posY][posX];
    pos.Visited = false;
    var player = new Player(
        pos
    ) {
        PosX = posX,
        PosY = posY
    };
    gameMap.Player = player;
    moveableObjects.Add(player);


    while (player.CoinCount < cointCount) {
        Console.Clear();
        Console.WriteLine(gameMap);
        Console.WriteLine(map[1][16].Visited ? "Visited" : "Not visited");
        gameMap.MovePlayer();
        Thread.Sleep(50);
    }


    Console.WriteLine(player.Moves);
}

public class GameMap {
    private Node[][] map;
    public Player Player { get; set; }
    public List<Ghost> Ghosts = new List<Ghost>();
    public string rotations = "LDRU";
    public int rotIndex = 0;

    public GameMap(Node[][] map) {
        this.map = map;
    }

    public void RotatePlayer() {
        rotIndex++;
        if (rotIndex == 4) {
            rotIndex = 0;
        }

        Player.Rotation = rotations[rotIndex];
    }

    public void MovePlayer() {
        if (Player.CurrentNode.Neighbors.Count(n => n.Type == 'W') == 3) {
            Player.CurrentNode.Type = 'W';
        }

        Move(Player.Rotation, Player);
    }

    public void MoveGhosts(int moveIndex) {
        foreach (var g in Ghosts) {
            if (moveIndex >= g.Moves.Length) continue;
            Move(g.Moves[moveIndex], g);
        }
    }

    public void Move(char direction, AMovable movableObject) {
        var prevPosX = movableObject.PosX;
        var prevPosY = movableObject.PosY;

        if (direction == 'L') {
            movableObject.PosX -= 1;
        }

        if (direction == 'D') {
            movableObject.PosY += 1;
        }

        if (direction == 'R') {
            movableObject.PosX += 1;
        }

        if (direction == 'U') {
            movableObject.PosY -= 1;
        }

        if (map[movableObject.PosY][movableObject.PosX].Type == 'W') {
            Player.CurrentNode = map[prevPosY][prevPosX];
            Player.PosX = prevPosX;
            Player.PosY = prevPosY;
            RotatePlayer();
        }
        else if (map[movableObject.PosY][movableObject.PosX].Visited && Player.CurrentNode.Type != 'W') {
            Node node;
            if (Player.CurrentNode.Neighbors.Where(n => n.Type != 'W').ToList().Exists(n => n.Visited == false)) {
                node = Player.CurrentNode.Neighbors.Where(n => n.Type != 'W').First(n => n.Visited == false);
            }
            else {
                node = Player.CurrentNode.Neighbors.Where(n => n.Type != 'W').MinBy(n => n.VisitedCount);
            }

            Player.PosX = node.PosX;
            Player.PosY = node.PosY;
            // check if neighbor is above, below, left or right
            if (node.PosX == Player.CurrentNode.PosX) {
                if (node.PosY > Player.CurrentNode.PosY) {
                    Player.Rotation = 'D';
                }
                else {
                    Player.Rotation = 'U';
                }
            }
            else {
                if (node.PosX > Player.CurrentNode.PosX) {
                    Player.Rotation = 'R';
                }
                else {
                    Player.Rotation = 'L';
                }
            }

            movableObject.SetNode(node, Player.CurrentNode);
        }
        else {
            movableObject.SetNode(map[movableObject.PosY][movableObject.PosX], map[prevPosY][prevPosX]);
        }
        if (prevPosX != movableObject.PosX || prevPosY != movableObject.PosY) {
            Player.Moves.Append(Player.Rotation);
        }
    }

    public override string ToString() {
        var sb = new StringBuilder();
        foreach (var row in map) {
            foreach (var node in row) {
                if (node == Player.CurrentNode) {
                    sb.Append('X');
                }
                else {
                    sb.Append(node.Type);
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}

public class Player : AMovable {
    public int CoinCount { get; set; }
    public bool Dead = false;
    public char Rotation = 'L';

    public override void SetNode(Node node, Node prevNode) {
        node.Visited = true;
        node.VisitedCount++;
        CurrentNode = node;
        /*if (node.Type == 'W') {
            Console.WriteLine(CoinCount + " NO");
            Dead = true;
        }#1#
        /*else if (node.Ghost) {
            Console.WriteLine(CoinCount + " NO");
            Dead = true;
        }#1#
        if (node.Type == 'C') {
            CurrentNode.Type = 'E';
            CoinCount++;
        }
    }


    public Player(Node currentNode) :
        base(currentNode) {
    }
}

public abstract class AMovable : ISetPosition {
    public StringBuilder Moves = new StringBuilder();

    protected AMovable(Node currentNode) {
        CurrentNode = currentNode;
    }

    public int PosX { get; set; }
    public int PosY { get; set; }
    public Node CurrentNode { get; set; }
    public abstract void SetNode(Node node, Node prevNode);
}

public interface ISetPosition {
    public void SetNode(Node node, Node prevNode);
}

public class Ghost : AMovable {
    public override void SetNode(Node node, Node prevNode) {
        prevNode.Ghost = false;
        CurrentNode = node;
        node.Ghost = true;
    }

    public Ghost(Node currentNode) : base(currentNode) {
    }
}*/