using System.Text;
using System.Text.RegularExpressions;
using aocTools;
using aocTools.Neo4J;

namespace aoc24.day4;

public class Day4 : AAocDay {
    private readonly NodeMap<char> _map;

    public Day4() : base() {
        _map = new NodeMap<char>(InputTokens);
    }

    public override void PuzzleOne() {
        // map is a matrix of letters
        // like the search riddle i am trying to find the word XMAS either horizontally, vertically or diagonally
        // count how often the word appears in the matrix
        var result = 0;

        foreach (var node in _map.NodeList.Where(n => n.Value == 'X')) {
            // node has neighbors (up, down, left, right, up-left, up-right, down-left, down-right)
            // we check all X nodes for the word XMAS
            result += CheckXmasWord(node, n => n.Top) ? 1 : 0;
            result += CheckXmasWord(node, n => n.Bottom) ? 1 : 0;
            result += CheckXmasWord(node, n => n.Left) ? 1 : 0;
            result += CheckXmasWord(node, n => n.Right) ? 1 : 0;
            result += CheckXmasWord(node, n => n.TopLeft) ? 1 : 0;
            result += CheckXmasWord(node, n => n.TopRight) ? 1 : 0;
            result += CheckXmasWord(node, n => n.BottomLeft) ? 1 : 0;
            result += CheckXmasWord(node, n => n.BottomRight) ? 1 : 0;
        }

        Console.WriteLine(result);
    }

    private static bool CheckXmasWord(Node<char> xNode, Func<Node<char>, Node<char>?> direction) {
        var word = "XMAS";
        var wordLength = word.Length;
        var node = xNode;
        var sb = new StringBuilder();
        sb.Append(node.Value);
        for (var i = 0; i < wordLength - 1; i++) {
            node = direction(node);
            if (node == null) {
                return false;
            }

            sb.Append(node.Value);
        }

        return sb.ToString() == word;
    }


    public override void PuzzleTwo() {
        // map is a matrix of letters
        // like the search riddle i am trying to find the following pattern
        // M.S
        // .A.
        // M.S
        // so mas in an X shape. the direction of the mas can be any direction
        // count how often this pattern appears in the matrix
        var validNodes = new List<Node<char>>();
        foreach (var node in _map.NodeList.Where(n => n.Value == 'A')) {
            if (CheckXmasWord2(node) != null) {
                validNodes.Add(node);
            }
        }
        
        //_map.PrintPath(validNodes);
        
        Console.WriteLine(validNodes.Count);
    }


    private static Node<char>? CheckXmasWord2(Node<char> aNode) {
        // node A has to have M and S as diagonal neighbors. M and S can be in any direction but on the opposite side of S has to be M and vice versa
        var diagonalNeighbors = aNode.FullNeighbors.Except(aNode.Neighbors);
        var mNodes = diagonalNeighbors.Where(n => n.Value == 'M').ToList();
        var sNodes = diagonalNeighbors.Where(n => n.Value == 'S').ToList();

        if (mNodes.Count != 2 || sNodes.Count != 2) {
            return null;
        }

        return sNodes.Any(sNode => GetOppositeDiagonalNeighbor(aNode, mNodes[0]) == sNode) ? aNode : null;
    }

    // map the opposite diagonal neighbors: node.TopLeft => node.BottomRight, node.TopRight => node.BottomLeft, ...
    private static Node<char>? GetOppositeDiagonalNeighbor(Node<char> node, Node<char> neighbor) {
        if (node.TopLeft == neighbor) {
            return node.BottomRight;
        }

        if (node.TopRight == neighbor) {
            return node.BottomLeft;
        }

        if (node.BottomLeft == neighbor) {
            return node.TopRight;
        }

        if (node.BottomRight == neighbor) {
            return node.TopLeft;
        }

        return null;
    }
}