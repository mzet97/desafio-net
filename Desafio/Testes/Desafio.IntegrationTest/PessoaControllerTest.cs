using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Desafio.WebAPI.ViewModels.Pessoa;
using Microsoft.AspNetCore.Hosting;
using Desafio.WebAPI.ViewModels.User;

namespace Desafio.IntegrationTest
{
    public class PessoaControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        private readonly string hostApi = "https://localhost:44372/api/v1/";

        public PessoaControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
            });
        }

        [Fact(DisplayName = "Register Pessoa with success")]
        [Trait("PessoaControllerTest", "Pessoa Controller Tests")]
        public async Task RegisterWithSuccess()
        {
            // Arrange
            var tokenAdmin = await this.GetTokenAdmin();
            var pessoaGere = new GenerateDataTest();
            var viewModel = pessoaGere.GetValidCPF();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                         tokenAdmin);

            // Act
            var response = await client.PostAsync($"{hostApi}pessoa",
                new StringContent(JsonConvert.SerializeObject(viewModel), System.Text.Encoding.UTF8, "application/json"));
            string json = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json.ToString());
            PessoaCreatedViewModel pessoa = JsonConvert.DeserializeObject<PessoaCreatedViewModel>(data.data.ToString());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(pessoa);
            Assert.Equal(viewModel.Nome, pessoa.Nome);
            Assert.Equal(viewModel.Sobrenome, pessoa.Sobrenome);
            Assert.Equal(viewModel.DataNascimento, pessoa.DataNascimento);
            Assert.Equal(viewModel.Email, pessoa.Email);
            Assert.Equal(viewModel.Telefone, pessoa.Telefone);
            Assert.Equal(viewModel.Endereco, pessoa.Endereco);
            Assert.Equal(viewModel.Cidade, pessoa.Cidade);
            Assert.Equal(viewModel.Estado, pessoa.Estado);
            Assert.Equal(viewModel.CEP, pessoa.CEP);
            Assert.Equal(viewModel.CPFCNPJ, pessoa.CPFCNPJ);
        }

        [Fact(DisplayName = "Register Pessoa with Failure")]
        [Trait("PessoaControllerTest", "Pessoa Controller Tests")]
        public async Task RegisterWithFailure()
        {
            // Arrange
            var tokenAdmin = await this.GetTokenAdmin();
            var viewModel = new PessoaCreatedViewModel();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                         tokenAdmin);

            // Act
            var response = await client.PostAsync($"{hostApi}pessoa",
                new StringContent(JsonConvert.SerializeObject(viewModel), System.Text.Encoding.UTF8, "application/json"));
            string json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact(DisplayName = "Get Pessoa with success")]
        [Trait("PessoaControllerTest", "Pessoa Controller Tests")]
        public async Task GetWithSuccess()
        {
            // Arrange
            var tokenAdmin = await this.GetTokenAdmin();
            var pessoaGere = new GenerateDataTest();
            var viewModel = pessoaGere.GetValidCPF();

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                         tokenAdmin);

            // Act
            var response = await client.GetAsync($"{hostApi}pessoa");
            string json = await response.Content.ReadAsStringAsync();
            JArray jsonArray = JArray.Parse(json);
            dynamic data = JObject.Parse(jsonArray[0].ToString());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(data);
        }

        [Fact(DisplayName = "Get Pessoa with Failure")]
        [Trait("PessoaControllerTest", "Pessoa Controller Tests")]
        public async Task GetWithFailure()
        {
            // Arrange
            var tokenAdmin = await this.GetTokenAdmin();
            var pessoaGere = new GenerateDataTest();
            var viewModel = pessoaGere.GetValidCPF();

            var client = _factory.CreateClient();
            
            // Act
            var response = await client.GetAsync($"{hostApi}pessoa");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        public async Task<string> GetTokenAdmin()
        {
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

            var responseLogin = await client.PostAsync($"{hostApi}auth/login",
                new StringContent(JsonConvert.SerializeObject(loginUserViewModel), System.Text.Encoding.UTF8, "application/json"));
            string jsonLogin = await responseLogin.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(jsonLogin.ToString());
            Desafio.Infra.CrossCutting.ViewModels.User.LoginResponseViewModel userData = JsonConvert.DeserializeObject<Desafio.Infra.CrossCutting.ViewModels.User.LoginResponseViewModel>(data.data.ToString());

            return userData.AccessToken;
        }
    }
}
