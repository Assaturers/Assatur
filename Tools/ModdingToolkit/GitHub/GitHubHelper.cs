using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ModdingToolkit.Core;

namespace ModdingToolkit.GitHub
{
    public class GitHubHelper
    {
        public static async Task<ReleaseResponse> GetLatest(HttpClient client)
        {
            var response = await client.GetStringAsync($"https://api.github.com/repos/Assaturers/{Constants.AssaturName}/releases/latest");
            var release = JsonSerializer.Deserialize<ReleaseResponse>(response);

            return release;

            // For some reason, this line below doesn't work??? It's the same thing.
            // return client.GetFromJsonAsync<ReleaseResponse>("https://api.github.com/repos/Assaturers/Assatur/releases/latest");
        }

        public static async Task<Stream> DownloadLatest()
        {
            using var client = MakeClient();
            var release = await GetLatest(client);

            return await client.GetStreamAsync(new Uri(release.zipball_url));
        }

        public static HttpClient MakeClient()
        {
            return new()
            {
                DefaultRequestHeaders =
                {
                    {"User-Agent", "AssaturModdingToolkit"},
                    {"Accept", "application/vnd.github.v3+json"}
                },
            };
        }
    }
}