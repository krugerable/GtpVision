using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class ApiCommunicator
{
    private readonly HttpClient _httpClient;

    public ApiCommunicator()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> SendImageAsync(string imagePath)
    {
        try
        {
            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(System.IO.File.ReadAllBytes(imagePath)), "image", "image.jpg" }
            };

            var response = await _httpClient.PostAsync("Your API Endpoint", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw;
        }
    }
}
