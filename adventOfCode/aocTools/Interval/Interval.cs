namespace aocTools.Interval;

public record Interval(long Start, long End) {
    // start is inclusive, end is exclusive
    public bool Contains(long value) => value >= Start && value < End;
    public bool Contains(Interval interval) => interval.Start >= Start && interval.End <= End;
    public bool Overlaps(Interval interval) => interval.Start < End && interval.End > Start;

    public Interval? Intersection(Interval interval) {
        if (!Overlaps(interval)) return null;
        return new Interval(Math.Max(Start, interval.Start), Math.Min(End, interval.End));
    }

    public Interval? Union(Interval interval) {
        if (!Overlaps(interval)) return null;
        return new Interval(Math.Min(Start, interval.Start), Math.Max(End, interval.End));
    }

    public Interval? Except(Interval interval) {
        if (!Overlaps(interval)) return null;
        if (interval.Start <= Start && interval.End >= End) return null;
        if (interval.Start > Start && interval.End < End) return new Interval(Start, interval.Start);
        if (interval.Start <= Start) return new Interval(interval.End, End);
        return new Interval(Start, interval.Start);
    }

    // opposite of Intersection - returns the two intervals that are not in common
    public List<Interval> ExceptMultiSide(Interval interval) {
        var intersection = Intersection(interval);

        if (intersection == null) {
            return new List<Interval> { this, interval };
        }

        // get the remaining ranges outside of the intersection
        var ranges = new List<Interval>();
        if (Start < intersection.Start) {
            ranges.Add(new Interval(Start, intersection.Start));
        }

        if (End > intersection.End) {
            ranges.Add(new Interval(intersection.End, End));
        }
        return ranges;
    }

    public override string ToString() => $"[{Start},{End})";

    public static Interval operator +(Interval a, Interval b) => new Interval(a.Start + b.Start, a.End + b.End);
    public static Interval operator -(Interval a, Interval b) => new Interval(a.Start - b.Start, a.End - b.End);
    public static Interval operator *(Interval a, Interval b) => new Interval(a.Start * b.Start, a.End * b.End);
    public static Interval operator /(Interval a, Interval b) => new Interval(a.Start / b.Start, a.End / b.End);
}