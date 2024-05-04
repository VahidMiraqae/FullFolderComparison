var oldPath = args[0];
var newPath = args[1];
var outputPath = args[2];

var oldFiles = Directory.GetFiles(oldPath, "*", SearchOption.AllDirectories)
    .Select(a => a.Replace(oldPath, "")[1..]).ToHashSet();
var newFiles = Directory.GetFiles(newPath, "*", SearchOption.AllDirectories)
    .Select(a => a.Replace(newPath, "")[1..]);

var filesToCopy = new List<string>();

foreach (var newFile in newFiles)
{
    var newFilePath = Path.Combine(newPath, newFile);
    if (oldFiles.Contains(newFile))
    {
        var oldFilePath = Path.Combine(oldPath, newFile);
        var oldBytes = File.ReadAllBytes(oldFilePath);
        var newBytes = File.ReadAllBytes(newFilePath);
        var result = oldBytes.SequenceEqual(newBytes);
        if (!result)
        {
            filesToCopy.Add(newFilePath);
        }
    }
    else
    {
        filesToCopy.Add(newFilePath);
    }
}

if (Directory.Exists(outputPath) &&
    (Directory.GetDirectories(outputPath).Length != 0 || Directory.GetFiles(outputPath).Length != 0))
{
    Directory.Delete(outputPath, true);
    Directory.CreateDirectory(outputPath);
}

foreach (var toCopy in filesToCopy)
{
    var relativePath = toCopy.Replace(newPath, "")[1..];
    var targetPath = Path.Combine(outputPath, relativePath);
    var dir = Path.GetDirectoryName(targetPath);
    _ = Directory.CreateDirectory(dir);
    File.Copy(toCopy, targetPath);
}
