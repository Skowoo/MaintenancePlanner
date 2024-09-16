using ActionService.FunctionalTests.Setup;
using ActionServiceAPI.Application.Action.Commands.CreateActionCommand;
using ActionServiceAPI.Application.Action.Commands.DeleteActionCommand;
using ActionServiceAPI.Application.Action.Commands.UpdateActionCommand;
using ActionServiceAPI.Application.DataTransferObjects.Models;
using Newtonsoft.Json;
using System.Net;

namespace ActionService.FunctionalTests
{
    [TestClass]
    public class CRUD_OkTests : TestScenarioBase
    {
        const string ActionServiceUri = "https://localhost:7120/api/v1/Action/";

        [TestMethod]
        public void GetAllActions_ShouldReturnOK()
        {
            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(ActionServiceUri + "GetAllActions"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var returnedItem = JsonConvert.DeserializeObject<List<ActionDto>>(responseContent);
            Assert.IsTrue(returnedItem is not null);
            Assert.AreEqual(1, returnedItem.Count);
        }

        [TestMethod]
        public void GetAction_ShouldReturnExistingItem()
        {
            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(ActionServiceUri + "GetAction/1"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var returnedItem = JsonConvert.DeserializeObject<ActionDto>(responseContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(returnedItem is not null);
            Assert.IsTrue(returnedItem.Id == 1);
        }

        [TestMethod]
        public void CreateAction_ShouldReturnOKAndId()
        {
            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(ActionServiceUri + "CreateAction"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                },
                Content = JsonContent.Create(new CreateActionCommand("New", "new", DateTime.Now, DateTime.Now, "TestEmployee", "TestEmployee", [new SparePartDto() { PartId = 1, Quantity = 5 }]))
            };

            var response = client.SendAsync(request).Result;
            var returnedId = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, returnedId);
        }

        [TestMethod]
        public void UpdateAction_ShouldReturnOKAndId()
        {
            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(ActionServiceUri + "UpdateAction"),
                Method = HttpMethod.Put,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                },
                Content = JsonContent.Create(new UpdateActionCommand(1, "Updated", "Updated", DateTime.Now, DateTime.Now, "TestEmployee", "TestEmployee", [new SparePartDto() { PartId = 1, Quantity = 2 }]))
            };

            var response = client.SendAsync(request).Result;
            var returnedId = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, returnedId);
        }

        [TestMethod]
        public void DeleteAction_ShouldReturnOKAndId()
        {
            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(ActionServiceUri + "DeleteAction/1"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                },
                Content = JsonContent.Create(new DeleteActionCommand(1))
            };

            var response = client.SendAsync(request).Result;
            var returnedId = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, returnedId);
        }
    }
}