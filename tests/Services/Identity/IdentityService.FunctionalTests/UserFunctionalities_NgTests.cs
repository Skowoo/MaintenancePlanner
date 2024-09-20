using IdentityService.FunctionalTests.Setup;
using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net;

namespace IdentityService.FunctionalTests
{
    [TestClass]
    public class UserFunctionalities_NgTests : TestScenarioBase
    {
        const string ValidLogin = "TestLogin";
        const string ValidEmail = "test@test.pl";
        const string ValidPassword = "T3sting1!";

        [DataTestMethod] // Test cases to be refactorized with final identity specification
        [DataRow(AdminLogin, ValidEmail, ValidPassword, "DuplicateUserName")]
        [DataRow(ValidLogin, ValidEmail, "Aa1@", "PasswordTooShort")]
        [DataRow(ValidLogin, ValidEmail, "noNoAlphanumr1c", "PasswordRequiresNonAlphanumeric")]
        [DataRow(ValidLogin, ValidEmail, "nouppercase1!", "PasswordRequiresUpper")]
        [DataRow(ValidLogin, ValidEmail, "NOLOWERCASE1!", "PasswordRequiresLower")]
        [DataRow(ValidLogin, ValidEmail, "111@@@@@@1!", "PasswordRequiresLower", "PasswordRequiresUpper")]
        public void RegisterNewUser_ReturnsBadRequestAndIdentityErrors(string login, string email, string password, params string[] expectedErrors)
        {
            var client = GetClient();
            RegisterModel registerModel = new()
            {
                Login = login,
                Email = email,
                Password = password
            };

            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"RegisterNewUser"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                },
                Content = JsonContent.Create(registerModel)
            };

            var response = client.SendAsync(request).Result;            
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var errors = JsonConvert.DeserializeObject<List<IdentityError>>(responseContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(errors);            

            foreach (var expectedError in expectedErrors)
                Assert.IsTrue(errors.Any(e => e.Code.Equals(expectedError)), $"{expectedError} not found");

            Assert.AreEqual(expectedErrors.Length, errors.Count);
        }

        [DataTestMethod] // To be refactorized with service - change test to use code and service to operate on original Identity codes
        [DataRow("NotExisting", AdminPassword, "not found")]
        [DataRow(AdminLogin, "Wr0ngP@sw0rd", "password")]
        public void Login_ReturnsBadRequestAndIdentityErrorsWithWrongData(string login, string password, params string[] expectedErrorsPartialDescription) // To be refactorized 
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"Login?login={login}&password={password}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var errors = JsonConvert.DeserializeObject<List<IdentityError>>(responseContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(errors);

            foreach (var expectedError in expectedErrorsPartialDescription)
                Assert.IsTrue(errors.Any(e => e.Description.Contains(expectedError)), $"{expectedError} not found");

            Assert.AreEqual(expectedErrorsPartialDescription.Length, errors.Count);
        }

        [DataTestMethod] // To be refactorized with service - change test to use code and service to operate on original Identity codes
        [DataRow("", AdminPassword, "login")]
        [DataRow("", "", "login", "password")]
        [DataRow(AdminLogin, "", "password")]
        public void Login_ReturnsBadRequestAndErrorsWithMissingData(string login, string password, params string[] expectedErrorsPartialDescription) // To be refactorized 
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"Login?login={login}&password={password}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(responseContent);

            foreach (var expectedError in expectedErrorsPartialDescription)
                Assert.IsTrue(responseContent.Contains(expectedError), $"{expectedError} not found");
        }

        [DataTestMethod]
        [DataRow("NotExistingUser")]
        public void GetUserByUserName_ReturnsBadRequestAndInputStringWithWrongData(string userName)
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetUserByUserName?userName={userName}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsNotNull(responseContent);
            Assert.AreEqual(userName, responseContent);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)] 
        public void GetUserByUserName_ReturnsBadRequestAndInputStringWithMissingData(string userName)
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetUserByUserName?userName={userName}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(responseContent);
            Assert.IsTrue(responseContent.Contains("userName"));
        }

        [DataTestMethod]
        [DataRow("NotExistingRoleName")]
        public void GetRoleByName_ReturnsNotFoundAndRoleName(string roleName)
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetRoleByName?roleName={roleName}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsNotNull(responseContent);
            Assert.AreEqual(roleName, responseContent);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void GetRoleByName_ReturnsBadRequest(string roleName)
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetRoleByName?roleName={roleName}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(responseContent);
            Assert.IsTrue(responseContent.Contains("roleName"));
        }

        [DataTestMethod]
        [DataRow(AdminLogin, AdminRoleName, "UserAlreadyInRole")]
        public void AddUserToRole_ShouldReturnBadRequestAndIdentityErrors(string userName, string roleName, params string[] expectedErrors)
        {
            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"AddUserToRole?userName={userName}&roleName={roleName}"),
                Method = HttpMethod.Patch
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var errors = JsonConvert.DeserializeObject<List<IdentityError>>(responseContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(errors);

            foreach (var expectedError in expectedErrors)
                Assert.IsTrue(errors.Any(e => e.Code.Equals(expectedError)), $"{expectedError} not found");

            Assert.AreEqual(expectedErrors.Length, errors.Count);
        }

        [DataTestMethod]
        [DataRow(NoRoleUserLogin, AdminRoleName, "UserNotInRole")]
        public void RemoveUserFromRole_ShouldReturnBadRequestAndIdentityErrors(string userName, string roleName, params string[] expectedErrors)
        {
            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"RemoveUserFromRole?userName={userName}&roleName={roleName}"),
                Method = HttpMethod.Patch
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var errors = JsonConvert.DeserializeObject<List<IdentityError>>(responseContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(errors);

            foreach (var expectedError in expectedErrors)
                Assert.IsTrue(errors.Any(e => e.Code.Equals(expectedError)), $"{expectedError} not found");

            Assert.AreEqual(expectedErrors.Length, errors.Count);
        }
    }
}
