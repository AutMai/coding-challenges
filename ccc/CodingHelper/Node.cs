namespace CodingHelper;

public class Node<T> {
    public Node(T value) {
        Value = value;
    }

    public Node() { }

    public T Value { get; set; }

    // position:
    public int PosX { get; set; }
    public int PosY { get; set; }

    public bool Visited { get; set; }


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
    
    public List<Node<T>> ExcludeNeighbors(HashSet<T> exclude) {
        return Neighbors.Where(k => !exclude.Contains(k.Value)).ToList();
    }
    
    public List<Node<T>> ExcludeFullNeighbors(HashSet<T> exclude) {
        return FullNeighbors.Where(k => !exclude.Contains(k.Value)).ToList();
    }
    
    public bool IsNeighbor(Node<T> node) {
        return Neighbors.Contains(node);
    }
    
    public bool IsFullNeighbor(Node<T> node) {
        return FullNeighbors.Contains(node);
    }

    public List<Node<T>> SortedNeighbors() {
        var list = new List<Node<T>>();
        if (Top != null) {
            list.Add(Top);
        }

        if (Right != null) {
            list.Add(Right);
        }

        if (Bottom != null) {
            list.Add(Bottom);
        }

        if (Left != null) {
            list.Add(Left);
        }

        return list;
    }

    public List<Node<T>> InverseSortedNeighbors() {
        var list = new List<Node<T>>();
        if (Left != null) {
            list.Add(Left);
        }

        if (Bottom != null) {
            list.Add(Bottom);
        }

        if (Right != null) {
            list.Add(Right);
        }

        if (Top != null) {
            list.Add(Top);
        }

        return list;
    }

    public List<Node<T>> GetNeighborOrder(int order) {
        // list is array of 8 lists

        var list = order switch {
            0 => new List<Node<T>>() { Top, Right, Bottom, Left },
            1 => new List<Node<T>>() { Top, Right, Bottom, Left },
            2 => new List<Node<T>>() { Bottom, Left, Top, Right },
            3 => new List<Node<T>>() { Left, Top, Right, Bottom },
            4 => new List<Node<T>>() { Left, Bottom, Right, Top },
            5 => new List<Node<T>>() { Bottom, Right, Top, Left },
            6 => new List<Node<T>>() { Right, Top, Left, Bottom },
            7 => new List<Node<T>>() { Top, Left, Bottom, Right },
            _ => new List<Node<T>>()
        };

        // remove nulls
        list.RemoveAll(k => k is null);

        return list;
    }

    public override string ToString() {
        return $"{PosX},{PosY} - {Value}";
    }

    public void RemoveNeighbor(Node<T> node) {
        // remove from neighbors, full neighbors and check where it is (top, bottom, left, right, ...) and remove it
        Neighbors.Remove(node);
        FullNeighbors.Remove(node);
        if (Top == node) {
            Top = null;
        }

        if (Bottom == node) {
            Bottom = null;
        }

        if (Left == node) {
            Left = null;
        }

        if (Right == node) {
            Right = null;
        }

        if (TopLeft == node) {
            TopLeft = null;
        }

        if (TopRight == node) {
            TopRight = null;
        }

        if (BottomLeft == node) {
            BottomLeft = null;
        }

        if (BottomRight == node) {
            BottomRight = null;
        }
    }
}