using Data.Models;
using Newtonsoft.Json;
using SpotifyApi.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SpotifyService : ISpotifyService
    {
        public async Task<TrackModel> GetTrackDetailsByISRCAsync(string isrc)
        {
            var token = await GetNewToken();
            var trackDetails = await GetTrackDetailsByISRC(token, isrc);
            return trackDetails;
        }

        private async Task<string> GetNewToken()
        {
            using (var httpClient = new HttpClient())
            {
                IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>
                    {
                        { new KeyValuePair<string, string>("grant_type", "client_credentials") }
                    };
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic YzhjNDliNmExMWVhNDY5MWE4OWNiZjRiODdlZDJlMDY6YzhmODJkYjRmNDQ2NGE2MDk3YmU5YmM1NjIyYTUyYzg=");
                using (var response = await httpClient.PostAsync("https://accounts.spotify.com/api/token", new FormUrlEncodedContent(nameValueCollection)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<TokenModel>(apiResponse);
                        return responseData.access_token;
                    }
                    return string.Empty;
                }
            }
        }

        private async Task<TrackModel> GetTrackDetailsByISRC(string token, string isrc)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
                using (var response = await httpClient.GetAsync(string.Format("https://api.spotify.com/v1/search?q=isrc:{0}&type=track", isrc)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<SearchResult>(apiResponse);

                        if (responseData == null || responseData.Tracks == null || responseData.Tracks.Items.Length.Equals(0))
                        {
                            return null;
                        }

                        var result = new TrackModel()
                        {
                            Name = responseData.Tracks.Items[0].Name,
                            duration_ms = responseData.Tracks.Items[0].DurationMs,
                            is_explicit = responseData.Tracks.Items[0].Explicit,
                            ISRC = isrc
                        };
                        return result;
                    }
                    return null;
                }
            }
        }

        public async Task<List<TrackModel>> GetTrackDetailsByTitle(string title)
        {
            var token = await GetNewToken();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", token));
                using (var response = await httpClient.GetAsync(string.Format("https://api.spotify.com/v1/search?q={0}&type=track", title)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<SearchResult>(apiResponse);
                        if (responseData == null || responseData.Tracks == null || responseData.Tracks.Items.Length.Equals(0))
                        {
                            return null;
                        }

                        var result = new List<TrackModel>();
                        foreach (Track track in responseData.Tracks.Items)
                        {
                            result.Add(new TrackModel()
                            {
                                Name = responseData.Tracks.Items[0].Name,
                                duration_ms = responseData.Tracks.Items[0].DurationMs,
                                is_explicit = responseData.Tracks.Items[0].Explicit
                            });
                        }

                        return result;
                    }
                    return null;
                }
            }
        }
    }
}
