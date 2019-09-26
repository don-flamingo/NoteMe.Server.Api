using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NoteMe.Common.Commands;
using NoteMe.Common.Dtos;
using NoteMe.Common.Extensions;
using NoteMe.Common.Services.Json;
using NoteMe.Server.Api;

namespace NoteMe.Server.Tests.Integration.Fixtures
{
    public class BackendFixture
    {
        private readonly HttpClient _client;
        private readonly TestServer _testServer;

        public JwtDto Jwt { get; private set; }

        public BackendFixture()
        {
            _testServer = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development"));

            _client = _testServer.CreateClient();
        }

        public Task LoginAsAdmin()
            => LoginToApi("Administrator@gmail.com", "admin123");

        public async Task LoginToApi(string login, string password)
        {
            var command = new LoginCommand()
            {
                Email = login,
                Password = password
            };

            Jwt = await PostAsync<JwtDto>("/api/login", command);
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", Jwt.Token);
        }

        public async Task FailLoginToApi(string login, string password)
        {
            var command = new LoginCommand()
            {
                Email = login,
                Password = password
            };

            Jwt = await PostAsync<JwtDto>("/api/login", command, HttpStatusCode.Unauthorized);
        }

        public async Task<TModel> GetAsync<TModel>(string url, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await GetAsync(url, expectedCode);

            var json = await response.Content.ReadAsStringAsync();
            return DeserializeObject<TModel>(json);
        }

        public async Task<HttpResponseMessage> GetAsync(string url, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await _client.GetAsync(url);
            response.StatusCode.Should().Be(expectedCode, await response.Content.ReadAsStringAsync());
            return response;
        }

        public async Task<TModel> PostAsync<TModel>(string url, object command,
            HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await PostAsync(url, command, expectedCode);
            var json = await response.Content.ReadAsStringAsync();
            return DeserializeObject<TModel>(json);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object command,
            HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var payload = GetPayload(command);
            var response = await _client.PostAsync(url, payload);
            response.StatusCode.Should().Be(expectedCode, await response.Content.ReadAsStringAsync());

            return response;
        }

        public async Task<TModel> PutAsync<TModel>(string url, object command,
            HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await PutAsync(url, command, expectedCode);

            var json = await response.Content.ReadAsStringAsync();
            return DeserializeObject<TModel>(json);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object command,
            HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var payload = GetPayload(command);
            var response = await _client.PutAsync(url, payload);
            response.StatusCode.Should().Be(expectedCode);

            return response;
        }

        public async Task<TModel> PatchAsync<TModel>(string url, object command,
            HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await PatchAsync(url, command, expectedCode);
            var json = await response.Content.ReadAsStringAsync();
            return DeserializeObject<TModel>(json);
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, object command,
            HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var payload = GetPayload(command);
            var response = await _client.PatchAsync(url, payload);
            response.StatusCode.Should().Be(expectedCode);

            return response;
        }

        public async Task DeleteAsync(string url,
            HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await _client.DeleteAsync(url);
            response.StatusCode.Should().Be(expectedCode);
        }

        private static StringContent GetPayload(object data)
            => data.GetPayload(JsonSerializeService.CamelCaseContractResolver);

        private static TModel DeserializeObject<TModel>(string data)
            => JsonSerializeService.Deserialize<TModel>(data);
    }
}
