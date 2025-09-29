using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SayIntentions
{



    public class SayIntentionsApiClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private const string BaseUrl = "https://apipri.sayintentions.ai/sapi/";

        public SayIntentionsApiClient([DisallowNull] string apiKey, HttpClient? httpClient = null)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            _http = httpClient ?? new HttpClient();
        }

        private async Task<T> GetAsync<T>(string endpoint, Dictionary<string, string>? parameters = null)
            where T : class, new()
        {
            var url = $"{BaseUrl}{endpoint}?api_key={_apiKey}";
            if (parameters != null)
            {
                foreach (var kv in parameters)
                    url += $"&{kv.Key}={Uri.EscapeDataString(kv.Value)}";
            }

            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
            catch
            {
                return new T(); // fallback empty object
            }
        }

        // === Communication ===
        public Task<SayAsResponse> SayAs(string channel, string message, bool rephrase = false,
                                         string? from = null, string? responseCode = null, string? messageType = null)
        {
            var p = new Dictionary<string, string>
            {
                ["channel"] = channel,
                ["message"] = message,
                ["rephrase"] = rephrase ? "1" : "0"
            };
            if (from != null) p["from"] = from;
            if (responseCode != null) p["response_code"] = responseCode;
            if (messageType != null) p["message_type"] = messageType;

            return GetAsync<SayAsResponse>("sayAs", p);
        }

        public Task<CommsHistoryResponse> GetCommsHistory() =>
            GetAsync<CommsHistoryResponse>("getCommsHistory");

        // === Weather ===
        public Task<WeatherResponse> GetWeather(string icao, bool withComms = false) =>
            GetAsync<WeatherResponse>("getWX", new Dictionary<string, string>
            {
                ["icao"] = icao,
                ["with_comms"] = withComms ? "1" : "0"
            });

        public Task<GeoJsonResponse> GetTFRs() => GetAsync<GeoJsonResponse>("getTFRs");
        public Task<GeoJsonResponse> GetVATSIM() => GetAsync<GeoJsonResponse>("getVATSIM");

        // === Gate & Parking ===
        public Task<AssignGateResponse> AssignGate(string gate, string airport) =>
            GetAsync<AssignGateResponse>("assignGate", new Dictionary<string, string>
            {
                ["gate"] = gate,
                ["airport"] = airport
            });

        public Task<ParkingResponse> GetParking() => GetAsync<ParkingResponse>("getParking");

        // === Frequencies ===
        public Task<FrequencyResponse> SetFrequency(double freq, int com = 1, string mode = "active") =>
            GetAsync<FrequencyResponse>("setFreq", new Dictionary<string, string>
            {
                ["freq"] = freq.ToString("F3"),
                ["com"] = com.ToString(),
                ["mode"] = mode
            });

        public Task<CurrentFrequenciesResponse> GetCurrentFrequencies() =>
            GetAsync<CurrentFrequenciesResponse>("getCurrentFrequencies");

        // === Flight Management ===
        public Task<FrequencyResponse> SetVar(string var, string value, string category = "L") =>
            GetAsync<FrequencyResponse>("setVar", new Dictionary<string, string>
            {
                ["var"] = var,
                ["value"] = value,
                ["category"] = category
            });

        public Task<FrequencyResponse> SetPause(bool pause) =>
            GetAsync<FrequencyResponse>("setPause", new Dictionary<string, string>
            {
                ["value"] = pause ? "1" : "0"
            });
    }
}

