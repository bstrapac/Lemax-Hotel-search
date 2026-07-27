using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using HotelSearch.Models;

namespace HotelSearch.ApiClient
{
    class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<List<Hotel>> FetchHotelsFromOsmAsync(double lat, double lng, int radius)
        {
            string overpassQuery = $@"
        [out:json][timeout:25];
        (
          node[""tourism""=""hotel""](around:{radius},{lat},{lng});
          way[""tourism""=""hotel""](around:{radius},{lat},{lng});
        );
        out center;";

            string overpassUrl = "https://overpass-api.de/api/interpreter";
            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("data", overpassQuery)
        });
            client.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpPetProjectApp/1.0");

            HttpResponseMessage response = await client.PostAsync(overpassUrl, content);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            return ParseOsmResponse(jsonResponse);
        }

        private static List<Hotel> ParseOsmResponse(string json)
        {
            var results = new List<Hotel>();
            using JsonDocument doc = JsonDocument.Parse(json);

            JsonElement root = doc.RootElement;
            if (!root.TryGetProperty("elements", out JsonElement elements))
                return results;

            foreach (JsonElement element in elements.EnumerateArray())
            {
                string name = "Unnamed Hotel";
                if (element.TryGetProperty("tags", out JsonElement tags))
                {
                    if (tags.TryGetProperty("name", out JsonElement nameProp))
                        name = nameProp.GetString();
                }

                double lat = 0, lon = 0;
                if (element.TryGetProperty("lat", out JsonElement latProp) && element.TryGetProperty("lon", out JsonElement lonProp))
                {
                    lat = latProp.GetDouble();
                    lon = lonProp.GetDouble();
                }
                else if (element.TryGetProperty("center", out JsonElement centerProp))
                {
                    lat = centerProp.GetProperty("lat").GetDouble();
                    lon = centerProp.GetProperty("lon").GetDouble();
                }

                results.Add(new Hotel(
                    name,
                    Random.Shared.Next(100, 500), // Price not available from OSM data
                    new Location { Lat = lat, Long = lon }));
            }

            return results;
        }
    }
}