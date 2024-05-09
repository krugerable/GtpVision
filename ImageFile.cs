public class ImageFile
{
    public string FilePath { get; }
    public Dictionary<string, string> Metadata { get; private set; }

    public ImageFile(string filePath)
    {
        FilePath = filePath;
        Metadata = new Dictionary<string, string>();
    }

    public void LoadMetadata()
    {
        Metadata = MetadataExtractor.ExtractMetadata(FilePath);
    }
}
