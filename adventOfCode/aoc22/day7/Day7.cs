using aocTools;

namespace aoc22.day7;

public class Day7 : AAocDay {
    private Directory _root;
    private List<Directory> _directories = new List<Directory>();
    public override void PuzzleOne() {
        var rootDir = InputTokens.Read();
        _root = new Directory("/");
        var currentDirectory = _root;
        while (InputTokens.HasMoreTokens()) {
            var x = InputTokens.Read();
            if (x[0] == '$') { // command
                switch (x.Split(" ")[1]) {
                    case "cd":
                        Cd(ref currentDirectory, x.Split(" ")[2]);
                        break;
                    case "ls":
                        Ls(currentDirectory);
                        break;
                }
            }
        }

        _root.GetAllDirectories(_root, ref _directories);
        var sum = _directories.Where(d=>d.GetTotalSize() <= 100000).Sum(d=>d.GetTotalSize());
        Console.WriteLine(sum);
    }

    private void Ls(Directory currentDirectory) {
        while (InputTokens.HasMoreTokens()) {
            if (InputTokens.JustRead()[0] == '$') break;
            var lsL = InputTokens.Read();
            if (lsL.Split(" ")[0] == "dir") { // directory
                currentDirectory.AddDirectory(new Directory(lsL.Split(" ")[1]));
            }
            else { // file
                currentDirectory.AddFile(new File(lsL));
            }
        }
    }

    private void Cd(ref Directory currentDirectory, string dirName) {
        switch (dirName) {
            case "..":
                currentDirectory = currentDirectory.Parent;
                break;
            default:
                currentDirectory =
                    (currentDirectory.FileSystemElements.Single(sd => sd is Directory && sd.Name == dirName) as
                        Directory)!;
                break;
        }
    }

    public override void PuzzleTwo() {
        const int filesystemSize = 70000000;
        const int spaceNeeded = 30000000;
        int freeSpace = filesystemSize - _root.GetTotalSize();

       var size = _directories.Where(d => d.GetTotalSize() + freeSpace >= spaceNeeded).MinBy(d => d.GetTotalSize())
            .GetTotalSize();
       Console.WriteLine(size);
    }
}

public class Directory : IFileSystemElement {
    public string Name { get; set; }
    public List<IFileSystemElement> FileSystemElements { get; set; }
    public Directory Parent { get; set; }

    public void AddDirectory(Directory directory) {
        directory.Parent = this;
        FileSystemElements.Add(directory);
    }

    public void AddFile(File file) {
        file.Parent = this;
        FileSystemElements.Add(file);
    }

    public Directory(string name) {
        Name = name;
        FileSystemElements = new List<IFileSystemElement>();
    }

    public void GetAllDirectories(Directory directory, ref List<Directory> directories) {
        directories.Add(directory);
        foreach (var fileSystemElement in directory.FileSystemElements) {
            if (fileSystemElement is Directory d) {
                GetAllDirectories(d, ref directories);
            }
        }
    }

    public int GetTotalSize() {
        var totalSize = 0;
        foreach (var fileSystemElement in FileSystemElements) {
            if (fileSystemElement is Directory d) {
                totalSize += d.GetTotalSize();
            }
            else {
                totalSize += (fileSystemElement as File)!.Size;
            }
        }

        return totalSize;
    }
}

public class File : IFileSystemElement {
    public string Name { get; set; }
    public int Size { get; set; }
    public Directory Parent { get; set; }

    public File(string name, int size) {
        Name = name;
        Size = size;
    }

    public File(string lsLine) {
        Name = lsLine.Split(" ")[1];
        Size = int.Parse(lsLine.Split(" ")[0]);
    }
}

public interface IFileSystemElement {
    string Name { get; set; }
    Directory Parent { get; set; }
}