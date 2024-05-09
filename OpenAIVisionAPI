using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenAIVisionAPI : IDisposable
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.openai.com/v1/chat/completions";

    public OpenAIVisionAPI()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> AnalyzeImageAsync(string imageUrl, int maxTokens = 300)
    {
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
        var messages = new List<object>();
        foreach (var url in imageUrls)
        {
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
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ConfigurationManager.ApiKey);

            var response = await _httpClient.PostAsync(BaseUrl, content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return jsonResponse;
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request exceptions
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            // Handle JSON parsing exceptions
            Console.WriteLine($"Error: Failed to parse JSON response. {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
