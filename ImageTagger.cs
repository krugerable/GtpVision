using System;
using System.Threading.Tasks;

public class ImageTagger
{
    private readonly ApiCommunicator _apiCommunicator;

    public ImageTagger()
    {
        _apiCommunicator = new ApiCommunicator();
    }

    public async Task<TaggingResult> TagImageAsync(string imagePath)
    {
        try
        {
            var imageFile = new ImageFile(imagePath);
            imageFile.LoadMetadata();

            var apiResponse = await _apiCommunicator.SendImageAsync(imagePath);

            return new TaggingResult
            {
                ImagePath = imagePath,
                ApiResponse = apiResponse,
                Metadata = imageFile.Metadata
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to tag image: {ex.Message}");
            throw;
        }
    }
}
