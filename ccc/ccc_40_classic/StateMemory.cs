namespace ccc_40_classic;

public class StateMemory : HashSet<State> {
    
    public State GetFailState(State state) {
        return this.First(s => s.Equals(state));
    }
    
    public bool ContainsStateWithOtherLastDeskOrientation(State state) {
        // if we would change the orientation of the last desk in the state - check if such state exists - but to dont change the value of the desk as this is reference type
        bool temp = state.ArrangedDesks.Last().Horizontal;
        state.ArrangedDesks.Last().Horizontal = !temp;
        bool contains = this.Contains(state);
        state.ArrangedDesks.Last().Horizontal = temp;
        return contains;
    }
}