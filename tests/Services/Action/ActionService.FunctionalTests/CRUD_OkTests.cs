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
            string name = "New";
            string description = "new";
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(7);
            List<SparePartDto> parts = [new SparePartDto() { PartId = 1, Quantity = 5 }];

            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(ActionServiceUri + "CreateAction"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                },
                Content = JsonContent.Create(new CreateActionCommand(name, description, startDate, endDate, FirstEmployeeId, SecondEmployeeId, parts))
            };

            var response = client.SendAsync(request).Result;
            var returnedId = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, returnedId);

            var newItem = client.DownloadAction(returnedId);
            Assert.IsTrue(newItem is not null);

            Assert.AreEqual(name, newItem.Name);
            Assert.AreEqual(description, newItem.Description);
            Assert.AreEqual(startDate, newItem.StartDate);
            Assert.AreEqual(endDate, newItem.EndDate);
            Assert.AreEqual(FirstEmployeeId, newItem.CreatedBy);
            Assert.AreEqual(SecondEmployeeId, newItem.ConductedBy);
            Assert.AreEqual(1, parts.Count);
            Assert.AreEqual(parts.Count, newItem.Parts.Count);
            Assert.AreEqual(parts[0].PartId, newItem.Parts[0].PartId);
            Assert.AreEqual(parts[0].Quantity, newItem.Parts[0].Quantity);
        }

        [TestMethod]
        public void UpdateAction_ShouldReturnOKAndId()
        {
            string name = "Updated";
            string description = "Updated";
            DateTime startDate = DateTime.Now.AddDays(-5);
            DateTime endDate = DateTime.Now.AddDays(5);
            List<SparePartDto> parts = [new SparePartDto() { PartId = 1, Quantity = 2 }];

            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(ActionServiceUri + "UpdateAction"),
                Method = HttpMethod.Put,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                },
                Content = JsonContent.Create(new UpdateActionCommand(1, name, description, startDate, endDate, SecondEmployeeId, FirstEmployeeId, parts))
            };

            var response = client.SendAsync(request).Result;
            var returnedId = int.Parse(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, returnedId);

            var updatedItem = client.DownloadAction(returnedId);
            Assert.IsTrue(updatedItem is not null);

            Assert.AreEqual(name, updatedItem.Name);
            Assert.AreEqual(description, updatedItem.Description);
            Assert.AreEqual(startDate, updatedItem.StartDate);
            Assert.AreEqual(endDate, updatedItem.EndDate);
            Assert.AreEqual(SecondEmployeeId, updatedItem.CreatedBy);
            Assert.AreEqual(FirstEmployeeId, updatedItem.ConductedBy);
            Assert.AreEqual(1, parts.Count);
            Assert.AreEqual(parts.Count, updatedItem.Parts.Count);
            Assert.AreEqual(parts[0].PartId, updatedItem.Parts[0].PartId);
            Assert.AreEqual(parts[0].Quantity, updatedItem.Parts[0].Quantity);
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

            var checkDeleted = client.DownloadAction(1);
            Assert.IsTrue(checkDeleted is null);
        }
    }
}