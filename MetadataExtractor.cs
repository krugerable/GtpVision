using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;

public class MetadataExtractor
{
    public static Dictionary<string, string> ExtractMetadata(string imagePath)
    {
        try
        {
            var directories = ImageMetadataReader.ReadMetadata(imagePath);
            return directories.SelectMany(directory => directory.Tags)
                              .ToDictionary(tag => tag.Name, tag => tag.Description ?? "N/A");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error extracting metadata: {ex.Message}");
            return new Dictionary<string, string>();
        }
    }
}
