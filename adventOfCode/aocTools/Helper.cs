namespace aocTools;

public static class Helper {
    private static readonly string WorkingDirectory = Environment.CurrentDirectory;

    public static string ReadFile(string filename) =>
        Helper.ReadFile("input.txt");
}