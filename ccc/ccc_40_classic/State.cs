namespace ccc_40_classic;

public class State: IEquatable<State> {
    public HashSet<Desk> ArrangedDesks { get; set; } = new();
    //public int CurrentId { get; set; } = 1;
    public int CheckpointX => Convert.ToInt32(ArrangedDesks.Last().Position.X);
    public int CheckpointY => Convert.ToInt32(ArrangedDesks.Last().Position.Y);
    
    public State(HashSet<Desk> arrangedDesks, int checkpointX, int checkpointY) {
        ArrangedDesks = new HashSet<Desk>(arrangedDesks);
        //CurrentId = currentId;
    }


    public bool Equals(State? other) {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ArrangedDesks.SetEquals(other.ArrangedDesks);
    }

    public override bool Equals(object? obj) {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((State)obj);
    }

    public override int GetHashCode() {
        return ArrangedDesks.GetHashCode();
    }
}