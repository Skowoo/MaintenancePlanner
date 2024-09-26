using IdentityService.FunctionalTests.Setup;
using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace IdentityService.FunctionalTests
{
    [TestClass]
    public class UserFunctionalities_OkTests : TestScenarioBase
    {
        [TestMethod]
        public void RegisterNewUser_ReturnsNewUserId()
        {
            string login = "TestUser",
                email = "test@email.pl",
                password = "Str0ngT3stP@ssw0rd";

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

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseContent = response.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(responseContent);
            Assert.IsTrue(Guid.TryParse(responseContent, out Guid _));
        }

        [TestMethod]
        public void Login_ReturnsOk() // To be refactorized
        {
            LoginModel loginModel = new(AdminLogin, AdminPassword);

            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"Login"),
                Method = HttpMethod.Get,
                Content = JsonContent.Create(loginModel)
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(responseContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(responseContent);            
            Assert.IsTrue(handler.CanReadToken(responseContent));
        }

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
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(AdminLogin, users[0].UserName);
        }

        [TestMethod]
        public void GetUserByUserName_ReturnsUser()
        {
            string userName = AdminLogin;

            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetUserByUserName?userName={userName}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<ApplicationUserExternalModel>(responseContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(user);
            Assert.AreEqual(AdminLogin, user.UserName);
        }

        [TestMethod]
        public void GetRoleByName_ReturnsOkAndRoleName()
        {
            string roleName = AdminRoleName;
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetRoleByName?roleName={roleName}"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var role = JsonConvert.DeserializeObject<IdentityRole>(responseContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(role);
            Assert.AreEqual(roleName, role.Name);
        }

        [TestMethod]
        public void GetAllRoles_ReturnsRolesList()
        {
            using var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"GetAllRoles"),
                Method = HttpMethod.Get
            };

            var response = client.SendAsync(request).Result;

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var roles = JsonConvert.DeserializeObject<List<IdentityRole>>(responseContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(roles);
            Assert.AreEqual(1, roles.Count);
            Assert.AreEqual(AdminRoleName, roles[0].Name);
        }

        [TestMethod]
        public void AddUserToRole_ShouldReturnOk()
        {
            RoleAssignChangeModel roleAssignChangeModel = new(NoRoleUserLogin, AdminRoleName);

            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"AddUserToRole"),
                Method = HttpMethod.Patch,
                Content = JsonContent.Create(roleAssignChangeModel)
            };

            var response = client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void RemoveUserFromRole_ShouldReturnOk()
        {
            RoleAssignChangeModel roleAssignChangeModel = new(AdminLogin, AdminRoleName);

            var client = GetClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri(IdentityServiceUri + $"RemoveUserFromRole"),
                Method = HttpMethod.Patch,
                Content = JsonContent.Create(roleAssignChangeModel)
            };

            var response = client.SendAsync(request).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
