using Desafio.Infra.CrossCutting.ViewModels.User;
using Desafio.WebAPI.ViewModels.Response;
using Desafio.WebAPI.ViewModels.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Desafio.IntegrationTest
{
    public class AuthControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        private readonly string hostApi = "https://localhost:44372/api/v1/";

        public AuthControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
            });
        }

        [Fact(DisplayName = "Register user with success")]
        [Trait("AuthControllerTest", "Auth Controller Tests")]
        public async Task RegisterWithSuccess()
        {
            // Arrange
            var viewModel = new RegisterUserViewModel();
            viewModel.Email = Faker.Internet.Email();
            viewModel.Password = "Admin@123";
            viewModel.ConfirmPassword = "Admin@123";

            // Act
            var client = _factory.CreateClient();
            var response = await client.PostAsync($"{hostApi}auth/register",
                new StringContent(JsonConvert.SerializeObject(viewModel), System.Text.Encoding.UTF8, "application/json"));
            string json = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json.ToString());
            LoginResponseViewModel userData = JsonConvert.DeserializeObject<LoginResponseViewModel>(data.data.ToString());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(userData);
            Assert.NotNull(userData?.UserToken);
            Assert.Equal(viewModel.Email, userData?.UserToken.Email);
            Assert.True(userData?.ExpiresIn > 0);
            Assert.True(userData?.AccessToken.Length > 0);
            Assert.True(userData?.UserToken.Id.Length > 0);
            Assert.True(userData?.UserToken.Claims.ToList().Count() > 0);
        }

        [Fact(DisplayName = "Register user with error")]
        [Trait("AuthControllerTest", "Auth Controller Tests")]
        public async Task RegisterWithError()
        {
            // Arrange
            var viewModel = new RegisterUserViewModel();
            viewModel.Email = Faker.Internet.Email();
            viewModel.Password = "Admin@123";
            viewModel.ConfirmPassword = "Admin@1234";


            // Act
            var client = _factory.CreateClient();
            var response = await client.PostAsync($"{hostApi}auth/register",
                new StringContent(JsonConvert.SerializeObject(viewModel), System.Text.Encoding.UTF8, "application/json"));
            string json = await response.Content.ReadAsStringAsync();
            ResponseErrorViewModel data = JsonConvert.DeserializeObject<ResponseErrorViewModel>(json.ToString());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(data);
            Assert.False(data.success);
            Assert.True(data.errors.ToList().Count > 0);
        }

        [Fact(DisplayName = "Login user with error")]
        [Trait("AuthControllerTest", "Auth Controller Tests")]
        public async Task LoginWithError()
        {
            // Arrange
            var viewModel = new LoginUserViewModel();
            viewModel.Email = Faker.Internet.Email();
            viewModel.Password = "Admin@123";

            // Act
            var client = _factory.CreateClient();
            var response = await client.PostAsync($"{hostApi}auth/login",
                new StringContent(JsonConvert.SerializeObject(viewModel), System.Text.Encoding.UTF8, "application/json"));
            string json = await response.Content.ReadAsStringAsync();
            ResponseErrorViewModel data = JsonConvert.DeserializeObject<ResponseErrorViewModel>(json.ToString());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(data);
            Assert.False(data.success);
            Assert.True(data.errors.ToList().Count > 0);
        }

        [Fact(DisplayName = "Login user with success")]
        [Trait("AuthControllerTest", "Auth Controller Tests")]
        public async Task LoginWithSuccess()
        {
            // Arrange
            var registerUserViewModel = new RegisterUserViewModel();
            registerUserViewModel.Email = Faker.Internet.Email();
            registerUserViewModel.Password = "Admin@123";
            registerUserViewModel.ConfirmPassword = "Admin@123";

            var client = _factory.CreateClient();
            var responseRegister = await client.PostAsync($"{hostApi}auth/register",
                new StringContent(JsonConvert.SerializeObject(registerUserViewModel), System.Text.Encoding.UTF8, "application/json"));
            string jsonRegister = await responseRegister.Content.ReadAsStringAsync();

            var loginUserViewModel = new LoginUserViewModel();
            loginUserViewModel.Email = registerUserViewModel.Email;
            loginUserViewModel.Password = "Admin@123";

            // Act
            var responseLogin = await client.PostAsync($"{hostApi}auth/login",
                new StringContent(JsonConvert.SerializeObject(loginUserViewModel), System.Text.Encoding.UTF8, "application/json"));
            string jsonLogin = await responseLogin.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(jsonLogin.ToString());
            Desafio.Infra.CrossCutting.ViewModels.User.LoginResponseViewModel userData = JsonConvert.DeserializeObject<Desafio.Infra.CrossCutting.ViewModels.User.LoginResponseViewModel>(data.data.ToString());

            // Assert
            Assert.Equal(HttpStatusCode.OK, responseRegister.StatusCode);
            Assert.Equal(HttpStatusCode.OK, responseLogin.StatusCode);
            Assert.NotNull(userData);
            Assert.NotNull(userData?.UserToken);
            Assert.Equal(loginUserViewModel.Email, userData?.UserToken.Email);
            Assert.True(userData?.ExpiresIn > 0);
            Assert.True(userData?.AccessToken.Length > 0);
            Assert.True(userData?.UserToken.Id.Length > 0);
            Assert.True(userData?.UserToken.Claims.ToList().Count() > 0);
        }
    }
}
