using System.Collections.Concurrent;
using aocTools;
using aocTools.Interval;

namespace aoc23.day8;

public class Day8 : AAocDay {
    public Day8() : base() {
        ReadInput();
    }

    public string LeftRightInstructions { get; set; }
    public HashSet<Node> Nodes { get; set; } = new();

    private void ReadInput() {
        LeftRightInstructions = InputTokens.Read();
        InputTokens.Remove(1);

        while (InputTokens.HasMoreTokens()) {
            var line = InputTokens.Read().Split(" ");
            var label = line[0];
            var leftNodeLabel = line[2].Substring(1, 3);
            var rightNodeLabel = line[3][..3];

            // check if node exists in Nodes
            var node = Nodes.FirstOrDefault(n => n.Label == label);
            if (node == null) {
                node = new Node();
                node.Label = label;
            }


            // check if left node exists in Nodes
            var leftNode = Nodes.FirstOrDefault(n => n.Label == leftNodeLabel);
            if (leftNode != null) {
                node.Left = leftNode;
            }
            else {
                node.Left = new Node();
                node.Left.Label = leftNodeLabel;
                Nodes.Add(node.Left);
            }

            // check if right node exists in Nodes
            var rightNode = Nodes.FirstOrDefault(n => n.Label == rightNodeLabel);
            if (rightNode != null) {
                node.Right = rightNode;
            }
            else {
                node.Right = new Node();
                node.Right.Label = rightNodeLabel;
                Nodes.Add(node.Right);
            }

            Nodes.Add(node);
        }
    }

    public override void PuzzleOne() {
        // start with node "AAA" cycle through leftrightinstructions and traverse the nodes until we find the node with the label "ZZZ" count the steps
        var currentNode = Nodes.FirstOrDefault(n => n.Label == "AAA");
        var steps = 0;
        while (currentNode.Label != "ZZZ") {
            if (LeftRightInstructions[(steps % LeftRightInstructions.Length)] == 'L') {
                currentNode = currentNode.Left;
            }
            else {
                currentNode = currentNode.Right;
            }

            steps++;
        }

        Console.WriteLine($"Puzzle one: {steps}");
    }

    public override void PuzzleTwo() {
        // start with all nodes that end with A cycle through leftrightinstructions and traverse the nodes until each node has reached a node that ends with Z count the steps
        var nodes = Nodes.Where(n => n.Label.EndsWith("A")).ToList();
        List<long> stepsList = new();
        for (int i = 0; i < nodes.Count; i++) {
            stepsList.Add(0);
        }

        for (int i = 0; i < nodes.Count; i++) {
            var currentNode = nodes[i];
            while (!currentNode.Label.EndsWith("Z")) {
                if (LeftRightInstructions[(int)(stepsList[i] % LeftRightInstructions.Length)] == 'L') {
                    currentNode = currentNode.Left;
                }
                else {
                    currentNode = currentNode.Right;
                }

                stepsList[i]++;
            }
        }

        // lcm of all steps
        var lcm = Lcm(stepsList.ToArray(), 0);
        Console.WriteLine($"Puzzle two: {lcm}");
    }

    private static long Gcd(long a, long b) {
        return a == 0 ? b : Gcd(b % a, a);
    }

    private static long Lcm(long[] arr, int idx) {
        // lcm(a,b) = (a*b/gcd(a,b))
        if (idx == arr.Length - 1) return arr[idx];

        var a = arr[idx];
        var b = Lcm(arr, idx + 1);
        return a * b / Gcd(a, b);
    }
}

public class Node {
    public string Label { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
}