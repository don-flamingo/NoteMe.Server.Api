using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Extensions;
using NoteMe.Common.Services.Json;
using NoteMe.Server.Api;

namespace NoteMe.Server.Tests.Integration.Fixtures
{
    public class BackendFixture
    {
        public HttpClient Client { get; }
        private WebApplicationFactory<Startup> _webApplicationFactory;
        private string _bearerFormat = "Bearer {0}";
        private string _token;

        public BackendFixture()
        {
            _webApplicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(webHost =>
                {
                    webHost.ConfigureServices(x => x.AddAutofac());
                });

            Client = _webApplicationFactory.CreateClient();
        }

        public async Task LoginAsync(string username, string password)
        {
            var loginCmd = new LoginCommand()
            {
                Id = Guid.NewGuid(),
                Email = username,
                Password = password
            };

            var jwt = await SendAsync<JwtDto>(HttpMethod.Post, "api/users/login", loginCmd);
            _token = jwt.Token;
        }

        public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string url, object body = null)
        {
            var request = new HttpRequestMessage();
            request.Method = method;
            request.RequestUri = new Uri(Client.BaseAddress, url);
            
            var hasToken = Client.DefaultRequestHeaders.Contains("Authorization");

            if (!_token.IsEmpty())
            {
                Client.DefaultRequestHeaders.Remove("Authorization");
                Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
            }

            if (body != null)
            {
                var json = JsonSerializeService.Serialize(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var httpResponseMessage = await Client.SendAsync(request);
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            httpResponseMessage.IsSuccessStatusCode.Should().BeTrue(responseString);

            return JsonSerializeService.Deserialize<TResponse>(responseString);
        }

        private static StringContent GetPayload(object data)
            => data.GetPayload(JsonSerializeService.CamelCaseContractResolver);

        private static TModel DeserializeObject<TModel>(string data)
            => JsonSerializeService.Deserialize<TModel>(data);
    }
}
