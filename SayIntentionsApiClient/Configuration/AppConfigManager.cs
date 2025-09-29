using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SayIntentionsApiClient.Configuration
{
    public sealed class AppConfigManager
    {
        private static readonly Lazy<AppConfigManager> _instance = new(() => new AppConfigManager());
        private readonly Dictionary<string, string> _configValues = new();
        private readonly object _lock = new();
        private bool _isLoaded = false;
        private string _configPath = "";

        public static AppConfigManager Instance => _instance.Value;

        private AppConfigManager() { }

        public void LoadOrCreate(string configFilePath, Dictionary<string, string> defaultValues, IEnumerable<string> requiredKeys)
        {
            lock (_lock)
            {
                if (_isLoaded)
                    throw new InvalidOperationException("Configuration already loaded.");

                _configPath = Path.IsPathRooted(configFilePath)
                    ? configFilePath
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFilePath);

                Console.WriteLine(_configPath);

                try
                {
                    var dir = Path.GetDirectoryName(_configPath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    if (!File.Exists(_configPath))
                    {
                        File.WriteAllText(_configPath, JsonSerializer.Serialize(defaultValues, new JsonSerializerOptions { WriteIndented = true }));
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException($"Failed to create config file at '{_configPath}': {ex.Message}", ex);
                }

                Dictionary<string, string>? dict;
                try
                {
                    var json = File.ReadAllText(_configPath);
                    dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException($"Failed to read or parse config file '{_configPath}': {ex.Message}", ex);
                }

                if (dict == null || dict.Count == 0)
                    throw new InvalidDataException($"Config file '{_configPath}' is empty or invalid.");

                foreach (var key in requiredKeys)
                {
                    if (!dict.ContainsKey(key))
                        throw new InvalidDataException($"Missing required config key: '{key}'");
                }

                foreach (var kvp in dict)
                    _configValues[kvp.Key] = kvp.Value;

                _isLoaded = true;
            }
        }



        public string Get(string key)
        {
            lock (_lock)
            {
                if (!_isLoaded)
                    throw new InvalidOperationException("Configuration not loaded.");

                if (!_configValues.TryGetValue(key, out var value))
                    throw new KeyNotFoundException($"Config key '{key}' not found.");

                return value;
            }
        }

        public void Set(string key, string value)
        {
            lock (_lock)
            {
                if (!_isLoaded)
                    throw new InvalidOperationException("Configuration not loaded.");

                _configValues[key] = value;
            }
        }

        public void Save()
        {
            lock (_lock)
            {
                if (!_isLoaded)
                    throw new InvalidOperationException("Configuration not loaded.");

                if (string.IsNullOrWhiteSpace(_configPath))
                    throw new InvalidOperationException("Config path is not set.");

                try
                {
                    File.WriteAllText(_configPath, JsonSerializer.Serialize(_configValues, new JsonSerializerOptions { WriteIndented = true }));
                }
                catch (Exception ex)
                {
                    throw new IOException($"Failed to save config to '{_configPath}': {ex.Message}", ex);
                }
            }
        }

        public IReadOnlyDictionary<string, string> Snapshot()
        {
            lock (_lock)
            {
                return new Dictionary<string, string>(_configValues);
            }
        }
    }
}
