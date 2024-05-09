using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// This class contains the logic for interacting with the OpenAI Vision API.
public class OpenAIVisionAPI : IDisposable
{
    // Instantiate a private instance of HttpClient
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.openai.com/v1/chat/completions";

    // Instantiate HttpClient in the constructor
    public OpenAIVisionAPI()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> AnalyzeImageAsync(string imageUrl, int maxTokens = 300)
    {
        // Instantiate a new list for the payload
        var payload = CreatePayload(new List<string> { imageUrl }, maxTokens);
        return await PostRequestAsync(payload).ConfigureAwait(false);
    }

    public async Task<string> AnalyzeImagesAsync(List<string> imageUrls, int maxTokens = 300)
    {
        var payload = CreatePayload(imageUrls, maxTokens);
        return await PostRequestAsync(payload).ConfigureAwait(false);
    }

    private string CreatePayload(List<string> imageUrls, int maxTokens)
    {
        // Instantiate a new list to store the messages
        var messages = new List<object>();
        foreach (var url in imageUrls)
        {
            // Instantiate an anonymous object for each message
            messages.Add(new
            {
                role = "user",
                content = new
                {
                    type = "image_url",
                    image_url = new { url }
                }
            });
        }

        // Instantiate an anonymous object for the request object
        var requestObject = new
        {
            model = "gpt-4-vision-preview",
            messages,
            max_tokens = maxTokens
        };

        return JsonSerializer.Serialize(requestObject);
    }

    private async Task<string> PostRequestAsync(string payload)
    {
        try
        {
            // Instantiate a new StringContent for the request body
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ConfigurationManager.ApiKey);

            var response = await _httpClient.PostAsync(BaseUrl, content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                // Instantiate a new HttpRequestException if the request fails
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return jsonResponse;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error: Failed to parse JSON response. {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
