namespace aocTools;

public static class Helper {
    private static readonly string WorkingDirectory = Environment.CurrentDirectory;

    public static string ReadFile(string filename) {
        return File.ReadAllText(Path.Combine(Directory.GetParent(WorkingDirectory).Parent.Parent.FullName, filename))
            .Replace("\r", "");
    }
}