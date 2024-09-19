using IdentityService.FunctionalTests.Setup;
using IdentityServiceAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace IdentityService.FunctionalTests
{
    [TestClass]
    public class UserFunctionalities_OkTests : TestScenarioBase
    {
        [TestMethod]
        public void GetAllUsers_ReturnsUsersList()
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetAllUsers"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<ApplicationUserExternalModel>>(responseContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(users);
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(AdminLogin, users[0].UserName);
        }
    }
}
