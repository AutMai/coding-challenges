namespace day12;

public class Cave {
    public string Name { get; set; }
    public bool IsLarge { get; set; }
    public Dictionary<string, Cave> ConnectedCaves { get; set; }

    public Cave(string name) {
        Name = name;
        IsLarge = char.IsUpper(name[0]);
        ConnectedCaves = new Dictionary<string, Cave>();
    }

    public void AddConnectedCave(string caveName, Cave cave) {
        ConnectedCaves.Add(caveName, cave);
    }
}