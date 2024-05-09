using System;
using System.IO;
using Microsoft.Extensions.Configuration;

// This class is responsible for loading and managing configuration settings, including the OpenAI API key.
public static class ConfigurationManager
{
    // Static instances are initialized in the static constructor
    private static readonly IConfiguration _configuration;
    private static readonly string _apiKey;

    static ConfigurationManager()
    {
        // Instantiate a ConfigurationBuilder
        var builder = new ConfigurationBuilder();

        // Set the base path to the current directory
        builder.SetBasePath(Directory.GetCurrentDirectory());

        // Add JSON file configuration source
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        // Build and instantiate the IConfiguration instance
        _configuration = builder.Build();

        // Load the API key from the configuration or environment variable
        _apiKey = _configuration["OpenAIApiKey"];

        if (string.IsNullOrEmpty(_apiKey))
        {
            _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        }

        if (string.IsNullOrEmpty(_apiKey))
        {
            // Instantiate a new Exception if the API key is not configured
            throw new Exception("API key is not configured.");
        }
    }

    public static string ApiKey => _apiKey;
}
