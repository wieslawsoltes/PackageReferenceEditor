using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PackageReferenceEditor
{
    public static class NuGetApi
    {
        public static async Task<string> GetJson(string uriString)
        {
            Logger.Log($"GetJson: {uriString}");
            var client = new HttpClient();
            client.BaseAddress = new Uri(uriString);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Logger.Log($"{(int)response.StatusCode} ({response.ReasonPhrase})");
            }
            return null;
        }

        public static async Task<IList<string>> GetPackageVersions(string urlIndex, string packageName)
        {
            Logger.Log($"GetPackageVersions: {urlIndex}");
            var jsonIndex = await GetJson(urlIndex);
            if (jsonIndex != null)
            {
                JObject objectIndex = JsonConvert.DeserializeObject<JObject>(jsonIndex);
                var urlTemplate = (string)objectIndex["resources"].FirstOrDefault(x => (string)x["@type"] == "PackageBaseAddress/3.0.0")["@id"];
                Logger.Log($"Template: {urlTemplate}");

                var urlVersions = $"{urlTemplate}{packageName}/index.json";
                Logger.Log($"Versions: {urlVersions}");
                var jsonVersions = await GetJson(urlVersions);
                if (jsonVersions != null)
                {
                    JObject objectVersions = JsonConvert.DeserializeObject<JObject>(jsonVersions);
                    var versions = objectVersions["versions"].Select(x => (string)x).ToList();
                    Logger.Log($"Latest Version: {packageName}: {versions.LastOrDefault()}");
                    return versions;
                }
            }
            return null;
        }
    }
}
