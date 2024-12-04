namespace ccc_40_classic;

public class DeskCollection : HashSet<Desk> {

    // // use sequence equal and iequalitycomparer
    // public override bool Equals(object obj) {
    //     if (obj == null || GetType() != obj.GetType()) {
    //         return false;
    //     }
    //
    //     var other = (DeskCollection)obj;
    //
    //     return other.SequenceEqual(this);
    // }
    
    // ctor that copies the collection
    public DeskCollection(DeskCollection collection) {
        foreach (var desk in collection) {
            Add(desk);
        }
    }
    
    public DeskCollection() {
    }
    
}