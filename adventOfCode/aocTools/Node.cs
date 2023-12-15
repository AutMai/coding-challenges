namespace aocTools;

public class Node<T> {
    public Node(T value) {
        Value = value;
    }

    public Node() {
    }

    public T Value { get; set; }

    // position:
    public int PosX { get; set; }
    public int PosY { get; set; }


    // neighbors:

    public Node<T>? Top { get; set; }
    public Node<T>? Bottom { get; set; }
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }

    public Node<T>? TopLeft { get; set; }
    public Node<T>? TopRight { get; set; }
    public Node<T>? BottomLeft { get; set; }
    public Node<T>? BottomRight { get; set; }
    public List<Node<T>> Neighbors { get; set; } = new();
    public List<Node<T>> FullNeighbors { get; set; } = new();

    public override string ToString() {
        return $"{PosX},{PosY} - {Value}";
    }
}