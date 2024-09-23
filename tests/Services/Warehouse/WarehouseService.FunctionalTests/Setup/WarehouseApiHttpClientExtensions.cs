using Newtonsoft.Json;
using System.Net;
using WarehouseServiceAPI.Models;


namespace WarehouseService.FunctionalTests.Setup
{
    public static class WarehouseApiHttpClientExtensions
    {
        public static Part? DownloadPart(this HttpClient client, int id)
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(TestingScenarioBase.WarehouseServiceUri + $"GetPart/{id}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseContent = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Part>(responseContent);
        }
    }
}
