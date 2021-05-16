using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System;
using world_time_app.Models;
using Microsoft.Extensions.Options;

namespace world_time_app.Services
{
    public interface ITimezone
    {
        Task<List<string>> GetTimezonesAsync();
        Task<Time> GetTimeAsync();
    }

    public class TimezonesService : ITimezone
    {
        private const string baseUrl = "http://worldtimeapi.org/api";
        private HttpClient _client;
        private readonly IOptions<TimezoneModel> _timezoneModel;

        public TimezonesService(HttpClient client, IOptions<TimezoneModel> timezoneModel)
        {
            _client = client;
            _timezoneModel = timezoneModel;
        }

        public async Task<List<string>> GetTimezonesAsync()
        {
            var response = await _client.GetAsync(baseUrl + "/timezone");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(jsonString);
        }

        public async Task<Time> GetTimeAsync()
        {
            (string area, string location, string region) = GetAppSettings();
            string url = string.Format("{0}/{1}/{2}", baseUrl, area, location);
            url = string.IsNullOrEmpty(region) ? url : string.Format("{0}/{1}", url, region);
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var jsonString = await response.Content.ReadAsStringAsync();
            var time = JsonSerializer.Deserialize<Time>(jsonString);
            (time.Area, time.Location, time.Region) = (area, location, region);
            return time;
        }

        private (string, string, string) GetAppSettings()
        {
            return (_timezoneModel.Value.Area,
                    _timezoneModel.Value.Location,
                    _timezoneModel.Value.Region);
        }
    }
}