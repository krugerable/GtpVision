using System;
using System.IO;
using Microsoft.Extensions.Configuration;

public static class ConfigurationManager
{
    private static readonly IConfiguration _configuration;
    private static readonly string _apiKey;

    static ConfigurationManager()
    {
        // Load configuration from appsettings.json
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();

        // Try to load the API key from the configuration first
        _apiKey = _configuration["OpenAIApiKey"];

        // If the configuration value is not set, load the API key from an environment variable
        if (string.IsNullOrEmpty(_apiKey))
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        }

        // If the API key is still not available, throw an exception
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new Exception("API key is not configured.");
        }
    }

    public static string ApiKey => _apiKey;
}
