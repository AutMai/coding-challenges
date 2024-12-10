using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using aocTools;
using aocTools.Neo4J;

namespace aoc24.day8;

public class Day8 : AAocDay {
    private NodeMap<char> _map;

    public Day8() : base() {
        _map = new NodeMap<char>(InputTokens);
    }


    public override void PuzzleOne() {
        var antennas = _map.NodeList.Where(n => n.Value != '.').ToList();
        var freqAntennas = antennas.GroupBy(n => n.Value);
        var antiNodes = new HashSet<Node<char>>();
        foreach (var fa in freqAntennas) {
            antiNodes.UnionWith(GenerateFrequencyAntennas(fa.ToList()));
        }
        
        Console.WriteLine($"P1 New antennas: {antiNodes.Count}");
    }

    private HashSet<Node<char>> GenerateFrequencyAntennas(List<Node<char>> antennas) {
        var antiNodes = new HashSet<Node<char>>();
        for (var i1 = 0; i1 < antennas.Count; i1++) {
            for (var i2 = i1 + 1; i2 < antennas.Count; i2++) {
                var a1 = antennas[i1];
                var a2 = antennas[i2];


                var v1 = a1.GetVector();
                var v2 = a2.GetVector();

                var deltaVector = v2 - v1;

                var v3 = v1 - deltaVector;
                var v4 = v2 + deltaVector;

                var n1 = _map.GetNode(v3);
                var n2 = _map.GetNode(v4);


                if (n1 is not null) {
                    antiNodes.Add(n1);
                    if (n1.Value is '.') {
                        n1.Value = '#';
                    }
                }

                if (n2 is not null) {
                    antiNodes.Add(n2);
                    if (n2.Value is '.') {
                        n2.Value = '#';
                    }
                }
            }
        }

        return antiNodes;
    }
    
    
    public override void PuzzleTwo() {
        ResetInput();
        _map = new NodeMap<char>(InputTokens);
        var antennas = _map.NodeList.Where(n => n.Value != '.').ToList();
        var freqAntennas = antennas.GroupBy(n => n.Value);
        var antiNodes = new HashSet<Node<char>>();
        foreach (var fa in freqAntennas) {
            antiNodes.UnionWith(GenerateFrequencyAntennas2(fa.ToList()));
        }
        
        Console.WriteLine($"P2 New antennas: {antiNodes.Count}");
    }
    
    private HashSet<Node<char>> GenerateFrequencyAntennas2(List<Node<char>> antennas) {
        var antiNodes = new HashSet<Node<char>>();
        
        if (antennas.Count < 2) {
            return antiNodes;
        }
        
        antiNodes.UnionWith(antennas);
        
        
        for (var i1 = 0; i1 < antennas.Count; i1++) {
            for (var i2 = i1 + 1; i2 < antennas.Count; i2++) {
                var a1 = antennas[i1];
                var a2 = antennas[i2];


                var v1 = a1.GetVector();
                var v2 = a2.GetVector();

                var deltaVector = v2 - v1;
                
                // calculate all possible vectors till the end of the map
                
                var currentVector = v2 + deltaVector;
                while (_map.GetNode(currentVector) is not null) {
                    var n = _map.GetNode(currentVector);
                    if (n is not null) {
                        antiNodes.Add(n);
                        if (n.Value is '.') {
                            n.Value = '#';
                        }
                    }
                    
                    currentVector += deltaVector;
                }
                
                currentVector = v1 - deltaVector;
                while (_map.GetNode(currentVector) is not null) {
                    var n = _map.GetNode(currentVector);
                    if (n is not null) {
                        antiNodes.Add(n);
                        if (n.Value is '.') {
                            n.Value = '#';
                        }
                    }
                    
                    currentVector -= deltaVector;
                }
            }
        }

        return antiNodes;
    }

}