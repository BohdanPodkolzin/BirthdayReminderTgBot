using Newtonsoft.Json.Linq;

namespace BirthdayReminder.Telegram.Helpers
{
    public class StartBotHelper
    {
        private static HttpClient GetConfiguredHttpClient()
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp");
            return client;
        }

        public static async Task<(string? lat, string? lon)> GetLatitudeAndLongitudeFromApi(string city)
        {
            var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(city)}&format=json";
            using var client = GetConfiguredHttpClient();

            var response = await client.GetStringAsync(url);
            var results = JArray.Parse(response);

            var jsonResult = results[0];

            var lat = jsonResult["lat"]?.Value<string>();
            var lon = jsonResult["lon"]?.Value<string>();

            return (lat, lon);
        }

        public static async Task<string> GetPlaceInformation(string? lat, string? lon)
        {
            var url =
                $"https://api.timezonedb.com/v2.1/get-time-zone?key=7TVJUMUJ9LMG&format=json&by=position&lat={lat}&lng={lon}";
            using var client = GetConfiguredHttpClient();

            var response = await client.GetStringAsync(url);
            var results = JObject.Parse(response);

            var town = results["cityName"]?.Value<string>();
            var region = results["countryName"]?.Value<string>();
            var time = results["formatted"]?.Value<DateTime>();

            var message =
                $"City: {town}, {region}\n" +
                $"Time: {time}";

            return message;
        }
    }
}
