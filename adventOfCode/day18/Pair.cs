using System.Text.RegularExpressions;

namespace day18;

public class Pair {
    private new object Item1 { get; set; }
    private new object Item2 { get; set; }
    public Pair? ParentPair { get; set; }

    public Pair(object item1, object item2, Pair parentPair) {
        Item1 = item1;
        Item2 = item2;
        ParentPair = parentPair;
    }

    public void SetItem(object item) {
        if (Item1 == null) Item1 = item;
        else Item2 = item;
    }

    public void Reduce(List<Pair> visited) {
        visited.Add(this);

        Explode();
        Split();

        // cycle
        if (this.Item1 is Pair p1 && !visited.Contains(Item1))
            p1.Reduce(visited);
        else if (this.Item2 is Pair p2 && !visited.Contains(Item2))
            p2.Reduce(visited);
        else {
            var pair = this.ParentPair;
            while (true) {
                if (pair.Item1 is Pair pp1) {
                    if (pair.Item2 is Pair pp2 && !visited.Contains(pair.Item2))
                        pp2.Reduce(visited);
                    break;
                }

                pair = pair.ParentPair;
            }
        }
    }

    private void Explode() {
        if (Has4Parents()) {
            if (ParentPair.Item1 == this) {
                IncreaseLeftMostPair(Convert.ToInt32(this.Item1));
                ParentPair.Item1 = 0;
                ParentPair.Item2 = Convert.ToInt32(this.Item2) + Convert.ToInt32(ParentPair.Item2);
            }
            else {
                IncreaseRightMostPair(Convert.ToInt32(this.Item2));
                ParentPair.Item2 = 0;
                ParentPair.Item1 = Convert.ToInt32(this.Item1) + Convert.ToInt32(ParentPair.Item1);
            }
        }
    }

    private void Split() {
        if (Item1 is int item1) {
            if (Regex.IsMatch(item1.ToString(), @"^\d\d$")) {
                var i1 = Math.Floor(Convert.ToDecimal(item1 / 2));
                var i2 = Math.Ceiling(Convert.ToDecimal(item1 / 2));
                Item1 = new Pair(i1, i2, this);
            }
        }

        if (Item2 is int item2) {
            if (Regex.IsMatch(item2.ToString(), @"^\d\d$")) {
                var i1 = Math.Floor(Convert.ToDecimal(item2 / 2));
                var i2 = Math.Ceiling(Convert.ToDecimal(item2 / 2));
                Item2 = new Pair(i1, i2, this);
            }
        }
    }

    private void IncreaseLeftMostPair(int inc) {
        if (GetLeftMostPair() != null) {
            var p = GetRightMostPair();
            if (p.Item2 is int z)
                z += inc;
        }
    }

    private void IncreaseRightMostPair(int inc) {
        if (GetRightMostPair() != null) {
            var p = GetRightMostPair();
            if (p.Item1 is int item1)
                p.Item1 = item1 + inc;
        }
    }

    private Pair GetRightMostPair() {
        List<Pair> visited = new List<Pair> { this };
        var pair = this.ParentPair;
        visited.Add(pair);

        while (true) {
            if (pair.Item2 is int z) {
                return pair;
            }
            else if (pair.Item2 is Pair p && !visited.Contains(pair.Item2)) {
                pair = p;
                visited.Add(pair);
                if (pair.Item1 is int z1) return pair;
            }
            else {
                if (pair.ParentPair == null) return null;
                pair = pair.ParentPair;
                visited.Add(pair);
            }
        }
    }

    private Pair GetLeftMostPair() {
        List<Pair> visited = new List<Pair> { this };
        var pair = this.ParentPair;
        visited.Add(pair);

        while (true) {
            if (pair.Item1 is int z) {
                return pair;
            }
            else if (pair.Item1 is Pair p && !visited.Contains(pair.Item1)) {
                pair = p;
                visited.Add(pair);
                if (pair.Item2 is int z2) return pair;
            }
            else {
                if (pair.ParentPair == null) return null;
                pair = pair.ParentPair;
                visited.Add(pair);
            }
        }
    }

    private bool Has4Parents() {
        var parentCount = 0;
        var pair = this;
        while (true) {
            if (pair.ParentPair == null) break;
            pair = pair.ParentPair;
            parentCount++;
        }

        return (parentCount >= 4);
    }
}