using ActionServiceAPI.Application.DataTransferObjects.Models;
using Newtonsoft.Json;
using System.Net;

namespace ActionService.FunctionalTests.Setup
{
    public static class ActionApiHttpClientExtensions
    {
        public static ActionDto? DownloadAction(this HttpClient client, int id)
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(TestScenarioBase.ActionServiceUri + $"GetAction/{id}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseContent = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ActionDto>(responseContent);
        }
    }
}
