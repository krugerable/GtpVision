using System;

public static class ConfigurationManager
{
    private static readonly string _apiKey;

    static ConfigurationManager()
    {
        // Try to load the API key from an environment variable first
        _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        // If the environment variable is not set, load the API key from a configuration file
        if (string.IsNullOrEmpty(_apiKey))
        {
            _apiKey = LoadApiKeyFromConfigFile();
        }
    }

    public static string ApiKey => _apiKey;

    private static string LoadApiKeyFromConfigFile()
    {
        // Implement logic to load the API key from a configuration file
        // For example, you could use a JSON file or an app.config/web.config file
        // Return the loaded API key or throw an exception if it cannot be loaded
        throw new NotImplementedException("API key loading from configuration file is not implemented.");
    }
}
